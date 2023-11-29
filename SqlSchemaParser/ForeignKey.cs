namespace SqlSchemaParser;
public sealed class ForeignKey {
	public List<Column> Columns = new();
	public Table? RefTable;
	public List<Column> RefColumns = new();
	public Action OnDelete = Action.NoAction;
	public Action OnUpdate = Action.NoAction;
}
