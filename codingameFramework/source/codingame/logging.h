#pragma once

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <iostream>
#else
CODING_EXLUDESYSTEMHEADERS#include <iostream>
#endif

/////////////////////////////////////////////////////////////////////////////////////

class Logging
{
public:
    static std::ostream& GetInputDataStream() { return std::cerr; }

    static std::ostream& LogDebug()
    {
        std::cerr << "Debug: ";
        return std::cerr;
    }

    static std::ostream& LogInputData(std::ostream& outputStream)
    {
        outputStream << "Log: ";
        return outputStream;
    }
};
