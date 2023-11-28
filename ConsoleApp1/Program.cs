using SqlSchemaParser;

class Program {
	static void Main(string[] args) {
		var schema = new Schema();
		foreach (var file in args)
			Parser.Parse(file, File.ReadAllText(file), schema);
		Console.Write(schema.IgnoredString());
	}
}
