﻿using System.Text.RegularExpressions;

namespace WinHAB.Core.Models
{
  public class WidgetLabel
  {
    public WidgetLabel()
    {
      LabelTag = new LabelTag();
    }

    public string Title { get; set; }
    public string Value { get; set; }
    public LabelTag LabelTag { get; set; }

    public static WidgetLabel Parse(string label)
    {
      var l = new WidgetLabel() { Title = label };

      if (string.IsNullOrWhiteSpace(label)) return l;

      // Extract LabelTag
      var tagMatch = Regex.Match(label.Trim(), @"\{(.*?)\}");
      if (tagMatch.Success)
      {
        if (tagMatch.Groups[1].Success)
          l.LabelTag = new LabelTag(tagMatch.Groups[1].Value.Trim());
      }
      // Extract Value
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