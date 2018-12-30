using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone.Services
{
    /// <summary>
    /// Observable Progress decorator
    /// </summary>
    public class ObservableProgress : ObservableObject, ICancelableProgress<ProgressMessage>, IService
    {
        private ICancelableProgress<ProgressMessage> _progress;

        private double _progressValue;
        public double Progress { get => _progressValue; set => Set(ref _progressValue, value); }

        private string _progressMessage = "";
        public string ProgressMessage { get => _progressMessage; set => Set(ref _progressMessage, value); }

        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        private bool _isMarquee;

        public bool IsMarquee
        {
            get { return _isMarquee; }
            set { Set(ref _isMarquee, value); }
        }

        public CancellationToken Token { get; set; }

        public void RegisterProgress(ICancelableProgress<ProgressMessage> progress)
        {
            _progress = progress;

            Token = progress.Token;
        }

        public void StartProgress()
        {
            IsOpen = true;
        }

        public void StopProgress()
        {
            IsOpen = false;

            ProgressMessage = "";

            Progress = 0D;
        }

        public void Report(ProgressMessage value)
        {
            _progress?.Report(value);

            IsMarquee = value.Value == 0;
            Progress = value.Value;
            ProgressMessage = value.Message;
        }
    }
}
