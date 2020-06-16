﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;


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

        // Setup overlays
        GMapOverlay cropOverlay = new GMapOverlay("Crop overlay");
        GMapOverlay uiOverlay = new GMapOverlay("UI overlay");
        private void gMapControl1_Load(object sender, EventArgs e)
        {


            // Initialize map
            gmap.MapProvider = GMapProviders.GoogleHybridMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;


            // Iterate over each shapefile
            foreach (CropProperties property in _properties)
            {

                gmap.Position = property.MaxExtent;
                gmap.MinZoom = 1;
                gmap.MaxZoom = 24;
                gmap.Zoom = 10;
                gmap.ShowCenter = false;



                // Initialize shapefile boundaries
                List<PointLatLng> points = new List<PointLatLng>();

                // Create polygons + add to map Lat = Y, Lng = X
                points.Add(property.MinExtent);
                points.Add(new PointLatLng(property.MaxExtent.Lat, property.MinExtent.Lng));
                points.Add(property.MaxExtent);
                points.Add(new PointLatLng(property.MinExtent.Lat, property.MaxExtent.Lng));

                // Creates extents and style
                Color col = property.Color;
                GMapOverlay extentOverlay = new GMapOverlay("Extent overlay");

                GMapPolygon polygon = new GMapPolygon(points, "Extents")
                {
                   Fill = new SolidBrush(Color.FromArgb(80, col.R, col.G, col.B)),
                   Stroke = new Pen(Color.FromArgb(col.R, col.G, col.B), 2)
                };

                extentOverlay.Polygons.Add(polygon);
                gmap.Overlays.Add(extentOverlay);


                // Add colour and shapefile names to list view
                ListViewItem item = new ListViewItem(property.shp.Name)
                {
                    BackColor = Color.FromArgb(50, col.R, col.G, col.B),
                    Checked = true
                };
                listView1.Items.Add(item);
            }


            // Create dynamic input crop
            CropProperties firstCrop = _properties[0];

            // Parametric input
            if (firstCrop != null)
            {
                List<PointLatLng> cropPts = new List<PointLatLng>();


                cropPts.Add(firstCrop.MinCrop);
                cropPts.Add(new PointLatLng(firstCrop.MaxCrop.Lat, firstCrop.MinCrop.Lng));
                cropPts.Add(firstCrop.MaxCrop);
                cropPts.Add(new PointLatLng(firstCrop.MinCrop.Lat, firstCrop.MaxCrop.Lng));



                // ADD POLYGON TO MAP
                var rand = new Random();
                GMapPolygon crop = new GMapPolygon(cropPts, "Crop");
                crop.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
                crop.Stroke = new Pen(Color.Red, 2);
                cropOverlay.Polygons.Add(crop);
                gmap.Overlays.Add(cropOverlay);

            }


        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Reset crop in all shapefiles
            foreach (CropProperties props in _properties)
            {
                props.MaxCrop = new PointLatLng(0, 0);
                props.MinCrop = new PointLatLng(0, 0);
            }
            uiOverlay.Clear();
            cropOverlay.Clear();
        }

        private void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            // Setup map overlay
            CropProperties crop = _properties[0];
            List<PointLatLng> pts = crop.UICrop;


            // Register left mouse button
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PointLatLng pt = gmap.FromLocalToLatLng(e.X, e.Y);
                pts.Add(pt);

                if (pts.Count > 2)
                {
                    pts.Clear();
                    cropOverlay.Clear();
                    uiOverlay.Clear();
                    pts.Add(pt);
                }

                // Add a bounding box to map
                if (crop.UICrop.Count == 2)
                {
                    
                    // Reorder pts to ensure max and min are correct.
                    List<double> Lats = new List<double>();
                    List<double> Lngs = new List<double>();

                    Lats.Add(pts[0].Lat);
                    Lats.Add(pts[1].Lat);
                    Lngs.Add(pts[0].Lng);
                    Lngs.Add(pts[1].Lng);

                    Lats.Sort();
                    Lngs.Sort();

                    PointLatLng min = new PointLatLng(Lats[0], Lngs[0]);
                    PointLatLng max = new PointLatLng(Lats[1], Lngs[1]);

                    pts.Clear();
                    pts.Add(min);
                    pts.Add(max);


                    // Clear the overlay and create rectangle polygon 
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

                    // Render the polygon
                    cropOverlay.Clear();
                    gmap.Overlays.Remove(uiOverlay);
                    GMapPolygon cropB = new GMapPolygon(finalPts, "Crop");
                    cropB.Fill = new SolidBrush(Color.FromArgb(80, Color.Red));
                    cropB.Stroke = new Pen(Color.Red, 4);
                    gmap.Overlays.Add(uiOverlay);
                    uiOverlay.Polygons.Add(cropB);

                    // Set the crop boundary max and min
                    foreach (CropProperties cropBoundary in _properties)
                    {
                        
                        cropBoundary.MinCrop = pt0;
                        cropBoundary.MaxCrop = pt2;
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Iterate through each properties of shapefiles
            foreach (CropProperties property in _properties)
            {
                // Test if checked or not
                foreach (ListViewItem item in listView1.CheckedItems)
                {
                    if (item.Text.Contains(property.shp.Name))
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
                        extent.SetValues(property.MinCrop.Lng, property.MinCrop.Lat, property.MaxCrop.Lng, property.MaxCrop.Lat);

                        // Copy feature data
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
                        // Project pts back to original and save
                        result.Reproject(originalPrj);
                        if (property.Path.Contains(".shp"))
                        {
                            result.SaveAs(property.Path, true);
                        }

                        else
                        {
                            result.SaveAs(property.Path + ".shp", true);
                        }

                    }
                }
                
            }
            this.Close();
        }

    }
}
