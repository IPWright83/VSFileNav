using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Helper class that returns a set of Images for a File Type
    /// </summary>
    public static class Images
    {
        /// <summary>
        /// Grab the set of Images
        /// </summary>
        public static IImages GetImages()
        {
            switch (Version.VSVersion)
            {
                case VS_Version.VS2010: return VS2010;
                case VS_Version.VS2012Dark: return VS2012Dark;
                case VS_Version.VS2012Light: return VS2012Light;
                default:
                    return VS2010;
            }
        }

        /// <summary>
        /// Returns the VS2010 Images
        /// </summary>
        public static IImages VS2010
        {
            get { return _vs2010; }
        }
        private static VS2010Images _vs2010 = new VS2010Images();

        /// <summary>
        /// Returns the VS2012 Light Images
        /// </summary>
        public static IImages VS2012Light
        {
            get { return _vs2012Light; }
        }
        private static VS2012LightImages _vs2012Light = new VS2012LightImages();

        /// <summary>
        /// Returns the VS2012 Dark Images
        /// </summary>
        public static IImages VS2012Dark
        {
            get { return _vs2012Dark; }
        }
        private static VS2012DarkImages _vs2012Dark = new VS2012DarkImages();

        /// <summary>
        /// Represents a set of Images with a cache
        /// </summary>
        private abstract class ImagesWithCache : IImages
        {
            /// <summary>
            /// Cache of the Images
            /// </summary>
            protected Dictionary<FileType, Image> imageCache = new Dictionary<FileType, Image>();

            /// <inheritdoc/>
            public Image GetIcon(FileType fType)
            {
                if (this.imageCache.ContainsKey(fType))
                    return this.imageCache[fType];

                this.imageCache[fType] = LoadIcon(fType);
                return this.imageCache[fType];
            }

            /// <summary>
            /// Load an Icon from Disk
            /// </summary>
            /// <param name="fType"></param>
            /// <returns></returns>
            protected abstract Image LoadIcon(FileType fType);
        }

        /// <summary>
        /// VS 2010 Colours
        /// </summary>
        private class VS2010Images : ImagesWithCache
        {
            /// <inheritdoc/>
            protected override Image LoadIcon(FileType fType)
            {
                // Grab the path to the Images
                Assembly assembly = typeof(Utils).Assembly;
                String fileName = GetIconName(fType);

                if (String.IsNullOrEmpty(fileName))
                    return new Bitmap(20, 20);

                using (Stream stream = assembly.GetManifestResourceStream("VSNav.Code.Resources.VS2010." + fileName))
                {
                    Image image = new Bitmap(stream);
                    return new Bitmap(image, new Size(20, 20));
                }
            }

            /// <summary>
            /// Grab the Name of an Icon
            /// </summary>
            private String GetIconName(FileType fType)
            {
                switch (fType)
                {
                    case FileType.ASP: return "ASP.png";
                    case FileType.ClassDiagram: return "ClassDiagram.png";
                    case FileType.Config: return "Settings.png";
                    case FileType.CPlusPlus: return "CPlusPlus.png";
                    case FileType.CPlusPlusHeader: return "Header.png";
                    case FileType.CSharp: return "CSharp.png";
                    case FileType.CSS: return "StyleSheet.png";
                    case FileType.FSharp: return "FSharp.png";
                    case FileType.HTML: return "Html.png";
                    case FileType.Icon: return "Icon.png";
                    case FileType.IDL: return "IDL.png";
                    case FileType.Image: return "Image.png";
                    case FileType.Resource: return "Resource.png";
                    case FileType.ScriptFile: return "Script.png";
                    case FileType.Settings: return "Settings.png";
                    case FileType.Template: return "Template.png";
                    case FileType.Text: return "Text.png";
                    case FileType.VisualBasic: return "VisualBasic.png";
                    case FileType.XAML: return "XAML.png";
                    case FileType.XML: return "XML.png";
                    case FileType.XSD: return "XSD.png";
                    case FileType.XSLT: return "XSLT.png";
                    default:
                        return String.Empty;
                }
            }
        }

        /// <summary>
        /// VS 2012 Light Images
        /// </summary>
        private class VS2012LightImages : ImagesWithCache
        {
            /// <inheritdoc/>
            protected override Image LoadIcon(FileType fType)
            {
                // Grab the path to the Images
                Assembly assembly = typeof(Utils).Assembly;
                String fileName = GetIconName(fType);
                
                if (String.IsNullOrEmpty(fileName))
                    return new Bitmap(20, 20);

                using (Stream stream = assembly.GetManifestResourceStream("VSNav.Code.Resources.VS2012Light." + fileName))
                {
                    Image image = new Bitmap(stream);
                    return new Bitmap(image, new Size(20, 20));
                }
            }

            /// <summary>
            /// Grab the Name of an Icon
            /// </summary>
            private String GetIconName(FileType fType)
            {
                switch (fType)
                {
                    case FileType.ASCX: return "ASCX.png";
                    case FileType.ASPX: return "ASPX.png";
                    case FileType.ASP: return "ASP.png";
                    case FileType.EDMX: return "EDMX.png";
                    case FileType.FSX: return "FScript.png";
                    case FileType.FSI: return "FSig.png";
                    case FileType.ClassDiagram: return "ClassDiagram.png";
                    case FileType.Config: return "Config.png";
                    case FileType.CPlusPlus: return "CPlusPlus.png";
                    case FileType.CPlusPlusHeader: return "Header.png";
                    case FileType.CSharp: return "CSharp.png";
                    case FileType.CSS: return "StyleSheet.png";
                    case FileType.FSharp: return "FSharp.png";
                    case FileType.HTML: return "Html.png";
                    case FileType.Icon: return "Icon.png";
                    case FileType.IDL: return "IDL.png";
                    case FileType.Image: return "Image.png";
                    case FileType.Resource: return "Resource.png";
                    case FileType.ScriptFile: return "Script.png";
                    case FileType.Settings: return "Settings.png";
                    case FileType.Template: return "Template.png";
                    case FileType.Text: return "Text.png";
                    case FileType.VisualBasic: return "VisualBasic.png";
                    case FileType.XAML: return "XAML.png";
                    case FileType.XML: return "XML.png";
                    case FileType.XSD: return "XSD.png";
                    case FileType.XSLT: return "XSLT.png";
                    case FileType.Manifest: return "Manifest.png";
                    case FileType.Master: return "Master.png";
                    case FileType.Ruleset: return "Ruleset.png";
                    case FileType.Sequence: return "Sequence.png";
                    case FileType.Sitemap: return "Sitemap.png";
                    case FileType.Skin: return "Skin.png";
                    default:
                        return String.Empty;
                }
            }
        }

        /// <summary>
        /// VS 2012 Dark Images
        /// </summary>
        private class VS2012DarkImages : ImagesWithCache
        {
            /// <inheritdoc/>
            protected override Image LoadIcon(FileType fType)
            {
                // Grab the path to the Images
                Assembly assembly = typeof(Utils).Assembly;
                String fileName = GetIconName(fType);

                if (String.IsNullOrEmpty(fileName))
                    return new Bitmap(20, 20);

                using (Stream stream = assembly.GetManifestResourceStream("VSNav.Code.Resources.VS2012Dark." + fileName))
                {
                    Image image = new Bitmap(stream);
                    return new Bitmap(image, new Size(20, 20));
                }
            }

            /// <summary>
            /// Grab the Name of an Icon
            /// </summary>
            private String GetIconName(FileType fType)
            {
                switch (fType)
                {
                    case FileType.ASCX: return "ASCX.png";
                    case FileType.ASPX: return "ASPX.png";
                    case FileType.ASP: return "ASP.png";
                    case FileType.EDMX: return "EDMX.png";
                    case FileType.FSX: return "FScript.png";
                    case FileType.FSI: return "FSig.png";
                    case FileType.ClassDiagram: return "ClassDiagram.png";
                    case FileType.Config: return "Config.png";
                    case FileType.CPlusPlus: return "CPlusPlus.png";
                    case FileType.CPlusPlusHeader: return "Header.png";
                    case FileType.CSharp: return "CSharp.png";
                    case FileType.CSS: return "StyleSheet.png";
                    case FileType.FSharp: return "FSharp.png";
                    case FileType.HTML: return "Html.png";
                    case FileType.Icon: return "Icon.png";
                    case FileType.IDL: return "IDL.png";
                    case FileType.Image: return "Image.png";
                    case FileType.Resource: return "Resource.png";
                    case FileType.ScriptFile: return "Script.png";
                    case FileType.Settings: return "Settings.png";
                    case FileType.Template: return "Template.png";
                    case FileType.Text: return "Text.png";
                    case FileType.VisualBasic: return "VisualBasic.png";
                    case FileType.XAML: return "XAML.png";
                    case FileType.XML: return "XML.png";
                    case FileType.XSD: return "XSD.png";
                    case FileType.XSLT: return "XSLT.png";
                    case FileType.Manifest: return "Manifest.png";
                    case FileType.Master: return "Master.png";
                    case FileType.Ruleset: return "Ruleset.png";
                    case FileType.Sequence: return "Sequence.png";
                    case FileType.Sitemap: return "Sitemap.png";
                    case FileType.Skin: return "Skin.png";
                    default:
                        return String.Empty;
                }
            }
        }
    }
}
