namespace SqlSchemaParser;
public sealed class Key {
	public List<Column> Columns = new();

	public void Add(Column column) {
		column.Nullable = false;
		Columns.Add(column);
	}

	public override string ToString() {
		return $"({string.Join(',',Columns.Select(column=>column.Name))})";
	}
}
