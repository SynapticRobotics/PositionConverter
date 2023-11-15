using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace FanucUtilities
{
    public class FanucProgram
    {
        public string Name { get; set; } = "";
        public string Comment { get; set; } = "";
        public List<string> Attributes { get; } = new List<string>();
        public bool IsLineTrackingProgram { get; } = false;
        public bool IsCondition { get; } = false;
        public bool IsMacro { get; } = false;
        public bool LocksMotionGroup { get; } = false;
        public bool HasPoints { get; } = false;
        public List<string> ProgramLines { get; } = new List<string>();
        public int NumberOfPoints { get; } = 0;
        public List<FanucPoint> Points = new List<FanucPoint>();
        public double[] BoundaryUpStream { get; } = new double[9];
        public double[] BoundaryDownStream { get; } = new double[9];

        public FanucProgram(string path)
        {
            //Parse the program.
            StreamReader s = new StreamReader(path);
            string currentLine;
            string searchString = "/MN";
            bool foundText = false;
            currentLine = s.ReadLine();
            string[] seperators = new string[] { " ", "\t" };
            string[] ProgNameSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            Name = ProgNameSplit[1];
            if (ProgNameSplit.Length > 2)
            {
                if (ProgNameSplit[2].Contains("Macro")) { IsMacro = true; }
                if (ProgNameSplit[2].Contains("Cond")) { IsCondition = true; }
            }
            do
            {
                currentLine = s.ReadLine();
                if (currentLine != null)
                    foundText = currentLine.Contains(searchString);
                if (currentLine.Contains("LINE_TRACK_SCHEDULE_NUMBER      : 1"))
                    IsLineTrackingProgram = true;
                if (currentLine.Contains("DEFAULT_GROUP") && currentLine.Contains("1"))
                    LocksMotionGroup = true;
                if (currentLine.Contains("COMMENT"))
                {
                    string[] commentSplit = new string[32];
                    commentSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 2; i < commentSplit.Length; i++)
                        Comment = Comment + ' ' + commentSplit[i];
                    Comment = Comment.Substring(2, Comment.Length - 4);
                }
                if (!foundText)
                    Attributes.Add(currentLine);
            }
            while (currentLine != null && !foundText);

            //pull in program
            searchString = "/POS";
            foundText = false;
            do
            {
                currentLine = s.ReadLine();
                if (currentLine != null)
                    foundText = currentLine.Contains(searchString);
                if (!foundText)
                    ProgramLines.Add(currentLine);
            }
            while (currentLine != null && !foundText);

            //Pull in positions
            int pointNum=1, utool=0, uframe=0;
            bool inGroup1 = false;
            bool inPoint = false;
            string config="N U T, 0, 0, 0", pointName="";
            seperators = new string[] { " ", "'", "\"", "\t", ":", "P[", "]", "{", "}", "X =","Y =","Z =","W =","P =","R =", "J1=", "J2=", "J3=", "J4=", "J5=", "J6=", ",", "deg","mm" };
            do
            {
                double[] joints = new double[6];
                double[] xyzwpr = new double[6];
                
                //Get Name
                currentLine = s.ReadLine();
                if (currentLine == null)
                    break;

                if (currentLine.Contains("P["))
                {
                    pointName = "";
                    HasPoints = true;
                    inPoint = true;
                    string[] PointNameSplit = new string[32];
                    PointNameSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    pointNum = Int32.Parse(PointNameSplit[0]);
                    PointNameSplit[0]="";
                    if (currentLine.Contains(":"))
                    {
                        try
                        {
                            foreach(string nameSection in PointNameSplit)
                                pointName += nameSection + " ";
                            pointName = pointName.Trim();
                        }
                        catch
                        {
                            pointName = "";
                        }
                    }
                    else
                        pointName = "";
                    continue;
                }

                //Set what group we are in
                if (currentLine.Contains("GP1:"))
                {
                    inGroup1 = true;
                    
                }
                if (currentLine.Contains("};") || currentLine.Contains("GP2:") || currentLine.Contains("GP3:") || currentLine.Contains("GP4:") || currentLine.Contains("GP5:"))
                {
                    //we only care about the first group.
                    inGroup1 = false;
                }
                if (currentLine.Contains("};"))
                {
                    inGroup1 = false;
                    inPoint = false;
                }

                //Get utool and uframe
                if (currentLine.Contains("UF"))
                {
                    string[] frameSplit = new string[32];
                    frameSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    uframe = Int32.Parse(frameSplit[1]);
                    utool = Int32.Parse(frameSplit[3]);
                    if (frameSplit.Length > 4)
                        config = frameSplit[5] + " " + frameSplit[6] + " " + frameSplit[7] + ", " + frameSplit[8] + ", " + frameSplit[9] + ", " + frameSplit[10];
                }

                //Parse point
                void AddAuxData()
                {
                    do
                    {
                        currentLine = s.ReadLine();
                        if (currentLine != null)
                        {
                            if (!currentLine.Contains("}"))
                            {
                                var temp = Points.Last();
                                temp.AuxData = temp.AuxData + currentLine + Environment.NewLine; ;
                                inPoint = false;
                            }
                            else
                                break;
                        }
                        else
                            inPoint = false;
                    } while (currentLine != null);
                }

                double degToRad = Math.PI / 180;
                if (currentLine.Contains("********")) //Check for uninitialized points
                {
                    if (inGroup1)
                    {
                        if (currentLine.Contains("X =")) Points.Add(new FanucPoint(pointName, pointNum, utool, uframe, xyzwpr, config, true));
                        if (currentLine.Contains("J1=")) Points.Add(new FanucPoint(pointName, pointNum, utool, uframe, joints, true));
                        currentLine = s.ReadLine();
                        AddAuxData();
                        inGroup1 = false;
                    }
                }
                else
                {
                    if (currentLine.Contains("X ="))
                    {
                        // Only readin the first group.
                        if (inGroup1)
                        {
                            string[] pointSplit = new string[32];
                            pointSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            xyzwpr[0] = Double.Parse(pointSplit[0]);
                            xyzwpr[1] = Double.Parse(pointSplit[1]);
                            xyzwpr[2] = Double.Parse(pointSplit[2]);
                            currentLine = s.ReadLine();
                            pointSplit = currentLine.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            xyzwpr[3] = Double.Parse(pointSplit[0]) * degToRad;
                            xyzwpr[4] = Double.Parse(pointSplit[1]) * degToRad;
                            xyzwpr[5] = Double.Parse(pointSplit[2]) * degToRad;
                            Points.Add(new FanucPoint(pointName, pointNum, utool, uframe, xyzwpr, config));
                            AddAuxData();
                            inGroup1 = false;
                        }
                    }
                    if (currentLine.Contains("J1="))
                    {
                        // Only readin the first group.
                        if (inGroup1)
                        {
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
                            Points.Add(new FanucPoint(pointName, pointNum, utool, uframe, joints));
                            AddAuxData();
                            inGroup1 = false;
                        }
                    }
                }
                
            }
            while (currentLine != null);

            //Parse boundaries and assign bounds if this is a line tracking program
            if (IsLineTrackingProgram && LocksMotionGroup && HasPoints)
                AssignBounds();
            s.Close();
        }

        private void AssignBounds()
        {
            int activeBound = 0;
            foreach (string prgString in ProgramLines)
            {
                string[] seperators = new string[] { " ", ":", "[", "]", "=", "(", ")" };
                string[] boundLine, pntLine;
                //Pull bounds out of the program, since they are always hardcoded in the program (at least on vows I work on.)
                if (prgString.Contains("$LNSCH[1].$BOUND1[")) //upstream bounds
                {
                    boundLine = prgString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    BoundaryUpStream[Convert.ToInt32(boundLine[4]) - 1]=Convert.ToDouble(boundLine[5]);
                }
                if (prgString.Contains("$LNSCH[1].$BOUND2[")) //downstream bounds
                {
                    boundLine = prgString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    BoundaryDownStream[Convert.ToInt32(boundLine[4]) - 1] = Convert.ToDouble(boundLine[5]);
                }
                if (prgString.Contains("SELBOUND LNSCH[1] BOUND["))
                {
                    boundLine = prgString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    activeBound = Convert.ToInt32(boundLine[5]);
                }
                if (prgString.Contains(":L P["))
                {
                    pntLine = prgString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    int pointNum = Convert.ToInt32(pntLine[3]);
                    foreach(FanucPoint point in Points)
                    {
                        if (point.Index == pointNum)
                        {
                            point.Bound = activeBound;
                            break;
                        }
                    }
                }

            }
        }

        public override string ToString()
        {
            string returnString="";
            returnString += "/PROG  " + Name;
            if (IsMacro)
                returnString += "/t  Marco";
            if (IsCondition)
                returnString += "/t  Cond";
            returnString += Environment.NewLine;
            foreach (string attrString in Attributes)
                returnString += attrString + Environment.NewLine;
            returnString += "/MN" + Environment.NewLine;
            foreach (string prgString in ProgramLines)
                returnString += prgString + Environment.NewLine;
            returnString += "/POS" + Environment.NewLine;
            foreach (FanucPoint point in Points)
                returnString += point.ToString();
            returnString += "/END" + Environment.NewLine; 
            return returnString;
        }


    }
}
