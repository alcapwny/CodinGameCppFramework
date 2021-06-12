#pragma once

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <iostream>
#include <fstream>
#include <limits>
#else
CODING_EXLUDESYSTEMHEADERS#include <iostream>
CODING_EXLUDESYSTEMHEADERS#include <fstream>
CODING_EXLUDESYSTEMHEADERS#include <limits>
#endif

#include <codingame/assert.h>
#include <singleton/singleton.h>

class Game : public Singleton<Game>
{
public:
    enum class LoggingState
    {
        On,
        Off,
        Default,
    };

    Game(const char* inputFilename, LoggingState dataLoggingState = LoggingState::On)
        : m_DataLoggingState(dataLoggingState)
    {
        //Logs must either be enabled or disabled
        CODING_ASSERT(m_DataLoggingState != LoggingState::Default);

        Logging::LogDebug().precision(std::numeric_limits<double>::max_digits10);

#if defined(CODING_LOCALDEBUGGING)
        m_DataStream.open(inputFilename);
        if (!m_DataStream.is_open())
        {
            constexpr int ErrorMessageSize = 1024;
            char errorMessage[ErrorMessageSize];
            strerror_s(errorMessage, ErrorMessageSize, errno);
            std::cerr << "Error opening " << inputFilename << ": " << errorMessage << std::endl;
            CODING_ASSERT(false);
        }
#endif
    }

    template <class T>
    void SerializeGlobalData(T& data, LoggingState dataLoggingState = LoggingState::Default)
    {
        Serialize(data, dataLoggingState);
    }

    template <class T>
    void SerializeFrameData(T& data, LoggingState dataLoggingState = LoggingState::Default)
    {
        Serialize(data, dataLoggingState);
        ++m_FrameCounter;
    }

    std::istream& GetDataStream()
    {
#if defined(CODING_LOCALDEBUGGING)
        return m_DataStream;
#else
        return std::cin;
#endif
    }

    bool RunGameLoop()
    {
#if defined(CODING_LOCALDEBUGGING)
        return m_DataStream.peek() > 0;
#else
        return true;
#endif
    }

    static int GetFrameCounter()
    {
        return GetInstance()->m_FrameCounter;
    }

private:
    template <class T>
    void Serialize(T& data, LoggingState dataLoggingState = LoggingState::Default)
    {
        std::istream& dataStream = GetDataStream();
        dataStream >> data;

        LoggingState loggingState = dataLoggingState == LoggingState::Default ? m_DataLoggingState : dataLoggingState;
        if (loggingState == LoggingState::On)
        {
            Logging::GetInputDataStream() << data;
        }
    }

#if defined(CODING_LOCALDEBUGGING)
    std::ifstream m_DataStream;
#endif

    LoggingState m_DataLoggingState;

    int m_FrameCounter = -1;
};
