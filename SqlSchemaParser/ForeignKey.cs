using System.Text;

namespace SqlSchemaParser;
public sealed class ForeignKey {
	public List<Column> Columns = new();
	public Table? RefTable;
	public List<Column> RefColumns = new();
	public Action OnDelete = Action.NO_ACTION;
	public Action OnUpdate = Action.NO_ACTION;

	public string Sql() {
		var sb = new StringBuilder("FOREIGN KEY(");
		sb.Append(string.Join(',', Columns));
		sb.Append(") REFERENCES ");
		sb.Append(RefTable);
		sb.Append('(');
		sb.Append(string.Join(',', RefColumns));
		sb.Append(')');
		if (OnDelete != Action.NO_ACTION) {
			sb.Append(" ON DELETE ");
			sb.Append(String(OnDelete));
		}
		if (OnUpdate != Action.NO_ACTION) {
			sb.Append(" ON UPDATE ");
			sb.Append(String(OnUpdate));
		}
		return sb.ToString();
	}

	static string String(Action action) {
		return action.ToString().Replace('_', ' ');
	}
}
