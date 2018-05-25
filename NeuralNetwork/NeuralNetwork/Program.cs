using System;
using System.IO;
using System.Collections.Generic;

namespace NeuralNetwork
{

	class Program
    {
		static readonly string IMAGEDATA_PATH = "C:/Users/Sebi/Documents/FH-Unterlagen/Projekte/SEMESTER 4/Machine Learning/MLE/NeuralNetwork/NeuralNetwork/ImageData";
		static readonly string TRAIN_IMAGES_DATA = "train-images-idx3-ubyte.dat";
		static readonly string TRAIN_IMAGES_LABELS = "train-labels-idx1-ubyte.dat";
		static readonly string TEST_IMAGES_DATA = "t10k-images-idx3-ubyte.dat";
		static readonly string TEST_IMAGES_LABELS = "t10k-labels-idx1-ubyte.dat";

		public static void Main(string[] args)
		{
			Console.WriteLine("--------Neural Network--------");
			List<Image> testImages = ImageLoader.LoadImages(Path.Combine(IMAGEDATA_PATH, TRAIN_IMAGES_LABELS), Path.Combine(IMAGEDATA_PATH, TRAIN_IMAGES_DATA));
			List<Image> trainImages = ImageLoader.LoadImages(Path.Combine(IMAGEDATA_PATH, TEST_IMAGES_LABELS), Path.Combine(IMAGEDATA_PATH, TEST_IMAGES_DATA));

			PrintStatistics(testImages, "Testing Image Set");
			Console.WriteLine();
			PrintStatistics(trainImages, "Training Image Set");
			Console.WriteLine("--------TestImage 1--------");
			Console.WriteLine(testImages[12533].ToString());

			Console.ReadKey();
		}

		public static void PrintStatistics(List<Image> Images, string TitleofDataset)
		{
			List<int> LabelCount = new List<int>();
			for(int i = 0; i < 10; i++) { LabelCount.Add(0); }

			foreach(Image img in Images)
			{
				LabelCount[img.Label]++;
			}
			Console.WriteLine("*************************************", TitleofDataset);
			Console.WriteLine("* Statistics for {0}", TitleofDataset);
			Console.WriteLine("*");
			Console.WriteLine("* Total Number of Images: {0}", Images.Count);
			for(int i = 0; i < 10; i++)
			{
				Console.WriteLine("* Number of Images in Label {0}: {1}",i, LabelCount[i]);
			}
			Console.WriteLine("*************************************", TitleofDataset);
		}
    }
}
