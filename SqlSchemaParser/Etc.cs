using System.Text;

namespace SqlSchemaParser;
public static class Etc {
	public static string QuoteName(string name) {
		return Quote('"', name);
	}

	public static string QuoteStringLiteral(string value) {
		return Quote('\'', value);
	}

	static string Quote(char quote, string s) {
		var sb = new StringBuilder();
		sb.Append(quote);
		foreach (var c in s) {
			if (c == quote)
				sb.Append(c);
			sb.Append(c);
		}
		sb.Append(quote);
		return sb.ToString();
	}
}
