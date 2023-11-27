﻿namespace SqlSchemaParser;
public sealed class TernaryExpression: Expression {
	public TernaryOp Op;
	public Expression First, Second, Third;

	public TernaryExpression(TernaryOp op, Expression first, Expression second, Expression third) {
		Op = op;
		First = first;
		Second = second;
		Third = third;
	}

	public override bool Eq(Expression b0) {
		if (b0 is TernaryExpression b)
			return Op == b.Op && First.Eq(b.First) && Second.Eq(b.Second) && Third.Eq(b.Third);
		return false;
	}
}