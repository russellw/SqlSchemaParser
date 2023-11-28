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
	public void UnclosedQuote() {
		var e = Assert.Throws<SqlError>(() => Parse("'"));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("\""));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("`"));
		Assert.Matches(".*:1: ", e.Message);

		e = Assert.Throws<SqlError>(() => Parse("["));
		Assert.Matches(".*:1: ", e.Message);
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

	[Fact]
	public void Ignored() {
		var schema = Parse(" \n");
		Assert.Empty(schema.Ignored);
		Assert.Equal("", schema.IgnoredString());

		schema = Parse("abc\n");
		Assert.Single(schema.Ignored);
		var span = schema.Ignored[0];
		Assert.Equal("abc\n", span.Location.Text);
		Assert.Equal(0, span.Location.Start);
		Assert.Equal(3, span.End);
		Assert.NotEqual("", schema.IgnoredString());

		schema = Parse("abc def\n");
		Assert.Single(schema.Ignored);
		span = schema.Ignored[0];
		Assert.Equal("abc def\n", span.Location.Text);
		Assert.Equal(0, span.Location.Start);
		Assert.Equal(7, span.End);
		Assert.NotEqual("", schema.IgnoredString());
	}

	[Fact]
	public void QualifiedName() {
		var a1 = new QualifiedName("a");
		var a2 = new QualifiedName("a");
		Assert.Equal(a1, a2);
		Assert.Equal("a", a1.ToString());

		a1.Names.Add("b");
		Assert.NotEqual(a1, a2);
		Assert.Equal("a.b", a1.ToString());
	}

	[Fact]
	public void Column() {
		var c = new Column("foo", new DataType("varchar"));
		Assert.Equal("foo varchar", c.ToString());
	}

	[Fact]
	public void CreateTable() {
		var schema = Parse("create table table1(column1 int)");
		Assert.Empty(schema.Ignored);
		Assert.Single(schema.Tables);
		var table = schema.Tables[0];
		Assert.Single(table.Name.Names);
		Assert.Equal("table1", table.Name.Names[0]);
		Assert.Single(table.Columns);
		var column = table.Columns[0];
		Assert.Equal("column1", column.Name);
		Assert.Equal(new DataType("int"), column.DataType);

		schema = Parse("create table table1(column1 int,column2 int)");
		Assert.Empty(schema.Ignored);
		Assert.Single(schema.Tables);
		table = schema.Tables[0];
		Assert.Single(table.Name.Names);
		Assert.Equal("table1", table.Name.Names[0]);
		Assert.Equal(2, table.Columns.Count);

		schema = Parse("create table table1(column1 int(10,5)) with cream and sugar");
		Assert.NotEmpty(schema.Ignored);
		Assert.Single(schema.Tables);
		table = schema.Tables[0];
		Assert.Single(table.Name.Names);
		Assert.Equal("table1", table.Name.Names[0]);
		Assert.Single(table.Columns);
		column = table.Columns[0];
		Assert.Equal("column1", column.Name);
		var dataType = new DataType("int");
		dataType.Size = 10;
		dataType.Scale = 5;
		Assert.Equal(dataType, column.DataType);
	}

	[Fact]
	public void SampleDB1() {
		var schema = ParseFile("sql-server-samples/sampleDB1.sql");
		Assert.Equal(2, schema.Tables.Count);

		var emp = schema.Tables[0];
		var key = emp.PrimaryKey!;
		Assert.Single(key.Columns);
		Assert.False(key.Columns[0].Nullable);

		var dept = schema.Tables[1];
		Assert.Null(dept.PrimaryKey);
	}

	[Fact]
	public void Northwind() {
		var schema = ParseFile("sql-server-samples/instnwnd.sql");
		Assert.Equal(13, schema.Tables.Count);
	}

	[Fact]
	public void NorthwindAzure() {
		var schema = ParseFile("sql-server-samples/instnwnd (Azure SQL Database).sql");
		Assert.Equal(13, schema.Tables.Count);
	}

	[Fact]
	public void Pubs() {
		var schema = ParseFile("sql-server-samples/instpubs.sql");
		Assert.Equal(11, schema.Tables.Count);
	}

	[Fact]
	public void PostgresNorthwind() {
		var schema = ParseFile("northwind_psql/northwind.sql");
		// The postgres version has an extra table for US states
		Assert.Equal(14, schema.Tables.Count);
	}

	[Fact]
	public void Employees() {
		var schema = ParseFile("mysql-samples/employees.sql");
		Assert.Equal(6, schema.Tables.Count);
	}

	static Schema Parse(string text) {
		var schema = new Schema();
		Parser.Parse("SQL", text, schema);
		return schema;
	}

	static Schema ParseFile(string file) {
		var schema = new Schema();
		Parser.Parse(file, File.ReadAllText(file), schema);
		return schema;
	}
}
