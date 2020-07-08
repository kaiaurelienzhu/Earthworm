using System;
using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Data;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace Earthworm
{
    public static class helpers_UI
    {
        public static void DisplayForm(List<CropProperties> properties)
        {
 
            // Create form instance
            form_mapBrowser form = new form_mapBrowser(properties);
            Grasshopper.GUI.GH_WindowsFormUtil.CenterFormOnCursor(form, true);
            form.ShowDialog();
            
        }

        public static List<PointLatLng> CreateRectangle(PointLatLng min, PointLatLng max)
        {
            var points = new List<PointLatLng>();
            points.Add(min);
            points.Add(new PointLatLng(max.Lat, min.Lng));
            points.Add(max);
            points.Add(new PointLatLng(min.Lat, max.Lng));
            return points;
        }

        internal static GMapOverlay DrawRectangle(List<PointLatLng> rectangle, GMapOverlay cropOverlay)
        {
            GMapPolygon crop = new GMapPolygon(rectangle, "Crop");
            crop.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
            crop.Stroke = new Pen(Color.Red, 2);
            cropOverlay.Polygons.Add(crop);
            return cropOverlay;
        }
    }

    // Create properties which must be met for each shapefile
    public class CropProperties
    {
        public PointLatLng MinExtent { get; set; }
        public PointLatLng MaxExtent { get; set; }
        public PointLatLng MinCrop { get; set; }
        public PointLatLng MaxCrop { get; set; }
        public List<PointLatLng> UICrop { get; set; }
        public Shapefile shp { get; set; }
        public string Path { get; set; }
        public Color Color { get; set; }
  
    }

    public class RandomColorGenerator
    {
        public static Color RandomColor()
        {

            Random r = new Random();
            byte red = (byte)r.Next(0, 255);
            byte green = (byte)r.Next(0, 255);
            byte blue = (byte)r.Next(0, 255);

            return Color.FromArgb(80, red, green, blue);
        }
    }


}
