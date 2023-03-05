using SSHRunner.Infrastructure.Commands;
using SSHRunner.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using fwRelik.SSHSetup.Extensions;
using fwRelik.SSHSetup.Enums;
using System;
using System.Threading;

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

        /// <summary>Window title</summary>
        private string _sshServiceButton = "Start service";
        /// <summary>Window title</summary>
        public string SSHServiceButton
        {
            get => _sshServiceButton;
            set => Set(ref _sshServiceButton, value);
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
                    SSHServiceStatusIndicator = "Green";
                    SSHServiceButton = "Stop service";
                }
                else
                {
                    _sshServiceControl.Start();
                    SSHServiceStatusIndicator = "Red";
                    SSHServiceButton = "Start service";
                }
            }
            catch (Exception ex)
            {
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
        }
    }
}
