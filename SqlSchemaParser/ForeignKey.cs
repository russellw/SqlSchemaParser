using System.Text;

namespace SqlSchemaParser;
public sealed class ForeignKey {
	public List<Column> Columns = new();
	public Table? RefTable;
	public List<Column> RefColumns = new();
	public Action OnDelete = Action.NoAction;
	public Action OnUpdate = Action.NoAction;

	public string Sql() {
		var sb = new StringBuilder("FOREIGN KEY");
		return sb.ToString();
	}
}
