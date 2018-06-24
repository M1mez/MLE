using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Interfaces;
using NeuralNetwork.StaticFunctions;

namespace NeuralNetwork
{
	/* Class structure might be a bit ugly here.
	 * Since the program is not too difiicult,
	 * I reduced Object Orientation to a minimum
	 * to optimize speed a little bit.
	 * 
	 * According to a short internet reasearch,
	 * using pointers in unsafe sections to go through
	 * the for loops seems hardly more effective than
	 * using the .Length attribute. The compiler
	 * seems to do a pretty good optimization speed
	 * on its own.
	 */

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
			Synapse_WeightChange = new double[numberOfNeurons][];
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
					Synapse_WeightChange[i] = new double[_childLayer.NeuronValues.Length];
				}
			}
		}

		public double[][] SynapseWeights { get; set; }
		public double[][] Synapse_WeightChange { get; set; }

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
					SynapseWeights[i][j] += dw + momentum * Synapse_WeightChange[i][j];
					Synapse_WeightChange[i][j] = dw;
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
			NeuronValues_noActivation = new double[numberOfNeurons];

			SynapseWeights = new double[numberOfNeurons][];
			Synapse_WeightChange = new double[numberOfNeurons][];
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
					Synapse_WeightChange[i] = new double[_childLayer.NeuronValues.Length];
				}
			}
		}

		public IParentLayer ParentLayer { get; set; }

		public ActivationFunctions ActivationFunction { get; set; }

		public double[] NeuronValues_noActivation { get; set; } //needed for weight change calculation in SiLU/dSiLU

		public double[][] SynapseWeights { get; set; }
		public double[][] Synapse_WeightChange { get; set; }

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
					SynapseWeights[i][j] += dw + momentum * Synapse_WeightChange[i][j];
					Synapse_WeightChange[i][j] = dw;
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
				Errors[i] = sum * Functions.ActivationFunctionDerivative(NeuronValues[i], NeuronValues_noActivation[i], ActivationFunction);
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

				NeuronValues_noActivation[j] = x;
				NeuronValues[j] = Functions.ActivationFunction(x, ActivationFunction);
			}
		}
	}

	public class OutputLayer : Layer, IChildLayer
	{
		public OutputLayer(int numberOfNeurons)
		{
			NeuronValues = new double[numberOfNeurons];
			NeuronValues_noActivation = new double[numberOfNeurons];
			DesiredNeuronValues = new double[numberOfNeurons];

			Errors = new double[numberOfNeurons];
		}

		public IParentLayer ParentLayer { get; set; }

		public ActivationFunctions ActivationFunction { get; set; }

		public double[] NeuronValues_noActivation { get; set; } //needed for weight change calculation in SiLU/dSiLU

		public double[] DesiredNeuronValues { get; set; }

		public double[] Errors { get; set; }

		public void CalculateErrors()
		{
			for (int i = 0; i < Errors.Length; i++)
			{
				Errors[i] = (DesiredNeuronValues[i] - NeuronValues[i]) * Functions.ActivationFunctionDerivative(NeuronValues[i], NeuronValues_noActivation[i], ActivationFunction);
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

				NeuronValues_noActivation[j] = x;
				NeuronValues[j] = Functions.ActivationFunction(x, ActivationFunction);
			}
		}
	}
}
