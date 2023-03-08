namespace ChartBuilderUtility.Interfaces;

public interface IChart<TKey> where TKey : notnull
{
    Dictionary<TKey, decimal> GetDataBySelection();
    public IEnumerable<TKey> GetDistributionMode(Dictionary<TKey, decimal> selection)
    {
        decimal max = 0;
        foreach (var (key, value) in selection)
        {
            if (selection[key] > max)
                max = selection[key];
        }

        return selection.Keys.Where(x => selection[x] == max);
    }
}