using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ComponentFactory.Krypton.Toolkit;

using Project.V14.Lib;

namespace Project.V14
{
    public partial class FormFunc : KryptonForm
    {
        public FormFunc()
        {
            InitializeComponent();
        }
        DataService ds = new DataService();

        public string filePath { get; set; }
        private bool deleterows = false;

        private void DynamicSearch(string searchText)
        {
            StringBuilder filterExpression = new StringBuilder();

            var columnNames = DataGridView_SDA.Columns.Cast<DataGridViewColumn>().Select(c => c.Name).ToArray();

            foreach (var columnName in columnNames)
            {
                if (filterExpression.Length > 0)
                    filterExpression.Append(" OR ");

                filterExpression.AppendFormat("[{0}] LIKE '%{1}%'", columnName, searchText);
            }

            (DataGridView_SDA.DataSource as DataTable).DefaultView.RowFilter = filterExpression.ToString();
        }

        private void ShowSuccessMessage(string message)
        {
            textBoxSuccessSave_SDA.Text = message;

            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (sender, e) =>
            {
                textBoxSuccessSave_SDA.Text = "";
                timer.Stop();
            };
            timer.Start();
        }

        private void AddDataToChart(string busNumber, double value)
        {
            if (chartAverage_SDA.Series.FindByName(busNumber) == null)
            {
                chartAverage_SDA.Series.Add(busNumber);
                chartAverage_SDA.Series[busNumber].ChartType = SeriesChartType.Column;

                if (chartAverage_SDA.Series.Count > 1)
                {
                    chartAverage_SDA.Series[busNumber].IsValueShownAsLabel = true;
                }
            }

            DataPoint dataPoint = new DataPoint();
            dataPoint.SetValueXY(busNumber, value);
            dataPoint.Label = busNumber;

            chartAverage_SDA.Series[busNumber].Points.Add(dataPoint);
        }

        private void FormFunc_Load(object sender, EventArgs e)
        {
            DataTable dataTable = LoadDataFromFile(filePath);
            DataGridView_SDA.DataSource = dataTable;

            DataGridView_SDA.Columns[2].Width = 150;
            DataGridView_SDA.Columns[3].Width = 150;
            DataGridView_SDA.Columns[5].Width = 150;

            this.Text = "Файл: " + System.IO.Path.GetFileName(filePath);

            textBoxCountRows_SDA.Text = "Кол-во строк: " + Convert.ToString(ds.СountRows(filePath));
            textBoxCountColumn_SDA.Text = "Кол-во столбцев: " + Convert.ToString(ds.CountColumns(filePath));
            textBoxMin_SDA.Text = "Минимальное время: " + Convert.ToString(ds.MinTime(filePath));
            textBoxMax_SDA.Text = "Максимальное время: " + Convert.ToString(ds.MaxTime(filePath));
            textBoxAverage_SDA.Text = "Cреднее время: " + Convert.ToString(ds.AverageTime(filePath));

            int busNumberColumnIndex = DataGridView_SDA.Columns["№ Маршрута"].Index;
            int valueColumnIndex = DataGridView_SDA.Columns["Время в пути(мин.)"].Index;
            chartAverage_SDA.Titles.Add("Диаграмма по времени в пути каждого маршрута");

            chartAverage_SDA.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chartAverage_SDA.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
            chartAverage_SDA.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartAverage_SDA.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartAverage_SDA.ChartAreas[0].AxisX.MajorGrid.Enabled = false; 
            chartAverage_SDA.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            foreach (DataGridViewRow row in DataGridView_SDA.Rows)
            {
                if (row.Cells[busNumberColumnIndex].Value != null && row.Cells[valueColumnIndex].Value != null)
                {
                    string busNumber = row.Cells[busNumberColumnIndex].Value.ToString();
                    double value = Convert.ToDouble(row.Cells[valueColumnIndex].Value);

                    AddDataToChart(busNumber, value);
                }
            }
            DataGridView_SDA.AllowUserToDeleteRows = false;
        }

        private DataTable LoadDataFromFile(string filePath)
        {
            DataTable dataTable = new DataTable();

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values = line.Split(';');
                    rows.Add(values);
                }
            }

            foreach (var header in rows.First())
            {
                dataTable.Columns.Add(header);
            }

            foreach (var row in rows.Skip(1))
            {
                if (row.All(string.IsNullOrWhiteSpace))
                    continue;
                dataTable.Rows.Add(row);
            }


            return dataTable;
        }

        private void ButtonSaveFile_SDA_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)DataGridView_SDA.DataSource;

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine(string.Join(";", dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));

                foreach (DataRow row in dataTable.Rows)
                {
                    writer.WriteLine(string.Join(";", row.ItemArray));
                }
            }
            ShowSuccessMessage("Успешно сохранено в: " + filePath);
        }

        private void ButtonRedact_SDA_Click(object sender, EventArgs e)
        {

            DataGridView_SDA.AllowUserToDeleteRows = true;
            ButtonRedact_SDA.Enabled = false;
            ButtonApply_SDA.Enabled = true;
            DataGridView_SDA.ReadOnly = false;
        }

        private void ButtonApply_SDA_Click(object sender, EventArgs e)
        {
            DataGridView_SDA.AllowUserToDeleteRows = false;
            ButtonRedact_SDA.Enabled = true;
            ButtonApply_SDA.Enabled = false;
            DataGridView_SDA.ReadOnly = true;
            textBoxCountRows_SDA.Text = "Кол-во строк: " + (DataGridView_SDA.RowCount - 1);
            textBoxCountColumn_SDA.Text = "Кол-во столбцев: " + DataGridView_SDA.ColumnCount;
        }


        private void ButtonSearch_SDA_Click(object sender, EventArgs e)
        {
            DynamicSearch(TextBoxSearch_SDA.Text);
        }
    }
}
