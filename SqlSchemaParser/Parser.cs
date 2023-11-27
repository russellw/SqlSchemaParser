namespace SqlSchemaParser;
public sealed class Parser {
	public static Schema Parse(string text, string file = "SQL") {
		var parser = new Parser(text, file);
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

	int textIndex;
	List<Token> tokens = new();

	Schema schema = new();

	Parser(string text, string file) {
		if (!text.EndsWith('\n'))
			text += '\n';
		this.text = text;
		this.file = file;
		Lex();
	}

	void Lex() {
		while (textIndex < text.Length) {
			var end = textIndex + 1;
			int k = text[textIndex];
			switch (k) {
			case '|':
				switch (text[end]) {
				case '|':
					end = textIndex + 2;
					k = kDoublePipe;
					break;
				}
				break;
			case '!':
				switch (k) {
				case '=':
					end = textIndex + 2;
					k = kNotEqual;
					break;
				case '<':
					// https://stackoverflow.com/questions/77475517/what-are-the-t-sql-and-operators-for
					end = textIndex + 2;
					k = kGreaterEqual;
					break;
				case '>':
					end = textIndex + 2;
					k = kLessEqual;
					break;
				}
				break;
			case '>':
				switch (k) {
				case '=':
					end = textIndex + 2;
					k = kGreaterEqual;
					break;
				}
				break;
			case '<':
				switch (k) {
				case '=':
					end = textIndex + 2;
					k = kLessEqual;
					break;
				case '>':
					end = textIndex + 2;
					k = kNotEqual;
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
					end = text.IndexOf("*/", textIndex + 2);
					if (end < 0)
						throw Error("unclosed /*");
					textIndex = end + 2;
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
			case 'A':
			case 'B':
			case 'C':
			case 'D':
			case 'E':
			case 'F':
			case 'G':
			case 'H':
			case 'I':
			case 'J':
			case 'K':
			case 'L':
			case 'M':
			case 'O':
			case 'P':
			case 'Q':
			case 'R':
			case 'S':
			case 'T':
			case 'U':
			case 'V':
			case 'W':
			case 'X':
			case 'Y':
			case 'Z':
			case '_':
			case 'a':
			case 'b':
			case 'c':
			case 'd':
			case 'e':
			case 'f':
			case 'g':
			case 'h':
			case 'i':
			case 'j':
			case 'k':
			case 'l':
			case 'm':
			case 'n':
			case 'o':
			case 'p':
			case 'q':
			case 'r':
			case 's':
			case 't':
			case 'u':
			case 'v':
			case 'w':
			case 'x':
			case 'y':
			case 'z':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				Word();
				continue;
			default:
				// Common letters are handled in the switch for speed
				// but there are other letters in Unicode
				if (char.IsLetterOrDigit((char)k)) {
					Word();
					continue;
				}

				// Likewise whitespace
				if (char.IsWhiteSpace((char)k)) {
					textIndex = end;
					continue;
				}

				throw Error("stray " + (char)k);
			}
			tokens.Add(new Token(textIndex, end, k));
			textIndex = end;
		}
		tokens.Add(new Token(textIndex, textIndex, -1));
	}

	void Word() {
		var i = textIndex;
		do
			i++;
		while (IsWordPart(text[i]));
		tokens.Add(new Token(textIndex, i, kWord, text[textIndex..i]));
		textIndex = i;
	}

	static bool IsWordPart(char c) {
		if (char.IsLetterOrDigit(c))
			return true;
		return c == '_';
	}

	// Error functions return exception objects instead of throwing immediately
	// so 'throw Error(...)' can mark the end of a case block
	Exception Error(string message) {
		int line = 1;
		for (int i = 0; i < textIndex; i++)
			if (text[i] == '\n')
				line++;

		return new FormatException($"{file}:{line}: {message}");
	}
}