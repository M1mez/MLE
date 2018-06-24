using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork.Interfaces
{
	public interface ILayer
	{
		double[] NeuronValues { get; }
	}

	public interface IParentLayer : ILayer
	{
		//When getting set, should initialize dimensions of SynapseWeights and SynapseWeights_Before
		//according to number of Neurons in the childlayer.
		IChildLayer ChildLayer { get; }

		//2-Dimensional listing connection weights for each neuron to the Childlayer Neurons.
		double[][] SynapseWeights { get; set; }
		double[][] Synapse_WeightChange { get; set; }

		double[] NeuronBiasses { get; set; }
		double[] BiasWeights { get; set; }

		void AdjustWeights(double learningRate, double momentum);
	}

	public interface IChildLayer : ILayer
	{
		IParentLayer ParentLayer { get; }
	
		//Define the activation function for the neurons in this layer.
		ActivationFunctions ActivationFunction { get; set; }

		double[] NeuronValues_noActivation { get; }

		double[] Errors { get; set; }

		void CalculateNeuronValues();

		void CalculateErrors();
	}
}
