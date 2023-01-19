using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Autodesk.Revit.DB
{
    public static class ViewScheduleExtensions
    {
        public static List<Element> GetElements(this ViewSchedule viewSchedule)
        {
            List<Element> elements;
            Document doc = viewSchedule.Document;
            elements = Enumerable.ToList(new FilteredElementCollector(doc, viewSchedule.Id).WhereElementIsNotElementType());

            ScheduleDefinition scheduleDefinition = viewSchedule.Definition;
            IList<ScheduleFilter> scheduleFilters = scheduleDefinition.GetFilters();
            foreach (ScheduleFilter scheduleFilter in scheduleFilters)
            {
                ScheduleFilterType scheduleFilterType = scheduleFilter.FilterType;
                ScheduleFieldId scheduleFieldId = scheduleFilter.FieldId;
                ScheduleField scheduleField = scheduleDefinition.GetField(scheduleFieldId);
                string value = "";
                try
                {
                    value = scheduleFilter.GetStringValue();
                }
                catch (Exception) { }
                switch (scheduleFilterType)
                {
                    case ScheduleFilterType.Equal:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Equals(value)));
                        break;
                    case ScheduleFilterType.NotEqual:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Equals(value)));
                        break;
                    case ScheduleFilterType.Contains:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Contains(value)));
                        break;
                    case ScheduleFilterType.NotContains:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Contains(value)));
                        break;
                    case ScheduleFilterType.BeginsWith:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().StartsWith(value)));
                        break;
                    case ScheduleFilterType.NotBeginsWith:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().StartsWith(value)));
                        break;
                    case ScheduleFilterType.EndsWith:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().EndsWith(value)));
                        break;
                    case ScheduleFilterType.NotEndsWith:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().EndsWith(value)));
                        break;
#if !Revit2018&&!Revit2019&&!Revit2018C&&!Revit2019C
                    case ScheduleFilterType.HasValue:
                        elements = Enumerable.ToList(elements.Where(x => !string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString())));
                        break;
                    case ScheduleFilterType.HasNoValue:
                        elements = Enumerable.ToList(elements.Where(x => string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString())));
                        break;
                    default:
                        MessageBox.Show("Can't get elements" + "\n" + scheduleField.GetName());
                        break;

#endif
                }
            }
            return elements;
        }
    }
}
