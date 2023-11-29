using System.Text;

namespace SqlSchemaParser;
public sealed class Table {
	public string Name;
	public List<Column> Columns = new();
	public Dictionary<string, Column> ColumnMap = new();
	public Key? PrimaryKey;
	public List<Key> UniqueKeys = new();
	public List<ForeignKey> ForeignKeys = new();

	public string Sql() {
		var sb = new StringBuilder("CREATE TABLE ");
		sb.Append(this);
		sb.Append('(');
		sb.Append(string.Join(", ", Columns.Select(column => column.Sql())));
		if (PrimaryKey != null) {
			sb.Append(", PRIMARY KEY");
			sb.Append(PrimaryKey.Sql());
		}
		foreach (var key in UniqueKeys) {
			sb.Append(", UNIQUE");
			sb.Append(key.Sql());
		}
		sb.Append(')');
		return sb.ToString();
	}

	public Table(string name) {
		Name = name;
	}

	public void Add(Location location, Column column) {
		Columns.Add(column);
		if (!ColumnMap.TryAdd(column.Name, column))
			throw new SqlError($"{location}: {this}.{column} already exists");
	}

	public void AddPrimaryKey(Location location, Key key) {
		if (PrimaryKey != null)
			throw new SqlError($"{location}: {this} already has a primary key");
		PrimaryKey = key;
	}

	public Column GetColumn(Location location, string name) {
		if(ColumnMap.TryGetValue(name, out Column? column))
			return column;
		throw new SqlError($"{location}: {this}.{name} not found");
	}

	public override string ToString() {
		return Etc.QuoteName(Name);
	}
}
