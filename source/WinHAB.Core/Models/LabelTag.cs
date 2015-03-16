using System.Collections.Generic;
using System.Linq;

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

    private string _value;

    public string Value
    {
      get { return _value; }
      set { _value = value; Parse(value); }
    }

    

    public string this[string key]
    {
      get
      {
        if (key == null) return null;
        string res = null;
        Items.TryGetValue(key, out res);
        return res;
      }
      set
      {
        if (string.IsNullOrWhiteSpace(key)) return;
        Items[key] = value;
      }
    }

    private readonly Dictionary<string, string> _items = new Dictionary<string, string>();
    public Dictionary<string, string> Items
    {
      get { return _items; }
    }

    public void Parse(string labelTagString)
    {
      Items.Clear();

      foreach (var i in labelTagString.Split(',')
        .Select(i => i.Split(':'))
        .Where(keyValue => keyValue.Length > 1 && !string.IsNullOrWhiteSpace(keyValue[0])))
      {
        Items[i[0].Trim()] = string.Join(":", i.Skip(1)).Trim();
      }
    }

    public override string ToString()
    {
      return Value;
    }
  }
}