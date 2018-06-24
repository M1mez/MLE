using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.StaticFunctions
{
	public enum ActivationFunctions
	{
		BinaryStep,
		Sigmoid,
		SoftPlus,
		ReLU,
		SiLU,
		dSiLU
	}
	

	public static class Functions
    {
        private static readonly Random Rnd = new Random();
        public static double RandomBias() => Rnd.NextDouble() * 2 - 1;

		private static double ePow(double x) => Math.Pow(Math.E, x);

		public static ActivationFunctions functionSet = ActivationFunctions.Sigmoid;

		public static double ActivationFunction(double x)
		{
			switch(functionSet)
			{
				case ActivationFunctions.BinaryStep:
					return (x <= 0 ? 0.0d : 1.0d);
					break;
				case ActivationFunctions.Sigmoid:
					return (1 / (1 + ePow(-x)));
					break;
				case ActivationFunctions.SoftPlus:
					return (Math.Log(1 + ePow(x)));
					break;
				case ActivationFunctions.ReLU:
					return (x <= 0 ? 0.0d : x);
					break;
				case ActivationFunctions.SiLU:
					return (x * (1 / (1 + ePow(-x))));
					break;
				case ActivationFunctions.dSiLU:
					double sigmoid = (1 / (1 + ePow(-x)));
					double silu = (x * sigmoid);
					return (silu + sigmoid * (1 - silu));
					break;
				default:
					throw new System.ArgumentException();
			}
		}
	}
}
