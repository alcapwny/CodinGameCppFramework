#if !defined(CODING_LOCALDEBUGGING)
#pragma GCC optimize "O3,omit-frame-pointer,inline"
#endif

//////////////////////////////////
// System includes

#if !defined(CODING_EXLUDESYSTEMHEADERS)
//#include <iostream>
#else
//CODING_EXLUDESYSTEMHEADERS#include <iostream>
#endif

//////////////////////////////////
// Includes

#include <codingame/logging.h>
#include <codingame/game.h>

#include <SampleAsciiArtInputData.h>

//////////////////////////////////

int main()
{
    Game game("SampleAsciiArt.txt");
    std::istream& dataStream = game.GetDataStream();

    SampleAsciiArtGlobalInputData globalData;
    dataStream >> globalData;
    Logging::GetInputDataStream() << globalData;

    const int textLength = globalData.m_Text.m_String.size();
    const char* text = globalData.m_Text.m_String.c_str();
    std::vector<int> letterIndices;
    letterIndices.reserve(textLength);

    //Determine letter index
    for (int i = 0; i < textLength; ++i)
    {
        const char letterValue = text[i];
        int index = 0;
        if (letterValue >= 'a' && letterValue <= 'z')
        {
            index = letterValue - 'a';
        }
        else if (letterValue >= 'A' && letterValue <= 'Z')
        {
            index = letterValue - 'A';
        }
        else
        {
            index = 26; //Question mark
        }

        letterIndices.push_back(index);
    }

    //Output ascii art
    for (int height = 0; height < globalData.m_Height; ++height)
    {
        for (int letterIndex : letterIndices)
        {
            const int characterOffset = letterIndex * globalData.m_Length;
            std::cout << globalData.m_AsciiArtRows[height].m_String.substr(characterOffset, globalData.m_Length);
        }

        std::cout << std::endl;
    }

    return 0;
}