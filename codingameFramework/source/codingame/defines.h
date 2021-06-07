#pragma once

#define CODING_ASSERT(condition) if (!condition) { Logging::LogDebug() << "CRASH" << std::endl; int* crashyTime = 0; *crashyTime = 3; }