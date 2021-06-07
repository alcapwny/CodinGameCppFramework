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

    $puzzlename$GlobalInputData globalData;
    game.SerializeGlobalData(globalData);

    // game loop
    while (game.RunGameLoop())
    {
        $puzzlename$FrameInputData frameData;
        game.SerializeFrameData(frameData);

        // game loop code
    }

    return 0;
}
