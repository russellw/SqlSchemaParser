using System.Diagnostics;
using System.Text;

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

	readonly string text;
	readonly string file;
	int textIndex;
	readonly List<Token> tokens = new();
	int tokenIndex;
	readonly List<int> ignored = new();
	readonly Schema schema = new();

	Parser(string text, string file) {
		if (!text.EndsWith('\n'))
			text += '\n';
		this.text = text;
		this.file = file;
		Lex();
		while (tokens[tokenIndex].Type != -1) {
			switch (Keyword()) {
			case "create":
				switch (Keyword(1)) {
				case "table":
					break;
				}
				break;
			}
			ignored.Add(tokenIndex++);
		}
	}

	string Name() {
		var token = tokens[tokenIndex];
		switch (token.Type) {
		case kWord:
		case kQuotedName:
			tokenIndex++;
			return token.Value!;
		}
		throw Error("expected name");
	}

	string? Keyword(int i = 0) {
		var token = tokens[tokenIndex + i];
		if (token.Type != kWord)
			return null;
		return token.Value!.ToLowerInvariant();
	}

	void Lex() {
		while (textIndex < text.Length) {
			int k = text[textIndex];
			var i = textIndex + 1;
			switch (k) {
			case '|':
				switch (text[i]) {
				case '|':
					i = textIndex + 2;
					k = kDoublePipe;
					break;
				}
				break;
			case '!':
				switch (k) {
				case '=':
					i = textIndex + 2;
					k = kNotEqual;
					break;
				case '<':
					// https://stackoverflow.com/questions/77475517/what-are-the-t-sql-and-operators-for
					i = textIndex + 2;
					k = kGreaterEqual;
					break;
				case '>':
					i = textIndex + 2;
					k = kLessEqual;
					break;
				}
				break;
			case '>':
				switch (k) {
				case '=':
					i = textIndex + 2;
					k = kGreaterEqual;
					break;
				}
				break;
			case '<':
				switch (k) {
				case '=':
					i = textIndex + 2;
					k = kLessEqual;
					break;
				case '>':
					i = textIndex + 2;
					k = kNotEqual;
					break;
				}
				break;
			case '-':
				switch (text[i]) {
				case '-':
					textIndex = text.IndexOf('\n', textIndex + 2);
					continue;
				}
				break;
			case '/':
				switch (text[i]) {
				case '*':
					i = text.IndexOf("*/", textIndex + 2);
					if (i < 0)
						throw Error("unclosed /*");
					textIndex = i + 2;
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
				textIndex = i;
				continue;
			case 'N':
				switch (text[i]) {
				case '\'':
					// We are reading everything as Unicode anyway
					// so the prefix has no special meaning
					textIndex = i;
					SingleQuote();
					return;
				}
				Word();
				return;
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
				Word();
				continue;
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
				Number();
				continue;
			case '.':
				if (char.IsDigit(text[i])) {
					Number();
					continue;
				}
				break;
			case '"':
				DoubleQuote();
				continue;
			case '\'':
				SingleQuote();
				continue;
			case '`':
				Backquote();
				continue;
			case '[':
				Square();
				continue;
			default:
				// Common letters are handled in the switch for speed
				// but there are other letters in Unicode
				if (char.IsLetter((char)k)) {
					Word();
					continue;
				}

				// Likewise digits
				if (char.IsDigit((char)k)) {
					Word();
					continue;
				}

				// And whitespace
				if (char.IsWhiteSpace((char)k)) {
					textIndex = i;
					continue;
				}

				throw Error("stray " + (char)k);
			}
			tokens.Add(new Token(textIndex, i, k));
			textIndex = i;
		}
		tokens.Add(new Token(textIndex, textIndex, -1));
	}

	// For string literals, single quote is reliably portable across dialects
	void SingleQuote() {
		Debug.Assert(text[textIndex] == '\'');
		var i = textIndex + 1;
		var sb = new StringBuilder();
		while (i < text.Length) {
			switch (text[i]) {
			case '\\':
				switch (text[i + 1]) {
				case '\'':
				case '\\':
					i++;
					break;
				}
				break;
			case '\'':
				i++;
				switch (text[i]) {
				case '\'':
					break;
				default:
					tokens.Add(new Token(textIndex, i, kStringLiteral, sb.ToString()));
					textIndex = i;
					return;
				}
				break;
			}
			sb.Append(text[i++]);
		}
		throw Error("unclosed '");
	}

	// For unusual identifiers, MySQL uses backquotes
	void Backquote() {
		Debug.Assert(text[textIndex] == '`');
		var i = textIndex + 1;
		var sb = new StringBuilder();
		while (i < text.Length) {
			switch (text[i]) {
			case '\\':
				switch (text[i + 1]) {
				case '`':
				case '\\':
					i++;
					break;
				}
				break;
			case '`':
				i++;
				switch (text[i]) {
				case '`':
					break;
				default:
					tokens.Add(new Token(textIndex, i, kQuotedName, sb.ToString()));
					textIndex = i;
					return;
				}
				break;
			}
			sb.Append(text[i++]);
		}
		throw Error("unclosed `");
	}

	// For unusual identifiers, SQL Server uses square brackets
	void Square() {
		Debug.Assert(text[textIndex] == '[');
		var i = textIndex + 1;
		var sb = new StringBuilder();
		while (i < text.Length) {
			switch (text[i]) {
			case ']':
				i++;
				switch (text[i]) {
				case ']':
					break;
				default:
					tokens.Add(new Token(textIndex, i, kQuotedName, sb.ToString()));
					textIndex = i;
					return;
				}
				break;
			}
			sb.Append(text[i++]);
		}
		throw Error("unclosed [");
	}

	// For unusual identifiers, standard SQL uses double quotes
	void DoubleQuote() {
		Debug.Assert(text[textIndex] == '"');
		var i = textIndex + 1;
		var sb = new StringBuilder();
		while (i < text.Length) {
			switch (text[i]) {
			case '\\':
				switch (text[i + 1]) {
				case '"':
				case '\\':
					i++;
					break;
				}
				break;
			case '"':
				i++;
				switch (text[i]) {
				case '"':
					break;
				default:
					tokens.Add(new Token(textIndex, i, kQuotedName, sb.ToString()));
					textIndex = i;
					return;
				}
				break;
			}
			sb.Append(text[i++]);
		}
		throw Error("unclosed \"");
	}

	void Word() {
		Debug.Assert(IsWordPart(text[textIndex]));
		var i = textIndex;
		do
			i++;
		while (IsWordPart(text[i]));
		tokens.Add(new Token(textIndex, i, kWord, text[textIndex..i]));
		textIndex = i;
	}

	void Number() {
		Debug.Assert(char.IsDigit(text[textIndex]) || text[textIndex] == '.');
		var i = textIndex;
		while (IsWordPart(text[i]))
			i++;
		if (text[i] == '.')
			do
				i++;
			while (IsWordPart(text[i]));
		tokens.Add(new Token(textIndex, i, kNumber, text[textIndex..i]));
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
