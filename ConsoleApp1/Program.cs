using SqlSchemaParser;

class Program {
	static void Main(string[] args) {
		foreach (var file in args) {
			var schema = new Schema();
			Parser.Parse(file, File.ReadAllText(file), schema);

			var outDir = "\\t";
			if (!Directory.Exists(outDir))
				outDir = Path.GetTempPath();

			var outFile = Path.Combine(outDir, Path.GetFileNameWithoutExtension(file) + "-ignored.sql");
			File.WriteAllText(outFile, schema.IgnoredString());
			Console.WriteLine(outFile);

			outFile = Path.Combine(outDir, Path.GetFileNameWithoutExtension(file) + "-roundtrip.sql");
			var roundtripSql = schema.Sql();
			File.WriteAllText(outFile, roundtripSql);
			Console.WriteLine(outFile);

			var roundtripSchema = new Schema();
			Parser.Parse(outFile, roundtripSql, roundtripSchema);
			if (roundtripSql != roundtripSchema.Sql()) {
				Console.WriteLine(roundtripSql);
				Console.Write(roundtripSchema.Sql());
				Environment.Exit(1);
			}
		}
	}
}
