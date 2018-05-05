using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Models.Regression.Linear;

namespace ConsoleApplication3
{
    class LinearRegression
    {
        public static void Main2(string[] args)
        {
            aa();
        Console.ReadLine();
        }

    private static void aa()
    {
           // We will try to model a plane as an equation in the form
// "ax + by + c = z". We have two input variables (x and y)
// and we will be trying to find two parameters a and b and 
// an intercept term c.

            // We will use Ordinary Least Squares to create a
            // linear regression model with an intercept term
            var ols = new OrdinaryLeastSquares()
{
    UseIntercept = true
};

            // Now suppose you have some points
            double[][] inputs =
            {
    new double[] { 1, 4 },
    new double[] { 2, 5 },
    new double[] { 3, 6 },
    new double[] { 8, 11 },
};

            // located in the same Z (z = 1)
            double[] outputs = { 5, 12, 21, 96 };

            // Use Ordinary Least Squares to estimate a regression model
            MultipleLinearRegression regression = ols.Learn(inputs, outputs);

            // As result, we will be given the following:
            double a = regression.Coefficients[0]; // a = 0
            double b = regression.Coefficients[1]; // b = 0
            double c = regression.Intercept; // c = 1

            // This is the plane described by the equation
            // ax + by + c = z => 0x + 0y + 1 = z => 1 = z.

            // We can compute the predicted points using
            double[] predicted = regression.Transform(inputs);

            // And the squared error loss using 
            double error = new SquareLoss(outputs).Loss(predicted);
            }
    }
}
