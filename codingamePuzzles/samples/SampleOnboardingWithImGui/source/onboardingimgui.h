#pragma once

#if defined(CODING_IMGUI)

#include <onboardingupdatemode.h>
#include <onboardingdatamodel.h>

class OnboardingImGui
{
public:
    void UpdateDataModel(OnboardingDataModel&& dataModel);
    OnboardingUpdateMode UpdateFrame();

private:
    void DrawGrid(ImDrawList* drawList, ImVec2 canvasPoint0, ImVec2 canvasPoint1, ImVec2 canvasSize);
    void DrawCharacters(ImDrawList* drawList, ImVec2 canvasPoint0, ImVec2 canvasPoint1, ImVec2 canvasSize);
    void DrawCharater(ImDrawList* drawList, ImVec2 position, const char* name, ImColor color);

    ImVec2 GetCanvasSize() const;

    OnboardingDataModel m_DataModel;

    ImColor m_CanvasBackgroundColor{ 50, 50, 50, 255 };
    ImColor m_CanvasBorderColor{ 255, 255, 255, 255 };

    ImColor m_GridColor{ 200, 200, 200, 40 };

    ImColor m_PlayerColor{ 255, 255, 255, 255 };
    ImColor m_EnemyColor{ 255, 0, 0, 255 };
};

#include <onboardingimgui.inl>

#endif //defined(CODING_IMGUI)