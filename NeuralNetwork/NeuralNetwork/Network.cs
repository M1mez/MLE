using NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork
{
	/// <summary
	/// Neural Network for the MNIST database 
	/// </summary>
	public class Network : INetwork<Image>
	{
		public Network(int nrInputNeurons, int nrHiddenNeurons, int nrOutputNeurons, double learningRate, double momentum, double goalErrorRate)
		{
			NrInputNeurons = nrInputNeurons;
			NrHiddenNeurons = nrHiddenNeurons;
			NrOutputNeurons = nrOutputNeurons;

			LearningRate = learningRate;
			Momentum = momentum;
			GoalErrorRate = goalErrorRate;

			InputLayer = new InputLayer(nrInputNeurons);
			HiddenLayer = new HiddenLayer(nrHiddenNeurons);
			OutputLayer = new OutputLayer(nrOutputNeurons);
		}

		private InputLayer InputLayer { get; set; }
		private HiddenLayer HiddenLayer { get; set; }
		private OutputLayer OutputLayer { get; set; }

		public int NrInputNeurons { get; }
		public int NrHiddenNeurons { get; }
		public int NrOutputNeurons { get; }

		public double LearningRate { get; }
		public double Momentum { get; }
		public double GoalErrorRate { get; }

		public void Initialize(
			ActivationFunctions hiddenLayerActFunc = ActivationFunctions.Sigmoid,
			ActivationFunctions outPutLayerActFunc = ActivationFunctions.Sigmoid)
		{
			//assign Layer dependencies
			InputLayer.ChildLayer = HiddenLayer;
			HiddenLayer.ParentLayer = InputLayer;
			HiddenLayer.ChildLayer = OutputLayer;
			OutputLayer.ParentLayer = HiddenLayer;

			//set Activation Functions
			HiddenLayer.ActivationFunction = hiddenLayerActFunc;
			OutputLayer.ActivationFunction = outPutLayerActFunc;

			//Assign random values to the Weights and set the bias
			for (int i = 0; i < NrInputNeurons; i++)
			{
				InputLayer.NeuronBiasses[i] = 1;
				InputLayer.BiasWeights[i] = Functions.RandomBias();
				for (int j = 0; j < InputLayer.ChildLayer.NeuronValues.Length; j++)
				{
					InputLayer.SynapseWeights[i][j] = Functions.RandomBias();
				}
			}

			for (int i = 0; i < NrHiddenNeurons; i++)
			{
				HiddenLayer.NeuronBiasses[i] = 1;
				HiddenLayer.BiasWeights[i] = Functions.RandomBias();
				for (int j = 0; j < HiddenLayer.ChildLayer.NeuronValues.Length; j++)
				{
					HiddenLayer.SynapseWeights[i][j] = Functions.RandomBias();
				}
			}
		}

		//optionally provide test-images to print a confusion matrix after each Cycle. Helps to find sweet spot between over and underfitting.
		public void Learn(List<Image> Images, List<Image> TestImages = null)
		{
			Stopwatch watch = new Stopwatch();
			int nrOfRuns = 0;
			double networkError = 0.0;

			//Train Network until desired error rate is reached
			do
			{
				double networkErrorSum = 0.0;
				nrOfRuns++;
				watch.Start();

				//For all Images in the training set
				for (int i = 0; i < Images.Count; i++)
				{
					//Set Image
					InputLayer.NeuronValues = Images[i].Data;
					for (int j = 0; j < NrOutputNeurons; j++)
					{
						if (j == (int)Images[i].Label)
							OutputLayer.DesiredNeuronValues[j] = 1.0;
						else OutputLayer.DesiredNeuronValues[j] = 0.0;
					}

					//Calculate values and adjust weights according to error.
					this.FeedForward();
					this.BackPropagate();

					networkErrorSum += CalculateError();
					networkError = networkErrorSum / (i + 1);
				}
				watch.Stop();

				//Print error rate and time elapsed for this epoch
				Console.Write("Error rate after epoch {0}: ", nrOfRuns, networkError);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("{0}", networkError);
				Console.ForegroundColor = ConsoleColor.Gray;

				Console.WriteLine("Time elapsed: {1}", nrOfRuns, watch.Elapsed.ToString());
				Console.WriteLine();

				if (TestImages != null)
				{
					ConfusionMatrixDrawer.Draw(Test(TestImages));
				}

			} while (networkError > GoalErrorRate);
		}

		public int[,] Test(List<Image> Images)
		{
			int[,] classificationMatrix = new int[10, 10];

			for (int i = 0; i < Images.Count; i++)
			{
				int classification = 0;
				double highest = 0.0;

				InputLayer.NeuronValues = Images[i].Data;

				this.FeedForward();

				for (int j = 0; j < OutputLayer.NeuronValues.Length; j++)
				{
					if (OutputLayer.NeuronValues[j] > highest)
					{
						highest = OutputLayer.NeuronValues[j];
						classification = j;
					}
				}
				classificationMatrix[Images[i].Label, classification]++;
			}
			return classificationMatrix;
		}

		public void FeedForward()
		{
			HiddenLayer.CalculateNeuronValues();
			OutputLayer.CalculateNeuronValues();
		}

		public void BackPropagate()
		{
			OutputLayer.CalculateErrors();
			HiddenLayer.CalculateErrors();
			HiddenLayer.AdjustWeights(LearningRate, Momentum);
			InputLayer.AdjustWeights(LearningRate, Momentum);
		}

		public double CalculateError()
		{
			double error = 0.0;
			for (int i = 0; i < OutputLayer.NeuronValues.Length; i++)
			{
				error += Math.Pow((OutputLayer.NeuronValues[i] - OutputLayer.DesiredNeuronValues[i]), 2);
			}
			error = error / OutputLayer.NeuronValues.Length;
			return error;
		}
	}
}
