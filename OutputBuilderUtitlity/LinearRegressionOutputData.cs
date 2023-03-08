using DataParseUtility;
using LinearRegressionUtility;

namespace OutputBuilderEntity;

public static class LinearRegressionOutputData
{
    public static Result GetCalculationsResult(CsvParser parser)
    {
        var util = new LinearRegression(parser.ParseData());
        return new Result(
            util.Values,
            util.CalculateCorrelationCoefficient(2),
            util.CalculateRegressionCoefficients(2),
            util.CalculateRegressionValueInPoint(118m),
            util.GeneratePointsForLinearFunction()
        );
    }
}