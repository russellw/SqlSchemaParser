using SqlSchemaParser;

namespace TestProject1;
public class UnitTest1 {
	[Fact]
	public void Blank() {
		Parser.Parse("");
		Parser.Parse("\t");
	}

	[Fact]
	public void LineComment() {
		Parser.Parse("--");
		Parser.Parse("--\r\n--\r\n");
	}

	[Fact]
	public void BlockComment() {
		Parser.Parse("/**/");
		Parser.Parse("/***/");
		Parser.Parse("/****/");
		Parser.Parse("/*****/");
		Parser.Parse("/* /*/");
		Parser.Parse("/* /**/");
		Parser.Parse("/* /***/");
		Parser.Parse("/* /****/");

		var e = Assert.Throws<SqlError>(() => Parser.Parse("/*/"));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parser.Parse("\n/*"));
		Assert.Matches(".*:2: ", e.Message);
	}
}