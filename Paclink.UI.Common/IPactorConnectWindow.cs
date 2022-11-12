﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IPactorConnectWindow : IWindow<IPactorConnectBacking>
    {
        bool ChannelBusy
        {
            get;
            set;
        }

        void UpdateChannelProperties();
    }
}
