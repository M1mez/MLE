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
	public interface INetwork
	{
		int NrInputNeurons { get; }
		int NrHiddenNeurons { get; }
		int NrOutputNeurons { get; }

		double LearningRate { get; }
		double Momentum { get; }
		double GoalErrorRate { get; }

		void FeedForward();

		void BackPropagate();

		double CalculateError();
	}
}
