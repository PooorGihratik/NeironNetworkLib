using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetworkLib
{
    class LeakyLinearUnit : IFuncOfActivation
    {
        public double FuncOfActivation(double input)
        {
            if (input > 0) return input;
            else return 0.01 * input;
        }

        public double Derivative(double output)
        {
            if (output >= 0) return 1;
            else return 0.01;
        }
    }
}
