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
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
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
        GMapOverlay extentOverlay = new GMapOverlay("Extent overlay");
        GMapOverlay cropOverlay = new GMapOverlay("Crop overlay");
        GMapOverlay uiOverlay = new GMapOverlay("UI overlay");
        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Initialize map
            gmap.MapProvider = GMapProviders.GoogleHybridMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;

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
                extentOverlay.Polygons.Add(polygon);
                gmap.Overlays.Add(extentOverlay);


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
                cropOverlay.Polygons.Add(crop);
                gmap.Overlays.Add(cropOverlay);

            }


        }



        private void gmap_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Reset crop in all shapefiles
            foreach (CropProperties props in _properties)
            {
                props.maxCrop = new PointLatLng(0, 0);
                props.minCrop = new PointLatLng(0, 0);
            }
            uiOverlay.Clear();
            cropOverlay.Clear();
        }

        private void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            // Setup map overlay
            CropProperties crop = _properties[0];
            List<PointLatLng> pts = crop.uiCrop;


            // Register left mouse button
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PointLatLng pt = gmap.FromLocalToLatLng(e.X, e.Y);
                pts.Add(pt);

                if (pts.Count > 2)
                {
                    pts.Clear();
                    uiOverlay.Clear();
                    pts.Add(pt);
                }

                // Add a bounding box to map
                if (crop.uiCrop.Count == 2)
                {
                    // Reorder pts to ensure correct bottom left and top right
                    if (pts[0].Lat > pts[1].Lat && pts[0].Lng > pts[1].Lng)
                    {
                        PointLatLng temp0 = pts[0];
                        PointLatLng temp1 = pts[1];
                        pts.Clear();
                        pts.Add(temp1);
                        pts.Add(temp0);
                    }


                    uiOverlay.Clear();
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
                    gmap.Overlays.Add(uiOverlay);
                    uiOverlay.Polygons.Add(cropB);

                    foreach (CropProperties cropBoundary in _properties)
                    {
                        
                        cropBoundary.minCrop = pt0;
                        cropBoundary.maxCrop = pt2;
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Iterate through each shapefile
            foreach (CropProperties property in _properties)
            {
                // Get original shapefile and reproject to WGS84
                Shapefile file = property.shp;
                ProjectionInfo originalPrj = file.Projection;
                ProjectionInfo WGS84 = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String());
                file.Reproject(WGS84);

                // Create new shapefile & set projection
                FeatureSet result = new FeatureSet(FeatureType.Polygon);
                result.Projection = file.Projection;




                // Set new extent
                Extent extent = new Extent();
                extent.SetValues(property.minCrop.Lng, property.minCrop.Lat, property.maxCrop.Lng, property.maxCrop.Lat);


                result.CopyTableSchema(file);
                
                foreach (Feature f in file.Features)
                {
                    // Test to see if coord is within extent
                    Shape shp = f.ToShape();
                    IGeometry geom = shp.ToGeometry();
                    IList<Coordinate> coords = geom.Coordinates;
                    int hit = 0;
                    foreach (Coordinate coord in coords)
                    {
                        if (extent.Contains(coord))
                        {
                            hit++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (hit != 0)
                    {
                        // Iterate through coords in list
                        Polygon poly = new Polygon(coords);
                        result.AddFeature(poly).CopyAttributes(f);
                    }
                    else
                    {
                        continue;
                    }

                }
                result.Reproject(originalPrj);
                result.SaveAs(file.FilePath + "_EW.shp", true);
                this.Close();
            }
        }
    }
}
