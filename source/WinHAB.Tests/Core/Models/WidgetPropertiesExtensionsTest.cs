using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class WidgetPropertiesExtensionsTest
  {
    [SetUp]
    public void SetUp()
    {
      wProps = new WidgetProperties();
    }

    private WidgetProperties wProps;

    [Test]
    public void GetSize_ReturnsValidValue()
    {
      wProps["Size"] = "Medium";
      Assert.That(wProps.GetSize(), Is.EqualTo(WidgetSize.Meduim));
      wProps["Size"] = "Wide";
      Assert.That(wProps.GetSize(), Is.EqualTo(WidgetSize.Wide));
      wProps["Size"] = "Large";
      Assert.That(wProps.GetSize(), Is.EqualTo(WidgetSize.Large));
    }

    [Test]
    public void GetSize_ReturnsValidValueCaseIsensetive()
    {
      wProps["SiZe"] = "MeDiUm";
      Assert.That(wProps.GetSize(), Is.EqualTo(WidgetSize.Meduim));
      wProps["SiZe"] = "WiDe";
      Assert.That(wProps.GetSize(), Is.EqualTo(WidgetSize.Wide));
      wProps["SiZe"] = "LaRgE";
      Assert.That(wProps.GetSize(), Is.EqualTo(WidgetSize.Large));
    }
  }
}