using ChartBuilderUtility;
using DataParseUtility;
using EstimatesUtility;
using OutputBuilderEntity;

var parser = new CsvParser(@"..\..\..\..\Data\r1z1.csv");
var estimates = new SelectionGeneralEstimates(parser.ParseData());
var barChart = new BarChart(estimates);
var empChart = new EmpiricalChart(estimates);
Console.WriteLine(EstimatesOutputData.Generate(estimates));





