using System;
using System.Windows;

namespace SSHRunner.Helper
{
    public class ErrorHandler
    {
        public static void ServiceNotFound(Exception ex)
        {
            string message = "Service name not found or insufficient rights." +
                "\n\nIf you see this message when you first start the program, then ignore it.";
            string caption = "Information";

#if DEBUG
            MessageBox.Show($"{message}\n\nError: {ex.Message}");
#else
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
#endif
        }

        public static void InsufficientRights(Exception ex)
        {
            string message = "Insufficient rights.";
            string caption = "Warning";

#if DEBUG
            MessageBox.Show($"{message}\n\nError: {ex.Message}");
#else
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
#endif
        }

        public static void HasAlready(Exception ex)
        {
            string message = "Most likely the state of the service already includes the state you selected.";
            string caption = "Error Information";

#if DEBUG
            MessageBox.Show($"{message}\n\nError: {ex.Message}");
#else
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
#endif
        }
    }
}
