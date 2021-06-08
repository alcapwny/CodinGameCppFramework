// Heavily inspired from the Dear ImGui Win32 DirectX11 example

#include <cgfimgui.h>

#include <backends/imgui_impl_win32.h>
#include <backends/imgui_impl_dx11.h>

#include <d3d11.h>
#include <tchar.h>

#include <iostream>

extern IMGUI_IMPL_API LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

/////////////////////////////////////////////////////////////////////////////////////
/// CgfImGui::InitParam

CGFImGui::InitParam::InitParam()
    : m_X(100)
    , m_Y(10)
    , m_Width(1280)
    , m_Height(1030)
    , m_ClearColor{0.45f, 0.55f, 0.60f, 1.00f}
{
}

/////////////////////////////////////////////////////////////////////////////////////
/// CgfImGui::Impl

class CGFImGui::Impl
{
public:
    static LRESULT WINAPI WndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

    void Initialize(const CGFImGui::InitParam& initParam);
    bool UpdateFrame();

private:
    void CreateImGuiWindow(const CGFImGui::InitParam& initParam);
    void CreateDeviceD3D();
    void CreateRenderTarget();

    void CleanupDeviceD3D();
    void CleanupRenderTarget();
    void CleanupImGuiWindow();

    void ShowWindow();
    void Resize(unsigned int sizeX, unsigned int sizeY);

    void InitializeImGui();

    void AssertAndLogError(const char* context, unsigned long errorCode);

    WNDCLASSEX m_WindowClass;
    HWND m_Window = nullptr;
    ID3D11Device* m_D3DDevice = nullptr;
    ID3D11DeviceContext* m_D3DDeviceContext = nullptr;
    IDXGISwapChain* m_SwapChain = nullptr;
    ID3D11RenderTargetView* m_MainRenderTargetView = nullptr;

    float m_ClearColor[4] = { 0.45f, 0.55f, 0.60f, 1.00f };
    //Used to avoid requiring users call a BeginFrame and EndFrame function.
    bool m_IsFirstFrame = true;
};

LRESULT WINAPI CGFImGui::Impl::WndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    if (ImGui_ImplWin32_WndProcHandler(hWnd, msg, wParam, lParam))
        return true;

    switch (msg)
    {
    case WM_SIZE:
    {
        CGFImGui* cgfImGui = CGFImGui::GetInstance();
        if (wParam != SIZE_MINIMIZED)
        {
            cgfImGui->m_Impl->Resize((UINT)LOWORD(lParam), (UINT)HIWORD(lParam));
        }
        return 0;
    }
    case WM_SYSCOMMAND:
    {
        if ((wParam & 0xfff0) == SC_KEYMENU) // Disable ALT application menu
            return 0;
        break;
    }
    case WM_DESTROY:
    {
        ::PostQuitMessage(0);
        return 0;
    }
    }

    return ::DefWindowProc(hWnd, msg, wParam, lParam);
}

void CGFImGui::Impl::Initialize(const CGFImGui::InitParam& initParam)
{
    std::copy_n(initParam.m_ClearColor, sizeof(initParam.m_ClearColor) / sizeof(*initParam.m_ClearColor), m_ClearColor);

    CreateImGuiWindow(initParam);
    CreateDeviceD3D();
    ShowWindow();
    InitializeImGui();
}

bool CGFImGui::Impl::UpdateFrame()
{
    if (!m_IsFirstFrame)
    {
        // Rendering
        ImGui::Render();
        m_D3DDeviceContext->OMSetRenderTargets(1, &m_MainRenderTargetView, nullptr);
        m_D3DDeviceContext->ClearRenderTargetView(m_MainRenderTargetView, m_ClearColor);
        ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());

        m_SwapChain->Present(0, 0);
    }
    else
    {
        m_IsFirstFrame = false;
    }

    bool done = false;

    // Poll and handle messages (inputs, window resize, etc.)
    // You can read the io.WantCaptureMouse, io.WantCaptureKeyboard flags to tell if dear imgui wants to use your inputs.
    // - When io.WantCaptureMouse is true, do not dispatch mouse input data to your main application.
    // - When io.WantCaptureKeyboard is true, do not dispatch keyboard input data to your main application.
    // Generally you may always pass all inputs to dear imgui, and hide them from your application based on those two flags.
    {
        MSG msg;
        while (::PeekMessage(&msg, NULL, 0U, 0U, PM_REMOVE))
        {
            ::TranslateMessage(&msg);
            ::DispatchMessage(&msg);
            if (msg.message == WM_QUIT)
                done = true;
        }
    }

    if (done)
    {
        return false;
    }

    // Start the Dear ImGui frame
    ImGui_ImplDX11_NewFrame();
    ImGui_ImplWin32_NewFrame();
    ImGui::NewFrame();

    return true;
}

