namespace SqlSchemaParser;
public sealed class Table {
	public string Name;
	public List<Column> Columns = new();

	public Table(string name) {
		Name = name;
	}
}
