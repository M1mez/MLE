using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NeuralNetwork
{

	class Program
	{
		static readonly string IMAGEDATA_PATH = "C:/Users/Sebi/Documents/FH-Unterlagen/Projekte/SEMESTER 4/Machine Learning/MLE/NeuralNetwork/NeuralNetwork/ImageData";
		static readonly string TRAIN_IMAGES_DATA = "train-images-idx3-ubyte.dat";
		static readonly string TRAIN_IMAGES_LABELS = "train-labels-idx1-ubyte.dat";
		static readonly string TEST_IMAGES_DATA = "t10k-images-idx3-ubyte.dat";
		static readonly string TEST_IMAGES_LABELS = "t10k-labels-idx1-ubyte.dat";

		static List<Image> testImages;
		static List<Image> trainImages;

		public static void Main(string[] args)
		{
			Console.WriteLine("--------Neural Network--------");
			Console.Write("Loading Images....");
			LoadImageData();
			Console.WriteLine("Success!");
			//PrintStatistics(testImages, "Testing Image Set");
			//PrintStatistics(trainImages, "Training Image Set");

			//PrintImage(1, 3737);

			Network imageNeuralNetwork = new Network(784, 89, 10, 0.2, 0.9, 0.005);
			Stopwatch sw = new Stopwatch();
			Console.Write("Initializing....");
			imageNeuralNetwork.Initialize();
			Console.WriteLine("Success!");
			Console.WriteLine();
			
			sw.Start();
			Console.WriteLine("*** Learning-Cycle ***");
			imageNeuralNetwork.Learn(testImages);
			sw.Stop();

			Console.Write("Test....");
			int[,] confusionMatrix = imageNeuralNetwork.Test(trainImages);
			Console.WriteLine("Success!");
			ConfusionMatrixDrawer.Draw(confusionMatrix);

			Console.WriteLine();
			Console.WriteLine("Total Learning Time taken: {0}", sw.Elapsed.ToString());
			Console.WriteLine();


			Console.ReadKey();
		}




		public static void LoadImageData()
		{
			testImages = ImageLoader.LoadImages(Path.Combine(IMAGEDATA_PATH, TRAIN_IMAGES_LABELS), Path.Combine(IMAGEDATA_PATH, TRAIN_IMAGES_DATA));
			trainImages = ImageLoader.LoadImages(Path.Combine(IMAGEDATA_PATH, TEST_IMAGES_LABELS), Path.Combine(IMAGEDATA_PATH, TEST_IMAGES_DATA));
		}

		//Bool = 1 -> train images.  Bool = 0 -> test images
		public void PrintImage(bool dataSet, int imagenumber)
		{
			if (dataSet) Console.WriteLine(trainImages[imagenumber].ToString());
			else         Console.WriteLine(testImages[imagenumber].ToString());
		}

		public static void PrintStatistics(List<Image> Images, string TitleofDataset)
		{
			List<int> LabelCount = new List<int>();
			for (int i = 0; i < 10; i++) { LabelCount.Add(0); }

			foreach (Image img in Images)
			{
				LabelCount[img.Label]++;
			}
			Console.WriteLine("*************************************", TitleofDataset);
			Console.WriteLine("* Statistics for {0}", TitleofDataset);
			Console.WriteLine("*");
			Console.WriteLine("* Total Number of Images: {0}", Images.Count);
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("* Number of Images in Label {0}: {1}", i, LabelCount[i]);
			}
			Console.WriteLine("*************************************", TitleofDataset);
		}
	}
}
