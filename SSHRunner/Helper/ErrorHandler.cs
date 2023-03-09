using System;
using System.Windows;

namespace SSHRunner.Helper
{
    public class ErrorHandler
    {
        public static void InsufficientRights(Exception ex)
        {
            string message = "Service name not found or insufficient rights";

#if DEBUG
                MessageBox.Show($"{message}\nError: {ex.Message}");
#else
            MessageBox.Show($"{message}");
            Application.Current.Shutdown();
#endif
        }
    }
}
