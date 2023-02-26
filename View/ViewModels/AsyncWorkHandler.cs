using DispelTools.Common.DataProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using View.Components;

namespace View.ViewModels
{
    public class AsyncWorkHandler
    {
        private readonly BackgroundWorker worker;
        private readonly List<StageInfo> stages;
        private readonly Dictionary<UIElement, ElementState> elements;

        public AsyncWorkHandler()
        {
            stages = new();
            worker = new();
            WorkReporter = new(worker);
            elements = new();
            worker.DoWork += DoWork;
            worker.ProgressChanged += ProgressChanged;
            worker.RunWorkerCompleted += WorkCompleted;
        }

        private record StageInfo(string? Name, Action Action);
        private class ElementState
        {
            public bool State { get; set; }
            public ElementState(bool state)
            {
                State = state;
            }
        }

        public void AddStage(Action stageAction, string? name = null)
        {
            stages.Add(new StageInfo(name, stageAction));
        }

        public ProgressBarWithText? ProgressBar { private get; init; }
        public Details? Details { private get; init; }
        public WorkReporter WorkReporter { get; }

        public void DisableWhileWorking(params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                this.elements[element] = new(false);
            }
        }

        public void Resetprogress()
        {
            Details?.ClearDetails();
            if (ProgressBar is not null) ProgressBar.Value = 0;
        }

        public bool IsWorking => worker.IsBusy;

        public void Start()
        {
            foreach (var elementEntry in elements)
            {
                elementEntry.Value.State = elementEntry.Key.IsEnabled;
                elementEntry.Key.IsEnabled = false;
            }
            if(ProgressBar is not null) ProgressBar.Maximum = 1000;
            worker.RunWorkerAsync();
        }

        private void DoWork(object? sender, DoWorkEventArgs e)
        {
            string stageName = string.Empty;
            for (int i = 0; i < stages.Count; i++)
            {
                stageName = stages[i].Name ?? stageName;
                WorkReporter.StartNewStage(i + 1, stageName);
                stages[i].Action.Invoke();
            }
        }

        private void ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (ProgressBar is not null)
            {
                ProgressBar.Value = e.ProgressPercentage;
                if (e.UserState is string text)
                {
                    ProgressBar.Text = text;
                }
            }
            if (Details is not null && e.UserState is SimpleDetail detail)
            {
                Details.AddDetails(detail.Details);
            }
            if (e.UserState is WorkReporter.WorkerWarning warning)
            {
                MessageBox.Show(warning.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void WorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var elementEntry in elements)
            {
                elementEntry.Key.IsEnabled = elementEntry.Value.State;
            }
        }
    }
}
