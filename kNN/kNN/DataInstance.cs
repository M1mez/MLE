using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
	/// <summary>
	/// A DataInstance object represents one row in a data table.
	/// /// </summary>
	public class DataInstance
	{
		/// <summary>
		/// Pass All values of the unique cells of the row.
		/// </summary>
		/// <param name="dataCells">All values of the unique cells of the row.</param>
		/// <param name="shouldSetCategory">Indicated if public string category should be set</param>
		/// <param name="index">Index of dataCells that points to category. Default is last element.</param>
		public DataInstance(string[] dataCells, bool shouldSetCategory, int index = -1)
		{
			int nrOfCells = dataCells.Length;

			CategoryIsSet = shouldSetCategory;
			if (index < 0 || index > nrOfCells)
			{
				index = nrOfCells - 1;
			}

			if (CategoryIsSet)
			{

				StringBuilder sb = new StringBuilder(DataSet.AttributeNames.Last(), 3);
				sb.Append(": ");
				sb.Append(dataCells[index]);

				string cat = sb.ToString();
				int Category = DataSet.Categories.IndexOf(cat);
				if(Category == -1)
				{
				
					DataSet.Categories.Add(cat);
					Category = DataSet.Categories.Count - 1;

					DataSet.CategoryInstances.Add(new List<DataInstance>());
				}
				DataSet.CategoryInstances[Category].Add(this);

				this.TrueCategory = Category;

				DataVector = new float[(nrOfCells - 1)];
			}
			else
			{
				DataVector = new float[(nrOfCells)];
			}
		
			for (int i = 0; i < DataVector.Length; i++)
			{
				try
				{
					DataVector[i] = float.Parse(dataCells[i], CultureInfo.InvariantCulture.NumberFormat);
				}
				catch(Exception e)
				{
					Console.WriteLine("Data cell value either couldn't be parsed to float or couldn't be stored in an Array! {0}", e.Source);
				}
			}
		}

		//Category cread from file .LookUp table in DataSet class containing this instance.
		public int TrueCategory { get; private set; }

		//Category assigned by Algorithm. LookUp table in DataSet class containing this instance.
		public int GuessedCategory { get; set; }
		
		//Attribute values of the instance
		public readonly float[] DataVector;
		private bool CategoryIsSet;
	}
}
