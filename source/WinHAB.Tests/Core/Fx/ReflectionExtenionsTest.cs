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
    public void GetPropertyName_TryToPassNullpropertyExpression_ThrowsArgumentNullException()
    {
      // assert
      Assert.That(() => this.GetPropertyName<object>(null), Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void GetPropertyName_TryToPassWrongPropertyExpression_ThrowsArgumentExpression()
    {
      // assert
      Assert.That(()=> this.GetPropertyName<int?>(()=> GetHashCode()), Throws.ArgumentException);
    }

    public int Field;

    [Test]
    public void GetPropertyName_TryPassFieldToExpression_ThrowsArgumentException()
    {
      // assert
      Assert.That(()=>this.GetPropertyName(()=>Field), Throws.ArgumentException);
    }
  }
}