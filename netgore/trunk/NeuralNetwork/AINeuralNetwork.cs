using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

//NOTE: I'm working on an implementation of this so far into the demogame.  This is the first time i've written this in c# so
//      i won't commit the implementation yet until i know it works 100%.  aPhRo_

namespace NetGore.AI
{
    /// <summary>
    /// AINeuralNetwork encapsulates the neural network.
    /// </summary>
    class AINeuralNetwork
    {
        private ArrayList _layers;
        private AINeuronLayer _inputLayer;
        private AINeuronLayer _outputLayer;

        /// <summary>
        /// Creates the neural network.
        /// </summary>
        /// <param name="Inputs">The number of neurons in the input layer.</param>
        /// <param name="Outputs">The number of neurons in the output layer.</param>
        /// <param name="HiddenLayers">The number of hidden layers in the neural netwrok. Must be 0>=</param>
        /// <param name="NeuronsPerHiddenLayer">Number of neurons in each hidden layer.</param>
        /// <param name="InputWeightSize">The number of weights in the input layer.
        /// The weights size for each layer above is the number of neurons in the previous layer.</param>
        /// <param name="Activation">The ActivationFunction to use in the neuron.</param>

        public AINeuralNetwork(int Inputs, int Outputs, int HiddenLayers, int NeuronsPerHiddenLayer, int InputWeightSize, AINeuron.ActivationFunction Activation)
        {
            _inputLayer = new AINeuronLayer(Inputs, InputWeightSize + 1, Activation);
            _layers = new ArrayList();

            int lastLayerSize = Inputs;

            for (int idx = 0; idx < HiddenLayers; ++idx)
            {
            _layers.Add(new AINeuronLayer(NeuronsPerHiddenLayer, lastLayerSize+1, Activation));
            lastLayerSize = NeuronsPerHiddenLayer;
            }

            _outputLayer = new AINeuronLayer(Outputs, lastLayerSize + 1, Activation);

        }

        /// <summary>
        /// Returns the total amount of weights in the neural network by adding up the number that 
        /// is returned from each layer.
        /// </summary>
        public int WeightCount
        {
            get 
            {
                int cnt = _inputLayer.NumWeights;

                foreach (AINeuronLayer n1 in _layers)
                {
                    cnt += n1.NumWeights;
                }
                cnt += _outputLayer.NumWeights;
                
                return cnt;
            }
        }

        /// <summary>
        /// Gets output of the neural network.
        /// </summary>
        /// <param name="Inputs">Inputs passed to neural network.</param>
        /// <returns>Output of neural network.</returns>
        public Single[] Outputs(Single[] Inputs)
        {
            Single[] lastOutputs = _inputLayer.Outputs(Inputs);

            foreach (AINeuronLayer n1 in _layers)
            {
                lastOutputs = n1.Outputs(lastOutputs);
            }

            lastOutputs = _outputLayer.Outputs(lastOutputs);
            return lastOutputs;        
        }

        /// <summary>
        /// Accessor method to get and assign weights to the whole neural network.
        /// The weights also include the threshold values so that it is easier to evolve the network with a genetic algorithm.
        /// </summary>
        public Single[] Weights
        {
            get
            {
                Single[] retWeights;
                retWeights = new Single[WeightCount - 1];

                int retIdx = 0;
                Single[] layerWeights;

                layerWeights = _inputLayer.Weights;

                for (int idx = 0; idx < layerWeights.Length; ++idx)
                {
                    retWeights[retIdx] = layerWeights[idx];
                    retIdx +=1;
                }

                foreach (AINeuronLayer n1 in _layers)
                {
                    layerWeights = n1.Weights;

                    for (int idx = 0; idx < layerWeights.Length; ++idx)
                    {
                        retWeights[retIdx] = layerWeights[idx];
                    }
                }

                return retWeights;            
            }

            set 
            {
                Single[] layerWeights;

                for (int vIdx = 0; vIdx < value.Length; ++vIdx)
                {
                    layerWeights = new Single[_inputLayer.NumWeights - 1];

                    for (int layerIdx = 0; layerIdx < layerWeights.Length; ++layerIdx)
                    {
                        layerWeights[layerIdx] = value[vIdx];
                        vIdx += 1;
                    }
                    _inputLayer.Weights = layerWeights;

                    foreach (AINeuronLayer n1 in _layers)
                    {
                        Array.Resize<Single>(ref layerWeights, n1.NumWeights - 1);

                        for (int layerIdx = 0; layerIdx < layerWeights.Length; ++layerIdx)
                        {
                            layerWeights[layerIdx] = value[vIdx];
                            vIdx += 1;                        
                        }
                        n1.Weights = layerWeights;
                    }

                    Array.Resize<Single>(ref layerWeights, _outputLayer.NumWeights - 1);

                    for (int layerIdx = 0; layerIdx < layerWeights.Length; ++layerIdx)
                    {
                        layerWeights[layerIdx] = value[vIdx];
                        vIdx += 1;
                    }

                    _outputLayer.Weights = layerWeights;
                }
            }
        }


    }
}
