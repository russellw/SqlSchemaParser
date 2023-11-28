namespace SqlSchemaParser;
public sealed class QualifiedName: Expression {
	public List<string> Names = new();

	public QualifiedName() {
	}

	public QualifiedName(string name) {
		Names.Add(name);
	}

	public override bool Equals(object? obj) {
		return obj is QualifiedName name && Names.SequenceEqual(name.Names);
	}

	public override int GetHashCode() {
		return HashCode.Combine(Names);
	}
}
