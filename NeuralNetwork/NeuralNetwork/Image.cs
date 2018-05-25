using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
	/// <summary>
	/// Image of the MNIST Database of handwritten digits
	/// </summary>
	public class Image
	{
		public int Label;
		public double[] Data;

		public Image(int label, byte[] data)
		{
			this.Label = label;

			this.Data = new double[data.Length];

			for (int i = 0; i < this.Data.Length; i++)
			{
				this.Data[i] = data[i] & 0xFF; //convert to unsigned
			}
			otsu();
		}
		
		public override String ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Label);
			sb.Append(" = ");
			sb.Append(Data.Length);
			sb.Append(": (");
			sb.Append("\n");
			for (int y = 0; y < 28; y++)
			{
				for (int x = 0; x < 28; x++)
				{
					if ((int)Data[(y * 28) + x] == 1)
					{
						sb.Append("*");
					}
					else
					{
						sb.Append(" ");
					}

				}
				sb.Append("\n");
			}
			sb.Append(")");

			return sb.ToString();
		}

		//Uses Otsu's Threshold algorithm to convert from grayscale to black and white
		private void otsu()
		{
			int[] histogram = new int[256];

			foreach (double datum in Data)
			{
				histogram[(int)datum]++;
			}

			double sum = 0;
			for (int j = 0; j < histogram.Length; j++)
			{
				sum += j * histogram[j];
			}

			double sumB = 0;
			int wB = 0;
			int wF = 0;

			double maxVariance = 0;
			int threshold = 0;

			int i = 0;
			bool found = false;

			while (i < histogram.Length && !found)
			{
				wB += histogram[i];

				if (wB != 0)
				{
					wF = Data.Length - wB;

					if (wF != 0)
					{
						sumB += (i * histogram[i]);

						double mB = sumB / wB;
						double mF = (sum - sumB) / wF;

						double varianceBetween = wB * Math.Pow((mB - mF), 2);

						if (varianceBetween > maxVariance)
						{
							maxVariance = varianceBetween;
							threshold = i;
						}
					}
					else
					{
						found = true;
					}
				}

				i++;
			}

			for (i = 0; i < Data.Length; i++)
			{
				Data[i] = Data[i] <= threshold ? 0 : 1;
			}
		}

		public int getLabel()
		{
			return Label;
		}

		public double[] getData()
		{
			return Data;
		}
	}
}
