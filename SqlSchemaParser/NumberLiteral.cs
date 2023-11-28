namespace SqlSchemaParser;
public sealed class NumberLiteral: Expression {
	public string Value;

	public NumberLiteral(string value) {
		Value = value;
	}

	public override bool Equals(object? obj) {
		return obj is NumberLiteral literal && Value == literal.Value;
	}

	public override int GetHashCode() {
		return HashCode.Combine(Value);
	}
}
