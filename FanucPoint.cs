using System;
using MathNet.Numerics.LinearAlgebra;

namespace FanucUtilities
{
    public class FanucPoint
    {
        public string Name { get; set; } = "";
        public string Config { get; set; } = "N U T, 0, 0, 0";
        public string AuxData { get; set; } = ""; //Ugly hack until I eventually add support for multiple groups. TODO: update this.
        public int Index { get; set; } = 0;
        public bool IsJointRep { get; set; }
        public bool IsCartRep { get; set; }
        public bool IsPosReg { get; set; } = false;
        public bool IsUnintialized { get; set; } = false;
        public int NumOfGroups { get; } = 1;
        public int Utool { get; set; } = 0;
        public int Uframe { get; set; } = 0;
        public double[] JointPos = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }; //radians
        public Matrix<double> CartPos = Matrix<double>.Build.DenseIdentity(4, 4);
        public int Bound { get; set; } = 0;
        private FanucMath fanucMath = new FanucMath();
        private const double degToRad = Math.PI / 180;
        private const double radToDeg = 180 / Math.PI;

        /// <summary>
        /// Set the point to a joint position.
        /// </summary>
        /// <param name="name">Name of the point, can be empty.</param>
        /// <param name="index">Index of the point. Cannot be less than 1, or greater than 32765.</param>
        /// <param name="utool">User tool of the point. Cannot be negative.</param>
        /// <param name="uframe">User frame of the point. Cannot be negative.</param>
        /// <param name="joints">Array of 6 double representing the joint position in radians.</param>
        /// /// <param name="uninitialized">Sets the point as uninitialzed, but in joint format.</param>
        public FanucPoint(string name, int index, int utool, int uframe, double[] joints, bool uninitialized = false)
        {
            Name = name;
            Index = index;
            IsUnintialized = uninitialized;
            if (joints.Length != 6 && !uninitialized) { throw new ArgumentException("Number of joints must be 6."); }
            if (!uninitialized)
            {
                JointPos = joints;
                CheckParams(index, utool, uframe);
            }
            IsJointRep = true;
            IsCartRep = false;
            Utool = utool;
            Uframe = uframe;
        }

        /// <summary>
        /// Set the point to a cartesian position via a matrix.
        /// </summary>
        /// <param name="name">Name of the point, can be empty.</param>
        /// <param name="index">Index of the point. Cannot be less than 1, or greater than 32765.</param>
        /// <param name="utool">User tool of the point. Cannot be negative.</param>
        /// <param name="uframe">User frame of the point. Cannot be negative.</param>
        /// <param name="point">Matrix representing the cartesian position of the point.</param>
        /// <param name="config">Robot configuration string, with each configuration seperated by spaces. Example: "N U T, 0, 0, 0".</param>
        /// /// <param name="uninitialized">Sets the point as uninitialzed, but in matrix format.</param>
        public FanucPoint(string name, int index, int utool, int uframe, Matrix<double> point, string config, bool uninitialized=false)
        {
            Name = name;
            Index = index;
            IsUnintialized = uninitialized;
            if (!uninitialized)
            {
                CartPos = point;
            }
            Config = config;
            CheckParams(index, utool, uframe);
            IsJointRep = false;
            IsCartRep = true;
            Utool = utool;
            Uframe = uframe;
        }

        /// <summary>
        /// Set the point to a cartesian position via X Y Z W P R.
        /// </summary>
        /// <param name="name">Name of the point, can be empty.</param>
        /// <param name="index">Index of the point. Cannot be less than 1, or greater than 32765.</param>
        /// <param name="utool">User tool of the point. Cannot be negative.</param>
        /// <param name="uframe">User frame of the point. Cannot be negative.</param>
        /// <param name="xyzwpr">Array of doubles representing the point, in X Y Z W P R. X Y Z in mm, W P R in radians.</param>
        /// <param name="config">Robot configuration string, with each configuration seperated by spaces. Example: "N U T, 0, 0, 0".</param>
        /// <param name="uninitialized">Sets the point as uninitialzed, but in XYZWPR format.</param>
        public FanucPoint(string name, int index, int utool, int uframe, double[] xyzwpr, string config, bool uninitialized = false)
        {
            Name = name;
            Index = index;
            IsUnintialized = uninitialized;
            if (xyzwpr.Length!=6 && !uninitialized) { throw new ArgumentException("Passed in xyzwpr not proper array length. Length must be equal to 6."); }
            if (!uninitialized)
            {
                CartPos = fanucMath.ConvertXYZWPRtoMatrix(xyzwpr);
                CheckParams(index, utool, uframe);
            }
            Config = config;
            IsJointRep = false;
            IsCartRep = true;
            Utool = utool;
            Uframe = uframe;
        }

