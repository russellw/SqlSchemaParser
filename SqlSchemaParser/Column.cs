namespace SqlSchemaParser;
public sealed class Column {
	public string Name;
	public DataType DataType;

	public Column(string name, DataType dataType) {
		Name = name;
		DataType = dataType;
	}
}
