using System.Text;

namespace SqlSchemaParser;
public sealed class Column {
	public string Name;
	public DataType DataType;
	public bool Nullable = true;

	public override string ToString() {
		var sb = new StringBuilder();
		sb.Append(Name);
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
}
