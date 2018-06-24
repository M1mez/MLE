using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork
{
	/*
	* Sebastian Stricker
	* May 2018
	*/

	/* IMPORTANT: Compiling in release mode more than doubles the speed. */
	class Program
	{
		//ImageData are available here http://yann.lecun.com/exdb/mnist/ (as of 30.05.2018)
		static readonly string IMAGEDATA_PATH = "C:/Users/Sebi/Documents/FH-Unterlagen/Projekte/SEMESTER 4/Machine Learning/MLE/NeuralNetwork/NeuralNetwork/ImageData";
		static readonly string TRAIN_IMAGES_DATA = "train-images-idx3-ubyte.dat";
		static readonly string TRAIN_IMAGES_LABELS = "train-labels-idx1-ubyte.dat";
		static readonly string TEST_IMAGES_DATA = "t10k-images-idx3-ubyte.dat";
		static readonly string TEST_IMAGES_LABELS = "t10k-labels-idx1-ubyte.dat";

		public static void Main(string[] args)
		{
			Console.WriteLine("--------Neural Network--------");
			Stopwatch sw = new Stopwatch();
			Network imageNeuralNetwork;

			Console.Write("Loading Images....");
			LoadImageData(out List<Image> trainImages, out List<Image> testImages);

			Console.WriteLine("Success!\n");
			//PrintStatistics(testImages, "Testing Image Set");
			//PrintStatistics(trainImages, "Training Image Set");
			//PrintImage(1, 3737);

			Console.Write("Initializing Network...");
			imageNeuralNetwork = new Network(784, 89, 10, 0.2, 0.9, 0.005);
			imageNeuralNetwork.Initialize(ActivationFunctions.Sigmoid, ActivationFunctions.Sigmoid);

			Console.WriteLine("Success!\n");
			Console.WriteLine("*** Learning-Cycle ***");
			sw.Start();
			//imageNeuralNetwork.Learn(trainImages, testImages);
			imageNeuralNetwork.Learn(trainImages);
			sw.Stop();

			Console.Write("Testing Network....");
			int[,] confusionMatrix = imageNeuralNetwork.Test(testImages);

			Console.WriteLine("Success!");
			ConfusionMatrixDrawer.Draw(confusionMatrix);

			Console.WriteLine("Total Learning Time taken: {0}\n", sw.Elapsed.ToString());

			Console.ReadKey();
		}

		public static void LoadImageData(out List<Image> trainImages, out List<Image> testImages)
		{
			testImages = ImageLoader.LoadImages(Path.Combine(IMAGEDATA_PATH, TEST_IMAGES_LABELS), Path.Combine(IMAGEDATA_PATH, TEST_IMAGES_DATA), false);
			trainImages = ImageLoader.LoadImages(Path.Combine(IMAGEDATA_PATH, TRAIN_IMAGES_LABELS), Path.Combine(IMAGEDATA_PATH, TRAIN_IMAGES_DATA), true);
		}

		//Prints an ascii representation of an image.
		public void PrintImage(List<Image> imageSet, int index)
		{
			Console.WriteLine(imageSet[index].ToString());
		}

		/// <summary>
		/// Prints number of Pictures in each label
		/// </summary>
		public static void PrintStatistics(List<Image> Images, string TitleofDataset = "Dataset")
		{
			List<int> LabelCount = new List<int>();
			for (int i = 0; i < 10; i++) { LabelCount.Add(0); }

			foreach (Image img in Images)
			{
				LabelCount[img.Label]++;
			}

			Console.WriteLine("*************************************");
			Console.WriteLine("* Statistics for {0}", TitleofDataset);
			Console.WriteLine("*");
			Console.WriteLine("* Total Number of Images: {0}", Images.Count);
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("* Number of Images in Label {0}: {1}", i, LabelCount[i]);
			}
			Console.WriteLine("*************************************");
		}
	}
}
