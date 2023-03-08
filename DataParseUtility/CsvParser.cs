using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace DataParseUtility;

public class CsvParser : IParser
{
    public readonly TextFieldParser Parser;
    
    public CsvParser(string path)
    {
        Parser = new TextFieldParser(path);
        Parser.TextFieldType = FieldType.Delimited;
        Parser.SetDelimiters(",");
        if (Parser.EndOfData)
            throw new Exception("Empty file");
    }

    public List<decimal> ParseData(int linesSkip = 1)
    {
        var result = new List<decimal>();
        while (!Parser.EndOfData)
        {
            if (Parser.LineNumber < linesSkip + 1)
            {
                Parser.ReadFields();
                continue;
            }
            var fields = Parser.ReadFields();
            foreach (var field in fields!)
            {
                if (decimal.TryParse(field, CultureInfo.InvariantCulture, out var number))
                    result.Add(number);
                else
                    throw new Exception($"Non-Digit given on line {Parser.ErrorLineNumber}");
            }
        }

        return result;
    }
}