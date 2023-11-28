namespace SqlSchemaParser;
public sealed class Table {
	public QualifiedName Name;
	public List<Column> Columns = new();
	public Key? PrimaryKey;

	public Table(QualifiedName name) {
		Name = name;
	}
}
