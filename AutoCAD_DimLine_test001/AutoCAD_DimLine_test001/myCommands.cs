// (C) Copyright 2023 by  
//
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System;
using System.Linq;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(AutoCAD_DimLine_test001.MyCommands))]

namespace AutoCAD_DimLine_test001
{
    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
       
        [CommandMethod("DimLineSetting")]
        public void MyCommand() // This method can have any name
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                DimStyleTable DST = (DimStyleTable)trans.GetObject(doc.Database.DimStyleTableId ,OpenMode.ForWrite);
                DimStyleTableRecord DSTR = new DimStyleTableRecord();

                DSTR.Name = "myDimStyle";
                DSTR.Dimclre = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 255);
                DSTR.Dimclrd = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 255, 0);
                DSTR.Dimclrt = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 255, 255);
                DSTR.Dimtxt = 2.5;

                DST.Add(DSTR);
                trans.AddNewlyCreatedDBObject(DSTR, true);
            }
        }

        // Modal Command with pickfirst selection
        [CommandMethod("DimLineMaker")]
        public void MyPickFirst() // This method can have any name
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            PromptKeywordOptions PKO = new PromptKeywordOptions("choose the line type to draw dim line [Horizontal/Vertical]", "Horizontal Vertical");
            String Keyword = ed.GetKeywords(PKO).StringResult;

            PromptSelectionResult PSR = ed.GetSelection();
            SelectionSet SS = PSR.Value;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                DimStyleTable DST = (DimStyleTable)trans.GetObject(doc.Database.DimStyleTableId, OpenMode.ForWrite);
                DimStyleTableRecord DSTR = new DimStyleTableRecord();

                if (DST.Has("MyDimStyle"))
                {
                    ObjectId dimStyleRecordId = DST["MyDimStyle"];
                    DSTR = (DimStyleTableRecord)trans.GetObject(dimStyleRecordId, OpenMode.ForRead);
                }
                else
                {
                    DSTR.Name = "MyDimStyle";
                    DSTR.Dimclre = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 255);
                    DSTR.Dimclrd = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 255, 0);
                    DSTR.Dimclrt = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 255, 255);
                    DSTR.Dimtxt = 2.5;

                    DST.Add(DSTR);
                    trans.AddNewlyCreatedDBObject(DSTR, true);
                }

                int position = 0;
                PromptPointOptions PPO = new PromptPointOptions("click the point you want dim line to be drawn");
                PromptPointResult PPR = ed.GetPoint(PPO);
                switch (Keyword)
                {
                    case "Vertical":
                        position = int.Parse(PPR.Value.X.ToString().Split((char)'.')[0]);
                        break;

                    case "Horizontal":
                        position = int.Parse(PPR.Value.Y.ToString().Split((char)'.')[0]);
                        break;
                }


                ObjectId myDimStyleId = DSTR.ObjectId;

                BlockTable bt = (BlockTable)trans.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForRead);
                    if (ent is Line)
                    {
                        Line protLine = (Line)ent;

                        Point3d ptA = protLine.StartPoint;
                        Point3d ptB = protLine.EndPoint;

                        double ptA_x = double.Parse(ptA.ToString().Replace("(", "").Replace(")", "").Split(","[0])[0]);
                        double ptA_y = double.Parse(ptA.ToString().Replace("(", "").Replace(")", "").Split(","[0])[1]);
                        double ptB_x = double.Parse(ptB.ToString().Replace("(", "").Replace(")", "").Split(","[0])[0]);
                        double ptB_y = double.Parse(ptB.ToString().Replace("(", "").Replace(")", "").Split(","[0])[1]);

                        switch(Keyword)
                        {
                            case "Vertical":
                                if (-1 < (ptA_x - ptB_x) && (ptA_x - ptB_x) < 1)
                                {
                                    Point3d dimPosition = new Point3d(position, (ptA_y + ptB_y) / 2, 0);
                                    AlignedDimension dimLine = new AlignedDimension(ptA, ptB, dimPosition, protLine.Length.ToString(), myDimStyleId);

                                    btr.AppendEntity(dimLine);
                                    trans.AddNewlyCreatedDBObject(dimLine, true);
                                }
                                break;

                            case "Horizontal":
                                if (-1 < (ptA_y - ptB_y) && (ptA_y - ptB_y) < 1)
                                {

                                    Point3d dimPosition = new Point3d((ptA_x + ptB_x) / 2, position, 0);
                                    AlignedDimension dimLine = new AlignedDimension(ptA, ptB, dimPosition, protLine.Length.ToString(), myDimStyleId);

                                    btr.AppendEntity(dimLine);
                                    trans.AddNewlyCreatedDBObject(dimLine, true);
                                }
                                break;
                        }
                        
                    }
                }

                trans.Commit();

            }
        }


        [CommandMethod("FindConnected")] // 정확하게 점으로 연결된 선들만 인식;;
        public void FindConnected()
        {
            bool AreLinesConnected(Line line1, Line line2)
            {
                if (line1.StartPoint.IsEqualTo(line2.StartPoint) || line1.StartPoint.IsEqualTo(line2.EndPoint) ||
                line1.EndPoint.IsEqualTo(line2.StartPoint) || line1.EndPoint.IsEqualTo(line2.EndPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionOptions PPO = new PromptSelectionOptions();
                PromptSelectionResult PPR = ed.GetSelection(PPO);

                SelectionSet SS = PPR.Value;

                List<Line> LOL = new List<Line>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForRead);

                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        LOL.Add(line);
                    }
                }

                foreach (Line e in LOL)
                {
                    foreach (Line el2 in LOL)
                    {
                        if (e != el2) //자기 자신과 비교 제외
                        {
                            if(AreLinesConnected(e, el2))
                            {
                                ed.WriteMessage("\nAline: "+e.StartPoint.ToString()+e.EndPoint.ToString()+" // Bline: "+el2.StartPoint.ToString()+el2.EndPoint+ToString());
                            }
                        }
                    }
                }
            }
        }


        [CommandMethod("GetDistanceMethod")] // 라인들 사이의 거리를 측정합니다.
        public void GetDistanceMethod()
        {
            // Put your command code here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionOptions PPO = new PromptSelectionOptions();
                PromptSelectionResult PPR = ed.GetSelection(PPO);

                SelectionSet SS = PPR.Value;

                List<Line> LOL = new List<Line>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForRead);

                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        if(line.StartPoint.X == line.EndPoint.X)
                        {
                            LOL.Add(line);
                        }
                    }
                }

                foreach(Line e in LOL)
                {
                    List<double> distances = new List<double>();

                    foreach (Line el2 in LOL)
                    {
                        if(e != el2 && e.StartPoint.X != el2.StartPoint.X) //자기 자신과 비교 or 같은 축의 선 제외
                        {
                            distances.Add(e.GetClosestPointTo(el2.StartPoint, true).DistanceTo(el2.StartPoint));
                        }
                    }

                    double result = distances.Min();

                    ed.WriteMessage("\n" + result.ToString());
                }
            }
        }


        [CommandMethod("LineIntegrate")] // 모든 교차점을 찾아 출력합니다.
        public void LineIntegration() 
        {
            // Put your command code here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionOptions PPO = new PromptSelectionOptions();
                PromptSelectionResult PPR = ed.GetSelection(PPO);

                SelectionSet SS = PPR.Value;
                
                List<Line> LOL = new List<Line>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForRead);


                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        LOL.Add(line);
                    }
                }
                Point3dCollection points = new Point3dCollection();

                foreach(Line e in LOL)
                {
                    foreach (Line e2 in LOL)
                    {
                        if(e != e2)
                        {
                            e.IntersectWith(e2, Intersect.OnBothOperands, points, IntPtr.Zero, IntPtr.Zero);
                        }
                    }
                }

                foreach(Point3d pt in points)
                {
                    ed.WriteMessage("\n"+pt.ToString());
                }
            }

        }





        // Application Session Command with localized name
        [CommandMethod("GetScale")] // under construction
        public void GetScale()
        {
            // Put your command code here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            using(Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionOptions PPO = new PromptSelectionOptions();
                PromptSelectionResult PPR = ed.GetSelection(PPO);

                List<Point3d> LoP = new List<Point3d>();

                SelectionSet SS = PPR.Value;

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForRead);
                    
                    if(ent is Line)
                    {
                        Line line = (Line)ent;
                        Point3d point = line.StartPoint;
                        LoP.Add(point);
                        point = line.EndPoint;
                        LoP.Add(point);
                    }
                }




            }

            

        }

        // LispFunction is similar to CommandMethod but it creates a lisp 
        // callable function. Many return types are supported not just string
        // or integer.
        [LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        public int MyLispFunction(ResultBuffer args) // This method can have any name
        {
            // Put your command code here

            // Return a value to the AutoCAD Lisp Interpreter
            return 1;
        }

    }

}
