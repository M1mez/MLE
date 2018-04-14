using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public static class FileHandling
    {
        public static void ReadFile(bool isDegbug = false)
        {
            var filePath = ChooseFile(isDegbug);
            using (var reader = new StreamReader(filePath))
            {
                string line;
                List<string> lineList;
                char delimiter = ';';
                if (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    delimiter = ChooseDelimiter(line, isDegbug);
                    lineList = SplitString(line, delimiter);
                    DataSet.SetAttributes(lineList);
                }
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line == null) continue;
                    lineList = SplitString(line, delimiter);
                    DataSet.Instances.Add(new DataInstance(lineList));
                }
            }
        }

        private static List<string> SplitString(string toSplit, char delimiter)
        {
            if (toSplit == null) return null;
            var values = toSplit.Split(delimiter).ToList();

            /*foreach (var value in values)
            {
                Console.Write(value);
            }
            Console.WriteLine();*/

            return values;
        }

        private static char ChooseDelimiter(string example, bool isDebug)
        {
            if (isDebug) return ',';
            bool success;
            char delimiter;

            Console.WriteLine($"Choose Delimiter from this string: \n\"{example}\"");
            do
            {
                var input = Console.ReadLine();
                success = char.TryParse(input, out delimiter) && example.Contains(delimiter) && example.Count(c => c == delimiter) > 1;
                if (!success) Console.WriteLine($"Please provide an existing char from this string: \n\"{example}\"");
            } while (!success);

            return delimiter;
        }

        private static string ChooseFile(bool isDebug)
        {
            var pdfFiles = Directory.GetFiles(Constants.CSVPath)
                .Select(System.IO.Path.GetFileName)
                .ToArray();
            var i = 0;
            if (isDebug) return System.IO.Path.Combine(Constants.CSVPath, pdfFiles[1]);

            switch (pdfFiles.Length)
            {
                case 0:
                    return null;
                case 1:
                    break;
                default:
                {
                    bool success;

                    Console.WriteLine("Choose File by providing the number:");
                    do
                    {
                        var fileCount = 0;
                        foreach (var file in pdfFiles)
                        {
                            Console.WriteLine($"{fileCount++ + 1}: {file}");
                        }

                        var input = Console.ReadLine();
                        success = int.TryParse(input, out i) && i > 0 && i <= fileCount;
                        if (!success) Console.WriteLine("Please provide a valid number!");
                    } while (!success);

                    i--;
                    break;
                }
            }
            return System.IO.Path.Combine(Constants.CSVPath, pdfFiles[i]);
        }

    }
}
