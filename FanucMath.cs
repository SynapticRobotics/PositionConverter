using System;
using MathNet.Numerics.LinearAlgebra;

namespace FanucUtilities
{
    public class FanucMath
    {
        private const double degToRad = Math.PI / 180;
        private const double radToDeg = 180 / Math.PI;

        /// <summary>
        /// Converts a translation and rotation matrix to the X Y Z W P R. 
        /// </summary>
        /// <param name="inputMatrix">The matrix to convert.</param>
        /// <returns>Returns a double array of X Y Z W P R. Rotations are in radians.</returns>
        public double[] ConvertMatrixToXYZWPR(Matrix<double> inputMatrix)
        {
            bool IsClose(double numToCompare, double Comparison) { return Math.Abs(numToCompare - Comparison) < 0.000000001; }

            double[] results = { 0, 0, 0, 0, 0, 0 };

            //X Y Z
            results[0] = inputMatrix[0, 3];
            results[1] = inputMatrix[1, 3];
            results[2] = inputMatrix[2, 3];

            //W P R
            double w, p, r, cosP;
            r = 0.0;
            if (IsClose(inputMatrix[2, 0], -1))
            {
                p = Math.PI / 2;
                w = Math.Atan2(inputMatrix[0, 1], inputMatrix[0, 2]);
            }
            else if (IsClose(inputMatrix[2, 0], 1))
            {
                p = -Math.PI / 2;
                w = Math.Atan2(-inputMatrix[0, 1], -inputMatrix[0, 2]);
            }
            else
            {
                p = -Math.Asin(inputMatrix[2, 0]);
                cosP = Math.Cos(p);
                w = Math.Atan2(inputMatrix[2, 1] / cosP, inputMatrix[2, 2] / cosP);
                r = Math.Atan2(inputMatrix[1, 0] / cosP, inputMatrix[0, 0] / cosP);
            }
            results[3] = w;
            results[4] = p;
            results[5] = r;

            return results;
        }

        public Matrix<double> ConvertXYZWPRtoMatrix(string[] input)
        {
            double[] xyzwpr = { 0, 0, 0, 0, 0, 0 };
            xyzwpr[0] = Convert.ToDouble(input[0]);
            xyzwpr[1] = Convert.ToDouble(input[1]);
            xyzwpr[2] = Convert.ToDouble(input[2]);
            xyzwpr[3] = Convert.ToDouble(input[3]) * degToRad;
            xyzwpr[4] = Convert.ToDouble(input[4]) * degToRad;
            xyzwpr[5] = Convert.ToDouble(input[5]) * degToRad;
            return ConvertXYZWPRtoMatrix(xyzwpr);
        }

        public Matrix<double> ConvertXYZWPRtoMatrix(double[] input)
        {
            Matrix<double> ResultMatrix = Matrix<double>.Build.DenseIdentity(4, 4);
            Matrix<double> rX = Matrix<double>.Build.DenseIdentity(4, 4);
            Matrix<double> rY = Matrix<double>.Build.DenseIdentity(4, 4);
            Matrix<double> rZ = Matrix<double>.Build.DenseIdentity(4, 4);
            double x = input[0];
            double y = input[1];
            double z = input[2];
            double w = input[3];
            double p = input[4];
            double r = input[5];

            //R
            rZ[0, 0] = Math.Cos(r);
            rZ[0, 1] = -Math.Sin(r);
            rZ[1, 0] = Math.Sin(r);
            rZ[1, 1] = Math.Cos(r);
            //P
            rY[0, 0] = Math.Cos(p);
            rY[0, 2] = Math.Sin(p);
            rY[2, 0] = -Math.Sin(p);
            rY[2, 2] = Math.Cos(p);
            //W
            rX[1, 1] = Math.Cos(w);
            rX[1, 2] = -Math.Sin(w);
            rX[2, 1] = Math.Sin(w);
            rX[2, 2] = Math.Cos(w);

            ResultMatrix = rZ * rY * rX;
            ResultMatrix[0, 3] = x;
            ResultMatrix[1, 3] = y;
            ResultMatrix[2, 3] = z;

            return ResultMatrix;
        }
    }
}
