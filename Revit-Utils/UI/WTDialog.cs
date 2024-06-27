using Autodesk.Revit.UI;

namespace WT.Revit.UI
{
    public class WTDialog
    {
        public static TaskDialogResult Show(string title, string mainInstruction, string detailMessage, TaskDialogIcon taskDialogIcon)
        {
            TaskDialog taskDialog = new(title)
            {
                MainInstruction = mainInstruction,
                ExpandedContent = detailMessage,
                MainIcon = taskDialogIcon
            };
            return taskDialog.Show();
        }

        public static TaskDialogResult Show(string title, string mainInstruction, string detailMessage)
        {
            TaskDialog taskDialog = new(title)
            {
                MainInstruction = mainInstruction,
                ExpandedContent = detailMessage
            };
            return taskDialog.Show();
        }
    }
}
