#pragma once

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <iostream>
#include <fstream>
#else
CODING_EXLUDESYSTEMHEADERS#include <iostream>
CODING_EXLUDESYSTEMHEADERS#include <fstream>
#endif

#include <codingame/defines.h>

class Game
{
public:
    Game(const char* inputFilename)
    {
#if defined(CODING_LOCALDEBUGGING)
        m_DataStream.open(inputFilename);
        if (!m_DataStream.is_open())
        {
            constexpr int ErrorMessageSize = 1024;
            char errorMessage[ErrorMessageSize];
            std::cerr << "Error: " << strerror_s(errorMessage, ErrorMessageSize, errno);
            __debugbreak();
        }
#endif
    }

    std::istream& GetDataStream()
    {
#if defined(CODING_LOCALDEBUGGING)
        return m_DataStream;
#else
        return std::cin;
#endif
    }

    static bool RunGameLoop(std::istream& cin)
    {
        ++g_FrameCounter;

#if defined(CODING_LOCALDEBUGGING)
        return cin.peek() > 0;
#else
        return true;
#endif
    }

    static void EndCurrentGameLoop()
    {
        g_FrameCounter++;
    }

    static int GetFrameCounter()
    {
        return g_FrameCounter;
    }

private:
#if defined(CODING_LOCALDEBUGGING)
    std::ifstream m_DataStream;
#endif

    static int g_FrameCounter;
};

int Game::g_FrameCounter = -1;