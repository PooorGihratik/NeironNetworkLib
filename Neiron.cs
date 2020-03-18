using System.Collections.Generic;

namespace NeironNetworkLib
{
    class Neiron
    {
        internal List<double> LastChangeWeight = new List<double>();
        internal List<double> WeightIn = new List<double>();
        private readonly IFuncOfActivation func;
        internal double Delta { get; private set; }
        internal double Output { get; private set; }

        internal Neiron(IFuncOfActivation func)
        {
            this.func = func;
            Delta = 0;
        }
        internal void FindOutput(List<double> inputs) //Для первого скрытого слоя
        {
            double Sum = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                Sum += inputs[i] * WeightIn[i];
            }
            Output = func.FuncOfActivation(Sum);
        }
        internal void FindOutput(Layer lastLayer) //Для последующих слоев
        {
            double Sum=0;
            for (int i=0;i<lastLayer.neirons.Count;i++)
            {
                Sum += lastLayer.neirons[i].Output * WeightIn[i];
            }
            Output = func.FuncOfActivation(Sum);
        }

        internal void FindDelta(double outIdeal)  //Для выходного слоя
        {
            Delta = (outIdeal - Output) * func.Derivative(Output);
        }
        internal void FindDelta(Layer nextLayer, int pos) //Для остальных слоев (pos - позиция в слое)
        {
            Delta = 0;
            for (int i=0;i<nextLayer.neirons.Count;i++)
            {
                Delta += nextLayer.neirons[i].WeightIn[pos] * nextLayer.neirons[i].Delta;
            }
            Delta *= func.Derivative(Output);
        }
        internal void ChangeWeight(double learningRate, double momentum, List<double> inputs) //Для первого скрытого слоя
        {
            for (int i=0;i<inputs.Count;i++)
            {
                double grad = inputs[i] * Delta;
                LastChangeWeight[i] = grad * learningRate + LastChangeWeight[i] * momentum;
                WeightIn[i] += LastChangeWeight[i];
            }
        }
        internal void ChangeWeight(double learningRate, double momentum, Layer lastLayer) //Для остальных слоев
        {
            for (int i = 0; i < lastLayer.neirons.Count; i++)
            {
                double grad = lastLayer.neirons[i].Output * Delta;
                LastChangeWeight[i] = grad * learningRate + LastChangeWeight[i] * momentum;
                WeightIn[i] += LastChangeWeight[i];
            }
        }
    }
}
