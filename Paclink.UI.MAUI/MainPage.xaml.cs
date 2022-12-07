using Paclink.UI.Common;

namespace Paclink.UI.MAUI;

public partial class MainPage : ContentPage, IMainWindow, IQueryAttributable
{
	public MainPage()
	{
		InitializeComponent();
	}

    public IMainFormBacking BackingObject
    {
        get;
        set;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        BackingObject = (IMainFormBacking)query["backing"];
        BackingObject.FormLoading();
        BackingObject.MainWindow = this;
        BackingObject.FormLoaded();
    }

    public void CloseWindow()
    {
        throw new NotImplementedException();
    }

    public void DisableWaitDisplay()
    {
        throw new NotImplementedException();
    }

    public void EnableMainWindowInterface()
    {
        throw new NotImplementedException();
    }

    public void EnableWaitDisplay()
    {
        throw new NotImplementedException();
    }

    public void RefreshWindow()
    {
        // empty
    }

    public UiDialogResult ShowModal()
    {
        throw new NotImplementedException();
    }

    public void UpdateChannelList()
    {
        throw new NotImplementedException();
    }

    public void UpdateSiteCallsign(string callsign)
    {
        throw new NotImplementedException();
    }
}


