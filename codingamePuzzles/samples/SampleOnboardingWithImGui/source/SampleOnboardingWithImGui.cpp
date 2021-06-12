#if !defined(CODING_LOCALDEBUGGING)
#pragma GCC optimize "O3,omit-frame-pointer,inline"
#endif

/////////////////////////////////////////////////////////////////////////////////////
// Includes

#include <codingame/defines.h>
#include <codingame/assert.h>
#include <codingame/logging.h>
#include <codingame/game.h>

#if defined(CODING_IMGUI)
#include <cgfimgui.h>
#include <imgui.h>
#endif

#include <SampleOnboardingWithImGuiInputData.h>
#include <onboardingupdatemode.h>
#include <onboardingdatamodel.h>
#include <onboardingimgui.h>

/////////////////////////////////////////////////////////////////////////////////////

int main()
{
    Game game("SampleOnboardingWithImGui.txt");

#if defined(CODING_IMGUI)
    CGFImGui::InitParam initParam;
    initParam.m_Width = 650;
    initParam.m_Height = 500;
    CGFImGui cgfImGui(initParam);
    OnboardingImGui onboardingImGui;
#endif

    // game loop
    while (game.RunGameLoop())
    {
        OnboardingUpdateMode updateMode = OnboardingUpdateMode::Step;

#if defined(CODING_IMGUI)
        bool shouldContinue = cgfImGui.UpdateFrame();
        if (!shouldContinue)
            break;

        updateMode = onboardingImGui.UpdateFrame();
#endif

        if (updateMode == OnboardingUpdateMode::Step)
        {
            SampleOnboardingWithImGuiFrameInputData frameData;
            game.SerializeFrameData(frameData);
            
            if (frameData.m_Enemy1.m_Distance < frameData.m_Enemy2.m_Distance)
            {
                std::cout << frameData.m_Enemy1.m_Name << std::endl;
            }
            else
            {
                std::cout << frameData.m_Enemy2.m_Name << std::endl;
            }

#if defined(CODING_IMGUI)
            onboardingImGui.UpdateDataModel(OnboardingDataModel{ frameData.m_Enemy1, frameData.m_Enemy2 });
#endif
        }
    }

    return 0;
}
