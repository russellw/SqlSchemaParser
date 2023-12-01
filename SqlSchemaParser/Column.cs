using System.Text;

namespace SqlSchemaParser;
public sealed class Column {
	public string Name;
	public DataType DataType;
	public bool AutoIncrement;
	public bool Nullable = true;

	public string Sql() {
		var sb = new StringBuilder();
		sb.Append(this);
		sb.Append(' ');
		sb.Append(DataType);
		if (!Nullable)
			sb.Append(" NOT NULL");
		return sb.ToString();
	}

	public Column(string name, DataType dataType) {
		Name = name;
		DataType = dataType;
	}

	public override string ToString() {
		return Etc.QuoteName(Name);
	}
}
