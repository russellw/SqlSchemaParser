namespace SqlSchemaParser;
public sealed class SqlError: Exception {
	public SqlError(string? message): base(message) {
	}
}
