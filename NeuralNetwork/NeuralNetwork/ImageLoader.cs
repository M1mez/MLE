using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NeuralNetwork
{
	/**  Converted from Java to C# **/
	public static class ImageLoader
	{
		/** the following constants are defined as per the values described at http://yann.lecun.com/exdb/mnist/ **/
		private static readonly int MAGIC_OFFSET = 0;
		private static readonly int OFFSET_SIZE = 4; //in bytes

		private static readonly int LABEL_MAGIC = 2049;
		private static readonly int IMAGE_MAGIC = 2051;

		private static readonly int NUMBER_ITEMS_OFFSET = 4;
		private static readonly int ITEMS_SIZE = 4;

		private static readonly int NUMBER_OF_ROWS_OFFSET = 8;
		private static readonly int ROWS_SIZE = 4;
		public static readonly int ROWS = 28;

		private static readonly int NUMBER_OF_COLUMNS_OFFSET = 12;
		private static readonly int COLUMNS_SIZE = 4;
		public static readonly int COLUMNS = 28;

		private static readonly int IMAGE_OFFSET = 16;
		private static readonly int IMAGE_SIZE = ROWS * COLUMNS;

		public static List<Image> LoadImages(String labelFileName, String imageFileName)
		{
			List<Image> images = new List<Image>();

			byte[] labelBytes = File.ReadAllBytes(labelFileName);
			byte[] imageBytes = File.ReadAllBytes(imageFileName);

			Array.Reverse(labelBytes, MAGIC_OFFSET, 4);
			int magicTest = BitConverter.ToInt32(labelBytes, MAGIC_OFFSET);
			if (BitConverter.ToInt32(labelBytes, MAGIC_OFFSET) != LABEL_MAGIC)
			{
				throw new IOException("Bad magic number in label file!");
			}

			Array.Reverse(imageBytes, MAGIC_OFFSET, 4);
			if (BitConverter.ToInt32(imageBytes, MAGIC_OFFSET) != IMAGE_MAGIC)
			{
				throw new IOException("Bad magic number in image file!");
			}

			Array.Reverse(labelBytes, NUMBER_ITEMS_OFFSET, 4);
			Array.Reverse(imageBytes, NUMBER_ITEMS_OFFSET, 4);
			int numberOfLabels = BitConverter.ToInt32(labelBytes, NUMBER_ITEMS_OFFSET);
			int numberOfImages = BitConverter.ToInt32(imageBytes, NUMBER_ITEMS_OFFSET);

			if (numberOfImages != numberOfLabels)
			{
				throw new IOException("The number of labels and images do not match!");
			}

			Array.Reverse(imageBytes, NUMBER_OF_ROWS_OFFSET, 4);
			Array.Reverse(imageBytes, NUMBER_OF_COLUMNS_OFFSET, 4);
			int numRows = BitConverter.ToInt32(imageBytes, NUMBER_OF_ROWS_OFFSET);
			int numCols = BitConverter.ToInt32(imageBytes, NUMBER_OF_COLUMNS_OFFSET);

			if (numRows != ROWS && numRows != COLUMNS)
			{
				throw new IOException("Bad image. Rows and columns do not equal " + ROWS + "x" + COLUMNS);
			}

			for (int i = 0; i < numberOfLabels; i++)
			{
				int label = labelBytes[OFFSET_SIZE + ITEMS_SIZE + i];
				byte[] imageData = new Byte[IMAGE_SIZE];
				Buffer.BlockCopy(imageBytes, ((i * IMAGE_SIZE) + IMAGE_OFFSET), imageData, 0, IMAGE_SIZE);

				images.Add(new Image(label, imageData));
			}

			return images;
		}
	}
}
