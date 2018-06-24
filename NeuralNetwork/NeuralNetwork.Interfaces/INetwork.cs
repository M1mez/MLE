using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Interfaces 
{
	/// <summary>
	/// The network itself
	/// </summary>
	public interface INetwork<T>
	{
		double LearningRate { get; }
		double Momentum { get; }
		double GoalErrorRate { get; }
		
		void Learn(List<T> trainData, List<T> testData);
		int[,] Test(List<T> testData); //returns confusion Matrix

		void FeedForward();
		void BackPropagate();

		double CalculateError();
	}
}
