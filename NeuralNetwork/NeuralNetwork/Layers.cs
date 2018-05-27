using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Interfaces;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork
{
	public abstract class Layer : ILayer
	{
		public double[] NeuronValues { get; set; }
	}

	public class InputLayer : Layer, IParentLayer
	{
		private IChildLayer _childLayer;

		public InputLayer(int numberOfNeurons)
		{
			NeuronValues = new double[numberOfNeurons];

			SynapseWeights = new double[numberOfNeurons][];
			SynapseWeights_Before = new double[numberOfNeurons][];
			NeuronBiasses = new double[numberOfNeurons];
			BiasWeights = new double[numberOfNeurons];
		}

		public IChildLayer ChildLayer
		{
			get => _childLayer;
			set
			{
				_childLayer = value;
				for (int i = 0; i < NeuronValues.Length; i++)
				{
					SynapseWeights[i] = new double[_childLayer.NeuronValues.Length];
					SynapseWeights_Before[i] = new double[_childLayer.NeuronValues.Length];
				}
			}
		}

		public double[][] SynapseWeights { get; set; }
		public double[][] SynapseWeights_Before { get; set; }

		public double[] NeuronBiasses { get; set; }
		public double[] BiasWeights { get; set; }


		public void AdjustWeights(double learningRate, double momentum)
		{
			double dw = 0.0;
			for (int i = 0; i < NeuronValues.Length; i++)
			{
				for (int j = 0; j < ChildLayer.NeuronValues.Length; j++)
				{
					dw = learningRate * ChildLayer.Errors[j] * NeuronValues[i];
					SynapseWeights[i][j] += dw + momentum * SynapseWeights_Before[i][j];
					SynapseWeights_Before[i][j] = dw;
				}
			}
			for (int j = 0; j < ChildLayer.NeuronValues.Length; j++)
			{
				BiasWeights[j] += (learningRate * ChildLayer.Errors[j] * NeuronBiasses[j]);
			}
		}
	}

	public class HiddenLayer : Layer, IParentLayer, IChildLayer
	{
		private IChildLayer _childLayer;

		public HiddenLayer(int numberOfNeurons)
		{
			NeuronValues = new double[numberOfNeurons];

			SynapseWeights = new double[numberOfNeurons][];
			SynapseWeights_Before = new double[numberOfNeurons][];
			NeuronBiasses = new double[numberOfNeurons];
			BiasWeights = new double[numberOfNeurons];

			Errors = new double[numberOfNeurons];
		}

		public IChildLayer ChildLayer
		{
			get => _childLayer;
			set
			{
				_childLayer = value;
				for (int i = 0; i < NeuronValues.Length; i++)
				{
					SynapseWeights[i] = new double[_childLayer.NeuronValues.Length];
					SynapseWeights_Before[i] = new double[_childLayer.NeuronValues.Length];
				}
			}
		}

		public IParentLayer ParentLayer { get; set; }

		public double[][] SynapseWeights { get; set; }
		public double[][] SynapseWeights_Before { get; set; }

		public double[] NeuronBiasses { get; set; }
		public double[] BiasWeights { get; set; }
		public double[] Errors { get; set; }

		public void AdjustWeights(double learningRate, double momentum)
		{
			double dw = 0.0;
			for (int i = 0; i < NeuronValues.Length; i++)
			{
				for (int j = 0; j < ChildLayer.NeuronValues.Length; j++)
				{
					dw = learningRate * ChildLayer.Errors[j] * NeuronValues[i];
					SynapseWeights[i][j] += dw + momentum * SynapseWeights_Before[i][j];
					SynapseWeights_Before[i][j] = dw;
				}
			}
			for (int j = 0; j < ChildLayer.NeuronValues.Length; j++)
			{
				BiasWeights[j] += (learningRate * ChildLayer.Errors[j] * NeuronBiasses[j]);
			}
		}

		public void CalculateErrors()
		{
			double sum = 0.0;
			for (int i = 0; i < NeuronValues.Length; i++)
			{
				sum = 0.0;
				for (int j = 0; j < ChildLayer.NeuronValues.Length; j++)
				{
					sum += ChildLayer.Errors[j] * SynapseWeights[i][j];
				}
				Errors[i] = sum * NeuronValues[i] * (1.0 - NeuronValues[i]);
			}
		}

		public void CalculateNeuronValues()
		{
			double x = 0.0;
			for (int j = 0; j < NeuronValues.Length; j++)
			{
				x = 0.0;
				for (int i = 0; i < ParentLayer.NeuronValues.Length; i++)
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
		public OutputLayer(int numberOfNeurons)
		{
			NeuronValues = new double[numberOfNeurons];
			DesiredNeuronValues = new double[numberOfNeurons];

			Errors = new double[numberOfNeurons];
		}

		public IParentLayer ParentLayer { get; set; }

		public double[] DesiredNeuronValues { get; set; }

		public double[] Errors { get; set; }

		public void CalculateErrors()
		{
			for (int i = 0; i < Errors.Length; i++)
			{
				Errors[i] = (DesiredNeuronValues[i] - NeuronValues[i]) * NeuronValues[i] * (1.0 - NeuronValues[i]);
			}
		}

		public void CalculateNeuronValues()
		{
			double x = 0.0;
			for (int j = 0; j < NeuronValues.Length; j++)
			{
				x = 0.0;
				for (int i = 0; i < ParentLayer.NeuronValues.Length; i++)
				{
					x += ParentLayer.NeuronValues[i] * ParentLayer.SynapseWeights[i][j];
				}
				x += ParentLayer.NeuronBiasses[j] * ParentLayer.BiasWeights[j];

				NeuronValues[j] = ActivationFunctions.Sigmoid(x);
			}
		}
	}
}
