using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;

namespace WT.ExternalGraphics
{
    public class RenderingPassBufferStorage
    {
        public RenderingPassBufferStorage(DisplayStyle displayStyle)
        {
            this.DisplayStyle = displayStyle;
            this.Meshes = new List<MeshInfo>();
            this.EdgeXYZs = new List<IList<XYZ>>();
        }

        public DisplayStyle DisplayStyle
        {
            get;
        }

        public VertexFormatBits FormatBits
        {
            get;
            set;
        }

        public List<MeshInfo> Meshes
        {
            get;
        }

        public List<IList<XYZ>> EdgeXYZs
        {
            get;
        }

        public int PrimitiveCount
        {
            get;
            set;
        }

        public int VertexBufferCount
        {
            get;
            set;
        }

        public int IndexBufferCount
        {
            get;
            set;
        }

        public VertexBuffer VertexBuffer
        {
            get;
            set;
        }

        public IndexBuffer IndexBuffer
        {
            get;
            set;
        }

        public VertexFormat VertexFormat
        {
            get;
            set;
        }

        public EffectInstance EffectInstance
        {
            get;
            set;
        }
    }
}