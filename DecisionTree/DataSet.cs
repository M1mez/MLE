using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DataSet
    {
        public static void SetAttributes(List<string> rows)
        {
            rows.ForEach(el =>
            {
                Attributes.Add(new Attribute(el));
            });
        }
        
        public static Node RootNode = new Node(-1);
        private static int _qualifierIndex;
        public static int QualifierIndex => _qualifierIndex == 0 ? (_qualifierIndex = Attributes.Count - 1) : _qualifierIndex;

        public static List<int> GetEmptyQualifierCount => new int[Attributes[QualifierIndex].ValueCount].ToList();
        public static int MaxLevel = 0;

        public static List<Attribute> Attributes = new List<Attribute>();
        public static List<DataInstance> Instances { get; set; } = new List<DataInstance>();
        public static int UpdateColumn(int index, string value) => Attributes[index].AddValue(value.Trim(' '));
    }

    public class Attribute
    {
        public Attribute(string name) => Name = name;
        public string Name { get; }

        public readonly List<string> Values = new List<string>();

        public int AddValue(string value)
        {
            var index = Values.IndexOf(value);
            if (index < 0)
            {
                Values.Add(value);
                index = Values.Count - 1;
            }
            return index;
        }

        private int _valueCount = 0;
        public int ValueCount => _valueCount == 0 ? (_valueCount = Values.Count) : _valueCount;
    }
}
