namespace Autodesk.Revit.DB
{
    public static class ParameterExtensions
    {
        public static string StringValue(this Parameter parameter)
        {
            string text = parameter.AsValueString();
            if (string.IsNullOrEmpty(text))
            {
                text = parameter.AsString();
            }

            if (string.IsNullOrEmpty(text))
            {
                text = "";
            }
            return text;
        }
    }
}
