using NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork
{
	public abstract class Layer : ILayer
	{
		public double[] NeuronValues { get; protected set; }

		public int NumberOfNeurons { get; protected set; }
	}

	public class InputLayer : Layer, IParentLayer
	{
		public InputLayer(int size)
		{
			NumberOfNeurons = size;
		}

		public IChildLayer ChildLayer { get; set; }

		public double[][] SynapseWeights { get; set; }
		public double[][] SynapseWeights_Before { get; set; }

		public double[] NeuronBiasses { get; set; }
		public double[] BiasWeights { get; set; }


		public void AdjustWeights(double learningRate, double momentum)
		{
			double dw = 0.0;
			for (int i = 0; i < NumberOfNeurons; i++)
			{
				for (int j = 0; j < ChildLayer.NumberOfNeurons; j++)
				{
					dw = learningRate * ChildLayer.Errors[j] * NeuronValues[i];
					SynapseWeights[i][j] += dw + momentum * SynapseWeights_Before[i][j];
					SynapseWeights_Before[i][j] = dw;
				}
			}
			for (int j = 0; j < ChildLayer.NumberOfNeurons; j++)
			{
				BiasWeights[j] += (learningRate * ChildLayer.Errors[j] * NeuronBiasses[j]);
			}
		}
	}

	public class HiddenLayer : Layer, IParentLayer, IChildLayer
	{
		public HiddenLayer()
		{

		}

		public IChildLayer ChildLayer { get; set; }
		public IParentLayer ParentLayer { get; set; }

		public double[][] SynapseWeights { get; set; }
		public double[][] SynapseWeights_Before { get; set; }

		public double[] NeuronBiasses { get; set; }
		public double[] BiasWeights { get; set; }
		public double[] Errors { get; set; }

		public void AdjustWeights(double learningRate, double momentum)
		{
			double dw = 0.0;
			for (int i = 0; i < NumberOfNeurons; i++)
			{
				for (int j = 0; j < ChildLayer.NumberOfNeurons; j++)
				{
					dw = learningRate * ChildLayer.Errors[j] * NeuronValues[i];
					SynapseWeights[i][j] += dw + momentum * SynapseWeights_Before[i][j];
					SynapseWeights_Before[i][j] = dw;
				}
			}
			for (int j = 0; j < ChildLayer.NumberOfNeurons; j++)
			{
				BiasWeights[j] += (learningRate * ChildLayer.Errors[j] * NeuronBiasses[j]);
			}
		}

		public void CalculateErrors()
		{
			double sum = 0.0;
			for (int i = 0; i < NumberOfNeurons; i++)
			{
				sum = 0.0;
				for (int j = 0; j < ChildLayer.NumberOfNeurons; j++)
				{
					sum += ChildLayer.Errors[j] * SynapseWeights[i][j];
				}
				Errors[i] = sum * NeuronValues[i] * (1.0 - NeuronValues[i]);
			}
		}

		public void CalculateNeuronValues()
		{
			double x = 0.0;
			for (int j = 0; j < NumberOfNeurons; j++)
			{
				x = 0.0;
				for (int i = 0; i < ParentLayer.NumberOfNeurons; i++)
				{
					x += ParentLayer.NeuronValues[i] * ParentLayer.SynapseWeights[i][j];
				}
				x += ParentLayer.NeuronBiasses[j] * ParentLayer.BiasWeights[j];

				NeuronValues[j] = ActivationFunctions.Sigmoid(x);
			}
		}
	}

	public class OutputLayer : Layer, IChildLayer
	{
		public IParentLayer ParentLayer { get; set; }

		public double[] DesiredNeuronValues { get; set; }

		public double[] Errors { get; set; }

		public void CalculateErrors()
		{
			for (int i = 0; i < NumberOfNeurons; i++)
			{
				Errors[i] = (DesiredNeuronValues[i] - NeuronValues[i]) * NeuronValues[i] * (1.0 - NeuronValues[i]);
			}
		}

		public void CalculateNeuronValues()
		{
			double x = 0.0;
			for (int j = 0; j < NumberOfNeurons; j++)
			{
				x = 0.0;
				for (int i = 0; i < ParentLayer.NumberOfNeurons; i++)
				{
					x += ParentLayer.NeuronValues[i] * ParentLayer.SynapseWeights[i][j];
				}
				x += ParentLayer.NeuronBiasses[j] * ParentLayer.BiasWeights[j];
				NeuronValues[j] = x;
			}
		}
	}
}
