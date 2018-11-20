﻿/*
 * Description:使用ReLU函数的激活层
 * Author:YunXiao An
 * Date:2018.11.19
 */

namespace MLStudy.Deep
{
    /// <summary>
    /// 使用ReLU函数的激活层
    /// </summary>
    public sealed class ReLU : Activations.Activation
    {
        /// <summary>
        /// 运行前的准备，用于初始化所有Tensor的结构
        /// </summary>
        /// <param name="input">输入Tensor</param>
        /// <returns></returns>
        public override Tensor PrepareTrain(Tensor input)
        {
            ForwardOutput = input.GetSameShape();
            BackwardOutput = input.GetSameShape();
            Derivative = input.GetSameShape();
            return ForwardOutput;
        }

        /// <summary>
        /// 正向传播或叫向后传播
        /// </summary>
        /// <param name="input">输入的数值</param>
        /// <returns>输出的数值</returns>
        public override Tensor Forward(Tensor input)
        {
            Tensor.Apply(input, ForwardOutput, Functions.ReLU);
            return ForwardOutput;
        }

        /// <summary>
        /// 反向传播或叫向前传播
        /// </summary>
        /// <param name="error">传回来的误差</param>
        /// <returns>传到前面的误差</returns>
        public override Tensor Backward(Tensor error)
        {
            Tensor.Apply(ForwardOutput, Derivative, Derivatives.ReLU);
            Tensor.MultipleElementWise(Derivative, error, BackwardOutput);
            return BackwardOutput;
        }
    }
}
