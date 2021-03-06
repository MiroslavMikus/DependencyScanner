﻿using DependencyScanner.Api.Services;
using GalaSoft.MvvmLight;
using System;

namespace DependencyScanner.Core.Gui.ViewModel
{
    public class ObservableProgressBase : ObservableObject, IProgress<ProgressMessage>
    {
        private double _progressValue;
        public double ProgressValue { get => _progressValue; set => Set(ref _progressValue, value); }

        private string _progressMessage;
        public string ProgressMessage { get => _progressMessage; private set => Set(ref _progressMessage, value); }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { Set(ref _isRunning, value); }
        }

        private bool _isMarquee;

        public bool IsMarquee
        {
            get { return _isMarquee; }
            set { Set(ref _isMarquee, value); }
        }

        public void StartProgress()
        {
            ProgressValue = 0D;
            IsRunning = true;
        }

        public void StopProgress()
        {
            IsRunning = false;

            ProgressValue = 0D;
        }

        public void Report(ProgressMessage value)
        {
            IsMarquee = value.Value == 0;
            ProgressValue = value.Value;
            ProgressMessage = value.Message;
        }

        protected double CalculateProgress(int current, int all) => Math.Round(current / (all / 100D), 2);
    }
}
