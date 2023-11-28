﻿using System.Text;

namespace SqlSchemaParser;
public struct DataType {
	public QualifiedName TypeName;
	public int Size = -1;
	public int Scale = -1;
	public List<string> Values = new();

	public DataType(string typeName) {
		TypeName = new QualifiedName(typeName);
	}

	public override readonly string ToString() {
		var sb = new StringBuilder();
		sb.Append(TypeName);
		if (Size >= 0) {
			sb.Append('(');
			sb.Append(Size);
			if (Scale >= 0) {
				sb.Append(',');
				sb.Append(Scale);
			}
			sb.Append(')');
		}
		return sb.ToString();
	}

	public DataType(QualifiedName typeName) {
		TypeName = typeName;
	}
}
