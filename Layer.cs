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
            if (countOfNeirons <= 0) throw new Exception("Нельзя создать слой без нейронов!");
            for (int i = 0; i < countOfNeirons; i++)
            {
                neirons.Add(new Neiron(func, countOfInputs));
            }
        }
        public Layer(Layer lastLayer,int countOfNeirons, IFuncOfActivation func) //Для последующих
        {
            if (countOfNeirons <= 0) throw new Exception("Нельзя создать слой без нейронов!");
            for (int i=0;i<countOfNeirons;i++)
            {
                neirons.Add(new Neiron(func, lastLayer.neirons.Count));
            }
        }
        internal void GetOutputs(double[] inputs) //Для первого скрытого слоя
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
