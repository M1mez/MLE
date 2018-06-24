using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.StaticFunctions
{
	public enum ActivationFunctions
	{
		Sigmoid,
		SoftMax,
		SiLU,
		dSiLU
	}

	public static class Functions
	{
		private static readonly Random Rnd = new Random();
		public static double RandomBias() => Rnd.NextDouble() * 2 - 1; //Random Bias between -1 and 1.

		private static double ePow(double x) => Math.Pow(Math.E, x);

		public static double ActivationFunction(double x, ActivationFunctions functionSet)
		{
			switch (functionSet)
			{
				case ActivationFunctions.Sigmoid:
					return (1 / (1 + ePow(-x)));
					break;
				case ActivationFunctions.SiLU: //(e^x (x + e^x + 1))/(e^x + 1)^2
					return (x * ActivationFunction(x, ActivationFunctions.Sigmoid));
					break;
				case ActivationFunctions.dSiLU: //(e^x (-e^x (x - 2) + x + 2))/(1 + e^x)^3
					double silu = ActivationFunction(x, ActivationFunctions.SiLU);
					double sigmoid = ActivationFunction(x, ActivationFunctions.Sigmoid);
					return (silu + sigmoid * (1 - silu));
					break;
				default:
					throw new System.ArgumentException();
			}
		}

		//x_noAF is the value of the neuron AFTER it was put into an activation function in a feedforward process
		//x_noAF is the value of the neuron BEFORE it was put into an activation function in a feedforward process
		public static double ActivationFunctionDerivative(double x, double x_noAF, ActivationFunctions functionSet)
		{
			switch (functionSet)
			{
				case ActivationFunctions.Sigmoid:
					return (ePow(-x_noAF)/ Math.Pow((1 + ePow(-x_noAF)), 2));
					break;
				case ActivationFunctions.SiLU:
					return (x + ActivationFunction(x_noAF, ActivationFunctions.Sigmoid) * (1 - x));
					break;
				case ActivationFunctions.dSiLU: //(e^x (e^(2 x) (x - 3) - 4 e^x x + x + 3))/(1 + e^x)^4
					double sigmoid = ActivationFunction(x_noAF, ActivationFunctions.Sigmoid);
					return ((sigmoid) * (1 - sigmoid) * (2 + x_noAF * (1 - sigmoid)) - x_noAF * sigmoid);
					break;
				default:
					throw new System.ArgumentException();
			}
		}
	}
}
