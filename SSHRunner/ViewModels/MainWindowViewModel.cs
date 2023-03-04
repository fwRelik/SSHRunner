using SSHRunner.Infrastructure.Commands;
using SSHRunner.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using fwRelik.SSHSetup.Extensions;
using fwRelik.SSHSetup.Enums;
using System;

namespace SSHRunner.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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

        #region Services

        private readonly SSHServiceControl _sshServiceControl = new();

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
                    _sshServiceControl.Stop();
                else _sshServiceControl.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
