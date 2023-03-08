using DataParseUtility;
using OutputBuilderEntity;

var parser = new CsvParser(@"..\..\..\..\Data\r4z2.csv");
var result = LinearRegressionOutputData.GetCalculationsResult(parser);
Console.WriteLine($"Correlation coefficient is {result.CorrelationCoefficient}");
Console.WriteLine($"Regression equation: y = {result.RegressionCoefficients.b}x + {result.RegressionCoefficients.a}");
Console.WriteLine($"Regression value in point with X = 118 is {result.YValue}");

