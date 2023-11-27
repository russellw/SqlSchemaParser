using SqlSchemaParser;

class Program {
	static void Main(string[] args) {
		try {
			Parser.Parse("/*");
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}
	}
}
