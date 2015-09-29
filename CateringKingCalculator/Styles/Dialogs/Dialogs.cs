using System;
using Windows.UI.Popups;


namespace hebestadt.CateringKingCalculator.Dialogs
{
    static class Dialogs
    {
        public static async void ShowYesNoDialog(string message, UICommandInvokedHandler invokeHandler)
        {
            var messageDialog = new MessageDialog(message);

            messageDialog.Commands.Add(new UICommand(
                "Ja",
                new UICommandInvokedHandler(invokeHandler)));

            messageDialog.Commands.Add(new UICommand(
                "Nein",
                new UICommandInvokedHandler(invokeHandler)));

            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
    }
}
