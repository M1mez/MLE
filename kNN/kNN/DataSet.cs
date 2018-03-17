using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
	class DataSet
	{
		/*
        public DataSet(Dictionary<string, List<List<string>>> test, int k)
        {
            //this.test = test;
            //train = DeepClone(test);
            //CreateKBlocks(k);
        }*/
		public DataSet()
		{

		}

		/// <summary>
		/// Reads in data file and constructs data Instances.
		/// First Line must contain Parameter Names
		/// </summary>
		/// <param name="DataFile">Path to the data file</param>
		public void ReadFile(string DataFile)
		{
			if (File.Exists(DataFile))
			{
				string[] lines = File.ReadAllLines(DataFile).ToArray();
				SetSeperator(lines[0]);
				int startIndex = SetAttributeNames(lines[0]);

				for (int i = startIndex; i < lines.Length; i++)
				{
					string[] dataCells = lines[i].Split(Seperator);
					AddInstance(dataCells, true);
				}
			}
		}

		/// <summary>
		/// Add Instance to Dataset
		/// </summary>
		/// <param name="dataCells">All values of the unique cells of the row.</param>
		/// <param name="shouldSetCategory">Indicated if public string category should be set</param>
		/// <param name="index">Index of dataCells that points to category. Default is last element.</param>
		public void AddInstance(string[] dataCells, bool shouldSetCategory, int index = -1)
		{
			if (dataCells.Length == ColoumnCount)
			{
				DataInstances.Add(new DataInstance(dataCells, shouldSetCategory));
			}
			else
			{
				Console.WriteLine("Data instance has to few or too many Attributes! Skipped data instance near row number {0}", DataInstances.Count);
			}
		}

		public static List<string> AttributeNames = new List<string>();
		public List<DataInstance> DataInstances = new List<DataInstance>(); //all Rows of the data table read.

		//List gets extended if a new Category is found during reading. Index of Categories references same Category in CategoryInstances
		public static List<string> Categories = new List<string>();
		public static List<List<DataInstance>> CategoryInstances = new List<List<DataInstance>>();


		public static int ColoumnCount { get; private set; } = 0; //used to check consistency of row length throughout data file.
		private char[] Seperator = new char[1]; //Data cell seperator

		/// <summary>
		/// Retrieves character used to seperate Data fields in the File.
		/// </summary>
		/// <returns>The seperating character</returns>
		/// <param name="sampleRow">Sample row of the Data Set</param>
		private void SetSeperator(string sampleRow)
		{
			char[] seperators = { ',', ';' };
			foreach (char c in sampleRow)
			{
				if (seperators.Contains(c))
				{
					Seperator[0] = c;
					return;
				}
			}
			throw new ArgumentException("Seperator could not be extracted from sampleRow given.");
		}

		/// <summary>
		/// Checks if first row contains strings and sets Attribute names accordingly
		/// </summary>
		/// <param name="firstRow"></param>
		/// <returns>0 if no attribute-name row was given, 1 if it has been read succesfully</returns>
		private int SetAttributeNames(string firstRow)
		{
			string[] splitCells = firstRow.Split(Seperator);
			char[] stringIndicators = { '"' };

			if (firstRow.StartsWith("\"", StringComparison.InvariantCultureIgnoreCase))
			{
				foreach (string name in splitCells)
				{
					AttributeNames.Add(ColoumnCount, name.Trim(stringIndicators));
					ColoumnCount++; //sadfsdf
				}
				return 1;
			}
			else
			{
				ColoumnCount = splitCells.Length;
				for (int i = 0; i < ColoumnCount; i++)
				{
					AttributeNames.Add(i, ("Attribute " + ColoumnCount));
				}
				return 0;
			}

			foreach (string name in A)
		}
	}
}
