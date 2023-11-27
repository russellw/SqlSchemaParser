namespace SqlSchemaParser;
public sealed class Parser {
	public static Schema Parse(string text, string file = "SQL", int line = 1) {
		var parser = new Parser(text, file, line);
		return parser.schema;
	}

	const int kDoublePipe = -2;
	const int kGreaterEqual = -3;
	const int kLessEqual = -4;
	const int kNotEqual = -5;
	const int kNumber = -6;
	const int kQuotedName = -7;
	const int kStringLiteral = -8;
	const int kWord = -9;

	string text;
	string file;
	int line;

	int textIndex;
	List<Token> tokens = new();

	Schema schema = new();

	Parser(string text, string file, int line) {
		if (!text.EndsWith('\n'))
			text += '\n';
		this.text = text;
		this.file = file;
		this.line = line;
		Lex();
	}

	void Lex() {
		while (textIndex < text.Length) {
			var end = textIndex + 1;
			int t = text[textIndex];
			switch (t) {
			case '|':
				switch (text[end]) {
				case '|':
					end = textIndex + 2;
					t = kDoublePipe;
					break;
				}
				break;
			case '!':
				switch (t) {
				case '=':
					end = textIndex + 2;
					t = kNotEqual;
					break;
				case '<':
					// https://stackoverflow.com/questions/77475517/what-are-the-t-sql-and-operators-for
					end = textIndex + 2;
					t = kGreaterEqual;
					break;
				case '>':
					end = textIndex + 2;
					t = kLessEqual;
					break;
				}
				break;
			case '>':
				switch (t) {
				case '=':
					end = textIndex + 2;
					t = kGreaterEqual;
					break;
				}
				break;
			case '<':
				switch (t) {
				case '=':
					end = textIndex + 2;
					t = kLessEqual;
					break;
				case '>':
					end = textIndex + 2;
					t = kNotEqual;
					break;
				}
				break;
			case '-':
				switch (text[end]) {
				case '-':
					textIndex = text.IndexOf('\n', textIndex + 2);
					continue;
				}
				break;
			case '/':
				switch (text[end]) {
				case '*':
					textIndex = text.IndexOf("*/", textIndex + 2);
					if (textIndex < 0)
						throw Error("unclosed /*");
					continue;
				}
				break;
			case ',':
			case '=':
			case '&':
			case ';':
			case '+':
			case '%':
			case '(':
			case ')':
			case '~':
			case '*':
			case '@':
				break;
			case '\n':
			case '\r':
			case '\t':
			case '\f':
			case '\v':
			case ' ':
				textIndex = end;
				continue;
			}
			tokens.Add(new Token(textIndex, end, t));
			textIndex = end;
		}
		tokens.Add(new Token(textIndex, textIndex, -1));
	}

	Exception Error(string message) {
		return Error(message, line);
	}

	Exception Error(string message, int line) {
		// Error functions return exception objects instead of throwing immediately
		// so 'throw Error(...)' can mark the end of a case block
		return new FormatException($"{file}:{line}: {message}");
	}
}