void CGFImGui::Impl::CreateImGuiWindow(const CGFImGui::InitParam& initParam)
{
    // Create application window
    //ImGui_ImplWin32_EnableDpiAwareness();

    WNDCLASSEX& windowClass = m_WindowClass;
    windowClass = { sizeof(WNDCLASSEX), CS_CLASSDC, CGFImGui::Impl::WndProc, 0L, 0L, GetModuleHandle(NULL), NULL, NULL, NULL, NULL, _T("CgfImGui"), NULL };
    ATOM result = ::RegisterClassEx(&windowClass);
    if (result == 0)
    {
        AssertAndLogError("Register window", GetLastError());
    }
    m_Window = ::CreateWindow(windowClass.lpszClassName, _T("CgfImGui"), WS_OVERLAPPEDWINDOW, initParam.m_X, initParam.m_Y, initParam.m_Width, initParam.m_Height, NULL, NULL, windowClass.hInstance, NULL);

    if (m_Window == INVALID_HANDLE_VALUE)
    {
        AssertAndLogError("Create window", GetLastError());
    }
}

void CGFImGui::Impl::CreateDeviceD3D()
{
    // Setup swap chain
    DXGI_SWAP_CHAIN_DESC swapChainDescriptor;
    ZeroMemory(&swapChainDescriptor, sizeof(swapChainDescriptor));
    swapChainDescriptor.BufferCount = 2;
    swapChainDescriptor.BufferDesc.Width = 0;
    swapChainDescriptor.BufferDesc.Height = 0;
    swapChainDescriptor.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
    swapChainDescriptor.BufferDesc.RefreshRate.Numerator = 60;
    swapChainDescriptor.BufferDesc.RefreshRate.Denominator = 1;
    swapChainDescriptor.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;
    swapChainDescriptor.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
    swapChainDescriptor.OutputWindow = m_Window;
    swapChainDescriptor.SampleDesc.Count = 1;
    swapChainDescriptor.SampleDesc.Quality = 0;
    swapChainDescriptor.Windowed = TRUE;
    swapChainDescriptor.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;

    UINT createDeviceFlags = 0;
    //createDeviceFlags |= D3D11_CREATE_DEVICE_DEBUG;
    D3D_FEATURE_LEVEL featureLevel;
    const D3D_FEATURE_LEVEL featureLevelArray[2] = { D3D_FEATURE_LEVEL_11_0, D3D_FEATURE_LEVEL_10_0, };
    HRESULT result = D3D11CreateDeviceAndSwapChain(NULL, D3D_DRIVER_TYPE_HARDWARE, NULL, createDeviceFlags, featureLevelArray, 2, D3D11_SDK_VERSION, &swapChainDescriptor, &m_SwapChain, &m_D3DDevice, &featureLevel, &m_D3DDeviceContext);
    if (result != S_OK)
    {
        AssertAndLogError("Create D3D11 device and swap chain", result);
    }

    CreateRenderTarget();
}

void CGFImGui::Impl::CreateRenderTarget()
{
    ID3D11Texture2D* backBuffer = nullptr;
    m_SwapChain->GetBuffer(0, IID_PPV_ARGS(&backBuffer));
    HRESULT result = m_D3DDevice->CreateRenderTargetView(backBuffer, nullptr, &m_MainRenderTargetView);
    if (result != S_OK)
    {
        AssertAndLogError("Create render target view", result);
    }

    backBuffer->Release();
}

void CGFImGui::Impl::CleanupDeviceD3D()
{
    CleanupRenderTarget();

    if (m_SwapChain)
    {
        m_SwapChain->Release();
        m_SwapChain = nullptr;
    }

    if (m_D3DDeviceContext)
    {
        m_D3DDeviceContext->Release();
        m_D3DDeviceContext = nullptr;
    }

    if (m_D3DDevice)
    {
        m_D3DDevice->Release();
        m_D3DDevice = nullptr;
    }
}

void CGFImGui::Impl::CleanupRenderTarget()
{
    if (m_MainRenderTargetView)
    {
        m_MainRenderTargetView->Release();
        m_MainRenderTargetView = nullptr;
    }
}

void CGFImGui::Impl::CleanupImGuiWindow()
{
    ::DestroyWindow(m_Window);

    WNDCLASSEX& windowClass = m_WindowClass;
    ::UnregisterClass(windowClass.lpszClassName, windowClass.hInstance);
}

void CGFImGui::Impl::ShowWindow()
{
    ::ShowWindow(m_Window, SW_SHOWDEFAULT);
    ::UpdateWindow(m_Window);
}

void CGFImGui::Impl::Resize(unsigned int sizeX, unsigned int sizeY)
{
    if (m_D3DDevice != nullptr)
    {
        CleanupRenderTarget();
        m_SwapChain->ResizeBuffers(0, sizeX, sizeY, DXGI_FORMAT_UNKNOWN, 0);
        CreateRenderTarget();
    }
}

void CGFImGui::Impl::InitializeImGui()
{
    IMGUI_CHECKVERSION();
    ImGui::CreateContext();
    ImGui::StyleColorsDark();

    ImGui::GetIO().ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;

    ImGui_ImplWin32_Init(m_Window);
    ImGui_ImplDX11_Init(m_D3DDevice, m_D3DDeviceContext);
}

void CGFImGui::Impl::AssertAndLogError(const char* context, unsigned long errorCode)
{
    std::cerr << context << " failed with error: " << std::hex << errorCode << std::endl;
    assert(false);
}

/////////////////////////////////////////////////////////////////////////////////////// 
/// CgfImGui

CGFImGui::CGFImGui(const InitParam& initParam)
{
    m_Impl = new Impl();
    m_Impl->Initialize(initParam);
}

bool CGFImGui::UpdateFrame()
{
    return m_Impl->UpdateFrame();
}