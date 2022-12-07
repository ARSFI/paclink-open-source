
using Microsoft.Maui.Controls;
using Paclink.UI.Common;
using CommunityToolkit.Maui.Views;

namespace Paclink.UI.MAUI;

public partial class App : Application, IUiPlatform
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        UserInterfaceFactory.Platform = this;
        DisplayMainForm(new Main());
	}

    public IWindowBase CreateForm(AvailableForms form, IFormBacking backingObject)
    {
        throw new System.NotImplementedException();
    }

    public void DisplayForm(AvailableForms form, IFormBacking backingObject)
    {
        throw new System.NotImplementedException();
    }

    public async void DisplayMainForm(IMainFormBacking backingObject)
    {
        var dict = new Dictionary<string, object>();
        dict["backing"] = backingObject;

        await Shell.Current.GoToAsync("//Home", false, dict);
    }

    public void DisplayModalError(string message, string title)
    {
        var popup = new Popup()
        {
            Content = new VerticalStackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = message
                    }
                }
            }
        };

        MainPage.ShowPopup(popup);
    }

    public IMainWindow GetMainForm()
    {
        //throw new System.NotImplementedException();
        return null;
    }

    public void RunUiEventLoop()
    {
        //throw new System.NotImplementedException();
    }

    public void Yield()
    {
        //throw new System.NotImplementedException();
    }
}

