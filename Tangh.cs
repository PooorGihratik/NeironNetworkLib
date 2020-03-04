using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetwork2
{
    public class Tangh : IFuncOfActivation
    {
        public double FuncOfActivation(double input)
        {
            return (Math.Exp(2 * input) - 1) / (Math.Exp(2 * input) + 1);
        }

        public double Derivative(double output)
        {
            return 1 - output * output ;
        }
    }
}
