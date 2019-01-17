using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace DependencyScanner.Api.Model
{
    public class Solution
    {
        public ICollection<Project> Projects { get; set; }
    }

    public class Project
    {
        public ICollection<ProjectReference> ProjectReferences { get; set; }
    }

    public class ProjectReference
    {
    }
}
