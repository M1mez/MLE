using System;

namespace NeuralNetwork.StaticFunctions
{
    public static class ActivationFunctions
    {
        private static double ePow(double x) => Math.Pow(Math.E, x);
		public static double Sigmoid(double x) => 1 / (1 + ePow(-x));
		public static double HyperbolicTangent(double x) => (ePow(x) - ePow(-x)) / (ePow(x) + ePow(-x));
		public static double Linear(double x) => x;
		public static double Step(double x) => x <= 0 ? 0.0d : 1.0d;
    }
}
