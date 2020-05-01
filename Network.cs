﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetworkLib
{
    public class Network
    {
        private readonly List<Layer> layers = new List<Layer>();
        public List<double> Outputs { get; private set; }
        public int CountOfLayers { get; }
        public double Error { get; private set; } //Ошибка за последнюю эпоху
        public double LearningRate { get; }
        public double Momentum { get; }

        //Сеть создается начиная со скрытых слоев
        /// <summary>
        /// Cписок функций активации(классы с функциями FuncOfActivation() и Deriative())
        /// 1.Линейная функция с утечкой: LeakyLinearUnit
        /// 2.Логистическая функция: Sigmoid
        /// 3.Гиперболический тангенс: Tangh
        /// </summary>
        public Network(IFuncOfActivation func,double learningRate, double momentum, params int[] countOfNeironsOnLayers)
        {
            LearningRate = learningRate;
            Momentum = momentum;
            Outputs = new List<double>();
            layers.Add(new Layer(countOfNeironsOnLayers[0],countOfNeironsOnLayers[1],func)); //Нулевой слой - количество входных данных
            for (int i = 2; i<countOfNeironsOnLayers.Length;i++)
            {
                layers.Add(new Layer(layers[i-2],countOfNeironsOnLayers[i],func));
            }
            for (int i = 0; i < layers[layers.Count - 1].neirons.Count; i++) //Добавление выходных данны последнего слоя
            {
                Outputs.Add(0);
            }
            CountOfLayers = countOfNeironsOnLayers.Length;
        }

        public void GetOutputs(double[] inputs)
        {
            layers[0].GetOutputs(inputs); //Для первого слоя
            for (int i = 1; i < layers.Count;i++)
            {
                layers[i].GetOutputs(layers[i-1]); //Прогон по другим слоям
            }
            for (int i =0;i<layers[layers.Count-1].neirons.Count;i++)
            {
                Outputs[i] = layers[layers.Count - 1].neirons[i].Output; //Сохранение выводов нейросети
            }
        }
        public double Train(double[] inputs, double[] waitingOutcomes)
        {
            double error;
            //Получаем результаты сети
            GetOutputs(inputs);
            //Находим ошибку
            double sum = 0,count=0;
            for (int i = 0; i<Outputs.Count; i++)
            {
                sum += (waitingOutcomes[i] - Outputs[i]) * (waitingOutcomes[i] - Outputs[i]);
                count++;
            }
            error = sum / count;
            ///Сам метод обратного распространения
            //Нахождение дельт выходного слоя
            for (int i=0;i<layers[layers.Count-1].neirons.Count;i++)
            {
                layers[layers.Count-1].neirons[i].FindDelta(waitingOutcomes[i]);
            }

            //Нахождение дельт у последующих
            for (int i=0;i<layers.Count-1;i++)
            {
                for (int k = 0; k < layers[layers.Count-2-i].neirons.Count; k++)
                {
                    layers[layers.Count-2-i].neirons[k].FindDelta(layers[(layers.Count)-i-1],k);
                }
            }
            ///Корректировка весов нейронов
            ///Делаю в таком порядке, чтобы изменения весов не влияли на нахождение дельт
            //Корректировка нейронов входного слоя
            for (int i = 0; i < layers[0].neirons.Count; i++)
            {
                layers[0].neirons[i].ChangeWeight(LearningRate, Momentum, inputs);
            }
            //Корректировка нейронов остальных слоев
            for (int i = 1; i < layers.Count; i++)
            {
                for (int k = 0; k < layers[i].neirons.Count; k++)
                {
                    layers[i].neirons[k].ChangeWeight(LearningRate, Momentum, layers[i-1]);
                }
            }
            return error;
        }
        //Тренировка по эпохам
        public void TrainEpoch(int countOfEpochs, double[,][] trainSets)
        {
            if (trainSets.GetUpperBound(0) != 1) throw new Exception("Массив тренировочных сетов должен состоять только из двух строк");
            for (int i=0;i<countOfEpochs; i++)
            {
                double error=0;
                for (int k=0;k<trainSets.GetUpperBound(1)+1;k++) //Последовательно прогоняем все тренировочные сеты
                {
                    error += (Train(trainSets[1,k], trainSets[2, k]))* (Train(trainSets[1, k], trainSets[2, k]));
                }
                Error = error / trainSets.GetUpperBound(1) + 1;
            }
        }
        public void TrainEpoch(int countOfEpochs, double minError, double[,][] trainSets)
        {
            if (trainSets.GetUpperBound(0) != 1) throw new Exception("Массив тренировочных сетов должен состоять только из двух строк");
            for (int i = 0; i < countOfEpochs; i++)
            {
                double error = 0;
                for (int k = 0; k < trainSets.GetUpperBound(1) + 1; k++) //Последовательно прогоняем все тренировочные сеты
                {
                    error += (Train(trainSets[1, k], trainSets[2, k])) * (Train(trainSets[1, k], trainSets[2, k]));
                }
                Error = error / trainSets.GetUpperBound(1) + 1;
                if (minError >= Error) break;
            }
        }
    }
}
