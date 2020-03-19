using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetworkLib
{
    class Layer
    {
        internal List<Neiron> neirons = new List<Neiron>();
        public Layer(int countOfInputs,int countOfNeirons, IFuncOfActivation func) //Для первого скрытого слоя
        {
            for (int i = 0; i < countOfNeirons; i++)
            {
                neirons.Add(new Neiron(func));
                
                //Добавление весов в соответствии с количеством входных данных
                for (int k = 0; k < countOfInputs; k++)
                {
                    Random rand = new Random();
                    neirons[i].LastChangeWeight.Add(0);
                    neirons[i].WeightIn.Add(rand.NextDouble());
                }
            }
        }
        public Layer(Layer lastLayer,int countOfNeirons, IFuncOfActivation func) //Для последующих
        {
            for (int i=0;i<countOfNeirons;i++)
            {
                neirons.Add(new Neiron(func));

                //Добавление весов в соответсвии с количеством нейронов на прошлом слое
                for (int k = 0; k < lastLayer.neirons.Count; k++)
                {
                    Random rand = new Random();
                    neirons[i].LastChangeWeight.Add(0);
                    neirons[i].WeightIn.Add(rand.NextDouble());
                }
            }
        }
        internal void GetOutputs(List<double> inputs) //Для первого скрытого слоя
        {
            for (int i = 0; i < neirons.Count; i++)
            {
                neirons[i].FindOutput(inputs);
            }
        }
        internal void GetOutputs(Layer lastLayer) //Для последующих слоев
        {
            for (int i = 0; i < neirons.Count; i++)
            {
                neirons[i].FindOutput(lastLayer);
            }
        }
    }
}
