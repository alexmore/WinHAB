﻿using System;
using System.ComponentModel;
using Moq;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Tests.Core.ViewModels
{
  public class ViewModelsTestHelper
  {
    public ViewModelsTestHelper()
    {
      ConfigurationProvider = new FakeConfigurationProvider();
      Configuration = new AppConfiguration(ConfigurationProvider);
      
      ClientFactoryMock = new Mock<IRestClientFactory>();
      RestClientMock = new Mock<IRestClient>();
      ClientFactoryMock.Setup(x => x.Create()).Returns(() => RestClientMock.Object);

      NavigationMock = new Mock<INavigationService>();

      WidgetsFactoryMock = new Mock<IWidgetsFactory>();

      TimerMock = new Mock<ITimer>();
    }

    public FakeConfigurationProvider ConfigurationProvider { get; set; }
    public AppConfiguration Configuration { get; set; }
    
    public Mock<IRestClientFactory> ClientFactoryMock { get; set; }
    public IRestClientFactory ClientFactory { get { return ClientFactoryMock.Object; }}
    public Mock<IRestClient> RestClientMock { get; set; }

    public Mock<INavigationService> NavigationMock { get; set; }
    public INavigationService Navigation { get { return NavigationMock.Object; }}

    public Mock<IWidgetsFactory> WidgetsFactoryMock { get; set; }
    public IWidgetsFactory WidgetFactory { get { return WidgetsFactoryMock.Object; } }

    public Mock<ITimer> TimerMock { get; set; }
    public ITimer Timer { get { return TimerMock.Object; }}
  }
}