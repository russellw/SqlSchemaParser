using SqlSchemaParser;

namespace TestProject1;
public class UnitTest1 {
	[Fact]
	public void Blank() {
		Parse("");
		Parse("\t");
	}

	[Fact]
	public void LineComment() {
		Parse("--");
		Parse("--\r\n--\r\n");
	}

	[Fact]
	public void BlockComment() {
		Parse("/**/");
		Parse("/***/");
		Parse("/****/");
		Parse("/*****/");
		Parse("/* /*/");
		Parse("/* /**/");
		Parse("/* /***/");
		Parse("/* /****/");

		var e = Assert.Throws<SqlError>(() => Parse("/*/"));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("\n/*"));
		Assert.Matches(".*:2: ", e.Message);
	}

	static void Parse(string text) {
		Parser.Parse("SQL", text);
	}
}