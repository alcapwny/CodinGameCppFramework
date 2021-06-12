#if defined(CODING_IMGUI)

#include <onboardingimgui.h>

void OnboardingImGui::UpdateDataModel(OnboardingDataModel&& dataModel)
{
    m_DataModel = std::move(dataModel);
}

OnboardingUpdateMode OnboardingImGui::UpdateFrame()
{
    OnboardingUpdateMode updateMode = OnboardingUpdateMode::Idle;

    ImGui::SetNextWindowSize(ImVec2{ 500.f, 300.f }, ImGuiCond_FirstUseEver);
    if (ImGui::Begin("Onboarding"))
    {
        if (ImGui::Button("Step"))
        {
            updateMode = OnboardingUpdateMode::Step;
        }

        //Get canvas dimensions
        ImVec2 canvasPoint0 = ImGui::GetCursorScreenPos(); //ImDrawList API uses screen coordinates!
        ImVec2 canvasSize = GetCanvasSize();
        ImVec2 canvasPoint1 = { canvasPoint0.x + canvasSize.x, canvasPoint0.y + canvasSize.y };

        //Draw canvas border and background color
        ImDrawList* drawList = ImGui::GetWindowDrawList();
        drawList->AddRectFilled(canvasPoint0, canvasPoint1, m_CanvasBackgroundColor);
        drawList->AddRect(canvasPoint0, canvasPoint1, m_CanvasBorderColor);

        //Push clipping rectangle to avoid drawing things outside of the canvas
        drawList->PushClipRect(canvasPoint0, canvasPoint1, true);

        DrawGrid(drawList, canvasPoint0, canvasPoint1, canvasSize);

        DrawCharacters(drawList, canvasPoint0, canvasPoint1, canvasSize);

        //Done with the clipping rectangle
        drawList->PopClipRect();
    }

    ImGui::End();

    return updateMode;
}

void OnboardingImGui::DrawGrid(ImDrawList* drawList, ImVec2 canvasPoint0, ImVec2 canvasPoint1, ImVec2 canvasSize)
{
    float m_GridStep = 50.0f;
    for (float x = canvasSize.x; x >= 0; x -= m_GridStep)
    {
        ImVec2 start(canvasPoint0.x + x, canvasPoint0.y);
        ImVec2 end(canvasPoint0.x + x, canvasPoint1.y);

        drawList->AddLine(ImVec2(start), ImVec2(end), m_GridColor);
    }
    for (float y = canvasSize.y; y >= 0; y -= m_GridStep)
    {
        ImVec2 start(canvasPoint0.x, canvasPoint0.y + y);
        ImVec2 end(canvasPoint1.x, canvasPoint0.y + y);

        drawList->AddLine(start, end, m_GridColor);
    }
}

void OnboardingImGui::DrawCharacters(ImDrawList* drawList, ImVec2 canvasPoint0, ImVec2 canvasPoint1, ImVec2 canvasSize)
{
    auto calculateCharacterX = [&](int distance)
    {
        float distanceMultipler = 3.0f;
        float offsetFromEdge = 50.0f;
        return canvasPoint1.x - (distance * distanceMultipler) - offsetFromEdge;
    };

    auto calculateCharacterY = [&](float canvasRatio)
    {
        return canvasPoint0.y + (canvasSize.y * canvasRatio);
    };

    auto calculateCharacterPosition = [&](int distance, float canvasHeightRatio)
    {
        float x = calculateCharacterX(distance);
        float y = calculateCharacterY(canvasHeightRatio);
        return ImVec2{ x, y };
    };

    float playerHeightRatio = 1.0f / 2.0f;
    float enemy1HeightRatio = 1.0f / 3.0f;
    float enemy2HeightRatio = 2.0f / 3.0f;

    {
        ImVec2 playerPosition = calculateCharacterPosition(0, playerHeightRatio);
        DrawCharater(drawList, playerPosition, "Player", m_PlayerColor);
    }

    if (m_DataModel.m_Enemy1.m_Distance != OnboardingDataModel::InvalidDistance)
    {
        ImVec2 enemyPosition = calculateCharacterPosition(m_DataModel.m_Enemy1.m_Distance, enemy1HeightRatio);
        DrawCharater(drawList, enemyPosition, m_DataModel.m_Enemy1.m_Name.c_str(), m_EnemyColor);
    }

    if (m_DataModel.m_Enemy2.m_Distance != OnboardingDataModel::InvalidDistance)
    {
        ImVec2 enemyPosition = calculateCharacterPosition(m_DataModel.m_Enemy2.m_Distance, enemy2HeightRatio);
        DrawCharater(drawList, enemyPosition, m_DataModel.m_Enemy2.m_Name.c_str(), m_EnemyColor);
    }
}

void OnboardingImGui::DrawCharater(ImDrawList* drawList, ImVec2 position, const char* name, ImColor color)
{
    float playerSize = 5.0f;
    drawList->AddCircle(position, playerSize, color);

    ImVec2 textSize = ImGui::CalcTextSize(name);
    ImVec2 textPosition{ position.x - (textSize.x / 2), position.y + playerSize };
    drawList->AddText(textPosition, color, name);
}

ImVec2 OnboardingImGui::GetCanvasSize() const
{
    ImVec2 canvasSize = ImGui::GetContentRegionAvail();     // Resize canvas to what's available
    if (canvasSize.x < 50.0f)
        canvasSize.x = 50.0f;
    if (canvasSize.y < 50.0f)
        canvasSize.y = 50.0f;

    return canvasSize;
}

#endif //defined(CODING_IMGUI)