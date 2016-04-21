using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNav
{
    public static class Utils
    {
        #region Dictionaries

        private static Dictionary<String, FileType> types = new Dictionary<string, FileType>()
        {
            { ".aspx", FileType.ASPX },
            { ".ascx", FileType.ASCX },
            { ".edmx", FileType.EDMX },
            { ".fsi", FileType.FSI },
            { ".fsx", FileType.FSX },
            { ".manifest", FileType.Manifest },
            { ".master", FileType.Master},
            { ".ruleset", FileType.Ruleset},
            { ".sequencediagram", FileType.Sequence},
            {".sitemap", FileType.Sitemap},
            { ".skin", FileType.Skin},
            { ".jpg", FileType.Image },
            { ".bmp", FileType.Image },
            { ".png", FileType.Image },
            { ".cpp", FileType.CPlusPlus },
            { ".config" , FileType.Config },
            { ".cfg" , FileType.Config },
            { ".h", FileType.CPlusPlusHeader},
            { ".cs", FileType.CSharp},
            {".css", FileType.CSS},
            {".html", FileType.HTML},
            {".xhtml", FileType.HTML},
            {".htm", FileType.HTML},
            {".ico", FileType.Icon},
            {".js", FileType.ScriptFile},
            {".wsf", FileType.ScriptFile},
            {".resx", FileType.Resource},
            {".txt", FileType.Text},
            {".xaml", FileType.XAML},
            {".xml", FileType.XML},
            {".xsd", FileType.XSD},
            {".xslt", FileType.XSLT},
            {".fs", FileType.FSharp},
            {".axd", FileType.ASP},
            {".cd", FileType.ClassDiagram},
            {".idl", FileType.IDL},
            {".odl", FileType.IDL},
            {".tt", FileType.Template},
            {".vb", FileType.VisualBasic},
        };

        #endregion

        /// <summary>
        /// Determine the type of a File based upon it's Extension
        /// </summary>
        public static FileType ToFileType(this String value)
        {
            string lowerVal = value.ToLowerInvariant();
            if (types.ContainsKey(lowerVal))
            {
                return types[lowerVal];
            }
            return FileType.Unknown;
        }

        ///<summary>
        ///Invokes if required.
        ///</summary>
        ///<typeparam name="TNode"></typeparam>
        ///<param name="control">The control.</param>
        ///<param name="action">The action.</param>
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : Control
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action, control);
            }
            else
            {
                action(control);
            }
        }
    }
}
