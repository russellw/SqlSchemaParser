using System.Text;

namespace SqlSchemaParser;
public sealed class ForeignKey {
	public List<Column> Columns = new();
	public Table? RefTable;
	public List<Column> RefColumns = new();
	public Action OnDelete = Action.NoAction;
	public Action OnUpdate = Action.NoAction;

	public string Sql() {
		var sb = new StringBuilder("FOREIGN KEY(");
		sb.Append(string.Join(',', Columns));
		sb.Append(") REFERENCES ");
		sb.Append(RefTable);
		sb.Append('(');
		sb.Append(string.Join(',', RefColumns));
		sb.Append(')');
		return sb.ToString();
	}
}
