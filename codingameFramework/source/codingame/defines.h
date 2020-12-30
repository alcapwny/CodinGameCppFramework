#pragma once

//Disable warnings
#if defined(CODING_LOCALDEBUGGING)
#pragma warning(disable:4100) //Warning	C4100 - unreferenced formal parameter
#endif

#define CODING_ASSERT(condition) if (!condition) { Logging::LogDebug() << "CRASH" << std::endl; int* crashyTime = 0; *crashyTime = 3; }