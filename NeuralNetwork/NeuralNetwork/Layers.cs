using NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
	public abstract class Layer : ILayer
	{
		public double[] NeuronValues { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public double[] NeuronBias { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public ILayer ParentLayer => throw new NotImplementedException();

		public ILayer ChildLayer => throw new NotImplementedException();

		public int NumberOfNeurons => throw new NotImplementedException();

		public void AdjustWeights()
		{
			throw new NotImplementedException();
		}

		public abstract void CalculateErrors();

		public void CalculateNeuronValues()
		{
			throw new NotImplementedException();
		}
	}
}
