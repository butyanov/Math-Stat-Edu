using EstimatesUtility;

namespace LinearRegressionUtility;

public class LinearRegression
{
    public readonly SelectionGeneralEstimates EstimatesX;
    public readonly SelectionGeneralEstimates EstimatesY;
    public List<(decimal a, decimal b)> Values = new();

    public LinearRegression(List<decimal> data)
    {
         EstimatesX = new SelectionGeneralEstimates(data.Where((x, i) => i % 2 == 0).ToList());
         EstimatesY = new SelectionGeneralEstimates(data.Where((y, i) => i % 2 != 0).ToList());
         for (var i = 0; i < EstimatesX.GetVol(); i++)
             Values.Add((EstimatesX.Selection[i], EstimatesY.Selection[i]));
    }
    
    decimal MultiplyAvg() => MultiplySum() / EstimatesX.GetVol();
    
    public (decimal a, decimal b) CalculateRegressionCoefficients(int accuracy) =>
        (Math.Round(RegressionCoefficients().a, accuracy), Math.Round(RegressionCoefficients().b, accuracy));
    
    public decimal CalculateCorrelationCoefficient(int accuracy) =>
        Math.Round(CorrelationCoefficient(), accuracy);
    
    public decimal CalculateRegressionValueInPoint(
        decimal xValue, int accuracy = 2) =>
        Math.Round(RegressionValueInPoint(xValue), accuracy);
    
    public List<(decimal X, decimal Y)> GeneratePointsForLinearFunction()
    {
        var coefficients = RegressionCoefficients();
        return EstimatesX.Selection.OrderBy(x => x).Select(x => (x, coefficients.b * x + coefficients.a)).ToList();
    }
    
    decimal CorrelationCoefficient() => (MultiplyAvg() - EstimatesX.GetAvg() * EstimatesY.GetAvg() ) /
                                               (EstimatesX.GetDeviation() * EstimatesY.GetDeviation());
    
    decimal RegressionValueInPoint(decimal xValue)
    {
        var coefficients = RegressionCoefficients();
        return coefficients.b * xValue + coefficients.a;
    }
    
    (decimal a, decimal b) RegressionCoefficients()
    {
        var xSum = EstimatesX.Selection.Sum(x => x);
        var ySum = EstimatesY.Selection.Sum(y => y);
        var xxSum = EstimatesX.Selection.Sum(x => x * x);
        var xySum = MultiplySum();
        var b = (ySum * xSum / EstimatesX.Selection.Count - xySum) / (xSum * xSum / EstimatesX.Selection.Count - xxSum);
        var a = (ySum - b * xSum) / EstimatesX.Selection.Count;
        return (a, b);
    }

    decimal MultiplySum() => EstimatesX.Selection.Select((t, i) => (t) * (EstimatesY.Selection[i])).Sum();
}

public record Result(List<(decimal X, decimal Y)>? Values, decimal CorrelationCoefficient,
    (decimal a, decimal b) RegressionCoefficients, decimal YValue, List<(decimal X, decimal Y)>? LinearFunctionPoints);

