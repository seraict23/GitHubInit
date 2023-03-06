// (C) Copyright 2023 by  
//
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(Winform_test01.MyCommands))]

namespace Winform_test01
{
    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
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
        [CommandMethod("ShowCommander")]
        public void MyCommand() // This method can have any name
        {

            MainForm mf = new MainForm();
            mf.Show();

            // Put your command code here

        }

        public ArrayList GetLayers()
        {
            ArrayList layers = new ArrayList();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                LayerTable lyTab = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (var ly in lyTab)
                {
                    LayerTableRecord lytr = trans.GetObject(ly, OpenMode.ForRead) as LayerTableRecord;
                    layers.Add(lytr.Name);
                }
            }

            return layers;
        }

        public ArrayList getLineTypes()
        {
            ArrayList linetypes = new ArrayList();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                LinetypeTable ltTab = (LinetypeTable)trans.GetObject(db.LinetypeTableId, OpenMode.ForRead);
                foreach (var lt in ltTab)
                {
                    LinetypeTableRecord lttr = trans.GetObject(lt, OpenMode.ForRead) as LinetypeTableRecord;
                    linetypes.Add(lttr.Name);
                }
            }

            return linetypes;
        }


        public ArrayList getTextStyles()
        {
            ArrayList textstyles = new ArrayList();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                TextStyleTable tsTab = (TextStyleTable)trans.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                foreach (var ts in tsTab)
                {
                    TextStyleTableRecord tstr = trans.GetObject(ts, OpenMode.ForRead) as TextStyleTableRecord;
                    textstyles.Add(tstr.Name);
                }
            }
             
            return textstyles;
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
