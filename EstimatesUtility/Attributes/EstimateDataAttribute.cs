namespace EstimatesUtility;

public class EstimateDataAttribute : Attribute
{
    public string? Name { get; }
    public EstimateDataAttribute(string name) => Name = name;
}