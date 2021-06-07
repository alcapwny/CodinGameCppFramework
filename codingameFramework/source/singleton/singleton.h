#pragma once

#include <codingame/assert.h>

template<class T>
class Singleton
{
public:
    Singleton()
    {
        CODING_ASSERT(ms_Instance == nullptr);
        ms_Instance = this;
    }

    ~Singleton()
    {
        ms_Instance = nullptr;
    }

    Singleton(const Singleton&) = delete;
    Singleton& operator=(const Singleton&) = delete;

    static T* GetInstance()
    {
        return (T*)ms_Instance;
    }

private:
    static inline Singleton<T>* ms_Instance = nullptr;
};