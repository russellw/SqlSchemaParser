using System.Text;

namespace SqlSchemaParser;
public sealed class Table {
	public string Name;
	public List<Column> Columns = new();
	public Dictionary<string, Column> ColumnMap = new();
	public Key? PrimaryKey;
	public List<Key> UniqueKeys = new();
	public List<ForeignKey> ForeignKeys = new();

	public override string ToString() {
		var sb = new StringBuilder("CREATE TABLE ");
		sb.Append(Name);
		sb.Append('(');
		sb.Append(string.Join(',', Columns));
		if (PrimaryKey != null) {
			sb.Append(",PRIMARY KEY");
			sb.Append(PrimaryKey);
		}
		foreach (var key in UniqueKeys) {
			sb.Append(",UNIQUE");
			sb.Append(key);
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
			throw new SqlError($"{location}: {Name}.{column.Name} already exists");
	}

	public void AddPrimaryKey(Location location, Key key) {
		if (PrimaryKey != null)
			throw new SqlError($"{location}: {Name} already has a primary key");
		PrimaryKey = key;
	}

	public Column GetColumn(Location location, string name) {
		if(ColumnMap.TryGetValue(name, out Column? column))
			return column;
		throw new SqlError($"{location}: {Name}.{name} not found");
	}
}
