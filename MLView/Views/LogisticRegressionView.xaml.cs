﻿using MLStudy;
using MLView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MLView.Views
{
    /// <summary>
    /// LogisticRegressionView.xaml 的交互逻辑
    /// </summary>
    public partial class LogisticRegressionView : UserControl
    {
        LogisticRegression lr;
        Trainer trainer;
        DataEmulator emu = new DataEmulator();
        Matrix trainX, testX;
        Vector trainY, testY;

        DataConfig dataConfig = new DataConfig { TrainSize = 200, TestSize = 50 };
        TrainerConfig trainerConfig = new TrainerConfig();
        LinearConfig lrConfig = new LinearConfig { LearningRate = 0.1 };

        public LogisticRegressionView()
        {
            InitializeComponent();

            TrainerConfig.Config = trainerConfig;
            DataConfig.Config = dataConfig;
            LinearRegConfig.Config = lrConfig;
            lr = new LogisticRegression();
            trainer = new Trainer(lr);
            TrainerControl.Trainer = trainer;

            trainer.BeforeStart += Trainer_BeforeStart;
            trainer.Started += Trainer_Started;
            trainer.Stopped += Trainer_Stopped;
            trainer.Notify += Trainer_Notify;
            trainer.Paused += Trainer_Paused;
            trainer.Continued += Trainer_Continued;
        }


        private void Trainer_BeforeStart(object sender, EventArgs e)
        {
            lr.SetWeights(0,0);
            lr.SetBias(1);

            Dispatcher.Invoke(() =>
            {
                lrConfig.SetToModel(lr);
                trainerConfig.SetToTrainer(trainer);
                (trainX, trainY, testX, testY) = dataConfig.GetClassificationData(2, m =>
                {
                    return m.GetColumn(1) - (m.GetColumn(0) * 3 + 5);
                });

                trainer.SetTrainData(trainX, trainY);
            });
        }

        private void Trainer_Continued(object sender, EventArgs e)
        {
            TextOutCross($"Continued!");
        }

        private void Trainer_Paused(object sender, NotifyEventArgs e)
        {
            TextOutCross($"Paused!");
            OutputInfo(e);
        }

        private void Trainer_Notify(object sender, NotifyEventArgs e)
        {
            OutputInfo(e);
        }

        private void Trainer_Stopped(object sender, NotifyEventArgs e)
        {
            TextOutCross($"Stopped!{trainer.State}!");
            OutputInfo(e);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LogText.Clear();
        }

        private void Trainer_Started(object sender, EventArgs e)
        {
            TextOutCross($"Started!");
        }

        private void TextOutCross(string text)
        {
            Dispatcher.Invoke(() =>
            {
                LogText.Text += text + "\n";
                LogText.ScrollToEnd();
            });
        }

        private void OutputInfo(NotifyEventArgs e)
        {
            var error = e.Machine.Error(lr.Predict(e.X), e.Y);
            TextOutCross($"Step:{e.Step} Weight:{lr.Weights}, Bias:{lr.Bias} Error:{error}");
        }
    }
}