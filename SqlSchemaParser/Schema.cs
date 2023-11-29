using System.Text;

namespace SqlSchemaParser;
public sealed class Schema {
	public List<Table> Tables = new();
	public Dictionary<string, Table> TableMap = new();
	public List<Span> Ignored = new();

	public string IgnoredString() {
		var sb = new StringBuilder();
		foreach (var span in Ignored) {
			if (sb.Length > 0)
				sb.Append('\n');
			sb.Append(span.Location);
			sb.Append(":\n");
			sb.Append(span.Location.Text[span.Location.Start..span.End]);
			sb.Append('\n');
		}
		return sb.ToString();
	}

	public override string ToString() {
		var sb = new StringBuilder();
		foreach (var table in Tables) {
			sb.Append(table);
			sb.Append('\n');
		}
		return sb.ToString();
	}

	public void Add(Location location, Table table) {
		Tables.Add(table);
		if (!TableMap.TryAdd(table.Name, table))
			throw new SqlError($"{location}: {table.Name} already exists");
	}

	public Table GetTable(Location location, string name) {
		if (TableMap.TryGetValue(name, out Table? table))
			return table;
		throw new SqlError($"{location}: {name} not found");
	}
}
