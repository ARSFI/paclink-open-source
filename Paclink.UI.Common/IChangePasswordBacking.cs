using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IChangePasswordBacking : IFormBacking
    {
        string SiteRootDirectory { get; }

        string OldPassword { get; }
        string NewPassword { get; set; }
        DialogFormResult DialogResult { get; set; }
    }
}
