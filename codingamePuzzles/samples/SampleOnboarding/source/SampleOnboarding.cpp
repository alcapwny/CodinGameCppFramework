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

#include <codingame/defines.h>
#include <codingame/assert.h>
#include <codingame/logging.h>
#include <codingame/game.h>

#include <SampleOnboardingInputData.h>

//////////////////////////////////

int main()
{
    Game game("SampleOnboarding.txt");

    // game loop
    while (game.RunGameLoop())
    {
        SampleOnboardingFrameInputData frameData;
        game.SerializeFrameData(frameData);

        if (frameData.m_Dist1 < frameData.m_Dist2)
        {
            std::cout << frameData.m_Enemy1 << std::endl;
        }
        else
        {
            std::cout << frameData.m_Enemy2 << std::endl;
        }
    }

    return 0;
}
