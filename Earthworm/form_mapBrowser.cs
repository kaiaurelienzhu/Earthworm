using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;


namespace Earthworm
{
    public partial class form_mapBrowser : Form
    {

        CropProperties _properties;

        public form_mapBrowser(CropProperties _prop)
        {
            _properties = _prop;
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Initialize map
            gmap.MapProvider = GMapProviders.GoogleHybridMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new PointLatLng(0, 0);
            gmap.MinZoom = 1;
            gmap.MaxZoom = 24;
            gmap.Zoom = 1;
            gmap.ShowCenter = false;

            // Initialize shapefile boundaries
            GMapOverlay polygons = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();

            // Create polygons + add to map Lat = Y, Lng = X
            points.Add(new PointLatLng(_properties.minLat, _properties.minLng));
            points.Add(new PointLatLng(_properties.maxLat, _properties.minLng));
            points.Add(new PointLatLng(_properties.maxLat, _properties.maxLng));
            points.Add(new PointLatLng(_properties.minLat, _properties.maxLng));

            // Creates polygons
            GMapPolygon polygon = new GMapPolygon(points, "Extents");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 1);
            polygons.Polygons.Add(polygon);
            gmap.Overlays.Add(polygons);

        }

        private void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

            }

        }
    }
}
