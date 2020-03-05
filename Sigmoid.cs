using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetworkLib
{
    public class Sigmoid : IFuncOfActivation
    {
        public double Derivative(double output)
        {
            return (1 - output) * output;
        }

        public double FuncOfActivation(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }
    }
}