        /// <summary>
        /// Adjusts the point in tool coordinates. Point must be in cartesion representation for this to work.
        /// </summary>
        /// <param name="adjustment">Matrix describing the adjustment.</param>
        public void AdjustPositionInTool(Matrix<double> adjustment)
        {
            if (!IsCartRep) { throw new ArgumentException("Cannot adjust joint represenation points. Convert to cartesian first."); }
            CartPos = CartPos * adjustment;
        }

        /// <summary>
        /// Adjusts the point relative to its local origin coordinates. Point must be in cartesion representation for this to work.
        /// </summary>
        /// <param name="adjustment">Matrix describing the adjustment.</param>
        public void AdjustPositionInLocalSpace(Matrix<double> adjustment)
        {
            if (!IsCartRep) { throw new ArgumentException("Cannot adjust joint represenation points. Convert to cartesian first."); }
            CartPos = adjustment * CartPos;
        }

        public void SetJpos(double[] joints)
        {
            IsJointRep = true;
            IsCartRep = false;
            JointPos = joints;
        }

        public void SetXYZWPR(double[] xyzwpr)
        {
            IsJointRep = false;
            IsCartRep = true;
            CartPos = fanucMath.ConvertXYZWPRtoMatrix(xyzwpr);
        }

        public void SetXYZWPR(Matrix<double> matrix)
        {
            IsJointRep = false;
            IsCartRep = true;
            CartPos = matrix;
        }

        private void CheckParams(int index, int utool, int uframe)
        {
            if (index < 1 || index > 32765) { throw new ArgumentException("Index out of range. Must be between 1 and 32765. Passed value was: {index}"); }
            if (utool < 0) { throw new ArgumentException("Utool must be greater than 0. Passed value was: {utool}"); }
            if (uframe < 0) { throw new ArgumentException("Uframe must be greater than 0. Passed value was: {uframe}"); }
        }

        public override string ToString()
        {
            string returnString;
            if (Name == "")
            {
                if (!IsPosReg)
                {
                    returnString = $"P[{Index}]{{" + Environment.NewLine;
                } else
                {
                    returnString = $"PR[{Index}]{{" + Environment.NewLine;
                }
            }
            else
            {
                if (!IsPosReg)
                {
                    returnString = String.Format("P[{0}:\"{1}\"]{{", Index, Name) + Environment.NewLine;
                } else
                {
                    returnString = String.Format("PR[{0}:\"{1}\"]{{", Index, Name) + Environment.NewLine;
                }
            }

            returnString += "   GP1:" + Environment.NewLine;

            //TODO: Evetually add in remote TCP representation here.
            if (IsJointRep)
            {
                if (!IsPosReg)
                {
                    returnString += String.Format("\tUF : {0}, UT : {1},\t", Uframe, Utool) + Environment.NewLine;
                } else
                {
                    returnString += "\tUF : *, UT : *,\t" + Environment.NewLine;
                }

                if (IsUnintialized)
                {
                    returnString += "\tJ1=  ******** deg,\tJ2=  ******** deg,\tJ3=  ******** deg," + Environment.NewLine;
                    returnString += "\tJ4=  ******** deg,\tJ5=  ******** deg,\tJ6=  ******** deg" + Environment.NewLine;
                }
                else
                {
                    returnString += String.Format("\tJ1=  {0,8: ###.000;-###.000; 0.000} deg,", Math.Round(JointPos[0] * radToDeg, 3));
                    returnString += String.Format("\tJ2=  {0,8: ###.000;-###.000; 0.000} deg,", Math.Round(JointPos[1] * radToDeg, 3));
                    returnString += String.Format("\tJ3=  {0,8: ###.000;-###.000; 0.000} deg,", Math.Round(JointPos[2] * radToDeg, 3)) + Environment.NewLine;
                    returnString += String.Format("\tJ4=  {0,8: ###.000;-###.000; 0.000} deg,", Math.Round(JointPos[3] * radToDeg, 3));
                    returnString += String.Format("\tJ5=  {0,8: ###.000;-###.000; 0.000} deg,", Math.Round(JointPos[4] * radToDeg, 3));
                    returnString += String.Format("\tJ6=  {0,8: ###.000;-###.000; 0.000} deg", Math.Round(JointPos[5] * radToDeg, 3)) + Environment.NewLine;
                }
            }
            else
            {
                if (!IsPosReg)
                {
                    returnString += String.Format("\tUF : {0}, UT : {1},\t", Uframe, Utool) + "\tCONFIG : '" + Config + "'," + Environment.NewLine;
                } else
                {
                    returnString +="\tUF : *, UT : *,\t" + "\tCONFIG : '" + Config + "'," + Environment.NewLine;
                }
                if (IsUnintialized)
                {
                    returnString += "\tX =  ********  mm,\tY =  ********  mm,\tZ =  ********  mm," + Environment.NewLine;
                    returnString += "\tW =  ******** deg,\tP =  ******** deg,\tR =  ******** deg" + Environment.NewLine;
                }
                else
                {
                    double[] xyzwpr = fanucMath.ConvertMatrixToXYZWPR(CartPos);
                    returnString += String.Format("\tX = {0,8:  ####.000;-####.000;  0.000}  mm,", Math.Round(xyzwpr[0], 3));
                    returnString += String.Format("\tY = {0,8:  ####.000;-####.000;  0.000}  mm,", Math.Round(xyzwpr[1], 3));
                    returnString += String.Format("\tZ = {0,8:  ####.000;-####.000;  0.000}  mm,", Math.Round(xyzwpr[2], 3)) + Environment.NewLine;
                    returnString += String.Format("\tW =  {0,8: ###.000;-###.000;0.000} deg,", Math.Round(xyzwpr[3] * radToDeg, 3));
                    returnString += String.Format("\tP =  {0,8: ###.000;-###.000;0.000} deg,", Math.Round(xyzwpr[4] * radToDeg, 3));
                    returnString += String.Format("\tR =  {0,8: ###.000;-###.000;0.000} deg", Math.Round(xyzwpr[5] * radToDeg, 3)) + Environment.NewLine;
                }
            }

            returnString += AuxData;
            returnString += "};" + Environment.NewLine;
            return returnString;
        }

