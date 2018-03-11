using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    class CSVHandle
    {
        public static Dictionary<string, List<List<string>>> GetSortedCSV(string file)
        {
            //var rawCSV = ReadCSV(file);
            var sortedCSV = new Dictionary<string, List<List<string>>>();

            foreach (var idx in ReadCSV(file))
            {
                var specType = idx.Last();
                var currentRow = new List<string>();

                currentRow = idx.Take(idx.Length).ToList();
                if (sortedCSV.ContainsKey(specType)) sortedCSV[idx.Last()].Add(currentRow);
                else sortedCSV[specType] = new List<List<string>>() { currentRow };
            }
            return sortedCSV;
        }

        private static string[][] ReadCSV(string file)
        {
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file).Skip(1).ToArray();

                string[][] parts = new string[lines.Length][];

                for (int i = 0; i < lines.Length; i++)
                {
                    parts[i] = lines[i].Split(';');
                }

                return parts;
            }

            else
                throw new FileNotFoundException();
        }


    }
}
