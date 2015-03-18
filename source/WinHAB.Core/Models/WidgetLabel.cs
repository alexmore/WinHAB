using System.Text.RegularExpressions;

namespace WinHAB.Core.Models
{
  public class WidgetLabel
  {
    public WidgetLabel()
    {
      Properties = new WidgetProperties();
    }

    public string Title { get; set; }
    public string Value { get; set; }
    public WidgetProperties Properties { get; set; }

    public static WidgetLabel Parse(string label)
    {
      var l = new WidgetLabel() { Title = label };

      if (string.IsNullOrWhiteSpace(label)) return l;

      // Extract Properties
      var propertyMatch = Regex.Match(label.Trim(), @"\{(.*?)\}");
      if (propertyMatch.Success)
      {
        if (propertyMatch.Groups[1].Success)
          l.Properties = new WidgetProperties(propertyMatch.Groups[1].Value.Trim());
      }
      // Extract propertiesString
      var valueMatch = Regex.Match(label.Trim(), @"\[(.*?)\]");
      if (valueMatch.Success)
      {
        if (valueMatch.Groups[1].Success)
          l.Value = valueMatch.Groups[1].Value.Trim();
      }

      // Extract Title
      l.Title = new Regex(@"\{.*?\}|\[.*?\]").Replace(label.Trim(), "").Trim();
      if (string.IsNullOrWhiteSpace(l.Title)) l.Title = null;

      return l;
    } 
  }
}