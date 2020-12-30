#include <iostream>
#include <fstream>
#include <string>
#include <algorithm>
#include <functional> 
#include <cctype>

class StringUtils
{
public:
	// trim from left
	static std::string& LeftTrim(std::string& input)
	{
		input.erase(input.begin(), std::find_if(input.begin(), input.end(), std::not1(std::ptr_fun<int, int>(std::isspace))));
		return input;
	}

	// trim from right
	static std::string& RightTrim(std::string& input)
	{
		input.erase(std::find_if(input.rbegin(), input.rend(), std::not1(std::ptr_fun<int, int>(std::isspace))).base(), input.end());
		return input;
	}

	// trim from left and right
	static std::string& Trim(std::string& input)
	{
		LeftTrim(input);
		RightTrim(input);
		return input;
	}
};

//
// Parses log file to create inputs for local debugging sessions.
//

int main(int argc, char** argv)
{
	if (argc < 3)
	{
		std::cout << "Usage: <inputFile> <outputFile>" << std::endl;
		return -1;
	}

	std::ifstream inputFile;
	inputFile.open(argv[1]);
	if (!inputFile.is_open())
	{
		std::cout << "Can't open " << argv[1] << std::endl;
		return -2;
	}

	std::ofstream outputFile;
	outputFile.open(argv[2]);
	if (!outputFile.is_open())
	{
		std::cout << "Can't open " << argv[2] << std::endl;
		return -3;
	}

	const std::string logString("Log: ");
	const size_t logStringLength = logString.length();
	const std::string globalInput("Global Input:");
	const std::string frameInput("Frame Input:");

	bool isFirst = true;
	std::string line;
	while (getline(inputFile, line).good())
	{
		StringUtils::Trim(line);
		if (line.compare(0, logStringLength, logString) == 0)
		{
			if (line.compare(logStringLength, globalInput.length(), globalInput) != 0 && line.compare(logStringLength, frameInput.length(), frameInput) != 0)
			{
				if (isFirst)
				{
					isFirst = false;
				}
				else
				{
					outputFile << std::endl;
				}

				outputFile << line.substr(logStringLength);
			}
		}
	}

	std::cout << "File " << argv[2] << " has been updated.";
	return 0;
}