using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//NOTE: I'm working on an implementation of this so far into the demogame.  This is the first time i've written this in c# so
//      i won't commit any implementation yet until i know it works 100%.  aPhRo_

namespace NetGore.AI
{
    /// <summary>
    /// AINeuronLayer organises the neurons into layers.  It totals the weights and output properties of
    /// the neuron
    /// </summary>
    public class AINeuronLayer : System.Collections.CollectionBase
    {
        readonly private int _weightsPerNeuron;

        //Constructor

        public AINeuronLayer(int Neurons, int WeightsPerNeuron, AINeuron.ActivationFunction ActivationFunction)
        {
            for (int idx = 0; idx < Neurons; ++idx)
            {
                AINeuron n = new AINeuron(WeightsPerNeuron, ActivationFunction);
                List.Add(n);
            }

            _weightsPerNeuron = WeightsPerNeuron;
        }

        public int NumWeights
        {
            get { return _weightsPerNeuron * List.Count; }
        }

        public int WeightsPerNeuron
        {
            get { return _weightsPerNeuron; }
        }

        public float[] Weights
        {
            get
            {
                float[] ret = new float[(_weightsPerNeuron * List.Count) - 1];

                int idx = 0;

                foreach (AINeuron n in List)
                {
                    float[] tmpw = n.Weights;

                    for (int idx2 = 0; idx2 < tmpw.Length; ++idx2)
                    {
                        ret[idx] = tmpw[idx2];
                        idx += 1;
                    }
                }
                return ret;
            }
            set
            {
                int idx = 0;

                foreach (AINeuron n in List)
                {
                    float[] tmpw = new float[_weightsPerNeuron - 1];

                    for (int idx2 = 0; idx2 < tmpw.Length; ++idx2)
                    {
                        tmpw[idx2] = value[idx];
                        idx += 1;
                    }
                    n.Weights = tmpw;
                }
            }
        }

        public float[] Outputs(float[] Inputs)
        {
            int outidx = 0;

            float[] tmpout = new float[List.Count - 1];

            foreach (AINeuron n in List)
            {
                tmpout[outidx] = n.Output(Inputs);
                outidx += 1;
            }

            return tmpout;
        }
    }
}
