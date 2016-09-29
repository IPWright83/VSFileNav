using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// Current Version
        /// </summary>
        public static VS_Version VSVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Update the version of Visual Studio
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Update(DTE dte, ServiceProvider serviceProvider)
        {
            string version = ((EnvDTE.DTE)serviceProvider.GetService(typeof(EnvDTE.DTE).GUID)).Version;
            if (version == "10.0")
            {
                VSVersion = VS_Version.VS2010;
            }
            else
            {
                VSVersion = VS_Version.VS2012Light;

                try
                {
                    FontsAndColorsItems f = (FontsAndColorsItems)dte.get_Properties("FontsAndColors", "TextEditor").Item("FontsAndColorsItems").Object;
                    UInt32 background = f.Item("Keyword").Background;
                    if (background != 16777215)
                    {
                        VSVersion = VS_Version.VS2012Dark;
                    }
                }
                catch (Exception)
                { /* Ignore - just use light */}
            }
        }
    }
}
