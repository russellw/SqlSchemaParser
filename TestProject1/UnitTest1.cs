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

	[Fact]
	public void UnclosedParen() {
		var e = Assert.Throws<SqlError>(() => Parse("("));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("foo("));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("create("));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("create table("));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("create table abc("));
		Assert.Matches(".*:2: ", e.Message);
	}

	static Schema Parse(string text) {
		var schema = new Schema();
		Parser.Parse("SQL", text, schema);
		return schema;
	}
}