﻿namespace SqlSchemaParser;
public sealed class StringLiteral: Expression {
	public string Value;

	public StringLiteral(string value) {
		Value = value;
	}

	public override bool Equals(object? b0) {
		if (b0 is StringLiteral b)
			return Value == b.Value;
		return false;
	}
}
