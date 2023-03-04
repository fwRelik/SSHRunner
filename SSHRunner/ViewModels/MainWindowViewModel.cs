using SSHRunner.ViewModels.Base;

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
    }
}
