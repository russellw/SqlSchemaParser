using System.Text;

namespace SqlSchemaParser;
public struct Separator {
	readonly StringBuilder sb;
	readonly char ch;
	bool more;

	public void Write() {
		if (more)
			sb.Append(ch);
		more = true;
	}

	public Separator(StringBuilder sb, char ch = ',') {
		this.sb = sb;
		this.ch = ch;
	}
}
