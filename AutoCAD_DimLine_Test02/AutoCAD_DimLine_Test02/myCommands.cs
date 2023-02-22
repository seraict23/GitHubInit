// (C) Copyright 2023 by  
//
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(AutoCAD_DimLine_Test02.MyCommands))]

namespace AutoCAD_DimLine_Test02
{
    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {

        [CommandMethod("ExtendLines")]
        public void LineExtender()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            PromptSelectionOptions PSO = new PromptSelectionOptions();
            PromptSelectionResult PSR = ed.GetSelection(PSO);

            SelectionSet SS = PSR.Value;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                List<Point3d> LOP = new List<Point3d>();
                List<Double> LOY = new List<double>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = trans.GetObject(SO.ObjectId, OpenMode.ForRead) as Entity;

                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        LOP.Add(line.StartPoint);
                        LOP.Add(line.EndPoint);
                        LOY.Add(line.StartPoint.Y);
                        LOY.Add(line.EndPoint.Y);
                    }
                }

                Double UpperY = LOY.Max() + ((LOY.Max() - LOY.Min()) * 0.15);
                Double UnderY = LOY.Min() - ((LOY.Max() - LOY.Min()) * 0.15);

                List<double> Result = new List<double>();


                foreach (SelectedObject SO in SS)
                {
                    Entity ent = trans.GetObject(SO.ObjectId, OpenMode.ForRead) as Entity;

                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        Line theLine = new Line();

                        if (Tolerance.Equals(line.StartPoint.X, line.EndPoint.X))
                        {
                            theLine = new Line(new Point3d(line.StartPoint.X, UpperY, 0), new Point3d(line.EndPoint.X, UnderY, 0));
                            theLine.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 255);

                            List<Double> ptYOntheLine = new List<Double>();
                            foreach (Point3d pt in LOP)
                            {

                                if (theLine.GetClosestPointTo(pt, false).DistanceTo(pt) < 0.001)
                                {
                                    ptYOntheLine.Add(pt.Y);
                                }
                            }
                            double resultptY = ptYOntheLine.Max();
                            Result.Add(resultptY);


                            ed.WriteMessage("\n" + resultptY.ToString());
                        }
                        /*                        else if(Tolerance.Equals(line.StartPoint.Y, line.EndPoint.Y))
                                                {

                                                }*/

                        btr.AppendEntity(theLine);
                        trans.AddNewlyCreatedDBObject(theLine, true);

                    }
                }
                trans.Commit();
            }
        }


        [CommandMethod("FindOutLines")]
        public void FindOutLines()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            PromptSelectionOptions PSO = new PromptSelectionOptions();
            PromptSelectionResult PSR = ed.GetSelection(PSO);

            SelectionSet SS = PSR.Value;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)trans.GetObject(doc.Database.BlockTableId, OpenMode.ForWrite);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);




                // DimStyleSetup - start
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
                ObjectId myDimStyleId = DSTR.ObjectId;
                // DimStyleSetup - fin


                List<Double> LOPY = new List<double>(); // to make a scale
                List<Double> LOPX = new List<double>();

                List<Line> LOL = new List<Line>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForWrite);
                    if(ent is Line)
                    {
                        Line line = (Line)ent;
                        if(Tolerance.Equals(line.StartPoint.Y, line.EndPoint.Y))
                        {
                            LOL.Add(line);
                            LOPY.Add(line.StartPoint.Y);
                            LOPY.Add(line.EndPoint.Y);
                            LOPX.Add(line.StartPoint.X);
                            LOPX.Add(line.EndPoint.X);
                        }
                    }
                }


                foreach (Line l1 in LOL)
                {
                    bool flag = true;
                    foreach(Line l2 in LOL)
                    {
                        if(Tolerance.Equals(l1, l2))
                        {

                        } 
                        else if(l1.StartPoint.Y < l2.StartPoint.Y)
                        {
                            if (new double[] { l1.StartPoint.X, l1.EndPoint.X }.Max() <= new double[] { l2.StartPoint.X, l2.EndPoint.X }.Min() || new double[] { l2.StartPoint.X, l2.EndPoint.X }.Max() <= new double[] { l1.StartPoint.X, l1.EndPoint.X }.Min() )
                            {
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }

                    if (flag)
                    {
                        ed.WriteMessage("\n" + l1.StartPoint.X + "~" + l1.EndPoint.X);



                        // Draw DimLine
                        Point3d dimPosition = new Point3d((l1.StartPoint.X + l1.EndPoint.X) / 2, (LOPY.Max()-LOPY.Min())*0.15+LOPY.Max(), 0);
                        AlignedDimension dimLine = new AlignedDimension(l1.StartPoint, l1.EndPoint, dimPosition, l1.Length.ToString(), myDimStyleId);

                        btr.AppendEntity(dimLine);
                        trans.AddNewlyCreatedDBObject(dimLine, true);

                    }
                }

                trans.Commit();

            }

        }


        [CommandMethod("DimLineOuter")]
        public void DimLineOuter()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            PromptSelectionOptions PSO = new PromptSelectionOptions();
            PromptSelectionResult PSR = ed.GetSelection(PSO);

            SelectionSet SS = PSR.Value;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)trans.GetObject(doc.Database.BlockTableId, OpenMode.ForWrite);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);




                // DimStyleSetup - start
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
                ObjectId myDimStyleId = DSTR.ObjectId;
                // DimStyleSetup - fin


                List<Double> LOPY = new List<double>(); // to make a scale
                List<Double> LOPX = new List<double>();
                List<Point3d> LOP = new List<Point3d>();

                List<Line> LOL = new List<Line>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForWrite);
                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        if (Tolerance.Equals(line.StartPoint.Y, line.EndPoint.Y))
                        {
                            LOL.Add(line);
                            LOPY.Add(line.StartPoint.Y);
                            LOPY.Add(line.EndPoint.Y);
                            LOPX.Add(line.StartPoint.X);
                            LOPX.Add(line.EndPoint.X);
                            LOP.Add(line.StartPoint);
                            LOP.Add(line.EndPoint);

                        }
                    }
                }


                Point3d lowPoint = new Point3d();
                Point3d highPoint = new Point3d();

                foreach (Point3d pt in LOP)
                {
                    if(pt.X == LOPX.Min())
                    {
                        lowPoint = pt;
                    }
                    if(pt.X == LOPX.Max())
                    {
                        highPoint = pt;
                    }
                }


                Line l3 = new Line(lowPoint, highPoint);
                Point3d dimPosition_outer = new Point3d((l3.StartPoint.X + l3.EndPoint.X) / 2, (LOPY.Max() - LOPY.Min()) * 0.25 + LOPY.Max(), 0);
                AlignedDimension dimLine_outer = new AlignedDimension(l3.StartPoint, l3.EndPoint, dimPosition_outer, l3.Length.ToString(), myDimStyleId);

                RotatedDimension rDim = new RotatedDimension(0, lowPoint, highPoint, dimPosition_outer, l3.Length.ToString(), myDimStyleId);

                btr.AppendEntity(rDim);
                trans.AddNewlyCreatedDBObject(rDim, true);

                trans.Commit();

            }
        }

        [CommandMethod("DimLineMaker")] 

        public void DimLineMaker()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;

            ed = doc.Editor;

            PromptSelectionOptions PSO = new PromptSelectionOptions();
            PromptSelectionResult PSR = ed.GetSelection(PSO);

            if(PSR.Status != PromptStatus.OK)
            {
                return;
            }

            SelectionSet SS = PSR.Value;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)trans.GetObject(doc.Database.BlockTableId, OpenMode.ForWrite);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);




                // DimStyleSetup - start
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
                ObjectId myDimStyleId = DSTR.ObjectId;
                // DimStyleSetup - fin


                // set lists and veriables
                List<Double> LOPY = new List<double>(); // to make a scale
                List<Double> LOPX = new List<double>();
                List<Point3d> LOP = new List<Point3d>();
                List<Line> LOL = new List<Line>();
                Point3d lowPoint = new Point3d();
                Point3d highPoint = new Point3d();
                //



                // dimline H - start
                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForWrite);
                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        if (Tolerance.Equals(line.StartPoint.Y, line.EndPoint.Y))
                        {
                            LOL.Add(line);
                            LOPY.Add(line.StartPoint.Y);
                            LOPY.Add(line.EndPoint.Y);
                            LOPX.Add(line.StartPoint.X);
                            LOPX.Add(line.EndPoint.X);
                            LOP.Add(line.StartPoint);
                            LOP.Add(line.EndPoint);
                        }
                    }
                }

                foreach (Line l1 in LOL)
                {
                    bool flag = true;
                    foreach (Line l2 in LOL)
                    {
                        if (Tolerance.Equals(l1, l2))
                        {

                        }
                        else if (l1.StartPoint.Y < l2.StartPoint.Y)
                        {
                            if (new double[] { l1.StartPoint.X, l1.EndPoint.X }.Max() <= new double[] { l2.StartPoint.X, l2.EndPoint.X }.Min() || new double[] { l2.StartPoint.X, l2.EndPoint.X }.Max() <= new double[] { l1.StartPoint.X, l1.EndPoint.X }.Min())
                            {
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }

                    if (flag)
                    {
                        // Draw DimLine
                        Point3d dimPosition = new Point3d((l1.StartPoint.X + l1.EndPoint.X) / 2, (LOPY.Max() - LOPY.Min()) * 0.15 + LOPY.Max(), 0);
                        AlignedDimension dimLine = new AlignedDimension(l1.StartPoint, l1.EndPoint, dimPosition, l1.Length.ToString(), myDimStyleId);

                        btr.AppendEntity(dimLine);
                        trans.AddNewlyCreatedDBObject(dimLine, true);
                    }
                }
                // DimlineH - end



                // OuterDimLineH -start
                foreach (Point3d pt in LOP)
                {
                    if (pt.X == LOPX.Min())
                    {
                        lowPoint = pt;
                    }
                    if (pt.X == LOPX.Max())
                    {
                        highPoint = pt;
                    }
                }

                Line l3 = new Line(lowPoint, highPoint);
                Point3d dimPosition_outer = new Point3d((l3.StartPoint.X + l3.EndPoint.X) / 2, (LOPY.Max() - LOPY.Min()) * 0.25 + LOPY.Max(), 0);
                AlignedDimension dimLine_outer = new AlignedDimension(l3.StartPoint, l3.EndPoint, dimPosition_outer, l3.Length.ToString(), myDimStyleId);

                RotatedDimension rDim = new RotatedDimension(0, lowPoint, highPoint, dimPosition_outer, l3.Length.ToString(), myDimStyleId);

                btr.AppendEntity(rDim);
                trans.AddNewlyCreatedDBObject(rDim, true);
                // OuterDimLineH -end



                // empty the lists
                LOP.Clear();
                LOPX.Clear();
                LOPY.Clear();
                LOL.Clear();
                //


                // DimLine V - start
                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForWrite);
                    if (ent is Line)
                    {
                        Line line = (Line)ent;
                        if (Tolerance.Equals(line.StartPoint.X, line.EndPoint.X))
                        {
                            LOL.Add(line);
                            LOPY.Add(line.StartPoint.Y);
                            LOPY.Add(line.EndPoint.Y);
                            LOPX.Add(line.StartPoint.X);
                            LOPX.Add(line.EndPoint.X);
                            LOP.Add(line.StartPoint);
                            LOP.Add(line.EndPoint);
                        }
                    }
                }

                foreach (Line l1 in LOL)
                {
                    bool flag = true;
                    foreach (Line l2 in LOL)
                    {
                        if (Tolerance.Equals(l1, l2))
                        {

                        }
                        else if (l1.StartPoint.X < l2.StartPoint.X)
                        {
                            if (new double[] { l1.StartPoint.Y, l1.EndPoint.Y }.Max() <= new double[] { l2.StartPoint.Y, l2.EndPoint.Y }.Min() || new double[] { l2.StartPoint.Y, l2.EndPoint.Y }.Max() <= new double[] { l1.StartPoint.Y, l1.EndPoint.Y }.Min())
                            {
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }

                    if (flag)
                    {
                        // Draw DimLine
                        Point3d dimPosition = new Point3d((LOPX.Max() - LOPX.Min()) * 0.15 + LOPX.Max(), (l1.StartPoint.Y + l1.EndPoint.Y) / 2, 0);
                        AlignedDimension dimLine = new AlignedDimension(l1.StartPoint, l1.EndPoint, dimPosition, l1.Length.ToString(), myDimStyleId);

                        btr.AppendEntity(dimLine);
                        trans.AddNewlyCreatedDBObject(dimLine, true);
                    }
                }
                // DimLine V - end



                // outerDimLineY -start
                foreach (Point3d pt in LOP)
                {
                    if (pt.Y == LOPY.Min())
                    {
                        lowPoint = pt;
                    }
                    if (pt.Y == LOPY.Max())
                    {
                        highPoint = pt;
                    }
                }

                l3 = new Line(lowPoint, highPoint);
                dimPosition_outer = new Point3d((l3.StartPoint.Y + l3.EndPoint.Y) / 2, (LOPX.Max() - LOPX.Min()) * 0.25 + LOPY.Max(), 0);
                dimLine_outer = new AlignedDimension(l3.StartPoint, l3.EndPoint, dimPosition_outer, l3.Length.ToString(), myDimStyleId);

                rDim = new RotatedDimension(-90, lowPoint, highPoint, dimPosition_outer, l3.Length.ToString(), myDimStyleId);

                btr.AppendEntity(rDim);
                trans.AddNewlyCreatedDBObject(rDim, true);
                // outerDimLineY -end


                trans.Commit();

            }
        }




        [CommandMethod("StretchLine")]
        public void StretchLine()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult PSR = (PromptSelectionResult)ed.GetSelection();

            SelectionSet SS = PSR.Value;

            if (PSR.Status != PromptStatus.OK) return;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                GripDataCollection gripData = new GripDataCollection();

                foreach (SelectedObject SO in SS)
                {
                    Line line = (Line)trans.GetObject(SO.ObjectId, OpenMode.ForWrite);

                    
                    line.GetGripPoints(gripData, 0, 0, ed.GetCurrentView().ViewDirection, GetGripPointsFlags.GripPointsOnly);

                    // PromptDragOptions PDT = new PromptDragOptions(SS, "", );
                }
                // Get the selected line and its grip points
                foreach(GripData grip in gripData)
                {
                    ed.WriteMessage("\n"+grip.GripPoint.ToString());
                }

            }
         
        }



        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("MyGroup", "MyCommand", "MyCommandLocal", CommandFlags.Modal)]
        public void MyCommand() // This method can have any name
        {
            // Put your command code here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;
            if (doc != null)
            {
                ed = doc.Editor;
                ed.WriteMessage("Hello, this is your first command.");

            }
        }

        // Modal Command with pickfirst selection
        [CommandMethod("MyGroup", "MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void MyPickFirst() // This method can have any name
        {
            PromptSelectionResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetSelection();
            if (result.Status == PromptStatus.OK)
            {
                // There are selected entities
                // Put your command using pickfirst set code here
            }
            else
            {
                // There are no selected entities
                // Put your command code here
            }
        }

        // Application Session Command with localized name
        [CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        public void MySessionCmd() // This method can have any name
        {
            // Put your command code here
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
