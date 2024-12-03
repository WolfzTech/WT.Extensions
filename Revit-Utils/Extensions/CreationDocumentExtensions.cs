using Autodesk.Revit.DB;
using System.Drawing;
using Color = Autodesk.Revit.DB.Color;
using CreationDoc = Autodesk.Revit.Creation.Document;


namespace WT.Revit.Extensions
{
    public static class CreationDocumentExtensions
    {
        public static DetailCurve NewDetailLine(this CreationDoc creationDoc, XYZ startPoint, XYZ endPoint, View view = null,global::System.Drawing.Color? color = null)
        {
            view ??= RevitApp.ActiveView;

            if (!RevitApp.Doc.IsModifiable)
            {
                var trans = new Transaction(RevitApp.Doc, "Create Detail Line");
                trans.Start();

                var detailCurve = creationDoc.NewDetailCurve(view, Line.CreateBound(startPoint, endPoint).Project(view.SketchPlane));
                if (color != null)
                {
                    var overrideGraphic = view.GetElementOverrides(detailCurve.Id);
                    overrideGraphic.SetProjectionLineColor(new Color(color.Value.R, color.Value.G, color.Value.B));
                    view.SetElementOverrides(detailCurve.Id, overrideGraphic);
                }

                trans.Commit();
                return detailCurve;
            }
            else
            {
                var detailCurve = creationDoc.NewDetailCurve(view, Line.CreateBound(startPoint, endPoint).Project(view.SketchPlane.GetPlane()));
                if (color != null)
                {
                    var overrideGraphic = view.GetElementOverrides(detailCurve.Id);
                    overrideGraphic.SetProjectionLineColor(new Color(color.Value.R, color.Value.G, color.Value.B));
                    view.SetElementOverrides(detailCurve.Id, overrideGraphic);
                }
                return detailCurve;
            }
        }
    }
}
