using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav.Code.UI
{
    public class OptionsPage : DialogPage
    {
        [Category("Style")]
        [DisplayName("Theme")]
        [Description("The Theme to use for the VSFileNav dialogs")]
        public Theme Theme
        {
            get; set;
        }
    }
}
