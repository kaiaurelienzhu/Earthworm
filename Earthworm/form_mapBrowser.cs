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


            // WIP CREATE CROP POLYGON WITH DYNAMIC INPUT
            CropProperties firstCrop = _properties[0];
            List<PointLatLng> cropPts = new List<PointLatLng>();
            //cropPts.Add(new PointLatLng(firstCrop.minCropLat, firstCrop.minCropLng));
            //cropPts.Add(new PointLatLng(firstCrop.maxCropLat, firstCrop.minCropLng));
            //cropPts.Add(new PointLatLng(firstCrop.maxCropLat, firstCrop.maxCropLng));
            //cropPts.Add(new PointLatLng(firstCrop.minCropLat, firstCrop.maxCropLng));
            cropPts.Add(new PointLatLng(0, 0));
            cropPts.Add(new PointLatLng(1, 0));
            cropPts.Add(new PointLatLng(1, 1));
            cropPts.Add(new PointLatLng(0, 1));


            // AWDD POLYGON TO MAP
            GMapPolygon crop = new GMapPolygon(cropPts, "Crop");
            crop.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
            crop.Stroke = new Pen(Color.Red, 2);
            polygons.Polygons.Add(crop);
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
