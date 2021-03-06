﻿using NuGet;
using System.Collections.Generic;
using System.Diagnostics;

namespace DependencyScanner.Core.Model
{
    [DebuggerDisplay("{Id}")]
    public class ConsolidateSolution
    {
        public string Id { get; set; }
        public Dictionary<SolutionResult, SemanticVersion> References { get; set; }
    }
}
