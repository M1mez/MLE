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
		ILayer InputLayer { get; set; }
		ILayer HiddenLayer { get; set; }
		ILayer OutputLayer { get; set; }
		
		void FeedForward();

		void BackPropagate();

		void CalculateError();
	}
}
