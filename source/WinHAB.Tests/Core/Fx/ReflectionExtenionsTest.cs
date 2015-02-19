using System;
using NUnit.Framework;
using WinHAB.Core.Fx;

namespace WinHAB.Tests.Core.Fx
{
  [TestFixture]
  public class ReflectionExtenionsTest
  {
    #region Test Fixture Setup

    [TestFixtureSetUp]
    public void SetupTestFixture()
    {
    }

    [TestFixtureTearDown]
    public void CleanupTestFixture()
    {
    }

    #endregion

    #region Test Setup

    [SetUp]
    public void SetupTest()
    {
    }

    [TearDown]
    public void CleanupTest()
    {
    }

    #endregion

    [Test]
    public void GetPropertyName_ThrowsArgumentNullException_WhenPassNullpropertyExpression()
    {
      // assert
      Assert.That(() => this.GetPropertyName<object>(null), Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void GetPropertyName_ThrowsArgumentExpression_WhenPassWrongPropertyExpression()
    {
      // assert
      Assert.That(()=> this.GetPropertyName<int?>(()=> GetHashCode()), Throws.ArgumentException);
    }

    public int Field;

    [Test]
    public void GetPropertyName_ThrowsArgumentException_WhenPassFieldToExpression()
    {
      // assert
      Assert.That(()=>this.GetPropertyName(()=>Field), Throws.ArgumentException);
    }
  }
}