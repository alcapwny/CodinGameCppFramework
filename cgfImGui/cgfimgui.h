#pragma once

#include <cgfsingleton.h>

class CGFImGui : public CGFSingleton<CGFImGui>
{
public:
    struct InitParam
    {
        InitParam();

        int m_X;
        int m_Y;
        int m_Width;
        int m_Height;

        float m_ClearColor[4];
    };

    CGFImGui(const InitParam& initParam = InitParam());

    //Must be called before making any ImGui call
    // Returns true if window is still open
    // Returns false if window is closed, meaning that the program can exit
    bool UpdateFrame();

private:
    class Impl;
    Impl* m_Impl = nullptr;
};