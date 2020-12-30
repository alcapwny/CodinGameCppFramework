using CGFCodeGenerator;

[GlobalData]
public class SampleAsciiArtGlobalInputData
{
    public int m_Length;
    public int m_Height;
    public LineString m_Text;

    [VariableSizeArray("m_Height")]
    public LineString m_AsciiArtRows;
}