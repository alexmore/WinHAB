using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinHAB.Core.Mvvm;

namespace WinHAB.Tests.Core.Mvvm
{
  [TestClass]
  public class ConstructorParametersTests
  {
    [TestMethod]
    public void ConstructorParametersCreateTest()
    {
      var cp = new ConstructorParameters();

      cp.Add("p1", 10).Add("p2", "pv1");

      Assert.AreEqual(2, cp.Parameters.Count);
      Assert.IsInstanceOfType(cp.Parameters[0], typeof(KeyValuePair<string, object>));

      Assert.AreEqual("p1", cp.Parameters[0].Key); Assert.AreEqual(10, cp.Parameters[0].Value);
      Assert.AreEqual("p2", cp.Parameters[1].Key); Assert.AreEqual("pv1", cp.Parameters[1].Value);
    }

    [TestMethod]
    public void GetConstructorParametersExtensionTest()
    {
      Action<ConstructorParameters> config = x => x.Add("p1", 10).Add("p2", "pv1");

      var cp = config.GetConstructorParameters();

      Assert.AreEqual(2, cp.Parameters.Count);
      Assert.IsInstanceOfType(cp.Parameters[0], typeof(KeyValuePair<string, object>));

      Assert.AreEqual("p1", cp.Parameters[0].Key); Assert.AreEqual(10, cp.Parameters[0].Value);
      Assert.AreEqual("p2", cp.Parameters[1].Key); Assert.AreEqual("pv1", cp.Parameters[1].Value);
    }

    [TestMethod]
    public void GetConstructorParametersExtensionActionIsNullTest()
    {
      Action<ConstructorParameters> config = null; 
      
      var cp = config.GetConstructorParameters();

      Assert.AreEqual(0, cp.Parameters.Count);
    }
  }
}
