#pragma once

template<class T>
class CGFSingleton
{
public:
    CGFSingleton()
    {
        ms_Instance = this;
    }

    ~CGFSingleton()
    {
        ms_Instance = nullptr;
    }

    CGFSingleton(const CGFSingleton&) = delete;
    CGFSingleton& operator=(const CGFSingleton&) = delete;

    static T* GetInstance()
    {
        return (T*)ms_Instance;
    }

private:
    static inline CGFSingleton<T>* ms_Instance = nullptr;
};