using SSHRunner.Infrastructure.Commands;
using SSHRunner.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using fwRelik.SSHSetup.Extensions;
using fwRelik.SSHSetup.Enums;
using System;
using System.Threading;
using System.Windows.Threading;
using fwRelik.SSHSetup.Structs;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SSHRunner.Services.Base;
using SSHRunner.Services;

namespace SSHRunner.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Services

        //private readonly SSHServiceControl _sshServiceControl = new();
        private readonly SSHService _sshService = new();
        //private readonly FirewallRuleControl _firewallRuleControl = new();
        private readonly FirewallService _firewallService = new();
        //private readonly NetworkInfo _networkInfo = new();
        private readonly NetworkService _networkService = new();
        //private readonly PackageControl _packageControl = new();
        private readonly PackageService _packageService = new();

        #endregion

        #region Fields

        #region Window title

        /// <summary>Window title</summary>
        private string _title = "SSH Runner";
        /// <summary>Window title</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region SSH Service status indicator

        /// <summary>SSH Service status indicator</summary>
        private string _sshServiceStatusIndicator = "Red";
        /// <summary>SSH Service status indicator</summary>
        public string SSHServiceStatusIndicator
        {
            get => _sshServiceStatusIndicator;
            set => Set(ref _sshServiceStatusIndicator, value);
        }

        #endregion

        #region Firewall Rule status indicator

        /// <summary>Firewall Rule status indicator</summary>
        private string _firewallRuleIndicator = "Red";
        /// <summary>Firewall Rule status indicator</summary>
        public string FirewallRuleIndicator
        {
            get => _firewallRuleIndicator;
            set => Set(ref _firewallRuleIndicator, value);
        }

        #endregion

        #region Network status indicator

        /// <summary>Network status indicator</summary>
        private string _networkStatusIndicator = "Red";
        /// <summary>Network status indicator</summary>
        public string NetworkStatusIndicator
        {
            get => _networkStatusIndicator;
            set => Set(ref _networkStatusIndicator, value);
        }

        #endregion

        #region Package installing status indicator

        /// <summary>Package installing status indicator</summary>
        private string _packageInstallingStatusIndicator = "Red";
        /// <summary>Package installing status indicator</summary>
        public string PackageInstallingStatusIndicator
        {
            get => _packageInstallingStatusIndicator;
            set => Set(ref _packageInstallingStatusIndicator, value);
        }

        #endregion

        #region SSH Service Button content

        /// <summary>SSH Service Button content</summary>
        private string _sshServiceButton = "Start Service";
        /// <summary>SSH Service Button content</summary>
        public string SSHServiceButton
        {
            get => _sshServiceButton;
            set => Set(ref _sshServiceButton, value);
        }

        #endregion     

        #region Connections

        /// <summary>Connections</summary>
        private List<ConnectionEntity>? _connections;
        /// <summary>Connections</summary>
        public List<ConnectionEntity>? Connections
        {
            get => _connections;
            set => Set(ref _connections, value);
        }

        #endregion

        #endregion

        #region Commands

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }
        public bool CanCloseApplicationCommandExecute(object p) => true;
        public void OnCloseApplicationCommandExecuted(object p) => Application.Current.Shutdown();

        #endregion

        #region StartSSHServiceCommand

        public ICommand StartSSHServiceCommand { get; }
        public bool CanStartSSHServiceCommandExecute(object p) => true;
        public void OnStartSSHServiceCommandExecuted(object p)
        {
            if (_sshService.ServiceStatus) _sshService.ServiceStop();
            else _sshService.ServiceStart();

            ServiceStartButtonContentChange();
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {

            #region Command

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            StartSSHServiceCommand = new LambdaCommand(OnStartSSHServiceCommandExecuted, CanStartSSHServiceCommandExecute);

            #endregion

            Connections = Enumerable.Range(0, 10).Select(i => new ConnectionEntity
            {
                LocalAddress = new IPEndPoint(IPAddress.Parse($"192.168.1.10{i}"), i),
                RemoteAddress = new IPEndPoint(IPAddress.Parse($"244.187.16.1{i}"), i),
                State = TcpState.Established
            }).ToList();

            //var timer = new DispatcherTimer();
            //timer.Tick += (_, __) =>
            //{
            //    NetworkStatusIndicator = _networkService.GetIndicator();
            //    FirewallRuleIndicator = _firewallService.GetIndicator();
            //    PackageInstallingStatusIndicator = _packageService.GetIndicator();
            //    SSHServiceStatusIndicator = _sshService.GetIndicator();

            //    ServiceStartButtonContentChange();
            //};
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Start();

            //InitializeState();
        }

        private void InitializeState()
        {
            new Thread(() => _packageService.CheckServiceStatus()).Start();

            new Thread(() => _firewallService.CheckServiceStatus()).Start();

            new Thread(() => _sshService.CheckServiceStatus()).Start();

            new Thread(() => _networkService.CheckServiceStatus()).Start();
        }

        private void ServiceStartButtonContentChange()
        {
            SSHServiceButton = _sshService.ServiceStatus ? "Stop Service" : "Start Service";
        }
    }
}
