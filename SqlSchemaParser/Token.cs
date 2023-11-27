namespace SqlSchemaParser;
sealed class Token {
	public int Start, End;
	public int Type;
	public string? Value;

	public Token(int start, int end, int type, string? value = null) {
		Start = start;
		End = end;
		Type = type;
		Value = value;
	}
}
