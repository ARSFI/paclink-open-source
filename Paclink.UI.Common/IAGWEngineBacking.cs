using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.UI.Common
{
    public interface IAGWEngineBacking : IFormBacking
    {
        string SiteRootDirectory { get; }

        int AgwLocation { get; } 

        string AgwPath { get; }

        string AgwHost { get; }

        int AgwTcpPort { get; }

        string AgwUserId { get; }

        string AgwPassword { get; }

        void TestProposedSettings(IAGWEngineWindow window, string host, int port, string userId, string password);

        void SaveSettings(int location, string path, string host, int port, string userId, string password);
    }
}
