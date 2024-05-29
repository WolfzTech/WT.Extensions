using WT.UI.DetailMessage.ViewModel;
using WT.UI.DetailMessage.ViewUI;

namespace WT.UI.DetailMessage
{
    public class DetailMessage
    {
        public string Title
        {
            get => ViewModel?.Title; set
            {
                ViewModel.Title = value;
            }
        }

        public string Message
        {
            get => ViewModel?.Message; set
            {
                ViewModel.Message = value;
            }
        }

        public string Detail
        {
            get => ViewModel?.Detail; set
            {
               ViewModel.Detail = value;
            }
        }

        private DetailMessageUI Dialog { get; set; }

        private DetailMessageVM ViewModel { get; set; }

        public DetailMessage(string title, string message, string detail)
        {
            ViewModel = new DetailMessageVM();
            Dialog = new DetailMessageUI() { DataContext = ViewModel };

            Title = title;
            Message = message;
            Detail = detail;
        }

        public DetailMessage()
        {
            ViewModel = new DetailMessageVM();
            Dialog = new DetailMessageUI() { DataContext = ViewModel };
        }

        public void Show()
        {
            Dialog.Show();
        }

        public bool? ShowDialog()
        {
            return Dialog.ShowDialog();
        }

        public static bool? ShowDialog(string title, string message, string detail)
        {
            return new DetailMessage(title, message, detail).ShowDialog();
        }

        public static void Show(string title, string message, string detail)
        {
            new DetailMessage(title, message, detail).Show();
        }
    }
}
