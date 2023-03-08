using ChartBuilderUtility.Interfaces;
using EstimatesUtility;

namespace ChartBuilderUtility;

public class EmpiricalChart : IChart<decimal>
{
    private readonly SelectionGeneralEstimates _estimates;
    
    public EmpiricalChart(SelectionGeneralEstimates estimates)
    {
        _estimates = estimates;
    }

    public Dictionary<decimal, decimal> GetDataBySelection()
    {
        var data = new Dictionary<decimal, decimal>();
        _estimates.Selection.ForEach(x =>
        {
            if (!data.ContainsKey(x))
                data.Add(x, _estimates.Selection.Count(y => y == x) / (decimal)_estimates.GetVol());
        });
        return data;
    }
}