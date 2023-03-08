namespace DataParseUtility;

public interface IParser
{
    public List<decimal> ParseData(int linesSkip);
}