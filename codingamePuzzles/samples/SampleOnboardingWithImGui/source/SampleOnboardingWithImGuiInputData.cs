using CGFCodeGenerator;

public class Enemy
{
    [GroupWithNext]
    public string m_Name;
    public int m_Distance;
}

[FrameData]
public class SampleOnboardingWithImGuiFrameInputData
{
    [GroupWithNext]
    public Enemy m_Enemy1;
    public Enemy m_Enemy2;
}
