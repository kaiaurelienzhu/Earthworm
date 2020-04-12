using DotSpatial.Data;
using GMap.NET;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Earthworm.Properties;
using System.Drawing;

namespace Earthworm.Components
{
    public class ghc_CropSHP : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_CropSHP class.
        /// </summary>
        public ghc_CropSHP()
          : base("Crop Shapefiles", "CropSHP",
              "Batch crop a list of shapefiles based off UI input or coordinates",
              "Earthworm", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {


            // Compulsory inputs
            pManager.AddBooleanParameter("Run", "Run", "Change to True to run", GH_ParamAccess.item);
            
            // Options inputs
            Params.Input[
            pManager.AddTextParameter("Path", "inPaths", "File path of shapefile as string.", GH_ParamAccess.list)
            ].Optional = true;

            Params.Input[
            pManager.AddNumberParameter("Min LatLng pt", "LatLng1", "South West extents as lat long coordinate", GH_ParamAccess.list)
            ].Optional = true;

            Params.Input[
            pManager.AddNumberParameter("Max LatLng pt", "LagLng2", "North East extents as lat long coordinate", GH_ParamAccess.list)
            ].Optional = true;

            Params.Input[
            pManager.AddTextParameter("Target Path", "outPaths", "File paths of shapefiles to crop.", GH_ParamAccess.list)
            ].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("North East point", "Pt1", "Max point in WGS84 coordinates", GH_ParamAccess.item);
            pManager.AddPointParameter("South West point", "Pt2", "Min point in WGS84 coordinates", GH_ParamAccess.item); ;
        }


        // Setup persistent data
        List<Point3d> extents = new List<Point3d>();
        List<CropProperties> properties = new List<CropProperties>();

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool runIt = false;
            if (!DA.GetData("Run", ref runIt)) return;

            // Setup variables
            List<string> paths = new List<string>();
            if (!DA.GetDataList(1, paths)) return;

            List<string> outPaths = new List<string>();
            if (!DA.GetDataList(4, outPaths)) return;

            if (paths.Count != outPaths.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Filepath counts do not match, please check your inputs");
            }

            // Setup default inputs
            List<double> Pt1 = new List<double>();
            DA.GetDataList(2, Pt1);
            if (Pt1.Count == 0)
            {
                Pt1.Add(0);
                Pt1.Add(0);
            }

            List<double> Pt2 = new List<double>();
            DA.GetDataList(3, Pt2);
            if (Pt2.Count == 0)
            {
                Pt2.Add(0);
                Pt2.Add(0);
            }


            // Enumerate through shapefile paths
            for (int i = 0; i < paths.Count; i++)
            {
                // Get file patgs
                string path = paths[i];
                string outPath = outPaths[i];

                // Open shapefile + data
                Shapefile shp = Shapefile.OpenFile(path);
                string prjStr = shp.ProjectionString;
 
                // Find extents & centre of shapefile
                double minLng = shp.Extent.MinX;
                double minLat = shp.Extent.MinY;

                double maxLng = shp.Extent.MaxX;
                double maxLat = shp.Extent.MaxY;

                double centreLng = shp.Extent.Center.X;
                double centreLat = shp.Extent.Center.Y;

                // Reorder pts to ensure max and min are valid inputs
                List<double> Lats = new List<double>();
                List<double> Lngs = new List<double>();

                Lats.Add(Pt1[0]);
                Lats.Add(Pt2[0]);

                Lngs.Add(Pt1[1]);
                Lngs.Add(Pt2[1]);

                Lats.Sort();
                Lngs.Sort();

                PointLatLng minLatLng = new PointLatLng(Lats[0], Lngs[0]);
                PointLatLng maxLatLng = new PointLatLng(Lats[1], Lngs[1]);


                // Convert shp XY vals to Lat Lng and pass into Form properties
                helpers_Conversions.UTMToLatLongDSP(shp.Extent.MinX, shp.Extent.MinY, prjStr, out minLat, out minLng);
                helpers_Conversions.UTMToLatLongDSP(shp.Extent.MaxX, shp.Extent.MaxY, prjStr, out maxLat, out maxLng);

                PointLatLng minExtent = new PointLatLng(minLat, minLng);
                PointLatLng maxExtent = new PointLatLng(maxLat, maxLng);
                List<PointLatLng> uiCrop = new List<PointLatLng>();

                // Apply random colour
                Color color = RandomColorGenerator.RandomColor();
                // Create crop properties + increment
                CropProperties crop = new CropProperties(minExtent, maxExtent, minLatLng, maxLatLng, uiCrop, shp, outPath, color);


                // Limit properties added to persistent data
                if (properties.Count < paths.Count)
                {
                    properties.Add(crop);
                }

                else if (properties.Count > paths.Count)
                {
                    properties.Clear();
                    properties.Add(crop);
                }

                else if (properties.Count == paths.Count)
                {
                    properties[i].shp = shp;
                    properties[i].path = crop.path;
                    properties[i].color = crop.color;
                    properties[i].minExtent = crop.minExtent;
                    properties[i].maxExtent = crop.maxExtent;

                    // If there is GH input: replace crop
                    if (Pt2[0] != 0 && Pt1[0] != 0)
                    {
                        properties[i].minCrop = crop.minCrop;
                        properties[i].maxCrop = crop.maxCrop;
                    }

                }


                

            }

            // Define extents
            CropProperties mainCrop = properties[0];
            if (extents.Count != 2)
            {
                Point3d min = new Point3d(mainCrop.minCrop.Lng, mainCrop.minCrop.Lat, 0);
                Point3d max = new Point3d(mainCrop.maxCrop.Lng, mainCrop.maxCrop.Lat, 0);
                extents.Add(min);
                extents.Add(max);
            }

            // Button press show data
            if (runIt)
            {
                // Display form
                helpers_UI.DisplayForm(properties);

            }


            // Set output 
            Point3d newmin = new Point3d(mainCrop.minCrop.Lng, mainCrop.minCrop.Lat, 0);
            Point3d newmax = new Point3d(mainCrop.maxCrop.Lng, mainCrop.maxCrop.Lat, 0);
            extents.Clear();
            extents.Add(newmin);
            extents.Add(newmax);
            DA.SetData(0, extents[0]);
            DA.SetData(1, extents[1]);
        }



        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.Earthworm_ParseSHP;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1694b71c-ff87-4548-9741-205dcd97c2ca"); }
        }
    }
}