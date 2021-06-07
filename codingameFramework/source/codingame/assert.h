#pragma once

#include <codingame/logging.h>

#if defined(CODING_LOCALDEBUGGING)
#define CODING_ASSERT(condition)                                    \
    if (!(condition))                                               \
    {                                                               \
        Logging::LogDebug() << "Assert failed" << std::endl <<      \
            "Condition:" << #condition << std::endl;                \
        __debugbreak();                                             \
    }
#else
#define CODING_ASSERT(condition)                                    \
    if (!(condition))                                               \
    {                                                               \
        Logging::LogDebug() << "Assert failed" << std::endl <<      \
            "Condition:" << #condition << std::endl;                \
        int* crashyTime = 0;                                        \
        *crashyTime = 3;                                            \
    }
#endif