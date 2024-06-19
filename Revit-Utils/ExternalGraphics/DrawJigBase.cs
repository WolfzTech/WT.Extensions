using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WT.ExternalGraphics
{
    public abstract class DrawJigBase : IDisposable
    {
        public UIApplication HostApplication { get; set; }
        public JigDrawingServer DrawingServer { get; set; }
        public UserActivityHook UserActivityHook { get; set; }
        public List<XYZ> PickedPoints { get; set; }
        public string UserInput { get; set; }

        private IntPtr revitWindow = IntPtr.Zero;
        protected DrawJigBase(UIApplication uiApplication)
        {
            UserInput = "";
            PickedPoints = new List<XYZ>();
            HostApplication = uiApplication;
            UserActivityHook = new UserActivityHook(true, true);
            UserActivityHook.OnMouseActivity += OnMouseActivity;
            UserActivityHook.KeyPress += OnKeyPressActivity;

            DrawingServer = new JigDrawingServer(HostApplication.ActiveUIDocument.Document);
            var externalGraphics = new DrawingServerHost();
            externalGraphics.RegisterServer(DrawingServer);

            revitWindow = GetActiveWindow();
        }

        public abstract void DrawJig();

        public virtual void OnKeyPressActivity(object sender, KeyPressEventArgs e)
        {
            var topWindow = GetActiveWindow();
            if (revitWindow != topWindow)
            {
                e.Handled = false;
                return;
            }

            if (DrawingServer != null && e.KeyChar == 27 && DrawingServer.BasePoint != null)
            {
                DrawingServer.BasePoint = null;
                DrawingServer.NextPoint = null;
                UserInput = "";
            }
            else if (DrawingServer != null && e.KeyChar == 27)
            {
                StopJig();
            }
            if (DrawingServer != null && e.KeyChar == 8 && UserInput.Length > 0)
            {
                UserInput = UserInput.Substring(0, UserInput.Length - 1);
            }
            else
            {
                if (char.IsLetterOrDigit(e.KeyChar))
                {
                    UserInput += e.KeyChar.ToString();
                }
            }

            e.Handled = false;
        }

        public void StopJig()
        {
            if (UserActivityHook != null)
            {
                UserActivityHook.OnMouseActivity -= OnMouseActivity;
                UserActivityHook.KeyPress -= OnKeyPressActivity;
                UserActivityHook.Stop();
            }

            if (DrawingServer != null)
            {
                DrawingServer.BasePoint = null;
                DrawingServer.NextPoint = null;

                var externalGraphics = new DrawingServerHost();
                externalGraphics.UnRegisterServer(DrawingServer.Document);

                DrawingServer = null;
            }
        }

        public virtual void OnMouseActivity(object sender, MouseEventArgs e)
        {
            var topWindow = GetActiveWindow();
            if (revitWindow != topWindow)
            {
                return;
            }

            try
            {
                var currPoint = GetMousePoint();
                //add points to the list:
                if (PickedPoints != null && e.Clicks > 0 && e.Button == MouseButtons.Left)
                {
                    PickedPoints.Add(currPoint);
                }

                if (DrawingServer.BasePoint == null && e.Clicks > 0 && e.Button == MouseButtons.Left)
                {
                    //start server
                    DrawingServer.BasePoint = currPoint;
                }
                else if (DrawingServer != null && e.Clicks > 0 && e.Button == MouseButtons.Left)
                {
                    DrawingServer.BasePoint = currPoint;
                    DrawingServer.NextPoint = null;
                }
                else if (DrawingServer != null)
                {
                    //mouse is moving
                    if (DrawingServer.NextPoint != null)
                    {
                        if (currPoint.DistanceTo(DrawingServer.NextPoint) > 0.01)
                        {
                            DrawingServer.NextPoint = currPoint;
                            DrawJig();
                            HostApplication.ActiveUIDocument.RefreshActiveView();
                        }
                    }
                    else
                    {
                        DrawingServer.NextPoint = currPoint;
                        DrawJig();
                        HostApplication.ActiveUIDocument.RefreshActiveView();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private UIView GetActiveUiView(UIDocument uidoc)
        {
            var doc = uidoc.Document;
            var view = doc.ActiveView;
            var uiviews = uidoc.GetOpenUIViews();
            UIView uiview = null;

            foreach (var uv in uiviews)
            {
                if (uv.ViewId.Equals(view.Id))
                {
                    uiview = uv;
                    break;
                }
            }
            return uiview;
        }

        protected XYZ GetMousePoint()
        {
            var uiView = GetActiveUiView(HostApplication.ActiveUIDocument);
            var corners = uiView.GetZoomCorners();
            var rect = uiView.GetWindowRectangle();
            var p = Cursor.Position;
            var dx = (double)(p.X - rect.Left) / (rect.Right - rect.Left);
            var dy = (double)(p.Y - rect.Bottom) / (rect.Top - rect.Bottom);
            var a = corners[0];
            var b = corners[1];
            var v = b - a;
            var q = a
                    + dx * v.X * XYZ.BasisX
                    + dy * v.Y * XYZ.BasisY;

            return q;
        }

        public void Dispose()
        {
            StopJig();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
    }
}
