using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace WT.Revit
{
    public static class RevitApp
    {
        public static void Init(UIApplication uiApp)
        {
            _uiApp = uiApp;
        }

        public static void Init(ExternalCommandData commandData)
        {
            Init(commandData.Application);
        }

        public static void Init(UIControlledApplication uiCApplication)
        {
            // var versionNumber = uiCApplication.ControlledApplication.VersionNumber;
            //var fieldName = versionNumber == "2019" ? "m_uiapplication" : "m_application";
            var fieldName = "m_uiapplication";
            var fieldInfo = uiCApplication.GetType().GetField(
                fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var uiApp = fieldInfo.GetValue(uiCApplication) as UIApplication;
            Init(uiApp);
        }

        private static UIApplication _uiApp;

        public static UIApplication UiApp => _uiApp;

        public static Application App => UiApp.Application;

        public static UIDocument UiDoc => UiApp.ActiveUIDocument;

        public static Document Doc => UiDoc.Document;

        public static Selection Selection => UiDoc.Selection;

        public static View ActiveView => Doc.ActiveView;

        public static Transaction NewTransaction(string name, Document doc = null)
        {
            if (doc == null)
            {
                return new Transaction(Doc, name);
            }
            else
            {
                return new Transaction(doc, name);
            }
        }

        public static TransactionGroup NewTransactionGroup(string name, Document doc = null)
        {
            if (doc == null)
            {
                return new TransactionGroup(Doc, name);
            }
            else
            {
                return new TransactionGroup(doc, name);
            }
        }

        public static LanguageType Language => App.Language;

        public static List<Element> FilteredElementCollector(BuiltInCategory category)
        {
            return new FilteredElementCollector(Doc).OfCategory(category).Cast<Element>().ToList();
        }

        public static List<T> FilteredElementCollector<T>(BuiltInCategory category) where T : Element
        {
            return new FilteredElementCollector(Doc).OfClass(typeof(T)).OfCategory(category).Cast<T>().ToList();
        }

        public static List<T> FilteredElementCollector<T>(BuiltInCategory category, bool whereElementIsElementType) where T : Element
        {
            if (whereElementIsElementType)
            {
                return new FilteredElementCollector(Doc).OfClass(typeof(T)).OfCategory(category).WhereElementIsElementType().Cast<T>().ToList();
            }
            else
            {
                return new FilteredElementCollector(Doc).OfClass(typeof(T)).OfCategory(category).WhereElementIsNotElementType().Cast<T>().ToList();
            }
        }

        public static List<T> FilteredElementCollector<T>() where T : Element
        {
            return new FilteredElementCollector(Doc).OfClass(typeof(T)).Cast<T>().ToList();
        }

        public static List<T> FilteredElementCollector<T>(bool whereElementIsElementType) where T : Element
        {
            if (whereElementIsElementType)
            {
                return new FilteredElementCollector(Doc).OfClass(typeof(T)).WhereElementIsElementType().Cast<T>().ToList();
            }
            else
            {
                return new FilteredElementCollector(Doc).OfClass(typeof(T)).WhereElementIsNotElementType().Cast<T>().ToList();
            }
        }

        public static List<T> ViewFilteredElementCollector<T>(View view, BuiltInCategory category) where T : Element
        {
            return new FilteredElementCollector(Doc, view.Id).OfClass(typeof(T)).OfCategory(category).Cast<T>().ToList();
        }

        public static List<T> ViewFilteredElementCollector<T>(View view, BuiltInCategory category, bool whereElementIsElementType) where T : Element
        {
            var elements = new FilteredElementCollector(Doc, view.Id).OfClass(typeof(T)).OfCategory(category);
            if (whereElementIsElementType)
            {
                return elements.WhereElementIsElementType().Cast<T>().ToList();
            }
            else
            {
                return elements.WhereElementIsNotElementType().Cast<T>().ToList();
            }
        }

        public static List<T> ActiveViewFilteredElementCollector<T>(BuiltInCategory category) where T : Element
        {
            return new FilteredElementCollector(Doc, Doc.ActiveView.Id).OfClass(typeof(T)).OfCategory(category).Cast<T>().ToList();
        }

        public static List<T> ActiveViewFilteredElementCollector<T>(BuiltInCategory category, bool whereElementIsElementType) where T : Element
        {
            if (whereElementIsElementType)
            {
                return new FilteredElementCollector(Doc, Doc.ActiveView.Id).OfClass(typeof(T)).OfCategory(category).WhereElementIsElementType().Cast<T>().ToList();
            }
            else
            {
                return new FilteredElementCollector(Doc, Doc.ActiveView.Id).OfClass(typeof(T)).OfCategory(category).WhereElementIsNotElementType().Cast<T>().ToList();
            }
        }

        public static List<T> SelectionElementCollector<T>(BuiltInCategory category) where T : Element
        {
            return new FilteredElementCollector(Doc, Selection.GetElementIds()).OfClass(typeof(T)).OfCategory(category).Cast<T>().ToList();
        }

        public static List<T> SelectionElementCollector<T>(BuiltInCategory category, bool whereElementIsElementType) where T : Element
        {
            if (whereElementIsElementType)
            {
                return new FilteredElementCollector(Doc, Selection.GetElementIds()).OfClass(typeof(T)).OfCategory(category).WhereElementIsElementType().Cast<T>().ToList();
            }
            else
            {
                return new FilteredElementCollector(Doc, Selection.GetElementIds()).OfClass(typeof(T)).OfCategory(category).WhereElementIsNotElementType().Cast<T>().ToList();
            }
        }
    }
}
