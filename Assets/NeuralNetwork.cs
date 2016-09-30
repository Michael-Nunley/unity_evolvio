using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public class NeuralNet
{
	static Random r = new Random();

	public class Dendrite
	{
		double _weight;
		public double Weight
		{
			get { return _weight; }
			set { _weight = value; }
		}

		public Dendrite()
		{
			this.Weight = NeuralNet.r.NextDouble();
		}
	}

	public class Neuron
	{
		List<Dendrite> _dendrites = new List<Dendrite>();
		int _dendriteCount;
		double _bias;
		double _value;
		double _delta;

		public List<Dendrite> Dendrites
		{
			get { return _dendrites; }
			set { _dendrites = value; }
		}

		public double Bias
		{
			get { return _bias; }
			set { _bias = value; }
		}

		public double Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public double Delta
		{
			get { return _delta; }
			set { _delta = value; }
		}

		public int DendriteCount
		{
			get { return _dendrites.Count; }
		}

		public Neuron()
		{
			this.Bias = NeuralNet.r.NextDouble();
		}
	}

	public class Layer
	{
		List<Neuron> _neurons = new List<Neuron>();

		int _neuronCount;
		public List<Neuron> Neurons
		{
			get { return _neurons; }
			set { _neurons = value; }
		}

		public int NeuronCount
		{
			get { return _neurons.Count; }
		}

		public Layer(int neuronNum)
		{
			_neuronCount = neuronNum;
		}
	}

	public class NeuralNetwork
	{
		List<Layer> _layers = new List<Layer>();

		double _learningRate;
		public List<Layer> Layers
		{
			get { return _layers; }
			set { _layers = value; }
		}

		public double LearningRate
		{
			get { return _learningRate; }
			set { _learningRate = value; }
		}

		public int LayerCount
		{
			get { return _layers.Count; }
		}

		public NeuralNetwork(double LearningRate, List<int> nLayers)
		{
			if (nLayers.Count < 2)
				return;

			this.LearningRate = LearningRate;


			for (int ii = 0; ii <= nLayers.Count - 1; ii++)
			{
				Layer l = new Layer(nLayers[ii] - 1);
				this.Layers.Add(l);

				for (int jj = 0; jj <= nLayers[ii] - 1; jj++)
				{
					l.Neurons.Add(new Neuron());
				}

				foreach (Neuron n in l.Neurons)
				{
					if (ii == 0)
						n.Bias = 0;

					if (ii > 0)
					{
						for (int k = 0; k <= nLayers[ii-1] - 1; k++)
						{
							n.Dendrites.Add(new Dendrite());
						}
					}

				}

			}
		}

		public List<double> Execute(List<double> inputs)
		{
			if (inputs.Count != this.Layers[0].NeuronCount)
			{
				return null;
			}

			for (int ii = 0; ii <= this.LayerCount - 1; ii++)
			{
				Layer curLayer = this.Layers[ii];

				for (int jj = 0; jj <= curLayer.NeuronCount - 1; jj++)
				{
					Neuron curNeuron = curLayer.Neurons[jj];

					if (ii == 0)
					{
						curNeuron.Value = inputs[jj];
					}
					else
					{
						curNeuron.Value = 0;
						for (int k = 0; k <= this.Layers[ii - 1].NeuronCount - 1; k++)
						{
							curNeuron.Value = curNeuron.Value + this.Layers[ii - 1].Neurons[k].Value * curNeuron.Dendrites[k].Weight;
						}

						curNeuron.Value = Sigmoid(curNeuron.Value + curNeuron.Bias);
					}

				}
			}

			List<double> outputs = new List<double>();
			Layer la = this.Layers[this.LayerCount - 1];
			for (int ii = 0; ii <= la.NeuronCount - 1; ii++)
			{
				outputs.Add(la.Neurons[ii].Value);
			}

			return outputs;
		}

		private double Sigmoid(double Value)
		{
			return 1 / (1 + Math.Exp(Value * -1));
		}

		public override string ToString()
		{
			StringBuilder nstr = new StringBuilder();

			foreach (Layer l in this.Layers)
			{
				nstr.AppendLine("--+-- Layer");
				nstr.AppendLine("  |   Neurons: " + l.NeuronCount);


				foreach (Neuron n in l.Neurons)
				{
					nstr.AppendLine("  |--+-- Neuron");
					nstr.AppendLine("  |  |   Bias: " + n.Bias);
					nstr.AppendLine("  |  |   Delta: " + n.Delta);
					nstr.AppendLine("  |  |   Value: " + n.Value);
					nstr.AppendLine("  |  |   Dendrites: " + n.DendriteCount);

					foreach (Dendrite d in n.Dendrites)
					{
						nstr.AppendLine("  |  |--+-- Dendrite");
						nstr.AppendLine("  |  |  |   Weight: " + d.Weight);
					}

				}
			}

			nstr.Append("====== EONN ======");
			return nstr.ToString();
		}

		public bool Train(List<double> inputs, List<double> outputs)
		{
			if (inputs.Count != this.Layers[0].NeuronCount | outputs.Count != this.Layers[this.LayerCount - 1].NeuronCount)
			{
				return false;
			}

			Execute(inputs);

			for (int ii = 0; ii <= this.Layers[this.LayerCount - 1].NeuronCount - 1; ii++)
			{
				Neuron curNeuron = this.Layers[this.LayerCount - 1].Neurons[ii];

				curNeuron.Delta = curNeuron.Value * (1 - curNeuron.Value) * (outputs[ii] - curNeuron.Value);

				for (int jj = this.LayerCount - 2; jj >= 1; jj += -1)
				{
					for (int kk = 0; kk <= this.Layers[jj].NeuronCount - 1; kk++)
					{
						Neuron iNeuron = this.Layers[jj].Neurons[kk];

						iNeuron.Delta = iNeuron.Value * (1 - iNeuron.Value) * this.Layers[jj + 1].Neurons[ii].Dendrites[kk].Weight * this.Layers[jj + 1].Neurons[ii].Delta;
					}
				}
			}


			for (int ii = this.LayerCount - 1; ii >= 0; ii += -1)
			{
				for (int jj = 0; jj <= this.Layers[ii].NeuronCount - 1; jj++)
				{
					Neuron iNeuron = this.Layers[ii].Neurons[jj];
					iNeuron.Bias = iNeuron.Bias + (this.LearningRate * iNeuron.Delta);

					for (int kk = 0; kk <= iNeuron.DendriteCount - 1; kk++)
					{
						iNeuron.Dendrites[kk].Weight = iNeuron.Dendrites[kk].Weight + (this.LearningRate * this.Layers[ii - 1].Neurons[kk].Value * iNeuron.Delta);
					}
				}
			}

			return true;
		}
		/*
		public void Draw(Graphics hDC, int startX, int startY, int scale, int hspace, int vspace, Color iColor, Color hColor, Color oColor)
		{
			int i = 0;
			int k = 0;
			int j = 0;

			int x = 0;
			int y = 0;

			hDC.Clear(Color.White);
			for (i = 0; i <= this.LayerCount - 1; i++)
			{
				x = startX - hspace * (this.Layers(i).NeuronCount / 2);
				y = startY - (vspace * i);

				for (k = 0; k <= this.Layers(i).NeuronCount - 1; k++)
				{
					SolidBrush b = default(SolidBrush);
					Pen p = default(Pen);
					switch (i)
					{
						case 0:
							b = new SolidBrush(iColor);
							p = new Pen(iColor);
							hDC.DrawEllipse(p, x, y, scale, scale);
							hDC.FillEllipse(b, x, y, scale, scale);
							break;
						case this.LayerCount - 1:
							b = new SolidBrush(oColor);
							p = new Pen(oColor);
							hDC.DrawEllipse(p, x, y, scale, scale);
							hDC.FillEllipse(b, x, y, scale, scale);
							break;
						default:
							b = new SolidBrush(hColor);
							p = new Pen(hColor);
							hDC.DrawEllipse(p, x, y, scale, scale);
							hDC.FillEllipse(b, x, y, scale, scale);
							break;
					}
					b.Dispose();
					p.Dispose();

					if (i > 0)
					{
						int denX1 = x + (scale / 2);
						int denY1 = y + scale;
						int denX2 = (startX - hspace * (this.Layers(i - 1).NeuronCount / 2)) + (scale / 2);
						int denY2 = y + vspace;
						for (j = 0; j <= this.Layers(i).Neurons(k).DendriteCount - 1; j++)
						{
							hDC.DrawLine(Pens.Black, denX1, denY1, denX2, denY2);
							denX2 = denX2 + hspace;
						}
					}
					x = x + hspace;

				}
			}
		}
		*/
	}
}
