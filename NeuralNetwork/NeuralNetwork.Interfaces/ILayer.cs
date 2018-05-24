using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Interfaces
{
	public interface ILayer
	{
		//Neurons in this layer
		double[] NeuronValues { get; set; }

		double[] NeuronBias { get; set; }

		ILayer ParentLayer { get; }

		ILayer ChildLayer { get; }

		int NumberOfNeurons { get; }

		void CalculateNeuronValues();

		void CalculateErrors();

		void AdjustWeights();
	}
}
