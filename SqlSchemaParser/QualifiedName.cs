using System.Text;

namespace SqlSchemaParser;
public sealed class QualifiedName: Expression {
	public List<string> Names = new();

	public QualifiedName() {
	}

	public QualifiedName(string name) {
		Names.Add(name);
	}

	public override string ToString() {
		var sb = new StringBuilder();
		var separator = new Separator(sb, '.');
		foreach (var name in Names) {
			separator.Write();
			sb.Append(name);
		}
		return sb.ToString();
	}

	public override bool Equals(object? obj) {
		return obj is QualifiedName name && Names.SequenceEqual(name.Names);
	}

	public override int GetHashCode() {
		return HashCode.Combine(Names);
	}
}
