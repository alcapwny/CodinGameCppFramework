using CGFCodeGenerator;

[GlobalData]
public class SampleAsciiArtGlobalInputData
{
    public int m_Length;
    public int m_Height;
    public LineString m_Text;

    [Array("m_Height")]
    public LineString m_AsciiArtRows;
}