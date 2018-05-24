using NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
	public class Network : INetwork
	{
		public ILayer InputLayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public ILayer HiddenLayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public ILayer OutputLayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void BackPropagate()
		{
			throw new NotImplementedException();
		}

		public void CalculateError()
		{
			throw new NotImplementedException();
		}

		public void FeedForward()
		{
			throw new NotImplementedException();
		}
	}
}
