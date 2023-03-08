using System.Windows.Forms.DataVisualization.Charting;
using DataParseUtility;
using LinearRegressionUtility;
using OutputBuilderEntity;

namespace UI.Task2;

public partial class Form1 : Form
{
    private Chart _chart1;
    private CsvParser _parser;

    public Form1()
    {
        _parser = new CsvParser(@"..\..\..\..\Data\r4z2.csv");
        InitializeComponent();
        Load += Form1_Load;
    }
    
    private void Form1_Load(object? sender, EventArgs e)
    {
        var result = LinearRegressionOutputData.GetCalculationsResult(_parser);
        result.Values!.ForEach(val => 
            _chart1.Series["Points"].Points.AddXY(val.X, val.Y));
        
        result.LinearFunctionPoints!.ForEach(val =>
            _chart1.Series["Regression"].Points.AddXY(val.X, val.Y));
    }
}