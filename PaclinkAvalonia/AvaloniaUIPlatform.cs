using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Paclink.UI.Common;

namespace Paclink.UI.Avalonia;

public class AvaloniaUIPlatform : Paclink.UI.Common.IUiPlatform
{
    private IMainWindow _mainWindow;
    private Application _app;
    
    public static AppBuilder BuildAvaloniaApp() 
        => AppBuilder.Configure<App>().UsePlatformDetect();
    
    public IMainWindow GetMainForm()
    {
        return _mainWindow;
    }

    public IWindowBase CreateForm(AvailableForms form, IFormBacking backingObject)
    {
        throw new System.NotImplementedException();
    }

    public void DisplayForm(AvailableForms form, IFormBacking backingObject)
    {
        Window window = (Window)CreateForm(form, backingObject);

        if (form == AvailableForms.MainWindow)
        {
            window.Show();
        }
        else
        {
            window.ShowDialog((Window)_mainWindow).Wait();
        }
    }

    public void DisplayMainForm(IMainFormBacking backingObject)
    {

        _mainWindow = new MainWindow(backingObject);
        //RunUiEventLoop();
        //}, new string[] { });
    }

    public void DisplayModalError(string message, string title)
    {
        throw new System.NotImplementedException();
    }

    public void RunUiEventLoop()
    {
        if (_app == null)
        {
            BuildAvaloniaApp().Start((Application app, string[] args) =>
            {
                _app = app;

                // A cancellation token source that will be used to stop the main loop
                var cts = new CancellationTokenSource();

                // Start the main loop
                app.Run(cts.Token);
            }, new string[] { });
        }
        else
        {
            // A cancellation token source that will be used to stop the main loop
            var cts = new CancellationTokenSource();

            // Start the main loop
            _app.Run(cts.Token);
        }
    }

    public void Yield()
    {
        throw new System.NotImplementedException();
    }
}