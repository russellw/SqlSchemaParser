using SqlSchemaParser;

namespace TestProject1;
public class UnitTest1
{
    [Fact]
    public void Blank()
    {
        Parser.Parse("");
        Parser.Parse("\t");
    }
}