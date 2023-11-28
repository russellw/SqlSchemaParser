namespace SqlSchemaParser;
public sealed class Span {
	public readonly Location Location;
	public readonly int End;

	public Span(Location location, int end) {
		Location = location;
		End = end;
	}
}
