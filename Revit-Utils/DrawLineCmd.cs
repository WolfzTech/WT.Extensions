using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using WT.ExternalGraphics;

namespace WT.Revit
{
    [Transaction(TransactionMode.Manual)]
    internal class DrawLineCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (commandData.Application.ActiveUIDocument == null)
            {
                return Result.Succeeded;
            }

            var uiDocument = commandData.Application.ActiveUIDocument;
            var document = uiDocument.Document;
            XYZ p1 = null;
            try
            {
                p1 = uiDocument.Selection.PickPoint("Pick Base Point:");
            }
            catch (Exception)
            {
                //
            }

            if (p1 == null)
            {
                return Result.Succeeded;
            }

            XYZ p2 = null;
            LineJig lineJig = null;
            try
            {
                lineJig = new LineJig(commandData.Application) { DrawingServer = { BasePoint = p1 } };
                lineJig.DrawJig();

                p2 = uiDocument.Selection.PickPoint("Pick Next Point:");
            }
            catch (Exception)
            {
                //
            }
            finally
            {
                if (lineJig != null)
                {
                    lineJig.Dispose();
                }
            }

            if (p2 == null)
            {
                return Result.Succeeded;
            }

            using (var trans = new Transaction(document, "Draw Rectangle"))
            {
                trans.Start();
                CreateModelLine(document, p1, p2);

                trans.Commit();
            }

            return Result.Succeeded;
        }

        public static ModelCurve CreateModelLine(Document doc, XYZ p, XYZ q)
        {
            if (p.DistanceTo(q) < doc.Application.ShortCurveTolerance)
            {
                return null;
            }

            var v = q - p;
            var dxy = Math.Abs(v.X) + Math.Abs(v.Y);
            var w = (dxy > doc.Application.ShortCurveTolerance)
                ? XYZ.BasisZ
                : XYZ.BasisY;

            var norm = v.CrossProduct(w)
                .Normalize();
            var plane = Plane.CreateByNormalAndOrigin(norm, p);
            var sketchPlane = SketchPlane.Create(doc, plane);
            var line = Line.CreateBound(p, q);
            var curve = doc.IsFamilyDocument
                ? doc.FamilyCreate.NewModelCurve(line, sketchPlane)
                : doc.Create.NewModelCurve(line, sketchPlane);

            return curve;
        }
    }
}
