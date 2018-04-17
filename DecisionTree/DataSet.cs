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
        #region Fields
        public static readonly List<Attribute> Attributes = new List<Attribute>();

        public static int MaxLevel = 0;

        private static List<bool> _wasLastPath;
        private static List<int> _levelWidths;

        //TODO Warum in DataSet RootNode?
        public static readonly Node RootNode = new Node(-1);
        private static int _qualifierIndex;
        public static int QualifierIndex => _qualifierIndex == 0 ? (_qualifierIndex = Attributes.Count - 1) : _qualifierIndex;

        public static List<int> GetEmptyQualifierCount => new int[Attributes[QualifierIndex].ValueCount].ToList();

        public static List<DataInstance> Instances { get; set; } = new List<DataInstance>();
        #endregion

        #region Properties
        private static List<int> LevelWidths =>
            _levelWidths ?? (_levelWidths = new int[MaxLevel].ToList());

        private static List<bool> WasLastPath
        {
            get
            {
                if (_wasLastPath == null)
                {
                    _wasLastPath = new bool[MaxLevel].ToList();
                    _wasLastPath[0] = true;
                }

                return _wasLastPath;
            }
        }
        
        public static int UpdateColumn(int index, string value) => Attributes[index].AddValue(value.Trim(' '));
        #endregion
        
        #region Methods
        public static void SetAttributes(List<string> rows)
        {
            rows.ForEach(el =>
            {
                Attributes.Add(new Attribute(el));
            });
        }

        public static List<FrequencyTable> GetFrequencyTables(List<DataInstance> instances)
        {
            var list = new List<FrequencyTable>();
            for (var index = 0; index < Attributes.Count - 1; index++)
            {
                list.Add(new FrequencyTable(instances, index));
            }

            return list;
        }

        public static void PrintDesignStructure() => PrintDesignStructure(RootNode);
        private static void PrintDesignStructure(Node n, int level = 0, bool isLastPath = true)
        {
            for (var index = 0; index <= level; index++)
            {
                if (index == level)
                    if (isLastPath)
                    {
                        WasLastPath[level] = true;
                        Console.Write("└");
                    }
                    else Console.Write("├");
                else
                if (WasLastPath[index]) Console.Write(" ");
                else Console.Write("│");
                Console.Write(new string(' ', LevelWidths[index]));
            }

            string name;
            if (n.PreviousAttribute >= 0)
            {
                name = GetName(n.PreviousAttribute, n.OriginEdge);
                Console.Write($"({name}) -> ");
            }

            if (n.IsLeaf)
            {
                Console.WriteLine($"{GetName(QualifierIndex)}: {GetName(QualifierIndex, n.Qualifier)}");
            }
            else
            {
                name = GetName(n.Attribute);
                LevelWidths[level] = name.Length;
                if (n.PreviousAttribute >= 0) LevelWidths[level] += 10;
                Console.WriteLine($"{name} ");
            }
            

            for (var index = 0; index < n.Paths.Count; index++)
            {
                LevelWidths[level + 1] = 0;
                var path = n.Paths[index];
                if (index == Attributes[n.Attribute].ValueCount - 1)
                {
                    PrintDesignStructure(path.Destination, level + 1, true);
                }
                else PrintDesignStructure(path.Destination, level + 1, false);
            }
        }

        private static string GetName(int attributeIndex, int valueIndex = -1) => valueIndex < 0 ? Attributes[attributeIndex].Name : Attributes[attributeIndex].Values[valueIndex];
        #endregion
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
        
        public int ValueCount => Values.Count;
    }
}
