using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
	/// <summary>
	/// Dynamically prints a square Matrix as a confusion matrix.
	/// </summary>
	public static class ConfusionMatrixDrawer
	{
		public static void Draw(int[,] confusionMatrix)
		{
			string title = "* C O N F U S I O N    M A T R I X *";
			string colName = "P r e d i c t e d   V a l u e";
			string rowName = "True Value ";

			//Check that the given matrix is indeed sqaure
			int nrOfColoumns = confusionMatrix.GetUpperBound(0) + 1;
			if (Math.Pow(nrOfColoumns, 2) != confusionMatrix.Length)
				throw new ArgumentException();

			//paddings
			int coloumnWidth = GetMimimumPadding(confusionMatrix) + 2;
			int totalWidth = coloumnWidth * nrOfColoumns;

			//title
			Console.WriteLine(" ".PadLeft(title.Length, '*'));
			Console.WriteLine(title);
			Console.WriteLine(" ".PadLeft(title.Length, '*'));
			Console.WriteLine();

			//Row Names and values
			Console.WriteLine(colName.PadLeft((totalWidth + colName.Length) / 2));
			for (int i = 0; i < 10; i++)
			{
				Console.Write(i.ToString().PadRight(coloumnWidth));
			}
			Console.WriteLine();
			Console.WriteLine(" ".PadLeft(totalWidth, '-'));

			//Matrix and Coloumn values.
			for (int i = 0; i < nrOfColoumns; i++)
			{
				for (int j = 0; j < nrOfColoumns; j++) //coloumns == rows, square.
				{
					if (i == j)
						Console.ForegroundColor = ConsoleColor.Green;
					else
						Console.ForegroundColor = ConsoleColor.Red;

					Console.Write(confusionMatrix[i, j].ToString().PadRight(coloumnWidth));
				}
				Console.ForegroundColor = ConsoleColor.Gray;

				if (i == 0)
					Console.WriteLine("| <- {0} {1}", i.ToString(), rowName);
				else
					Console.WriteLine("| <- {0}", i.ToString());
			}

			//Accuracy
			double accuracy = GetAccuracy(confusionMatrix);
			Console.WriteLine("Accuracy = {0}%", accuracy.ToString("n2"));
			Console.WriteLine("Error Rate = {0}%", (100 - accuracy).ToString("n2"));
			Console.WriteLine();
		}

		private static int GetMimimumPadding(int[,] confusionMatrix)
		{
			int max = 0;
			for (int i = 0; i < confusionMatrix.GetUpperBound(0); i++)
			{
				for (int j = 0; j < 10; j++)
				{
					if (confusionMatrix[i, j] > max) max = confusionMatrix[i, j];
				}
			}
			return (int)(Math.Log10(max) + 1); //log10+1 gives number of digits
		}

		private static double GetAccuracy(int[,] confusionMatrix)
		{
			double total = 0;
			double truePos = 0;
			double accuracy = 0.0F;

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					total += confusionMatrix[i, j];
					if (i == j) truePos += confusionMatrix[i, j];
				}
			}

			accuracy = truePos / total;
			accuracy *= 100;
			return accuracy;
		}
	}
}
