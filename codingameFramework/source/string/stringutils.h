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
};

#include <string/stringutils.inl>