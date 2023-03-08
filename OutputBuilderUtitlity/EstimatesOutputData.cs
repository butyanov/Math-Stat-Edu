using System.Reflection;
using System.Text;
using EstimatesUtility;

namespace OutputBuilderEntity;

public static class EstimatesOutputData
{
    public static string Generate(IEstimates estimates)
    {
        var sb = new StringBuilder();
        var methods = Assembly.GetAssembly(typeof(IEstimates))
            ?.GetTypes().Where(type => typeof(IEstimates)
                .IsAssignableFrom(type))
            .SelectMany(type => type.GetMethods()
                .Where(method => method.GetCustomAttributes(true).Any(attr => attr is EstimateDataAttribute)));

        foreach (var mi in methods)
        {
            var attribute = (EstimateDataAttribute)mi.GetCustomAttributes(typeof(EstimateDataAttribute), false).First();
            sb.AppendLine($"{attribute.Name} :" +
                          $" {mi.Invoke(estimates, mi.GetParameters()
                              .Any(x => x.DefaultValue != null)
                              ? Enumerable.Repeat(0, mi.GetParameters()
                                  .Count(x => x.DefaultValue != null)).Select(x => Type.Missing).ToArray()
                              : Array.Empty<object>())}");
        }
        return sb.ToString();
    }
}