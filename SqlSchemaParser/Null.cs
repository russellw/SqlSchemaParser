namespace SqlSchemaParser;
public sealed class Null: Expression {
	public override bool Equals(object b) {
		return b is Null;
	}
}
