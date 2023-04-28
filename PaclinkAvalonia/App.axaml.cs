using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Paclink.UI.Common;

namespace Paclink.UI.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var uiSystem = UserInterfaceFactory.GetUiSystem();
            uiSystem.DisplayMainForm(new Paclink.Main());
            desktop.MainWindow = (Window)uiSystem.GetMainForm();
        }

        base.OnFrameworkInitializationCompleted();
    }
}