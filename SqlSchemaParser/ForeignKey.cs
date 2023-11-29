namespace SqlSchemaParser;
public sealed class ForeignKey {
	// Parser may need to initially refer to things by name
	public List<string> ColumnNames = new();
	public QualifiedName? RefTableName;
	public List<string> RefColumnNames = new();

	// And then resolve references to actual objects
	public List<Column> Columns = new();
	public Table? RefTable;
	public List<Column> RefColumns = new();

	public Action OnDelete = Action.NoAction;
	public Action OnUpdate = Action.NoAction;
}
