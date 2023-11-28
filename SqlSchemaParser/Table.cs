using System.Text;

namespace SqlSchemaParser;
public sealed class Table {
	public QualifiedName Name;
	public List<Column> Columns = new();
	public Key? PrimaryKey;
	public List<Key> UniqueKeys = new();

	public override string ToString() {
		var sb = new StringBuilder("CREATE TABLE ");
		sb.Append(Name);
		sb.Append('(');
		var separator = new Separator(sb);
		foreach (var column in Columns) {
			separator.Write();
			sb.Append(column);
		}
		sb.Append(')');
		return sb.ToString();
	}

	public Table(QualifiedName name) {
		Name = name;
	}

	public void AddPrimaryKey(Key key) {
		if (PrimaryKey != null)
			throw new SqlError($"{key.Location}: {Name} already has a primary key");
		PrimaryKey = key;
	}
}
