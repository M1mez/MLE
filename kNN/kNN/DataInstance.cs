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
	class DataInstance
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
				Category = dataCells[index];
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

		//Category of the instance
		public string Category
		{
			get { return this._category; }
			set
			{
				if (!String.IsNullOrEmpty(_category)) decreaseCategoryCounter(_category); //in case someone decides to change already set Category

				_category = value;
				increaseCategoryCounter(value);
			}
		}

		//Category cread from file.
		public int TrueCategory { get; private set; }

		//Category assigned by Algorithm. LookUp table in DataSet class containing this instance.
		public int GuessedCategory { get; set; }

		private string _category = null;

		//Attribute values of the instance
		public readonly float[] DataVector;

		//static map to count objects of all categories
		public static Dictionary<string, int> CategoriesInstancesCounter = new Dictionary<string, int>();
		private bool CategoryIsSet;






		private void increaseCategoryCounter (string categoryOfInstance)
		{
			if(CategoriesInstancesCounter.ContainsKey(categoryOfInstance))
			{
				CategoriesInstancesCounter[categoryOfInstance]++;
			}
			else
			{
				CategoriesInstancesCounter.Add(categoryOfInstance, 1);
			}
		}

		private void decreaseCategoryCounter (string oldCategoryOfInstance)
		{
			CategoriesInstancesCounter[oldCategoryOfInstance]--;
			if(CategoriesInstancesCounter[oldCategoryOfInstance] < 1)
			{
				CategoriesInstancesCounter.Remove(oldCategoryOfInstance);
			}

		}
	}
}
