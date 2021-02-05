using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameProj
{
    public class ImageSourceAttachedProperty : DependencyObject
    {
        public static string GetSvgSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SvgSourceProperty);
        }

        public static void SetSvgSource(DependencyObject obj, string value)
        {
            obj.SetValue(SvgSourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var svgControl = obj as ImageBrush;
            var path = (string)e.NewValue;
            if (svgControl != null && !string.IsNullOrEmpty(path))
            {
                
                WpfDrawingSettings settings = new WpfDrawingSettings();
                settings.IncludeRuntime = true;
                settings.TextAsGeometry = false;

                // 2. Select a file to be converted
                string svgTestFile = path;

                // 3. Create a file reader
                FileSvgReader converter = new FileSvgReader(settings);
                // 4. Read the SVG file
                DrawingGroup drawing = converter.Read(svgTestFile);

                if (drawing != null)
                {
                    svgControl.ImageSource = new DrawingImage(drawing);
                }
            }
        }

        public static readonly DependencyProperty SvgSourceProperty =
            DependencyProperty.RegisterAttached("SvgSource",
                typeof(string), typeof(ImageSourceAttachedProperty),
                // default value: null
                new PropertyMetadata(null, OnSourceChanged));
    }
}
