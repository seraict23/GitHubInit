// (C) Copyright 2023 by  
//
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System;

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


        [CommandMethod("LineIntegrate")]
        public void LineIntegration() // This method can have any name
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

                foreach (Line el in LOL)
                {
                    foreach (Line el2 in LOL)
                    {
                        if (el.StartPoint.X == el2.StartPoint.X && el.EndPoint.X == el2.EndPoint.X)
                        {

                        }
                    }


                    
                }


            }



        }





        // Application Session Command with localized name
        [CommandMethod("GetScale")]
        public void GetScale() // This method can have any name
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
