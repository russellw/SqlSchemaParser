namespace SqlSchemaParser;
public sealed class BinaryExpression: Expression {
	public BinaryOp Op;
	public Expression Left, Right;

	public BinaryExpression(BinaryOp op, Expression left, Expression right) {
		Op = op;
		Left = left;
		Right = right;
	}

	public override bool Equals(object b0) {
		if (b0 is BinaryExpression b)
			return Op == b.Op && Left.Equals(b.Left) && Right.Equals(b.Right);
		return false;
	}
}
