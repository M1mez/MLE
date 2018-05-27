using NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork
{
	public class Network : INetwork
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

		public void Initialize()
		{
			InputLayer.ChildLayer = HiddenLayer;
			HiddenLayer.ParentLayer = InputLayer;
			HiddenLayer.ChildLayer = OutputLayer;
			OutputLayer.ParentLayer = HiddenLayer;

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

		public void Learn(List<Image> Images)
		{
			Stopwatch watch = new Stopwatch();
			int nrOfRuns = 1;
			double networkError = 0.0;
			
			do
			{

				int networkErrorMeanCount = 0;
				double networkErrorSum = 0.0;

				watch.Start();
				for (int i = 0; i < Images.Count; i++)
				{
					InputLayer.NeuronValues = Images[i].Data;
					for (int j = 0; j < NrOutputNeurons; j++)
					{
						if (j == (int)Images[i].Label)
							OutputLayer.DesiredNeuronValues[j] = 1.0;
						else OutputLayer.DesiredNeuronValues[j] = 0.0;
					}
					this.FeedForward();
					this.BackPropagate();
					networkErrorSum += CalculateError();
					networkErrorMeanCount++;
					networkError = networkErrorSum / networkErrorMeanCount;
				}
				watch.Stop();

				Console.Write("Error Rate after generation {0}: ", nrOfRuns, networkError);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("{0}", networkError);
				Console.ForegroundColor = ConsoleColor.Gray;

				Console.WriteLine("Time Elapsed: {1}", nrOfRuns, watch.Elapsed.ToString());
				Console.WriteLine();
				nrOfRuns++;
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

		public void FeedForward()
		{
			HiddenLayer.CalculateNeuronValues();
			OutputLayer.CalculateNeuronValues();
		}
	}
}
