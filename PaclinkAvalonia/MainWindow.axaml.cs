using Avalonia.Controls;
using Paclink.UI.Common;

namespace Paclink.UI.Avalonia;

public partial class MainWindow : Window, IMainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    public MainWindow(IMainFormBacking backing)
    {
        InitializeComponent();
    }

    public UiDialogResult ShowModal()
    {
        throw new System.NotImplementedException();
    }

    public void RefreshWindow()
    {
        throw new System.NotImplementedException();
    }

    public void CloseWindow()
    {
        throw new System.NotImplementedException();
    }

    public IMainFormBacking BackingObject { get; }
    public void EnableWaitDisplay()
    {
        throw new System.NotImplementedException();
    }

    public void DisableWaitDisplay()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateSiteCallsign(string callsign)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateChannelList()
    {
        throw new System.NotImplementedException();
    }

    public void EnableMainWindowInterface()
    {
        throw new System.NotImplementedException();
    }
}