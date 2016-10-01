using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNav.Code.UI;

namespace VSNav
{
    /// <summary>
    /// Version of Visual Studio
    /// </summary>
    public enum VS_Version
    {
        VS2010,
        VS2012Light,
        VS2012Dark
    };

    public static class Version
    {
        private static VSNavPackage package;

        public static void Initialize(VSNavPackage pckg)
        {
            if (pckg == null) throw new ArgumentNullException("pckg");
            package = pckg;
        }

        /// <summary>
        /// Current Version
        /// </summary>
        public static VS_Version VSVersion
        {
            get
            {
                OptionsPage page = package.GetPage();
                if(page != null)
                {
                    switch(page.Theme)
                    {
                        case Code.Theme.Blue: return VS_Version.VS2010;
                        case Code.Theme.Light: return VS_Version.VS2012Light;
                        case Code.Theme.Dark: return VS_Version.VS2012Dark;
                    }
                }

                return VS_Version.VS2012Light;
            }
        }
    }
}
