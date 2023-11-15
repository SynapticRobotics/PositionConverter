using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace FanucUtilities
{
    public class FanucRobot
    {
        private const double degToRad = Math.PI / 180;
        private const double radToDeg = 180 / Math.PI;
        private double[] currentJointPosition = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
        private double[] negativeJointLimits = { -Math.PI, -Math.PI/4, -Math.PI, -2*Math.PI, -125 * degToRad, -2*Math.PI };
        private double[] positiveJointLimits = { Math.PI, 155 * degToRad, Math.PI, 2*Math.PI, 125 * degToRad, 2*Math.PI };
        private int utoolNum = 0 ;
        private int uframeNum = 0;
        private int lntkframeNum = 1;
        public double facePlateThickness = 0.0;
        private Matrix<double> currentLntkOffset = Matrix<double>.Build.DenseIdentity(4, 4);
        private Matrix<double>[] uTools = new Matrix<double>[256];
        private Matrix<double>[] uFrames = new Matrix<double>[256];
        private Matrix<double>[] lntkFrames = new Matrix<double>[7];
        private Matrix<double> currentPos = Matrix<double>.Build.DenseIdentity(4, 4);
        private Matrix<double> joint3Pos = Matrix<double>.Build.DenseIdentity(4, 4); //set by computeFK, used in computeIK.
        public double j1LinkA, j2LinkA, j3LinkLength, j3LinkA, j3AtZero, j4LinkD; //set by object constructor, used in computeIK.
        private double maxReachable; //used in computeIK.
        public List<FanucProgram> programs = new List<FanucProgram>();
        public int numOfUFrames, numOfUTools;
        public string robotArmType;
        private FanucMath fanucMath = new FanucMath();
        public List<FanucPoint> positionRegisters = new List<FanucPoint>();

        public override string ToString()
        {
            return base.ToString();
        }

        public void ParseBackup(string path)
        {
            string directory = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
            StreamReader s = new StreamReader(directory + "version.dg");
            string currentLine, robotType;
            string searchString;
            bool foundText = false;
            //Pull in robot type and set DH Parameters.
            do
            {
                currentLine = s.ReadLine();
                if (currentLine != null)
                {
                    string[] supportedRobots = new string[] { "R-1000", "R-2000", "ARC Mate 120iC", "M-710iC" };
                    foundText = supportedRobots.Any(currentLine.Contains);
                }
            }
            while (currentLine != null && !foundText);

            if (foundText)
            {
                string[] robotTypeList;
                robotTypeList = currentLine.Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);
                //robotTypeList = robotTypeList.Take(robotTypeList.Count() - 1).ToArray();
                robotType = string.Join(" ", robotTypeList);
                //robotType = currentLine.Split(' ')[0];
                robotArmType = robotType;
                SetDHParams(robotType);
            }
            s.Close();
            if (!foundText & robotArmType != "CUSTOM") { throw new Exception("Robot model not supported."); }

            //Pull in the frames
            try
            {
                s = new StreamReader(directory + "sysframe.va");
            }
            catch (Exception)
            {
                try
                {
                    s = new StreamReader(directory + "system.va");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            string[] frameXYZWPR = new string[6];
            string[] frameXYZ, frameWPR;
            char[] delimiter = { ' ' };
            string[] seperators = new string[] {" ", "X:", "Y:", "Z:", "W:", "P:", "R:"};


            //Pull in the user frames
            searchString = "$MNUFRAME";
            int i = 1; //Index starts at one because uframe zero is always be world.
            foundText = false;
            do
            {
                currentLine = s.ReadLine();
                if (currentLine != null) { foundText = currentLine.Contains(searchString); }
            }
            while (currentLine != null && !foundText);
            do
            {
                currentLine = s.ReadLine();  //Typically    "[1,1] ="
                if (currentLine == "") { break; } //check to see if this was the last user frame.
                currentLine = s.ReadLine();  //Typically    "  Group: 1   Config: N D B, 0, 0, 0"
                currentLine = s.ReadLine();  //Start of frame data, X Y Z
                frameXYZ = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                frameXYZ.CopyTo(frameXYZWPR, 0);
                currentLine = s.ReadLine();  //Start of frame data, W P R
                frameWPR = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                frameWPR.CopyTo(frameXYZWPR, frameXYZ.Length);

                if (frameXYZWPR.Length >= 6)
                {

                    uFrames[i] = fanucMath.ConvertXYZWPRtoMatrix(frameXYZWPR);
                    i++;
                }
            } while (currentLine != null && currentLine != "");
            numOfUFrames = i;


            searchString = "$MNUTOOL";
            i = 1; //Index starts at one because utool zero should always be the faceplate.
            foundText = false;
            do
            {
                currentLine = s.ReadLine();
                if (currentLine != null) { foundText = currentLine.Contains(searchString); }
            }
            while (currentLine != null && !foundText);
            do
            {
                currentLine = s.ReadLine();  //Typically    "[1,1] ="
                if (currentLine == "") { break; } //check to see if this was the last tool frame.
                currentLine = s.ReadLine();  //Typically    "  Group: 1   Config: N D B, 0, 0, 0"
                currentLine = s.ReadLine();  //Start of frame data, X Y Z
                frameXYZ = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                frameXYZ.CopyTo(frameXYZWPR, 0);
                currentLine = s.ReadLine();  //Start of frame data, W P R
                frameWPR = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                frameWPR.CopyTo(frameXYZWPR, frameXYZ.Length);

                if (frameXYZWPR.Length >= 6)
                {
                    uTools[i] = fanucMath.ConvertXYZWPRtoMatrix(frameXYZWPR);
                    i++;
                }
            } while (currentLine != null && currentLine!="");
            numOfUTools = i;
            s.Close();


            //Pull in line tracking frame, if one exists.
            try
            {
                s = new StreamReader(directory + "lntkbck.va");
                searchString = "$LNSCH[1].$TRK_FRAME";
                foundText = false;
                do
                {
                    currentLine = s.ReadLine();
                    if (currentLine != null) { foundText = currentLine.Contains(searchString); }
                }
                while (currentLine != null && !foundText);
                currentLine = s.ReadLine();  //Typically    "  Group: 1   Config: N D B, 0, 0, 0"
                currentLine = s.ReadLine();  //Start of frame data, X Y Z
                frameXYZ = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                frameXYZ.CopyTo(frameXYZWPR, 0);
                currentLine = s.ReadLine();  //Start of frame data, W P R
                frameWPR = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                frameWPR.CopyTo(frameXYZWPR, frameXYZ.Length);
                lntkFrames[0] = fanucMath.ConvertXYZWPRtoMatrix(frameXYZWPR);
                s.Close();
            }
            catch
            {
                //Not a line tracking robot.
            }


            //Pull in lower axes limits
            s = new StreamReader(directory + "symotn.va");
            string[] jointLimit;
            searchString = "$PARAM_GROUP[1].$LOWERLIMS";
            foundText = false;
            do
            {
                currentLine = s.ReadLine();
                if (currentLine != null) { foundText = currentLine.Contains(searchString); }
            }
            while (currentLine != null && !foundText);
            if (foundText)
            {
                for (i = 0; i <= 5; i++)
                {
                    currentLine = s.ReadLine();  //Typically    "[1] = -1.700000e+02"
                    jointLimit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    negativeJointLimits[i] = Convert.ToDouble(jointLimit[2]) * degToRad;
                }
                //Pull in upper limits.
                searchString = "$PARAM_GROUP[1].$UPPERLIMS";
                foundText = false;
                do
                {
                    currentLine = s.ReadLine();
                    if (currentLine != null) { foundText = currentLine.Contains(searchString); }
                }
                while (currentLine != null && !foundText);
                for (i = 0; i <= 5; i++)
                {
                    currentLine = s.ReadLine();  //Typically    "[1] = -1.700000e+02"
                    jointLimit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    positiveJointLimits[i] = Convert.ToDouble(jointLimit[2]) * degToRad;
                }
            }
            s.Close();


            //Pull in the programs.
            //Don't parse error logs.
            string[] noParse = { "erract.ls", "errall.ls", "errapp.ls" , "errcomm.ls", "errcurr.ls", "errext.ls", "errhist.ls", "errmot.ls", "errpwd.ls", "errsys.ls", "hist.ls", "logbook.ls", "statpage.ls", "updtlog.ls", "vtrndiag.ls"};
            string[] fileEntries = Directory.GetFiles(directory,"*.ls");
            foreach (string lsFilePath in fileEntries)
            {
                string lsFileName = Path.GetFileName(lsFilePath);
                //Don't parse background edits either.
                if (!noParse.Contains(lsFileName.ToLower()) && !lsFilePath.ToLower().Contains("-bcked"))
                {
                    Console.WriteLine("Parsing: " + lsFileName);
                    programs.Add(new FanucProgram(lsFilePath));
                }
            }

            //Pull in PRs.
            positionRegisters.Clear();
            s = new StreamReader(directory + "posreg.va");
            Console.WriteLine("Parsing: POSREG.VA");
            currentLine = s.ReadLine();
            string prComment, prNumStr;
            int prNum;
            do
            {
                if (currentLine.Contains("[1,") & !currentLine.Contains("$POSREG"))
                {
                    //Found a group 1 PR.
                    //Pull out the comment, if it exists.
                    prNumStr = currentLine.Substring(currentLine.IndexOf(",") + 1, currentLine.IndexOf("]") - 1 - currentLine.IndexOf(",") );
                    prNum = Convert.ToInt32(prNumStr);
                    prComment = currentLine.Substring(currentLine.IndexOf("'") + 1, currentLine.LastIndexOf("'") - 1 - currentLine.IndexOf("'"));
                    prComment.Trim();
                    if (currentLine.Contains("Uninit"))
                    {
                        //PR is uninitialized, create it as an uninitialized XYZWPR point
                        double[] temp = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                        positionRegisters.Add(new FanucPoint(prComment, prNum, 0, 0, temp, "N U T, 0, 0, 0", true));
                    }
                    else
                    {
                        seperators = new string[] { " ", "\t", "X:", "Y:", "Z:", "W:", "P:", "R:", "J1 =", "J2 =", "J3 =", "J4 =", "J5 =", "J6 =", ",", "deg", "mm" };
                        //PR is not uninitalized. Parse in what type it is.
                        currentLine = s.ReadLine();
                        if (currentLine.Contains("J1")) //JPOS
                        {
                            double[] joints = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                            string[] pointSplit = new string[32];
                            pointSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            joints[0] = Double.Parse(pointSplit[0]) * degToRad;
                            joints[1] = Double.Parse(pointSplit[1]) * degToRad;
                            joints[2] = Double.Parse(pointSplit[2]) * degToRad;
                            currentLine = s.ReadLine();
                            pointSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            joints[3] = Double.Parse(pointSplit[0]) * degToRad;
                            joints[4] = Double.Parse(pointSplit[1]) * degToRad;
                            joints[5] = Double.Parse(pointSplit[2]) * degToRad;
                            positionRegisters.Add(new FanucPoint(prComment, prNum, 0, 0, joints));
                        }
                        else if (currentLine.Contains("Config:")) //XYZWPR
                        {
                            double[] xyzwpr = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                            string[] pointSplit = new string[32];
                            string config = currentLine.Substring(currentLine.LastIndexOf(":") + 2);
                            config.Trim();
                            currentLine = s.ReadLine();
                            pointSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            try { xyzwpr[0] = Double.Parse(pointSplit[0]); } catch { xyzwpr[0] = 0; };
                            try { xyzwpr[1] = Double.Parse(pointSplit[1]); } catch { xyzwpr[1] = 0; };
                            try { xyzwpr[2] = Double.Parse(pointSplit[2]); } catch { xyzwpr[2] = 0; };
                    currentLine = s.ReadLine();
                            pointSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            try { xyzwpr[3] = Double.Parse(pointSplit[0]) * degToRad; } catch { xyzwpr[3] = 0; };
                            try { xyzwpr[4] = Double.Parse(pointSplit[1]) * degToRad; } catch { xyzwpr[4] = 0; };
                            try { xyzwpr[5] = Double.Parse(pointSplit[2]) * degToRad; } catch { xyzwpr[5] = 0; };
                            positionRegisters.Add(new FanucPoint(prComment, prNum, 0, 0, xyzwpr, config));
                        }
                        else //Matrix
                        {
                            //Looks like the Fanuc controller just outputs these as XYZWPR
                        }
                    }
                }
                currentLine = s.ReadLine();
            }
            while (currentLine != null);
            s.Close();
            //Set all PR points to say they are PRs.
            foreach (FanucPoint pr in positionRegisters){
                pr.IsPosReg = true;
            }
        }

        public void SetDHParams(string robotType)
        {
            double ComputeJ3AtZero(double J1a, double J2a, double J3a, double J4d)
            {
                double result, J3aJ4dLinkLength, distJ2toWC;
                J3aJ4dLinkLength = Math.Sqrt(Math.Pow(J3a,2.0)+Math.Pow(J4d,2.0));
                distJ2toWC = Math.Sqrt(Math.Pow(J4d, 2.0) + Math.Pow(J2a + J3a, 2.0));
                result = Math.Acos((Math.Pow(J3aJ4dLinkLength, 2.0)+Math.Pow(J2a,2.0)-Math.Pow(distJ2toWC, 2.0)) / (2.0* J3aJ4dLinkLength* J2a))+Math.Acos(J4d / J3aJ4dLinkLength);
                return result;
            }
            bool throwEx=false;
            //Set the DH Parameters.
            if (robotType.Contains("R-1000"))
            {
                j1LinkA = 320.0;
                j2LinkA = 870.0;
                j3LinkA = 225.0;
                j4LinkD = 1015.0;
                if (robotType.Contains("80F-IF")) { facePlateThickness = 198.0; }
                else if (robotType.Contains("80F")) { facePlateThickness = 175.0; }
                else if (robotType.Contains("100F-IF")) { facePlateThickness = 213.0; }
                else if (robotType.Contains("100F")) { facePlateThickness = 190.0; }
                else { throwEx = true; }
            }
            else if (robotType.Contains("R-2000iB"))
            {
                j1LinkA = 312.0;
                j2LinkA = 1075.0;
                j3LinkA = 225.0;
                j4LinkD = 1280.0;
                if (robotType.Contains("125L-IF")) { j4LinkD = 1635.0; facePlateThickness = 238.0; }
                else if (robotType.Contains("125L")) { j4LinkD = 1635.0; facePlateThickness = 215.0; }
                else if (robotType.Contains("165F-IF")) { facePlateThickness = 238.0; } //TODO: Check this.
                else if (robotType.Contains("165F")) { facePlateThickness = 215.0; }
                else if (robotType.Contains("210F-IF")) { facePlateThickness = 258; }
                else if (robotType.Contains("210F")) { facePlateThickness = 235.0; }
                else { throwEx = true; }
            }
            else if (robotType.Contains("R-2000iC"))
            {
                j1LinkA = 312.0;
                j2LinkA = 1075.0;
                j3LinkA = 225.0;
                j4LinkD = 1280.0;
                if (robotType.Contains("125L-IF")) { j4LinkD = 1730.0; facePlateThickness = 238.0; } //unverified
                else if (robotType.Contains("125L")) { j4LinkD = 1730.0; facePlateThickness = 215.0; } //unverified
                else if (robotType.Contains("165F-IF")) { facePlateThickness = 238.0; } //TODO: Check this.
                else if (robotType.Contains("165F")) { facePlateThickness = 215.0; }
                else if (robotType.Contains("210F-IF")) { facePlateThickness = 233.0; }
                else if (robotType.Contains("210F")) { facePlateThickness = 215.0; }
                else { throwEx = true; }
            }
            else if (robotType.Contains("R-2000i")) //R2000iAs show up as this.
            {
                j1LinkA = 312.0;
                j2LinkA = 1075.0;
                j3LinkA = 225.0;
                j4LinkD = 1280.0;
                if (robotType.Contains("165F-IF")) { facePlateThickness = 238.0; }
                else if (robotType.Contains("165F")) { facePlateThickness = 215.0; }
                else if (robotType.Contains("200F-IF")) { j4LinkD = 1110.0; facePlateThickness = 275.0; }
                else if (robotType.Contains("200F")) { j4LinkD = 1110.0; facePlateThickness = 260.0; }
                else { throwEx = true; }
            }
            else if (robotType.Contains("ARC Mate 120iC"))
            {
                j1LinkA = 150.0;
                j2LinkA = 790.0;
                j3LinkA = 250.0;
                j4LinkD = 835.0;
                facePlateThickness = 100.0;
            }
            else if (robotType.Contains("M-710"))
            {
                j1LinkA = 150.0;

                if (robotType.Contains("12L"))
                {
                    j2LinkA = 1150.0;
                    j3LinkA = 255.0;
                    j4LinkD = 1805.0;
                    facePlateThickness = 100.0;
                }
                else if (robotType.Contains("20L"))
                {
                    j2LinkA = 1150.0;
                    j3LinkA = 190.0;
                    j4LinkD = 1800.0;
                    facePlateThickness = 100.0;
                }
                else if (robotType.Contains("20M"))
                {
                    j2LinkA = 870.0;
                    j3LinkA = 190.0;
                    j4LinkD = 1550.0;
                    facePlateThickness = 100.0;
                }
                else if (robotType.Contains("45M"))
                {
                    j2LinkA = 1150;
                    j3LinkA = 170.0;
                    j4LinkD = 1295.0;
                    facePlateThickness = 175.0;
                }
                else if (robotType.Contains("50") | robotType.Contains("70"))
                {
                    j2LinkA = 870.0;
                    j3LinkA = 170.0;
                    j4LinkD = 1016.0;
                    facePlateThickness = 175.0;
                }
                else { throwEx = true; }
            }
            else if (robotType.Contains("CUSTOM"))
            {
                //Set DH Form will set the link lengths directly. Just make sure we don't throw an exception
                throwEx = false;
            }
            else
            {
                throwEx = true;
            }

            if (throwEx) { throw new System.ArgumentException("Unsupported Fanuc Robot Type. Got: " + robotType); }

            //These are used in the computeIK method.
            j3AtZero = ComputeJ3AtZero(j1LinkA, j2LinkA, j3LinkA, j4LinkD);
            j3LinkLength = Math.Sqrt(j3LinkA * j3LinkA + j4LinkD * j4LinkD);
            maxReachable = j1LinkA + j2LinkA + j3LinkLength;
            //Set the Uframes and Utools to identity matricies.
            for (int i=0; i < 256; i++)
            {
                uTools[i] = Matrix<double>.Build.DenseIdentity(4, 4);
                uFrames[i] = Matrix<double>.Build.DenseIdentity(4, 4);
            }
            for (int i=0; i< 7; i++)
            {
                lntkFrames[i] = Matrix<double>.Build.DenseIdentity(4, 4);
            }
            currentPos = ComputeFK(currentJointPosition, utoolNum, uframeNum);
        }

        /// <summary>
        ///Returns the robot's current joint positions, in radians.
        ///</summary>
        public double[] GetCurrentJointPositions(){ return currentJointPosition; }

        /// <summary>
        ///Returns the robot's current tcp position, in world.
        ///</summary>
        public Matrix<double> GetCurrentPosition() { return currentPos; }

        /// <summary>
        ///Returns the theoretical TCP location given a set of joint angles (in radians), but does not set the robot to that position.
        ///</summary>
        Matrix<double> ComputeFK(double[] inputJointAng, int utool, int uframe)
        {
            Matrix<double> joint1DH = DH_Matrix(Math.PI / 2, j1LinkA, 0.0, inputJointAng[0]);
            Matrix<double> joint2DH = DH_Matrix(0, j2LinkA, 0.0, Math.PI/2 - inputJointAng[1] );
            Matrix<double> joint3DH = DH_Matrix(Math.PI / 2, j3LinkA, 0.0, inputJointAng[2]-(Math.PI / 2 - inputJointAng[1]) + Math.PI / 2);
            Matrix<double> joint4DH = DH_Matrix(-Math.PI / 2, 0.0, j4LinkD, inputJointAng[3]*-1);
            Matrix<double> joint5DH = DH_Matrix(Math.PI / 2, 0.0, 0.0, inputJointAng[4]);
            Matrix<double> joint6DH = DH_Matrix(0.0, 0.0, 0.0, inputJointAng[5]*-1);
            Matrix<double> facePlateDH = DH_Matrix(0.0, 0.0, facePlateThickness, 0.0);
#if DEBUG
            Matrix<double> temp = Matrix<double>.Build.DenseIdentity(4, 4);
            temp = joint1DH;
            temp *= joint2DH;
            temp *= joint3DH;
            temp *= joint4DH;
            temp *= joint5DH;
            temp *= joint6DH;
            temp *= facePlateDH;
#endif
            //Used in computeIK
            joint3Pos = joint1DH * joint2DH * joint3DH;

            Matrix<double> worldPoint = Matrix<double>.Build.DenseIdentity(4, 4);
            worldPoint = joint3Pos * joint4DH * joint5DH * joint6DH * facePlateDH * uTools[utool];
            Matrix<double> ufPoint = Matrix<double>.Build.DenseIdentity(4, 4);
            ufPoint = uFrames[uframe].Inverse() * worldPoint;

            //return joint3Pos * joint4DH * joint5DH * joint6DH * facePlateDH * uTools[utool] * uFrames[uframe].Inverse();
            return ufPoint;
        }

        //Assumes spherical wrist.
        private double[] ComputeIK(Matrix<double> commandedPostion, int utool, int uframe, string config, bool checkLimits=false)
        {
            bool NearZero(double input) { return input < 0.000000001 && input > -0.000000001; }
            double[] foundJoints = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            
            //Get back to wrist center from passed in position.
            Matrix<double> facePlateInverse = Matrix<double>.Build.DenseIdentity(4, 4);
            facePlateInverse[2, 3] = -facePlateThickness;
            Matrix<double> wristCenter = uFrames[uframe] * commandedPostion * uTools[utool].Inverse() * facePlateInverse;

            //Basic distance check for wrist center reachability.
            double wcDist, wcX, wcY, wcZ;
            wcX = wristCenter[0, 3];
            wcY = wristCenter[1, 3];
            wcZ = wristCenter[2, 3];
            wcDist = Math.Sqrt(wcX * wcX + wcY * wcY + wcZ * wcZ);
            if (wcDist > maxReachable) { throw new Exception("Commanded postion beyond wrist center."); }

            //Start of J1, J2, and J3 IK
            //Check for shoulder sigularity
            if (NearZero(wcX) && NearZero(wcY)) { throw new Exception("Commanded postion in shoulder singularity.");}
            //Check for unsupported solve methods
            //TODO: Update the solver so these are supported.
            string[] seperators= new string[] { " ", "\'", "," };
            if ((config.Split(seperators, StringSplitOptions.RemoveEmptyEntries)[1] != "U") || (config.Split(seperators, StringSplitOptions.RemoveEmptyEntries)[2] != "T"))
                throw new Exception("Down and back configurations not implemented.");
            foundJoints[0] = Math.Atan2(wcY, wcX);
            double j1X = j1LinkA * Math.Cos(foundJoints[0]);
            double j1Y = j1LinkA * Math.Sin(foundJoints[0]);
            double j2ToWc = Math.Sqrt(Math.Pow(j1X - wcX, 2) + Math.Pow(j1Y - wcY, 2) + Math.Pow(wcZ, 2));
            double j2ToWcXYPlane = Math.Sqrt(Math.Pow(j1X - wcX, 2) + Math.Pow(j1Y - wcY, 2));
            foundJoints[1] = Math.Acos((Math.Pow(j2LinkA, 2) + Math.Pow(j2ToWc,2) - Math.Pow(j3LinkLength,2)) / (2 * j2LinkA * j2ToWc));
            foundJoints[1] = -1 * (Math.Acos(j2ToWcXYPlane / j2ToWc) + foundJoints[1] - Math.PI / 2);
            foundJoints[2] = Math.Acos((Math.Pow(j3LinkLength, 2) + Math.Pow(j2LinkA, 2) - Math.Pow(j2ToWc, 2)) / (2 * j3LinkLength * j2LinkA)) + Math.Acos(j4LinkD / j3LinkLength);
            foundJoints[2] = foundJoints[2] - foundJoints[1] - j3AtZero; 

            //Rerun forward kinematics for an updated joint3Pos matrix
            ComputeFK(foundJoints, utool, uframe);

            //Start of J4, J5, and J6 IK
            wristCenter =  joint3Pos.Inverse() * wristCenter ;
            //Check for wrist sigularity
            if (NearZero(wristCenter[0,2]) && NearZero(wristCenter[1,2])) { throw new Exception("Commanded postion in wrist singularity."); }
            if (config.Split(seperators, StringSplitOptions.RemoveEmptyEntries)[0] == "N")
            {
                //Non flipped wrist
                foundJoints[3] = Math.Atan2(-wristCenter[0, 2], -wristCenter[1, 2]) -Math.PI/2;
                foundJoints[4] = Math.PI/2-Math.Atan2(wristCenter[2, 2], -Math.Sqrt(1 - wristCenter[2, 2] * wristCenter[2, 2]));
                foundJoints[5] = Math.Atan2(-wristCenter[2, 1], wristCenter[2, 0])*-1;
            }
            else
            {
                //Flipped wrist
                foundJoints[3] = Math.Atan2(wristCenter[0, 2], wristCenter[1, 2]) - Math.PI / 2;
                foundJoints[4] = Math.PI / 2 - Math.Atan2(wristCenter[2, 2], Math.Sqrt(1 - wristCenter[2, 2] * wristCenter[2, 2]));
                foundJoints[5] = Math.Atan2(wristCenter[2, 1], -wristCenter[2, 0]) * -1;
            }

            //Parse and use turn numbers in J1, J4, and J6 IK calcs.
            int j1TurnNumber = int.Parse(config.Split(seperators, StringSplitOptions.RemoveEmptyEntries)[3]);
            int j4TurnNumber = int.Parse(config.Split(seperators, StringSplitOptions.RemoveEmptyEntries)[4]);
            int j6TurnNumber = int.Parse(config.Split(seperators, StringSplitOptions.RemoveEmptyEntries)[5]);
            foundJoints[0] += 2 * Math.PI * j1TurnNumber;
            foundJoints[3] += 2 * Math.PI * j4TurnNumber;
            foundJoints[5] += 2 * Math.PI * j6TurnNumber;

            //See if the results are within the robot's soft limits
            //TODO: Move this into its own method.;

            if (checkLimits && !WithinJointLimits(foundJoints)) { throw new Exception("Commanded postion outside robot joint limits."); }

            //All results good.
            return foundJoints;
        }

        ///<summary>
        ///Sets the robot to the passed in position (in world), and returns the current joint angles, if successful.
        ///Throws an "Unreachable" exception if the position is not reachable. ;
        ///</summary>
        public double[] SetPostion(Matrix<double> commandedPostion, string config)
        {
            try
            {
                currentJointPosition = ComputeIK(commandedPostion, uframeNum, utoolNum, config);
                return currentJointPosition;
            } catch (Exception reachExceptionDetail)
            {
                throw new Exception("Commanded position unreachable.", reachExceptionDetail);
            }
        }

        ///<summary>
        ///Sets the robot to the passed in joint angles, and returns the new TCP position.
        ///</summary>
        public Matrix<double> SetJointAngles(double[] inputJointAng) 
        {
            currentPos = ComputeFK(inputJointAng, utoolNum, uframeNum);
            return currentPos;
        }
        ///<summary>
        ///Checks to see if the passed in joint angles are within the limits
        ///</summary>
        private bool WithinJointLimits(double[] inputJointAng)
        {
            for(int i = 0; i <= 5; i++)
            {
                if (i == 2) //Special case for J3, since Fanucs do the whale tail thing.
                {
                    if ((inputJointAng[i] + inputJointAng[i - 1]) < negativeJointLimits[i] || (inputJointAng[i] + inputJointAng[i - 1]) > positiveJointLimits[i]) { return false; }
                    if (robotArmType.Contains("R-1000"))
                    {
                        //Bit of a hack here, for cases when the arm is too scrunched up on itself.
                        //Formula dirived from experimatal testing in roboguide on a R1000.
                        // Data in degrees:
                        // J1       J2
                        //-79.828   -2.056
                        //-70.852  -11.128
                        //-65.252  -17.608
                        //-52.900  -28.840
                        //-32.908  -45.664
                        //-30.460  -51.304
                        //-20.427  -61.240
                        // -9.603  -72.040
                        //  2.277  -84.137
                        // 12.572  -94.505
                        // 37.125 -113.945
                        if ((inputJointAng[i]) < (-0.9741*inputJointAng[i - 1] - 1.4)) { return false; }
                    }
                }
                else
                {
                    if (inputJointAng[i] < negativeJointLimits[i] || inputJointAng[i] > positiveJointLimits[i]) { return false; }
                }
            }

            return true;
        }

        private Matrix<double> DH_Matrix(double alpha, double a, double d, double theta)
        {
            Matrix<double> ResultMatrix = Matrix<double>.Build.DenseIdentity(4, 4);

            //Below is the transform matrix from the article on wikipedia about DH parameters.
            //It does not match the way robot assist (by NRK) does its transforms.
            double cosAlpha = Math.Cos(alpha);
            double sinAlpha = Math.Sin(alpha);
            double cosTheta = Math.Cos(theta);
            double sinTheta = Math.Sin(theta);

            ResultMatrix[0, 0] = cosTheta;
            ResultMatrix[0, 1] = -sinTheta * cosAlpha;
            ResultMatrix[0, 2] = sinTheta * sinAlpha;
            ResultMatrix[0, 3] = a * cosTheta;
            ResultMatrix[1, 0] = sinTheta;
            ResultMatrix[1, 1] = cosTheta * cosAlpha;
            ResultMatrix[1, 2] = -cosTheta * sinAlpha;
            ResultMatrix[1, 3] = a * sinTheta;
            ResultMatrix[2, 0] = 0;
            ResultMatrix[2, 1] = sinAlpha;
            ResultMatrix[2, 2] = cosAlpha;
            ResultMatrix[2, 3] = d;

            return ResultMatrix;
        }

        public void ConvertPointToJoint(string program, int pointindex)
        {
            FanucProgram fanucProgram = ProgramExists(program);
            if (fanucProgram != null)
            {
                foreach (FanucPoint point in fanucProgram.Points)
                {
                    if (point.Index == pointindex)
                    {
                        if (point.IsJointRep)
                        {
                            //Point is already in joint rep. Do nothing.
                            return;
                        }
                        else if (point.IsCartRep)
                        {
                            point.SetJpos(ComputeIK(point.CartPos, point.Utool, point.Uframe, point.Config));
                            return;
                        }
                    }
                }
                throw new Exception("Point index not found in program: " + fanucProgram.Name);
            }
            throw new Exception("Program " + program + " not present in robot.");
        }

        public void ConvertPointToXYZWPR(string program, int pointindex)
        {
            FanucProgram fanucProgram = ProgramExists(program);
            if (fanucProgram != null)
            {
                foreach(FanucPoint point in fanucProgram.Points)
                {
                    if(point.Index == pointindex)
                    {
                        if (point.IsJointRep)
                        {
                            if (point.JointPos[4] > 0)
                            {// "N U T, 0, 0, 0"
                                point.Config = "F U T, 0, ";
                            } else
                            {
                                point.Config = "N U T, 0, ";
                            }
                            //Left off here:
                            point.Config += String.Format("{0:0}, {1:0}", point.JointPos[3] / (2 * Math.PI), point.JointPos[5] / (2 * Math.PI));

                            point.SetXYZWPR(ComputeFK(point.JointPos, point.Utool, point.Uframe));
                            
                            return;
                        }
                        else if(point.IsCartRep)
                        {
                            //Point is already in cart rep. Do nothing.
                            return;
                        }
                    }
                }
                throw new Exception("Point index not found in program: " + fanucProgram.Name);
            }
            throw new Exception("Program " + program + " not present in robot.");
        }

        private FanucProgram ProgramExists(string programName)
        {
            foreach(FanucProgram program in programs)
            {
                if (program.Name.ToUpper() == programName.ToUpper())
                    return program;
            }
            return null;
        }

        public string GetUtool(int index)
        {
            string returnString = "";
            double[] utool;
            utool = fanucMath.ConvertMatrixToXYZWPR(uTools[index]);
            returnString += String.Format("\tX =  {0,8: ####.000;-####.000;0.000}  mm,", Math.Round(utool[0], 3));
            returnString += String.Format("\tY =  {0,8: ####.000;-####.000;0.000}  mm,", Math.Round(utool[1], 3));
            returnString += String.Format("\tZ =  {0,8: ####.000;-####.000;0.000}  mm,", Math.Round(utool[2], 3)) + Environment.NewLine;
            returnString += String.Format("\tW =  {0,8: ###.000;-###.000;0.000} deg,", Math.Round(utool[3] * radToDeg, 3));
            returnString += String.Format("\tP =  {0,8: ###.000;-###.000;0.000} deg,", Math.Round(utool[4] * radToDeg, 3));
            returnString += String.Format("\tR =  {0,8: ###.000;-###.000;0.000} deg", Math.Round(utool[5] * radToDeg, 3)) + Environment.NewLine;
            return returnString;
        }

        public string GetUframe(int index)
        {
            string returnString = "";
            double[] uframe;
            uframe = fanucMath.ConvertMatrixToXYZWPR(uFrames[index]);
            returnString += String.Format("\tX = {0,8:  ####.000;-####.000;   0.000}  mm,", Math.Round(uframe[0], 3));
            returnString += String.Format("\tY = {0,8:  ####.000;-####.000;   0.000}  mm,", Math.Round(uframe[1], 3));
            returnString += String.Format("\tZ = {0,8:  ####.000;-####.000;   0.000}  mm,", Math.Round(uframe[2], 3)) + Environment.NewLine;
            returnString += String.Format("\tW =  {0,8: ###.000;-###.000;0.000} deg,", Math.Round(uframe[3] * radToDeg, 3));
            returnString += String.Format("\tP =  {0,8: ###.000;-###.000;0.000} deg,", Math.Round(uframe[4] * radToDeg, 3));
            returnString += String.Format("\tR =  {0,8: ###.000;-###.000;0.000} deg", Math.Round(uframe[5] * radToDeg, 3)) + Environment.NewLine;
            return returnString;
        }
    }
}
