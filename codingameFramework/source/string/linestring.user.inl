/////////////////////////////////////////////////////////////////////////////////////
///Hash:b2ccc98f3cc85795b2ab8c04f79cefc846f03a62f863fd2e0561d403bf9524ea
/////////////////////////////////////////////////////////////////////////////////////
// This file is generated when it doesn't exist or when the generated content changes
// and this file hasn't been modified by the user (that is, the calculated hash
// matches the hash in this file).
//
// *Modify as needed!*
/////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////
// System Includes

#if !defined(CODING_EXLUDESYSTEMHEADERS)
#include <iostream>
#include <string>
#else
CODING_EXLUDESYSTEMHEADERS#include <iostream>
CODING_EXLUDESYSTEMHEADERS#include <string>
#endif

/////////////////////////////////////////////////////////////////////////////////////
// Includes

#include <codingame/logging.h>

/////////////////////////////////////////////////////////////////////////////////////
class LineString : public LineString_Generated
{
public:
    friend std::istream& operator>>(std::istream& inputStream, LineString& lineString);
    friend std::ostream& operator<<(std::ostream& outputStream, const LineString& lineString);

};

/////////////////////////////////////////////////////////////////////////////////////
// LineString
std::istream& operator>>(std::istream& inputStream, LineString& lineString)
{
    getline(inputStream, lineString.m_String);

    return inputStream;
}

std::ostream& operator<<(std::ostream& outputStream, const LineString& lineString)
{
    Logging::LogInputData(outputStream) << lineString.m_String << std::endl;

    return outputStream;
}

/////////////////////////////////////////////////////////////////////////////////////
