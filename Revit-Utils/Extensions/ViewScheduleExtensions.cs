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
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().Equals(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Equals(value)));
#endif
                        break;
                    case ScheduleFilterType.NotEqual:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().Equals(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Equals(value)));
#endif
                        break;
                    case ScheduleFilterType.Contains:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().Contains(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Contains(value)));
#endif
                        break;
                    case ScheduleFilterType.NotContains:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().Contains(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Contains(value)));
#endif
                        break;
                    case ScheduleFilterType.BeginsWith:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().StartsWith(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().StartsWith(value)));
#endif
                        break;
                    case ScheduleFilterType.NotBeginsWith:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().StartsWith(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().StartsWith(value)));
#endif
                        break;
                    case ScheduleFilterType.EndsWith:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().EndsWith(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().EndsWith(value)));
#endif
                        break;
                    case ScheduleFilterType.NotEndsWith:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString().EndsWith(value)));
#else
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().EndsWith(value)));
#endif
                        break;
                    case ScheduleFilterType.HasValue:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => !string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString())));
#else
                        elements = Enumerable.ToList(elements.Where(x => !string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString())));
#endif
                        break;
                    case ScheduleFilterType.HasNoValue:
#if R24_OR_GREATER
                        elements = Enumerable.ToList(elements.Where(x => string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.Value).AsString())));
#else
                        elements = Enumerable.ToList(elements.Where(x => string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString())));
#endif
                        break;
                    default:
                        MessageBox.Show("Can't get elements" + "\n" + scheduleField.GetName());
                        break;
                }
            }
            return elements;
        }
    }
}
