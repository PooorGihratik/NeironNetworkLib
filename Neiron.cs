using System;
using System.Collections.Generic;
using System.Linq;

namespace NeironNetworkLib
{
    class Neiron
    {
        private double[] LastChangeWeight;
        private double[] WeightIn;
        /// <summary>
        /// Cписок функций активации(классы с функциями FuncOfActivation() и Deriative())
        /// 1.Линейная функция с утечкой: LeakyLinearUnit
        /// 2.Логистическая функция: Sigmoid
        /// 3.Гиперболический тангенс: Tangh
        /// </summary>
        private readonly IFuncOfActivation func;
        internal double Delta { get; private set; }
        internal double Output { get; private set; }

        internal Neiron(IFuncOfActivation func, int countOfNeironsOnPrevious)
        {
            if (countOfNeironsOnPrevious <= 0) throw new Exception("Нельзя создать нейрон без входящих связей!");
            this.func = func;
            Delta = 0;
            Random rand = new Random();
            LastChangeWeight = new double[countOfNeironsOnPrevious];
            WeightIn = new double[countOfNeironsOnPrevious];
            for (int i=0;i< countOfNeironsOnPrevious;i++)
            {
                LastChangeWeight[i] = 0;
                WeightIn[i] = rand.NextDouble();
            }
        }
        internal void FindOutput(double[] inputs) //Для первого скрытого слоя
        {
            double Sum = 0;
            for (int i = 0; i < inputs.Length; i++)
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
        internal void ChangeWeight(double learningRate, double momentum, double[] inputs) //Для первого скрытого слоя
        {
            for (int i=0;i<inputs.Length;i++)
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
