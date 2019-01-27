using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DependencyScanner.Core.Gui.Interfaces
{
    public interface IWindowAccess
    {
        Window MainWindow { get; }
    }
}
