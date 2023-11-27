﻿namespace SqlSchemaParser;
public sealed class NumberLiteral: Expression {
	public string Value;

	public NumberLiteral(string value) {
		Value = value;
	}

	public override bool Eq(Expression b0) {
		if (b0 is NumberLiteral b)
			return Value == b.Value;
		return false;
	}
}