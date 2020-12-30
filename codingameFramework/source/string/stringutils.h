#pragma once

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <string>
#include <vector>
#include <algorithm>
#include <functional> 
#include <cctype>
#include <locale>
#else
CODING_EXLUDESYSTEMHEADERS#include <string>
CODING_EXLUDESYSTEMHEADERS#include <vector>
CODING_EXLUDESYSTEMHEADERS#include <algorithm>
CODING_EXLUDESYSTEMHEADERS#include <functional>
CODING_EXLUDESYSTEMHEADERS#include <cctype>
CODING_EXLUDESYSTEMHEADERS#include <locale>
#endif

class StringUtils
{
public:
    static std::vector<std::string> Split(const std::string& input, const char* delimiter);

    // trim from left
    static std::string& LeftTrim(std::string& input);

    // trim from right
    static std::string& RightTrim(std::string& input);

    // trim from left and right
    static std::string& Trim(std::string& input);
};

#include <string/stringutils.inl>