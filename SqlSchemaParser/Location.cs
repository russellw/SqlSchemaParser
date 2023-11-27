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
}
