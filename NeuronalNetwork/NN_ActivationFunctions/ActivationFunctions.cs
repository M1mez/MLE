using System;

namespace NN_staticFunctions
{
    public static class ActivationFunctions
    {
        private static double ePow(double x) => Math.Pow(Math.E, x);
        static double Sigmoid(double x) => 1 / (1 + ePow(-x));
        static double HyperbolicTangent(double x) => (ePow(x) - ePow(-x)) / (ePow(x) + ePow(-x));
        static double Linear(double x) => x;
        static double Step(double x) => x <= 0 ? 0.0d : 1.0d;
    }
}
