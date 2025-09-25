using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.ViewModels
{
    public partial class AddUserViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;

        [ObservableProperty]
        private string name = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string addedUser = "";

        [ObservableProperty]
        private string loginMessage = "";

        public AddUserViewModel(IAuthService authService, GlobalViewModel global)
        {
            _authService = authService;
            _global = global;
        }

        [RelayCommand]
        private void GoBack()
        {
            var loginViewModel = new LoginViewModel(_authService, _global);
            Application.Current!.MainPage = new LoginView(loginViewModel);
        }

        [RelayCommand]
        private void AddUser()
        {
            // Validate input: if input is empty, give error message
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                LoginMessage = "Vul alle velden in.";
                AddedUser = "";

                return;
            }

            // Check if email is an email with an "@" and a "."
            if (!Email.Contains("@") || !Email.Contains("."))
            {
                LoginMessage = "Voer een geldig emailadres in";
                AddedUser = "";
                return;
            }

            try
            {
                var clientRepo = new ClientRepository();

                // Hash the password
                var hashedPassword = PasswordHelper.HashPassword(Password);
                clientRepo.Add(Name, Email, hashedPassword);

                AddedUser = "Account succesvol aangemaakt!";

                // Clear entries after adding account succesfully
                Name = "";
                Email = "";
                Password = "";

                // If previously an error message was shown, it will disappear after creating account succesfuly
                LoginMessage = "";

            }
            catch (InvalidOperationException ex)
            {
                LoginMessage = ex.Message;
                AddedUser = "";
            }
            catch (Exception ex)
            {
                LoginMessage = $"Fout bij aanmaken gebruiker: {ex.Message}";
                AddedUser = "";
            }
        }
    }
}
