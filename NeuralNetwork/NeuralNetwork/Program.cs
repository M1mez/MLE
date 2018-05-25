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

			Console.WriteLine("--------TestImage 1--------");
			Console.WriteLine(testImages[12533].ToString());

			Console.ReadKey();
		}
    }
}
