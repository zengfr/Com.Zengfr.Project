using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Math;

namespace ConsoleApplication3
{
    public class NeuralNetworks
    {
        public static void Main(string[] args)
        {
            aa();
            Console.ReadLine(); 
        }
        
        private static void aa()
        {
            int num = 16;
            // 整理输入输出数据
double[][] input = new double[num][]; double[][] output = new double[num][];
            //input[0] = new double[] { 0, 0 }; output[0] = new double[] { 0 };
            //input[1] = new double[] { 0, 1 }; output[1] = new double[] { 0 };
            //input[2] = new double[] { 1, 0 }; output[2] = new double[] { 0 };
            //input[3] = new double[] { 1, 1 }; output[3] = new double[] { 1 };

            input[0] = new double[] { 1, 4 }; output[0] = new double[] { 5 };
            input[1] = new double[] { 2, 5 }; output[1] = new double[] {12 };
            input[2] = new double[] { 3, 6 }; output[2] = new double[] { 21 };
            input[3] = new double[] { 8, 11 }; output[3] = new double[] { 96 };

            input[4] = new double[] { 1, 1 }; output[4] = new double[] { 2 };
            input[5] = new double[] { 1, 2 }; output[5] = new double[] { 3 };
            input[6] = new double[] { 1, 3 }; output[6] = new double[] { 4 };
            input[7] = new double[] { 1, 5 }; output[7] = new double[] { 6 };


            input[8] = new double[] { 2, 1 }; output[8] = new double[] { 4 };
            input[9] = new double[] { 2, 2 }; output[9] = new double[] { 6 };
            input[10] = new double[] { 2, 3 }; output[10] = new double[] { 8 };
            input[11] = new double[] { 2,4 }; output[11] = new double[] { 10 };


            input[12] = new double[] { 3, 1 }; output[12] = new double[] { 6 };
            input[13] = new double[] { 3, 2 }; output[13] = new double[] { 9 };
            input[14] = new double[] { 3, 3 }; output[14] = new double[] { 12 };
            input[15] = new double[] { 3, 4 }; output[15] = new double[] { 15 };

            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("input{0}:  ===>  {1},{2}  output{0}:  ===>  {3}", i, input[i][0], input[i][1], output[i][0]);
            }
            INormalizationMethod normalization;
            normalization = new NormalizationMinMaxMethod(0.1,0.9);
            //normalization = new NormalizationLogMethod();

            normalization.InitParams(input);
            normalization.InitParams(output);

            input =normalization.Scale(input);
            output = normalization.Scale(output);
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("input{0}:  ===>  {1},{2}  output{0}:  ===>  {3}", i, input[i][0], input[i][1], output[i][0]);
            }

            //建立网络，层数1，输入2，输出1，激励函数阈函数 
            // ActivationNetwork network = new ActivationNetwork(new ThresholdFunction(), 2, 1);
            ActivationNetwork network = new ActivationNetwork(new LinearFunction(new DoubleRange(0,1)), 2, 1);
            
               //学习方法为感知器学习算法 
               //PerceptronLearning teacher = new PerceptronLearning(network);
               BackPropagationLearning teacher = new BackPropagationLearning(network);
            //定义绝对误差 
            double error = 1.0;
            Console.WriteLine();
            Console.WriteLine("learning error  ===>  {0}", error);

            //输出学习速率 
            Console.WriteLine(); 
            Console.WriteLine("learning rate ===>  {0}", teacher.LearningRate);

            //迭代次数 
            int iterations = 0;
            Console.WriteLine();
            while (error > 0.0099)
            {
                error = teacher.RunEpoch(input, output);
                Console.WriteLine("learning error  ===>  {0},{1}", error, iterations);
                iterations++;
            }
            Console.WriteLine("iterations  ===>  {0}", iterations);
            Console.WriteLine();
            Console.WriteLine("sim:");

            //模拟 
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("input{0}:  ===>  {1},{2}  sim{0}:  ===>  {3}", i, input[i][0], input[i][1], network.Compute(input[i])[0]);
            }

            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("input{0}:  ===>  {1},{2}  sim{0}:  ===>  {3}", i, 
                    normalization.UnScale(input[i][0]),
                    normalization.UnScale(input[i][1]), normalization.UnScale(network.Compute(input[i])[0]));
            }
        }
    }
}
