
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

    public void DisplayMainForm(IMainFormBacking backingObject)
    {
        MainPage.Navigation.PushAsync(new MainPage(backingObject)).Wait();
        /*var dict = new Dictionary<string, object>();
        dict["backing"] = backingObject;

        Shell.Current.GoToAsync("//Home", false, dict).Wait();*/
    }

    public void DisplayModalError(string message, string title)
    {
        MainThread.InvokeOnMainThreadAsync(() =>
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

            ((MainPage)GetMainForm()).ShowPopup(popup);
        }).Wait();
    }

    public IMainWindow GetMainForm()
    {
        var navStack = Shell.Current.Navigation.NavigationStack;
        return (IMainWindow)navStack[navStack.Count - 1];
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

