using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//NOTE: I'm working on an implementation of this so far into the demogame.  This is the first time i've written this in c# so
//      i won't commit the implementation yet until i know it works 100%.  aPhRo_

namespace NetGore.AI
{

    public class AINeuron
    {
        static readonly Random _rand = new Random();

        /// <summary>
        /// Enumerator for the activation function used.  Only Step and Sigmoid are currently implemented.
        /// </summary>
        public enum ActivationFunction
        {
            AFStep = 0,
            AFSigmoid = 1
        }

        private float[] _weights;
        private ActivationFunction _activation;

        /// <summary>
        /// Constructor for the Neuron.
        /// </summary>
        /// <param name="numWeights">Number of weights.</param>
        /// <param name="activation">What activation function is to be used with the neuron.</param>
        public AINeuron(int numWeights, ActivationFunction activation)
        {
            _weights = new float[numWeights - 1];
           
         for (int idx = 0; idx < _weights.Length; ++idx)
         { 
             _weights[idx] = Convert.ToSingle(_rand.NextDouble()); 
         }

         Activation = activation;
        }

        /// <summary>
        /// Used to change the activation function.
        /// </summary>
        public ActivationFunction Activation
        {
            get { return _activation;   }
            set { _activation = value;  }
        }
        
        /// <summary>
        /// The input weights and threshold value.
        /// </summary>
        public float[] Weights
        {
            get { return _weights;  }
            set { _weights = value; }
        }

        /// <summary>
        /// This is the most important function in a neuron.  It takes an array of floats and calculates the output.
        /// </summary>
        public float Output(float[] inputs)
        {
            float total = 0;
            int i = 0;
               
                //inputs should be 1 less than the number of weights since the last weight is the threshold for activation.
                for (int idx =0; idx < inputs.Length; ++idx)
                {
                    total += inputs[idx]*_weights[idx];
                    i = idx;
                }
                float threshold = _weights[i];

                switch(_activation)
                {
                    case ActivationFunction.AFStep:
                        if(threshold < total)
                        { return 1;}
                        else
                        { return 0;}
                
                    case ActivationFunction.AFSigmoid:
                        return Convert.ToSingle(1 / (1+Math.Exp((-total) / threshold)));
                    
                    default:                        //Just use AFStep.
                        if (threshold < total)
                        { return 1; }
                        else
                        { return 0; }
                }

        }



    }
}
