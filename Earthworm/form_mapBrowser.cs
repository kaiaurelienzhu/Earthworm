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
            List<PointLatLng> uiCropPts = _properties[0].uiCrop;

            foreach (CropProperties property in _properties)
            {
                gmap.Position = property.maxExtent;
                gmap.MinZoom = 1;
                gmap.MaxZoom = 24;
                gmap.Zoom = 10;
                gmap.ShowCenter = false;


                // Initialize shapefile boundaries
                List<PointLatLng> points = new List<PointLatLng>();

                // Create polygons + add to map Lat = Y, Lng = X
                points.Add(property.minExtent);
                points.Add(new PointLatLng(property.maxExtent.Lat, property.minExtent.Lng));
                points.Add(property.maxExtent);
                points.Add(new PointLatLng(property.minExtent.Lat, property.maxExtent.Lng));

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


                cropPts.Add(firstCrop.minCrop);
                cropPts.Add(new PointLatLng(firstCrop.maxCrop.Lat, firstCrop.minCrop.Lng));
                cropPts.Add(firstCrop.maxCrop);
                cropPts.Add(new PointLatLng(firstCrop.minCrop.Lat, firstCrop.maxCrop.Lng));


                // ADD POLYGON TO MAP
                GMapPolygon crop = new GMapPolygon(cropPts, "Crop");
                crop.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
                crop.Stroke = new Pen(Color.Red, 2);
                polygons.Polygons.Add(crop);
                gmap.Overlays.Add(polygons);

            }


        }



        private void gmap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Setup map overlay
            GMapOverlay polygonOverlay = new GMapOverlay("Polygons");
            CropProperties crop = _properties[0];
            List<PointLatLng> pts = crop.uiCrop;


            // Register left mouse button
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PointLatLng pt = gmap.FromLocalToLatLng(e.X, e.Y);
                if (pts.Count < 2)
                {
                    pts.Add(pt);
                }


                // Add a bounding box to map
                if (crop.uiCrop.Count == 2)
                {
                    List<PointLatLng> finalPts = new List<PointLatLng>();

                    PointLatLng pt0 = pts[0];
                    PointLatLng pt1 = new PointLatLng(pts[1].Lat, pts[0].Lng);
                    PointLatLng pt2 = pts[1];
                    PointLatLng pt3 = new PointLatLng(pts[0].Lat, pts[1].Lng);

                    finalPts.Add(pt0);
                    finalPts.Add(pt1);
                    finalPts.Add(pt2);
                    finalPts.Add(pt3);

                    GMapPolygon cropB = new GMapPolygon(finalPts, "Crop");
                    cropB.Fill = new SolidBrush(Color.FromArgb(20, Color.White));
                    cropB.Stroke = new Pen(Color.Red, 2);
                    gmap.Overlays.Add(polygonOverlay);
                    polygonOverlay.Polygons.Add(cropB);

                }
            }
        }
    } 
}
