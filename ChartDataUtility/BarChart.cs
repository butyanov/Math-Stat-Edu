using ChartBuilderUtility.Interfaces;
using EstimatesUtility;

namespace ChartBuilderUtility;

public class BarChart : IChart<(decimal, decimal)>
{
    private readonly SelectionGeneralEstimates _estimates;
    
    public BarChart(SelectionGeneralEstimates estimates)
    {
        _estimates = estimates;
    }

    public Dictionary<(decimal, decimal), decimal> GetDataBySelection()
    {
        var numIntervals = Math.Floor((double)_estimates.GetVol() / 10);
        var intervalWidth = _estimates.GetRange() / (decimal)numIntervals;
        var intervalFreq = new Dictionary<(decimal, decimal), decimal>();
        
        var intervalStart = _estimates.GetMin();
        var intervalEnd = intervalStart + intervalWidth;
        for (var i = 0; i < numIntervals; i++) {
            intervalFreq.Add((intervalStart, intervalEnd), 0);
            _estimates.Selection.ForEach( _  =>
                intervalFreq[(intervalStart, intervalEnd)] = _estimates.Selection.Count(x => x > intervalStart && x <= intervalEnd));

            intervalFreq[(intervalStart, intervalEnd)] /= _estimates.Selection.Count;
            
            intervalStart = intervalEnd;
            intervalEnd += intervalWidth;
        }

        return intervalFreq;
    }
}