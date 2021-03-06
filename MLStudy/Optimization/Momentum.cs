﻿/*
 * Description: Momentum optimizer.
 *                Based on this paper:http://proceedings.mlr.press/v28/sutskever13.pdf
 * Author:YunXiao An
 * Date:2018.11.27
 */


using MLStudy.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLStudy.Optimization
{
    public class Momentum : IOptimizer
    {
        public double LearningRate { get; private set; }
        public double Moment { get; private set; }

        private Dictionary<TensorOld, TensorOld> last = new Dictionary<TensorOld, TensorOld>();

        public Momentum(double learningRate, double momentum = 0.9)
        {
            LearningRate = learningRate;
            Moment = momentum;
        }

        public void Optimize(TensorOld target, TensorOld gradient)
        {
            if (!last.ContainsKey(gradient))
            {
                last[gradient] = gradient.GetSameShape();
                TensorOld.Apply(gradient, last[gradient], g => LearningRate * g);
                target.Minus(last[gradient]);
                return;
            }

            var prev = last[gradient];
            TensorOld.Apply(prev, gradient, prev, (p, g) => g * LearningRate - p * Moment);
            target.Minus(prev);
        }
    }
}
