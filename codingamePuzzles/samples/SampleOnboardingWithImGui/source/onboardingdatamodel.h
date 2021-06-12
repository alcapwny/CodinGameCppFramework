#pragma once

/////////////////////////////////////////////////////////////////////////////////////
// System includes

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <limits>
#else
CODING_EXLUDESYSTEMHEADERS#include <limits>
#endif

/////////////////////////////////////////////////////////////////////////////////////
struct OnboardingDataModel
{
    static constexpr int InvalidDistance = std::numeric_limits<int>::max();

    OnboardingDataModel(const Enemy& enemy1, const Enemy& enemy2)
        : m_Enemy1(enemy1)
        , m_Enemy2(enemy2)
    {
    }

    OnboardingDataModel()
    {
        m_Enemy1.m_Distance = InvalidDistance;
        m_Enemy2.m_Distance = InvalidDistance;
    }

    Enemy m_Enemy1;
    Enemy m_Enemy2;
};