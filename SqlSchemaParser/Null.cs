namespace SqlSchemaParser;
public sealed class Null: Expression {
	public override bool Equals(object? obj) {
		return obj is Null;
	}

	public override int GetHashCode() {
		return 0;
	}
}
