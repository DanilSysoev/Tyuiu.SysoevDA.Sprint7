using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.V14.Lib
{
    public class DataService
    {
        public int СountRows(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int nonEmptyRowCount = 0;

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    nonEmptyRowCount++;
                }
            }

            return nonEmptyRowCount - 1;
        }
        public int CountColumns(string filePath)
        {
            string firstLine = File.ReadLines(filePath).FirstOrDefault();
            string[] columns = firstLine.Split(';');

            return columns.Length;
        }
        public double AverageTime(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            var dataLines = lines.Skip(1);

            var fifthColumn = dataLines.Select(line => double.Parse(line.Split(';')[4]));

            double average = fifthColumn.Average();

            return average;
        }
        public double MinTime(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            var dataLines = lines.Skip(1);

            var fifthColumn = dataLines.Select(line => double.Parse(line.Split(';')[4]));

            double average = fifthColumn.Min();

            return average;
        }
        public double MaxTime(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            var dataLines = lines.Skip(1);

            var fifthColumn = dataLines.Select(line => double.Parse(line.Split(';')[4]));

            double average = fifthColumn.Max();

            return average;
        }
    }
}
