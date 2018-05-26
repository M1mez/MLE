using NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
			HiddenLayer = new HiddenLayer();
			OutputLayer = new OutputLayer();
		}

		private InputLayer InputLayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		private HiddenLayer HiddenLayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		private OutputLayer OutputLayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public int NrInputNeurons { get; }
		public int NrHiddenNeurons { get; }
		public int NrOutputNeurons { get; }

		public double LearningRate { get; }
		public double Momentum { get; }
		public double GoalErrorRate { get; }

		public void Learn(List<Image> Images)
		{

		}

		public void BackPropagate()
		{
			OutputLayer.CalculateErrors();
			HiddenLayer.CalculateErrors();
			HiddenLayer.AdjustWeights();
			InputLayer.AdjustWeights();
		}

		public double CalculateError()
		{
			double error = 0.0;
			for (int i = 0; i < OutputLayer.NumberOfNeurons; i++)
			{
				error += Math.Pow((OutputLayer.NeuronValues[i] - OutputLayer.DesiredNeuronValues[i]), 2) ;
			}
			error = error / OutputLayer.NumberOfNeurons;
			return error;
		}

		public void FeedForward()
		{
			HiddenLayer.CalculateNeuronValues();
			OutputLayer.CalculateNeuronValues();
		}
	}
}
