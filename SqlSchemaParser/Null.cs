namespace SqlSchemaParser;
public sealed class Null: Expression {
	public override bool Eq(Expression b) {
		return b is Null;
	}
}
