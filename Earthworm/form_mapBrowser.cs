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

        List<CropProperties> _properties = new List<CropProperties>();

        public form_mapBrowser(List<CropProperties> properties)
        {
            _properties = properties;
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Initialize map
            gmap.MapProvider = GMapProviders.GoogleHybridMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            GMapOverlay polygons = new GMapOverlay("polygons");

            foreach (CropProperties property in _properties)
            {


                gmap.Position = new PointLatLng(property.maxLat, property.maxLng);
                gmap.MinZoom = 1;
                gmap.MaxZoom = 24;
                gmap.Zoom = 10;
                gmap.ShowCenter = false;


                // Initialize shapefile boundaries

                List<PointLatLng> points = new List<PointLatLng>();

                // Create polygons + add to map Lat = Y, Lng = X
                points.Add(new PointLatLng(property.minLat, property.minLng));
                points.Add(new PointLatLng(property.maxLat, property.minLng));
                points.Add(new PointLatLng(property.maxLat, property.maxLng));
                points.Add(new PointLatLng(property.minLat, property.maxLng));

                // Creates polygons
                GMapPolygon polygon = new GMapPolygon(points, "Extents");
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Orange));
                polygon.Stroke = new Pen(Color.Orange, 1);
                polygons.Polygons.Add(polygon);


            }


            // Create dynamic input crop
            CropProperties firstCrop = _properties[0];

            // Parametric input
            if (firstCrop != null)
            {
                List<PointLatLng> cropPts = new List<PointLatLng>();


                cropPts.Add(new PointLatLng(firstCrop.minCropLat, firstCrop.minCropLng));
                cropPts.Add(new PointLatLng(firstCrop.maxCropLat, firstCrop.minCropLng));
                cropPts.Add(new PointLatLng(firstCrop.maxCropLat, firstCrop.maxCropLng));
                cropPts.Add(new PointLatLng(firstCrop.minCropLat, firstCrop.maxCropLng));


                // ADD POLYGON TO MAP
                GMapPolygon crop = new GMapPolygon(cropPts, "Crop");
                crop.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
                crop.Stroke = new Pen(Color.Red, 2);
                polygons.Polygons.Add(crop);
                gmap.Overlays.Add(polygons);

            }

            // UI input
            else
            {
                List<PointLatLng> cropPts = new List<PointLatLng>();


                cropPts.Add(new PointLatLng(0,0));
                cropPts.Add(new PointLatLng(1,0));
                cropPts.Add(new PointLatLng(1,1));
                cropPts.Add(new PointLatLng(0,1));


                // add crop polygon to map
                GMapPolygon crop = new GMapPolygon(cropPts, "Crop");
                crop.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
                crop.Stroke = new Pen(Color.Red, 2);
                polygons.Polygons.Add(crop);
                gmap.Overlays.Add(polygons);
            }

        }

        private void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            // Setup map overlay
            GMapOverlay polygons = new GMapOverlay("Polygons");
            CropProperties crop = _properties[0];

            // Register left mouse button
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
               
                PointLatLng pt = gmap.FromLocalToLatLng(e.X, e.Y);
       
                GMapMarker marker = new GMarkerGoogle(pt, GMarkerGoogleType.blue_pushpin);
                polygons.Markers.Add(marker);
                gmap.Overlays.Add(polygons);

                List<PointLatLng> cropPts = new List<PointLatLng>();
                cropPts.Add(pt);
                cropPts.Add(new PointLatLng(1, 0));
                cropPts.Add(new PointLatLng(1, 1));
                cropPts.Add(new PointLatLng(0, 1));

                // add crop polygon to map
                GMapPolygon cropB = new GMapPolygon(cropPts, "Crop");
                cropB.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
                cropB.Stroke = new Pen(Color.Red, 2);
                polygons.Polygons.Add(cropB);
                gmap.Overlays.Add(polygons);

                

            }

            

        }
    }
}
