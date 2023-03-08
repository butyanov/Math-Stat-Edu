using System.Windows.Forms.DataVisualization.Charting;
using ChartBuilderUtility;
using ChartBuilderUtility.Interfaces;
using DataParseUtility;
using EstimatesUtility;
using OutputBuilderEntity;

namespace UI;

public partial class Form1 : Form
{
    private static readonly SelectionGeneralEstimates Estimates =
        new(new CsvParser(@"..\..\..\..\Data\r1z1.csv").ParseData());

    private readonly IChart<(decimal, decimal)> _barChartUtility = new BarChart(Estimates);
    private readonly IChart<decimal> _empChartUtility = new EmpiricalChart(Estimates);
    private readonly TabControl _tabControl;

    public Form1()
    {
        InitializeComponent();
        _tabControl = new TabControl();
        _tabControl.Size = new Size(1000, 800);

        Controls.Add(_tabControl);
        Load += Form1_Load;
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        LoadEmpiricalChart("EDC", SeriesChartType.Line, Color.LightBlue, Color.LightBlue, Color.DeepPink);
        LoadHistogram("Histogram", SeriesChartType.Column, Color.LightBlue, Color.LightBlue, Color.DeepPink);
        LoadEstimatesData();
    }

    private void LoadHistogram(string name, SeriesChartType chartType, Color chartColor, Color pointColor,
        Color modePointColor)
    {
        var tabPage = new TabPage(name);
        var chart = new Chart();
        chart.Dock = DockStyle.Fill;
        chart.Titles.Add(new Title(name, Docking.Top, new Font("Arial", 16, FontStyle.Bold), Color.Black));
        chart.Legends.Add(new Legend("Legend"));
        chart.ChartAreas.Add(new ChartArea());
        chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        chart.ChartAreas[0].AxisX.Interval = 1;
        chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

        var series = new Series(name);
        series.ChartType = chartType;
        series.Color = chartColor;
        series["BorderColor"] = "Black";
        series["PointWidth"] = "1";

        var frequencies = _barChartUtility.GetDataBySelection();
        foreach (var interval in frequencies)
        {
            var point = series.Points.Add((double)interval.Value);
            point.AxisLabel = $"({interval.Key.Item1:F3}, {interval.Key.Item2:F3})";
            point.Color =
                _barChartUtility.GetDistributionMode(frequencies).Contains((interval.Key.Item1, interval.Key.Item2))
                    ? modePointColor
                    : pointColor;
        }

        chart.Series.Add(series);
        tabPage.Controls.Add(chart);
        _tabControl.TabPages.Add(tabPage);
    }

    private void LoadEmpiricalChart(string name, SeriesChartType chartType, Color chartColor, Color pointColor,
        Color modePointColor)
    {
        var tabPage = new TabPage(name);
        var chart = new Chart();
        chart.Dock = DockStyle.Fill;
        chart.Titles.Add(new Title(name, Docking.Top, new Font("Arial", 16, FontStyle.Bold), Color.Black));
        chart.Legends.Add(new Legend("Legend"));
        chart.ChartAreas.Add(new ChartArea());
        chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        chart.ChartAreas[0].AxisX.Interval = 1;
        chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

        var series = new Series(name);
        series.ChartType = chartType;
        series.Color = chartColor;
        series["BorderColor"] = "Black";

        var frequencies = _empChartUtility.GetDataBySelection();
        foreach (var interval in frequencies.OrderBy(x => x.Value).ToList())
        {
            var point = series.Points.Add((double)interval.Value);
            point.AxisLabel = $"({interval.Key:F3})";
            point.Color = _empChartUtility.GetDistributionMode(frequencies).Contains(interval.Key)
                ? modePointColor
                : pointColor;
        }

        chart.Series.Add(series);
        tabPage.Controls.Add(chart);
        _tabControl.TabPages.Add(tabPage);
    }

    private void LoadEstimatesData(string name = "Estimates")
    {
        var tabPage = new TabPage(name);
        EstimatesOutputData.Generate(Estimates);
        var richTextBox = new RichTextBox();
        richTextBox.ReadOnly = true;
        richTextBox.Dock = DockStyle.Fill;
        richTextBox.Text = EstimatesOutputData.Generate(Estimates);
        tabPage.Controls.Add(richTextBox);
        _tabControl.TabPages.Add(tabPage);
    }
}