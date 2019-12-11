using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example2
{
    public interface IData
    {
        void RegisterDisplay(IDisplay display);
        void UnRegisterDisplay(IDisplay display);
        void NotifyDisplay();
    }
}
