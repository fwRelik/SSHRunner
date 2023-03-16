using SSHRunner.Infrastructure.Commands;
using SSHRunner.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using System;
using System.Threading;
using System.Windows.Threading;
using fwRelik.SSHSetup.Structs;
using System.Linq;
using System.Collections.Generic;
using SSHRunner.Services;
using System.Security.Principal;
using SSHRunner.Helper;

namespace SSHRunner.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Services

        private readonly SSHService _sshService = new();
        private readonly FirewallService _firewallService = new();
        private readonly NetworkService _networkService = new();
        private readonly PackageService _packageService = new();

        #endregion

        #region Fields

        private static string _defaultIndicator = "DarkRed";

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
        private string _sshServiceStatusIndicator = _defaultIndicator;
        /// <summary>SSH Service status indicator</summary>
        public string SSHServiceStatusIndicator
        {
            get => _sshServiceStatusIndicator;
            set => Set(ref _sshServiceStatusIndicator, value);
        }

        #endregion

        #region Firewall Rule status indicator

        /// <summary>Firewall Rule status indicator</summary>
        private string _firewallRuleIndicator = _defaultIndicator;
        /// <summary>Firewall Rule status indicator</summary>
        public string FirewallRuleIndicator
        {
            get => _firewallRuleIndicator;
            set => Set(ref _firewallRuleIndicator, value);
        }

        #endregion

        #region Network status indicator

        /// <summary>Network status indicator</summary>
        private string _networkStatusIndicator = _defaultIndicator;
        /// <summary>Network status indicator</summary>
        public string NetworkStatusIndicator
        {
            get => _networkStatusIndicator;
            set => Set(ref _networkStatusIndicator, value);
        }

        #endregion

        #region Package installing status indicator

        /// <summary>Package installing status indicator</summary>
        private string _packageInstallingStatusIndicator = _defaultIndicator;
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

        #region Host name

        /// <summary>Host name</summary>
        private string _hostName = "Undefined";
        /// <summary>Host name</summary>
        public string HostName
        {
            get => _hostName;
            set => Set(ref _hostName, value);
        }

        #endregion

        #region User name

        /// <summary>User name</summary>
        private string _userName = "Undefined";
        /// <summary>User name</summary>
        public string UserName
        {
            get => _userName;
            set => Set(ref _userName, value);
        }

        #endregion

        #region Server local IP addresses

        /// <summary>Server local IP addresses</summary>
        private string[]? _localIPAddresses;
        /// <summary>Server local IP addresses</summary>
        public string[]? LocalIPAddresses
        {
            get => _localIPAddresses;
            set => Set(ref _localIPAddresses, value);
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

        #region PackageInstallCommand

        public ICommand PackageInstallCommand { get; }
        public bool CanPackageInstallCommandExecute(object p) => true;
        public void OnPackageInstallCommandExecuted(object p) => _packageService.Install();

        #endregion

        #region PackageUnInstallCommand

        public ICommand PackageUnInstallCommand { get; }
        public bool CanPackageUnInstallCommandExecute(object p) => true;
        public void OnPackageUnInstallCommandExecuted(object p) => _packageService.UnInstall();

        #endregion

        #region FirewallRuleSetCommand

        public ICommand FirewallRuleSetCommand { get; }
        public bool CanFirewallRuleSetCommandExecute(object p) => true;
        public void OnFirewallRuleSetCommandExecuted(object p) => _firewallService.Set();

        #endregion

        #region FirewallRuleRemoveCommand

        public ICommand FirewallRuleRemoveCommand { get; }
        public bool CanFirewallRuleRemoveCommandExecute(object p) => true;
        public void OnFirewallRuleRemoveCommandExecuted(object p) => _firewallService.Remove();

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Command

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            StartSSHServiceCommand = new LambdaCommand(OnStartSSHServiceCommandExecuted, CanStartSSHServiceCommandExecute);
            PackageInstallCommand = new LambdaCommand(OnPackageInstallCommandExecuted, CanPackageInstallCommandExecute);
            PackageUnInstallCommand = new LambdaCommand(OnPackageUnInstallCommandExecuted, CanPackageUnInstallCommandExecute);
            FirewallRuleSetCommand = new LambdaCommand(OnFirewallRuleSetCommandExecuted, CanFirewallRuleSetCommandExecute);
            FirewallRuleRemoveCommand = new LambdaCommand(OnFirewallRuleRemoveCommandExecuted, CanFirewallRuleRemoveCommandExecute);

            #endregion

            HostName = _networkService.Service.HostName;
            UserName = _networkService.Service.UserName;
            LocalIPAddresses = _networkService.Service.IpAddresses.Select(i => i.ToString()).ToArray();
            Connections = _networkService.Service.GetConnections().Select(i => i.Value).ToList();

            if (!App.IsDesignMode)
            {
                var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    ErrorHandler.InsufficientRights(new Exception());
                    Application.Current.Shutdown();
                    return;
                }

                var timer = new DispatcherTimer();
                timer.Tick += (_, __) =>
                {
                    FirewallRuleIndicator = _firewallService.GetIndicator();
                    PackageInstallingStatusIndicator = _packageService.GetIndicator();
                    SSHServiceStatusIndicator = _sshService.GetIndicator();

                    _networkService.CheckServiceStatus();
                    NetworkStatusIndicator = _networkService.GetIndicator();

                    Connections = _networkService.Service.GetConnections().Select(i => i.Value).ToList();

                    ServiceStartButtonContentChange();
                };
                timer.Interval = TimeSpan.FromSeconds(2);
                timer.Start();

                InitializeState();
            }
        }

        private void InitializeState()
        {
            new Thread(() => _packageService.CheckServiceStatus()).Start();

            new Thread(() => _firewallService.CheckServiceStatus()).Start();

            new Thread(() => _sshService.CheckServiceStatus()).Start();
        }

        private void ServiceStartButtonContentChange()
        {
            SSHServiceButton = _sshService.ServiceStatus ? "Stop Service" : "Start Service";
        }
    }
}
