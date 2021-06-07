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

#include <$puzzlename$InputData.h>

//////////////////////////////////

int main()
{
    Game game("$puzzlename$.txt");
    std::istream& dataStream = game.GetDataStream();

    $puzzlename$GlobalInputData globalData;
    dataStream >> globalData;
    Logging::GetInputDataStream() << globalData;

    // game loop
    while (Game::RunGameLoop(dataStream))
    {
        $puzzlename$FrameInputData frameData;
        dataStream >> frameData;
        Logging::GetInputDataStream() << frameData;

        // game loop code
    }

    return 0;
}
