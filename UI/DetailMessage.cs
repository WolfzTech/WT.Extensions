using System;
using System.Windows.Forms;

namespace WT
{
    public class DetailMessage
    {
        public static DialogResult Show(string Title, string Message, string Detail)
        {
            // Get reference to the dialog type.
            var dialogTypeName = "System.Windows.Forms.PropertyGridInternal.GridErrorDlg";
            var dialogType = typeof(Form).Assembly.GetType(dialogTypeName);

            // Create dialog instance.
            var dialog = (Form)Activator.CreateInstance(dialogType, new PropertyGrid());

            // Populate relevant properties on the dialog instance.
            dialog.Text = Title;
            dialogType.GetProperty("Details").SetValue(dialog, Detail, null);
            dialogType.GetProperty("Message").SetValue(dialog, Message, null);

            // Display dialog.
            return dialog.ShowDialog();
        }
    }
}
