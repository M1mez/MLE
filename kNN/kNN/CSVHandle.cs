using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    /// <summary>
    /// Handles Input Files.
    /// </summary>
    class CSVHandle
    {
        /// <summary>
        /// Reads in Data File.
        /// </summary>
        /// <returns>
        /// Dictionary of Data.
        /// String(Name/Result) of uniuque catagory paired with List of all rows of the catagory.
        /// An element of the row list is itself a list.
        /// This most inner list contains the value of each Attribute as a string.
        /// </returns>
        /// <param name="file">File.</param>
        public static Dictionary<string, List<List<string>>> Read(string file)
        {
            //var rawCSV = ReadCSV(file);
            var sortedCSV = new Dictionary<string, List<List<string>>>();

            foreach (var idx in SplitCSV(file))
            {
                var specType = idx.Last();
                List<string> currentRow = idx.Take(idx.Length).ToList();

                if (sortedCSV.ContainsKey(specType)) sortedCSV[idx.Last()].Add(currentRow);
                else sortedCSV[specType] = new List<List<string>>() { currentRow };
            }
            return sortedCSV;
        }

        /// <summary>
        /// Splits input data into seperate values.
        /// </summary>
        /// <returns>
        /// Array of all data rows.
        /// A row is itself an array, containing the split data.
        /// </returns>
        /// <param name="file">File.</param>
        private static string[][] SplitCSV(string file)
        {
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file).Skip(1).ToArray();
                string[][] parts = new string[lines.Length][];

				char seperator = GetSeperator(lines[0]);
                for (int i = 0; i < lines.Length; i++)
                {
                    parts[i] = lines[i].Split(seperator);
                }

                return parts;
            }
            else
                throw new FileNotFoundException();
        }

        /// <summary>
        /// Retrieves character used to seperate Data fields in the File.
        /// </summary>
        /// <returns>The seperating character</returns>
        /// <param name="sampleRow">Sample row of the Data Set</param>
        private static char GetSeperator(string sampleRow)
        {
            char[] seperators = { ',', ';' };
            foreach (char c in sampleRow)
            {
                if (seperators.Contains(c))
                {
                    return c;
                }
            }
            return '0';
        }
    }
}
