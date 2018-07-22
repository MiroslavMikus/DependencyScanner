using DependencyScanner.ViewModel.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone.Converters
{
    public class NullToBoolConverter : AbstractNullConverter<bool>
    {
        public override bool Positive { get; set; } = true;
        public override bool Negative { get; set; } = false;
    }
}
