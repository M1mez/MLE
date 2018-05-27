using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
	public static class ConfusionMatrixDrawer
	{
		private const string title = "* C O N F U S I O N    M A T R I X *";
		private const int coloumnLength = 8;


		public static void Draw(int[,] classificationMatrix)
		{
			int totalSize = coloumnLength * 10;
			double accuracy = GetAccuracy(classificationMatrix);
			string colName = "P r e d i c t e d   V a l u e";
			string rowName = "True Value";
			//title
			Console.WriteLine(" ".PadLeft(title.Length, '*'));
			Console.WriteLine(title);
			Console.WriteLine(" ".PadLeft(title.Length, '*'));
			Console.WriteLine();

			//Row Names and values
			Console.WriteLine(colName.PadLeft((totalSize + colName.Length) / 2));
			for (int i = 0; i < 10; i++)
			{
				Console.Write(i.ToString().PadRight(coloumnLength));
			}
			Console.WriteLine();
			Console.WriteLine(" ".PadLeft(totalSize, '-'));
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					if (i == j) Console.ForegroundColor = ConsoleColor.Green;
					else Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(classificationMatrix[i, j].ToString().PadRight(coloumnLength));
				}
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("| <- {0} {1}", i.ToString(), rowName[i]);
			}
			Console.WriteLine("Accuracy = {0}%", accuracy.ToString("n2"));
			Console.WriteLine("Error Rate = {0}%", (100 - accuracy).ToString("n2"));
		}

		private static double GetAccuracy(int[,] classificationMatrix)
		{
			double total = 0;
			double truePos = 0;
			double accuracy = 0.0F;

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					total += classificationMatrix[i, j];
					if (i == j) truePos += classificationMatrix[i, j];
				}
			}

			accuracy = truePos / total;
			accuracy *= 100;
			return accuracy;
		}
	}
}
