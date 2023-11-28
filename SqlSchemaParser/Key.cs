namespace SqlSchemaParser;
public sealed class Key {
	public readonly Location Location;
	public readonly List<string> ColumnNames = new();
	public readonly List<Column> Columns = new();

	public Key(Location location) {
		Location = location;
	}

	public void Add(Column column) {
		column.Nullable = false;
		Columns.Add(column);
	}
}
