using System.Collections.Generic;

namespace Autodesk.Revit.DB.Structure
{
    public static class RebarExtensions
    {
        public static List<DirectShape> GetDirectShapeHosts(this List<Rebar> rebars)
        {
            List<DirectShape> directShapes = new List<DirectShape>();
            foreach (Rebar rebar in rebars)
            {
                if (rebar.GetDirectShapeHost() != null)
                {
                    directShapes.Add(rebar.GetDirectShapeHost());
                }
            }
            return directShapes;
        }
        
        public static DirectShape GetDirectShapeHost(this Rebar rebar)
        {
            Element host = rebar.GetHostId().Element(rebar.Document);
            if (host is DirectShape && string.IsNullOrEmpty(host.Name))
            {
                return host as DirectShape;
            }
            else return null;
        }
        
        public static RebarStyle RebarStyle(this Rebar rebar)
        {
            return (RebarStyle)rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_HOOK_STYLE).AsInteger();
        }
    }
}
