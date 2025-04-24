using System.Text;

namespace CanFrameBuilder.Utility;

public class CodeWriter(string indentString = "    ")
{
    private readonly StringBuilder _sb = new();
    private int _indentLevel = 0;
    private readonly string _indentString = indentString;

    public void Indent() => _indentLevel++;
    public void Unindent() => _indentLevel = Math.Max(0, _indentLevel - 1);

    public void WriteLine(string line = "")
    {
        if (line.Length > 0)
            _sb.AppendLine($"{new string(' ', _indentLevel * _indentString.Length)}{line}");
        else
            _sb.AppendLine();
    }

    public void WriteLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
            WriteLine(line);
    }

    public override string ToString() => _sb.ToString();
}
