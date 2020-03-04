using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetwork2
{
    class Layer
    {
        internal List<Neiron> neirons = new List<Neiron>();
        public Layer(int countOfInputs, IFuncOfActivation func) //Для первого скрытого слоя
        {
            for (int i = 0; i < countOfInputs; i++)
            {
                neirons.Add(new Neiron(func));
                
                //Добавление весов в соответсвии с количеством входных данных
                for (int k = 0; k < countOfInputs; k++)
                {
                    neirons[i].LastChangeWeight.Add(0);
                    neirons[i].WeightIn.Add(0);
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
                    neirons[i].LastChangeWeight.Add(0);
                    neirons[i].WeightIn.Add(0);
                }
            }
        }
        internal void GetOutputs(List<double> inputs) //Для первого скрытого слоя
        {
            if (inputs.Count != neirons.Count) throw new Exception();
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
