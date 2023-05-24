using System;
using System.Collections.Generic;
using System.Text;

namespace MiBud.Interfaces
{
    public interface IVersionAndBuildNumber
    {
        string GetVersionNumber();
        string GetBuildNumber();
    }
}