        public string ToCsvString()
        {
            string returnString = "";
            if (Name == "")
            {
                if (!IsPosReg)
                {
                    returnString = $"P[{Index}],";
                }
                else
                {
                    returnString = $"PR[{Index}],";
                }
            }
            else
            {
                if (!IsPosReg)
                {
                    returnString = String.Format("P[{0}:\"{1}\"],", Index, Name);
                }
                else
                {
                    returnString = String.Format("PR[{0}:\"{1}\"],", Index, Name);
                }
            }
            if (IsJointRep)
            {

                returnString += "Joint,";

                if (!IsPosReg)
                {
                    returnString += String.Format("UF : {0}, UT : {1},", Uframe, Utool);
                }
                else
                {
                    returnString += "UF : *, UT : *,";
                }

                if (IsUnintialized)
                {
                    returnString += "********,********,********,********,********,********," + Environment.NewLine;
                }
                else
                {

                    returnString += String.Format("{0,8: ###.000;-###.000; 0.000},", Math.Round(JointPos[0] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000; 0.000},", Math.Round(JointPos[1] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000; 0.000},", Math.Round(JointPos[2] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000; 0.000},", Math.Round(JointPos[3] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000; 0.000},", Math.Round(JointPos[4] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000; 0.000}", Math.Round(JointPos[5] * radToDeg, 3)) + Environment.NewLine;
                }
            }
            else
            {

                returnString += "Linear,";

                if (!IsPosReg)
                {
                    returnString += String.Format("UF : {0}, UT : {1},", Uframe, Utool);
                }
                else
                {
                    returnString += "UF : *, UT : *,";
                }



                if (IsUnintialized)
                {
                    returnString += "********,********,********,********,********,********" +Environment.NewLine;
                }
                else
                {
                    double[] xyzwpr = fanucMath.ConvertMatrixToXYZWPR(CartPos);
                    returnString += String.Format("{0,8:  ####.000;-####.000;  0.000},", Math.Round(xyzwpr[0], 3));
                    returnString += String.Format("{0,8:  ####.000;-####.000;  0.000},", Math.Round(xyzwpr[1], 3));
                    returnString += String.Format("{0,8:  ####.000;-####.000;  0.000},", Math.Round(xyzwpr[2], 3));
                    returnString += String.Format("{0,8: ###.000;-###.000;0.000},", Math.Round(xyzwpr[3] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000;0.000},", Math.Round(xyzwpr[4] * radToDeg, 3));
                    returnString += String.Format("{0,8: ###.000;-###.000;0.000}", Math.Round(xyzwpr[5] * radToDeg, 3)) + Environment.NewLine;
                }
            }
            return returnString;
        }

    }
}
