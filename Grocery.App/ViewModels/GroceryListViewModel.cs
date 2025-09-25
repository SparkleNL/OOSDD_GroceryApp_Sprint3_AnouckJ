using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;

        public GroceryListViewModel(IGroceryListService groceryListService, IAuthService authService, GlobalViewModel global) 
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            _authService = authService;
            _global = global;
            GroceryLists = new(_groceryListService.GetAll());
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }

        [RelayCommand]
        private void LogOut()
        {
            var loginViewModel = new LoginViewModel(_authService, _global);
            Application.Current!.MainPage = new LoginView(loginViewModel);
        }
    }
}
