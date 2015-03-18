using System.Collections.Generic;
using System.Linq;
using WinHAB.Core.Fx;
using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Models
{
  public class WidgetProperties
  {
    #region Constructor
    public WidgetProperties()
    {
    }

    public WidgetProperties(string propertiesString)
    {
      PropertiesString = propertiesString;
    }
    #endregion

    #region Values
    private string _propertiesString;
    public string PropertiesString
    {
      get { return _propertiesString; }
      set { _propertiesString = value; Parse(value); }
    }

    public string this[string key]
    {
      get
      {
        if (key == null) return null;
        string res = null;
        Items.TryGetValue(key.ToLower(), out res);
        return res;
      }
      set
      {
        if (string.IsNullOrWhiteSpace(key)) return;
        Items[key.ToLower()] = value.Trim();
      }
    }

    private readonly Dictionary<string, string> _items = new Dictionary<string, string>();
    public Dictionary<string, string> Items
    {
      get { return _items; }
    }

    public override string ToString()
    {
      return PropertiesString;
    }

    public WidgetPropertiesValues Values { get { return new WidgetPropertiesValues(this);} }

    #endregion

    #region Parsing
    public void Parse(string labelTagString)
    {
      Items.Clear();

      foreach (var i in labelTagString.Split(',')
        .Select(i => i.Split('='))
        .Where(keyValue => keyValue.Length > 0 && !keyValue[0].IsNullOrWhitespace()))
      {
        Items[i[0].Trim().ToLower()] = string.Join("=", i.Skip(1)).Trim();
      }
    }
    #endregion

  }
}