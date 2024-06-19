using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace WT.ExternalGraphics
{
    public class JigDrawingServer : DrawingServer
    {
        public List<Line> LineList { get; set; }
        public JigDrawingServer(Document doc) : base(doc)
        {
            LineList = new List<Line>();
        }

        public override string GetName()
        {
            return "IMPACT Jig Drawing Server";
        }

        public override string GetDescription()
        {
            return "IMPACT Jig Drawing Server";
        }

        public XYZ BasePoint { get; set; }

        public XYZ NextPoint { get; set; }

        public override List<Line> PrepareProfile()
        {
            return LineList;
        }

        public override bool CanExecute(View view)
        {
            return true;
        }

        public override Outline GetBoundingBox(View view)
        {
            if (LineList.Count > 0)
            {
                return new Outline(LineList[0].GetEndPoint(0), LineList[0].GetEndPoint(1));
            }

            return null;
        }
    }
}
