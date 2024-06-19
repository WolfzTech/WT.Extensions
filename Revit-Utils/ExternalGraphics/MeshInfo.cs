using Autodesk.Revit.DB;

namespace WT.ExternalGraphics
{
    public class MeshInfo
    {
        public readonly ColorWithTransparency ColorWithTransparency;

        public readonly Mesh Mesh;

        public readonly XYZ Normal;

        public MeshInfo(Mesh mesh, XYZ normal, ColorWithTransparency color)
        {
          Mesh = mesh;
          Normal = normal;
          ColorWithTransparency = color;
        }
    }
}