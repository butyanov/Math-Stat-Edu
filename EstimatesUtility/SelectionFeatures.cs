using EstimatesUtility.Exceptions;

namespace EstimatesUtility;

public class SelectionGeneralEstimates : IEstimates
{
    public List<decimal> Selection { get; }
    private bool _isSorted;
    public SelectionGeneralEstimates(List<decimal> selection, bool isSorted = true)
    {
        Selection = selection.Count == 0 
            ? throw new EmptyCollectionException("Была подана пустая коллекция.") 
            : selection;
        _isSorted = isSorted;
        if(isSorted)
            Selection = Selection.OrderBy(x => x).ToList();
    }
    
    
    [EstimateData("Медиана")]
    public decimal GetMedian()
    {
        return Selection.Count % 2 == 0
            ? (Selection.ElementAt(Selection.Count / 2 - 1) + Selection.ElementAt(Selection.Count / 2)) * (decimal)0.5
            : Selection.ElementAt(Selection.Count / 2);
    }

    [EstimateData("Ассиметрия")]
    public decimal GetSkewness() => 
        (decimal)(Selection.Sum(x => Math.Pow((double)(x - GetAvg()), 3)) / ((Selection.Count - 1) *
                                                                             Math.Pow((double)GetDeviation(), 3)));
    [EstimateData("Стандартное отклонение")]
    public decimal GetDeviation(bool isBiased = false) => (decimal)Math.Sqrt((double)GetDispersion(isBiased));
    
    [EstimateData("Дисперсия")]
    public decimal GetDispersion(bool isBiased = false) =>
        (decimal)(Selection.Sum(x => Math.Pow((double)(x - GetAvg()), 2)) / (isBiased ? Selection.Count : Selection.Count - 1));
    
    [EstimateData("Интерквантильная широта")]
    public decimal GetInterQuantileWidth(List<decimal> selection) => CalculateQuantile(75) - CalculateQuantile(25);
    
    [EstimateData("Размах")]
    public decimal GetRange() => GetMax() - GetMin();
    
    [EstimateData("Среднее")]
    public decimal GetAvg() => Selection.Average();
    
    [EstimateData("Объем")]
    public uint GetVol() => (uint)Selection.Count;
    
    [EstimateData("Минимум")]
    public decimal GetMin() => Selection[0];
    
    [EstimateData("Максимум")]
    public decimal GetMax() => Selection[^1];
    
    private decimal CalculateQuantile(int percentile)
    {
        if (!_isSorted)
            throw new FormatException("Cannot be executed on unsorted selection");
        
        if (percentile is < 0 or > 100)
            throw new ArgumentException("Percentile must be between 0 and 100.");
        
        var i = (Selection.Count * percentile) / 100.0m;
        return i % 1 == 0 ? (Selection[(int)i]) : (Selection[(int)Math.Floor(i)] + Selection[(int)Math.Ceiling(i)]) / 2.0m;
    }
    
}