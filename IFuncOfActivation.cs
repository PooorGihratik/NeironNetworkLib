using System;
using System.Collections.Generic;
using System.Text;

namespace NeironNetworkLib
{
    public interface IFuncOfActivation
    {
        double FuncOfActivation(double input);
        double Derivative(double output);
    }
}
