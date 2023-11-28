﻿namespace SqlSchemaParser;
public sealed class QualifiedName: Expression {
	public List<string> Names = new();

	public QualifiedName() {
	}

	public QualifiedName(string name) {
		Names.Add(name);
	}

	public override bool Equals(object? b0) {
		if (b0 is QualifiedName b)
			return Names.SequenceEqual(b.Names);
		return false;
	}
}
