#pragma once

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <string>
#include <vector>
#else
CODING_EXLUDESYSTEMHEADERS#include <string>
CODING_EXLUDESYSTEMHEADERS#include <vector>
#endif

class StringUtils
{
public:
    static std::vector<std::string> Split(const std::string& input, const char* delimiter);
};

#include <string/stringutils.inl>