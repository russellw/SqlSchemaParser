namespace SqlSchemaParser;
public sealed class Parser {
	public static Schema Parse(string text, string file = "SQL", int line = 1) {
		var parser = new Parser(text, file, line);
		return parser.schema;
	}

	string text;
	string file;
	int line;
	Schema schema = new();

	Parser(string text, string file, int line) {
		this.text = text;
		this.file = file;
		this.line = line;
	}
}