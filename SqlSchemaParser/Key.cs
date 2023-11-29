namespace SqlSchemaParser;
public sealed class Key {
	public readonly Location Location;
	public List<string> ColumnNames = new();
	public List<Column> Columns = new();

	public Key(Location location) {
		Location = location;
	}

	public void Add(Column column) {
		column.Nullable = false;
		Columns.Add(column);
	}
}
