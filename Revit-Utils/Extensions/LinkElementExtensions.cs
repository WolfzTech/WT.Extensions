using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using WT.Revit.Models;

namespace WT.Revit.Extensions
{
    public static class LinkElementExtensions
    {
        public static IEnumerable<Element> GetInsulations(this IList<LinkElement> selectedElements, Document doc, View view)
        {
            var hostElementIds = selectedElements.Where(e => e.Id.HostElementId != ElementId.InvalidElementId).Select(e => e.Id.HostElementId);
            var hostElementIdHashSet = new HashSet<ElementId>(hostElementIds);

            var nativeElementInsulations = new FilteredElementCollector(doc, view.Id)
                .OfClass(typeof(InsulationLiningBase))
                .Cast<InsulationLiningBase>()
                .Where(i => hostElementIdHashSet.Contains(i.HostElementId))
                .ToList();

            var categoryFilter = new ElementMulticategoryFilter(new HashSet<BuiltInCategory>
                {
                    BuiltInCategory.OST_FabricationDuctworkInsulation,
                    BuiltInCategory.OST_FabricationPipeworkInsulation,
                    BuiltInCategory.OST_FabricationDuctworkLining,
            });

            var fabricationInsulations = new FilteredElementCollector(doc, view.Id)
                .WherePasses(categoryFilter)
                .Where(i => i.IsValidObject)
                .ToList();

            return nativeElementInsulations.Union(fabricationInsulations);
        }
    }
}
