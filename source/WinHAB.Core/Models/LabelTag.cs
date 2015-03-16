namespace WinHAB.Core.Models
{
  public class LabelTag
  {
    public LabelTag()
    {
    }

    public LabelTag(string value)
    {
      Value = value;
    }

    public string Value { get; set; }
  }
}