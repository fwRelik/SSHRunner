using SSHRunner.Infrastructure.Commands;
using SSHRunner.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

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

        #region Commands

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }
        public bool CanCloseApplicationCommandExecute(object p) => true;
        public void OnCloseApplicationCommandExecuted(object p) => Application.Current.Shutdown();

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Command

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion
        }
    }
}
