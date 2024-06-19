using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WT.ExternalGraphics
{
    public class LineJig : DrawJigBase
    {
        public LineJig(UIApplication uiApplication) : base(uiApplication)
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

            var p1 = DrawingServer.BasePoint;
            var p2 = DrawingServer.NextPoint;

            if(p1.DistanceTo(p2)>.001)
            {
                DrawingServer.LineList.Add(Line.CreateBound(p1, p2));
            }
        }
    }
}
