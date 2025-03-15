using Autodesk.Revit.UI.Selection;

namespace WT.Revit.SelectionFilter
{
    public class SelectionFilter
    {
        public static ISelectionFilter Room
        {
            get { return new RoomSelectionFilter(); }
        }

        public static ISelectionFilter Door
        {
            get { return new DoorSelectionFilter(); }
        }

        public static ISelectionFilter DetailLine
        {
            get { return new DetailLineSelectionFilter(); }
        }

        public static ISelectionFilter DetailCurve
        {
            get { return new DetailCurveSelectionFilter(); }
        }

        public static ISelectionFilter ModelLine
        {
            get { return new ModelLineSelectionFilter(); }
        }
    }
}
