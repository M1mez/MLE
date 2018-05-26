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
		double[] NeuronValues { get; }

		//initialize in constructor
		int NumberOfNeurons { get; }
	}

	public interface IParentLayer : ILayer
	{
		IChildLayer ChildLayer { get; }

		//2-Dimensional listing connection weights for each neuron
		double[][] SynapseWeights { get; set; }
		double[][] SynapseWeights_Before { get; set; }

		double[] NeuronBiasses { get; set; }

		double[] BiasWeights { get; set; }

		void AdjustWeights(double learningRate, double momentum);
	}

	public interface IChildLayer : ILayer
	{
		IParentLayer ParentLayer { get; }

		double[] Errors { get; set; }

		void CalculateNeuronValues();

		void CalculateErrors();
	}
}
