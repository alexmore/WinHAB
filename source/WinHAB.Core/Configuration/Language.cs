namespace WinHAB.Core.Configuration
{
  public class Language
  {
    public Language(string title, string culture)
    {
      Title = title;
      Culture = culture;
    }

    public string Title { get; set; }
    public string Culture { get; set; }
  }
}