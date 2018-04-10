using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DataSet
    {
        public static void SetColumns(List<string> rows)
        {
            rows.ForEach(el =>
            {
                Columns.Add(new Column(el));
            });
        }
        public static List<Column> Columns = new List<Column>();
        public static List<DataInstance> Instances { get; set; } = new List<DataInstance>();

        public static int UpdateColumn(int index, string value)
        {
            return Columns[index].AddValue(value);
        }
    }

    public class Column
    {
        public Column(string name) => Name = name;

        private int _currentIndex = 0;
        public int AddValue(string value)
        {
            int index;
            if (Attributes.TryGetValue(value, out int oldIndex))
            {
                OccurenceCount[oldIndex]++;
                index = oldIndex;
            }
            else
            {
                index = _currentIndex;
                OccurenceCount[_currentIndex] = 1;
                Attributes[value] = _currentIndex++;
            }
            return index;
        }

        public string GetValueName(int value) => Attributes.FirstOrDefault(el => el.Value == value).Key;
        public string Name { get; }
        private Dictionary<string, int> Attributes { get; set; } = new Dictionary<string, int>();
        public Dictionary<int, int> OccurenceCount = new Dictionary<int, int>();
    }
}
