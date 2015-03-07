using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.System.Display;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using OpenTrack.WindowsPhone.Providers;
using OpenTrack.WindowsPhone.Services;
using OpenTrack.WindowsPhone.Services.SensorReaders;
using OpenTrack.WindowsPhone.ViewModels;
using OpenTrack.WindowsPhone.Views;

namespace OpenTrack.WindowsPhone
{
    public sealed partial class App
    {
        private WinRTContainer _container;
        private DisplayRequest _displayRequest;

        public App()
        {
            InitializeComponent();

            //Hack to keep Device from Sleeping.
            //Otherwise headtracking stops. 
            _displayRequest = new Windows.System.Display.DisplayRequest();
            _displayRequest.RequestActive();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            RegisterViewModels();
            RegisterServices();
        }

        private void RegisterViewModels()
        {
            _container.PerRequest<MainPageViewModel>();
            _container.PerRequest<SettingsViewModel>();
            _container.PerRequest<SensorReadingViewModel>();
        }

        private void RegisterServices()
        {
            _container.PerRequest<SensorReaderFactory>();
            _container.PerRequest<OpenTrackService>();
            _container.PerRequest<SettingsProvider>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<MainPageView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}