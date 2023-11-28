using System.Text;

namespace SqlSchemaParser;
public sealed class Schema {
	public List<Table> Tables = new();
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
}
