namespace SqlSchemaParser;
public readonly struct Location {
	public readonly string File;
	public readonly string Text;
	public readonly int Start;

	public Location(string file, string text, int start) {
		File = file;
		Text = text;
		Start = start;
	}

	public override string ToString() {
		int line = 1;
		for (int i = 0; i < Start; i++)
			if (Text[i] == '\n')
				line++;
		return $"{File}:{line}";
	}
}
