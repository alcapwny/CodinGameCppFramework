std::vector<std::string> StringUtils::Split(const std::string& input, const char* delimiter)
{
    size_t index = input.find_first_not_of(delimiter);
    if (index == std::string::npos)
    {
        return std::vector<std::string>();
    }

    size_t splitIndex = 0;
    std::vector<std::string> splitString;

    index = input.find_first_of(delimiter, splitIndex);
    do
    {
        if (index == std::string::npos)
        {
            splitString.push_back(input.substr(splitIndex));
            break;
        }

        splitString.push_back(input.substr(splitIndex, index - splitIndex));
        splitIndex = index + 1; //1 to skip delimiter
        index = input.find_first_of(delimiter, splitIndex);
    } while (true);

    return splitString;
}