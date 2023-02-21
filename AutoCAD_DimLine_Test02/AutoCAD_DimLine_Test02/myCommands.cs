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

                BlockTable bt = (BlockTable)trans.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                List<Line> LOL = new List<Line>();

                foreach (SelectedObject SO in SS)
                {
                    Entity ent = (Entity)trans.GetObject(SO.ObjectId, OpenMode.ForRead);
                    if(ent is Line)
                    {
                        Line line = (Line)ent;
                        if(Tolerance.Equals(line.StartPoint.Y, line.EndPoint.Y))
                        {
                            LOL.Add(line);
                        }
                    }
                }

                List<Line> OutLines = new List<Line>();

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

                        }
                    }

                    if (flag = true)
                    {
                        OutLines.Add(l1);
                    }
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
