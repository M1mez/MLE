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
		
			for (int i = 0; i < DataVector.Length - 1; i++)
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

		public string Category;
		public readonly float[] DataVector;
		private bool CategoryIsSet;
	}
}
