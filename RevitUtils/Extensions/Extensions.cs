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

namespace System.Collections.Generic
{
    public static class IEnumratorExtension
    {
        public static List<T> ToList<T>(this IEnumerator<T> e)
        {
            var list = new List<T>();
            while (e.MoveNext())
            {
                list.Add(e.Current);
            }
            return list;
        }
        
        public static void Add<T>(this IList<T> e,List<T> list)
        {
            foreach(T item in list)
            {
                e.Add(item);
            }
        }
        
        public static void Add<T>(this IList<T> e, IEnumerable<T> list)
        {
            foreach (T item in list)
            {
                e.Add(item);
            }
        }
    }
}
namespace System
{
    public static class DateTimeExtension
    {
        public static string ToSQLString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}