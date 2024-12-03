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
    }
}
