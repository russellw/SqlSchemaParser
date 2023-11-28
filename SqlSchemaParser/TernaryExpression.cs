namespace SqlSchemaParser;
public sealed class TernaryExpression: Expression {
	public TernaryOp Op;
	public Expression First, Second, Third;

	public TernaryExpression(TernaryOp op, Expression first, Expression second, Expression third) {
		Op = op;
		First = first;
		Second = second;
		Third = third;
	}

	public override bool Equals(object? b0) {
		if (b0 is TernaryExpression b)
			return Op == b.Op && First.Equals(b.First) && Second.Equals(b.Second) && Third.Equals(b.Third);
		return false;
	}
}
