using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class AddUserView : ContentPage
{
	public AddUserView(AddUserViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}