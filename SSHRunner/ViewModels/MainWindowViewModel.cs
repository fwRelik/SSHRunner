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

namespace SSHRunner.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Services

        private readonly SSHServiceControl _sshServiceControl = new();
        private readonly FirewallRuleControl _firewallRuleControl = new();
        private readonly NetworkInfo _networkInfo = new();
        private readonly PackageControl _packageControl = new();

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
        private string _sshServiceButton = "Start service";
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
            try
            {
                if (_sshServiceControl.ServiceStatus == SSHServiceState.Running)
                {
                    _sshServiceControl.Stop();
                    SSHServiceStatusIndicator = "Red";
                    SSHServiceButton = "Start service";
                }
                else
                {
                    _sshServiceControl.Start();
                    SSHServiceStatusIndicator = "Green";
                    SSHServiceButton = "Stop service";
                }
            }
            catch (Exception ex)
            {
                SSHServiceStatusIndicator = "Red";
                SSHServiceButton = "Start service";
                MessageBox.Show("A half unexpected error has occurred, try running the program with administrator privileges.");
            }
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
            //    if (_networkInfo.GetNetworkConnectionStatus()) NetworkStatusIndicator = "Green";
            //    else NetworkStatusIndicator = "Red";

            //    if (_firewallRuleControl.RuleState) FirewallRuleIndicator = "Green";
            //    else FirewallRuleIndicator = "Red";

            //    if (_packageControl.GetPackagesState().AllPackageInstalling) PackageInstallingStatusIndicator = "Green";
            //    else PackageInstallingStatusIndicator = "Red";
            //};
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Start();

            //InitializeState();
        }

        private void InitializeState()
        {
            new Thread(() =>
            {
                try
                {
                    _packageControl.CheckPackageForInitializaitonValue();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }).Start();
            new Thread(() =>
            {
                try
                {
                    _firewallRuleControl.GetFirewallRule();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }).Start();
        }
    }
}
