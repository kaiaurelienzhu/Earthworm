using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public form_mapBrowser()
        {
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            //Initialize map
            gmap.MapProvider = GMapProviders.GoogleHybridMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new PointLatLng(0, 0);
            gmap.MinZoom = 1;
            gmap.MaxZoom = 24;
            gmap.Zoom = 1;
            gmap.ShowCenter = false;
            GMapOverlay polygons = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();
            points.Add(new PointLatLng(48.866383, 2.323575));
            points.Add(new PointLatLng(48.863868, 2.321554));
            points.Add(new PointLatLng(48.861017, 2.330030));
            points.Add(new PointLatLng(48.863727, 2.331918));
            GMapPolygon polygon = new GMapPolygon(points, "Jardin des Tuileries");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 1);
            polygons.Polygons.Add(polygon);
            gmap.Overlays.Add(polygons);



        }

        private void gmap_OnPolygonClick(GMapPolygon item, MouseEventArgs e)
        {
            Console.WriteLine(String.Format("Polygon {0} with tag {1} was clicked",
                item.Name, item.Tag));
        }

        private void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("This is just a test");
        }

        private void gmap_Click(object sender, EventArgs e)
        {
            Console.WriteLine("This is just a test");
        }

        private void gmap_OnPolygonEnter(GMapPolygon item, MouseEventArgs e)
        {
            Console.WriteLine("This is just a test");
        }

        private void form_mapBrowser_Load(object sender, EventArgs e)
        {

        }
    }
}
