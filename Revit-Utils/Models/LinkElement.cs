using Autodesk.Revit.DB;

namespace WT.Revit.Models
{
        public class LinkElement
        {
            public LinkElement(Element element, LinkElementId id)
            {
                Element = element;
                Id = id;
            }

            public LinkElement(Element element, ElementId linkInstanceId)
                : this(element, new LinkElementId(linkInstanceId, element.Id))
            {

            }

            public LinkElement(Element element)
                : this(element, new LinkElementId(element.Id))
            {

            }

            public Element Element { get; }
            public LinkElementId Id { get; }
        }
    }
