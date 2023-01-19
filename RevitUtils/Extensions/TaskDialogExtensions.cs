namespace Autodesk.Revit.UI
{
    public static class TaskDialogExtensions
    {
        public static TaskDialogResult Show(this TaskDialog taskDialog, string Message, string Detail, TaskDialogIcon taskDialogIcon)
        {
            taskDialog.MainInstruction = Message;
            taskDialog.ExpandedContent = Detail;
            taskDialog.MainIcon = taskDialogIcon;
            return taskDialog.Show();
        }
        public static TaskDialogResult Show(this TaskDialog taskDialog, string Message, string Detail)
        {
            taskDialog.MainInstruction = Message;
            taskDialog.ExpandedContent = Detail;
            return taskDialog.Show();
        }
    }
}
