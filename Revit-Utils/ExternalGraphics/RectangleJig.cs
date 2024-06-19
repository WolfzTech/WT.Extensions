using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WT.ExternalGraphics
{
    public class RectangleJig : DrawJigBase
    {
        public RectangleJig(UIApplication uiApplication) : base(uiApplication)
        {
        }

        public override void DrawJig()
        {
            DrawingServer.LineList.Clear();

            if (DrawingServer == null ||
                DrawingServer.BasePoint == null ||
                DrawingServer.NextPoint == null ||
                DrawingServer.BasePoint.DistanceTo(DrawingServer.NextPoint) <= 0.001)
            {
                return;
            }

            var points = GetCornerPoints();
            if (points != null)
            {
                points.Add(points[0]);
                var lpt = points[0];
                for (var k = 1; k < points.Count; k++)
                {
                    var cpt = points[k];
                    if (lpt.DistanceTo(cpt) > 0.001)
                    {
                        DrawingServer.LineList.Add(Line.CreateBound(lpt, cpt));
                    }

                    lpt = cpt;
                }
            }
        }

        private List<XYZ> GetCornerPoints()
        {
            if (DrawingServer.BasePoint == null || DrawingServer.NextPoint == null)
            {
                return null;
            }

            var mpt = (DrawingServer.BasePoint + DrawingServer.NextPoint) * 0.5;
            var currView = HostApplication.ActiveUIDocument.Document.ActiveView;
            var plane = Plane.CreateByNormalAndOrigin(currView.RightDirection, mpt);
            var mirrorMat = Transform.CreateReflection(plane);

            var p1 = DrawingServer.BasePoint;
            var p2 = mirrorMat.OfPoint(p1);
            var p3 = DrawingServer.NextPoint;
            var p4 = mirrorMat.OfPoint(p3);

            return new List<XYZ> {p1, p2, p3, p4};
        }
    }
}
