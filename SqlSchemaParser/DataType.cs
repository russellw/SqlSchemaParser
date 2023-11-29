using System.Text;

namespace SqlSchemaParser;
public struct DataType {
	public string TypeName;
	public int Size = -1;
	public int Scale = -1;
	public List<string>? Values;

	public DataType(string typeName) {
		TypeName = typeName;
	}

	public override readonly string ToString() {
		var sb = new StringBuilder();
		sb.Append(TypeName);
		if (Size >= 0) {
			sb.Append('(');
			sb.Append(Size);
			if (Scale >= 0) {
				sb.Append(',');
				sb.Append(Scale);
			}
			sb.Append(')');
		}
		if (Values != null) {
			sb.Append('(');
			sb.Append(string.Join(',', Values.Select(value => Etc.QuoteStringLiteral(value))));
			sb.Append(')');
		}
		return sb.ToString();
	}
}
