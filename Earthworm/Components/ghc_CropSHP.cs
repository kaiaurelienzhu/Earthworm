using DotSpatial.Data;
using GMap.NET;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Earthworm.Components
{
    public class ghc_CropSHP : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_CropSHP class.
        /// </summary>
        public ghc_CropSHP()
          : base("Crop Shapefile", "CropSHP",
              "Crop a shapefile",
              "Earthworm", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {


            // Compulsory inputs
            pManager.AddBooleanParameter("Run", "R", "Press to run", GH_ParamAccess.item);
            
            // Options inputs
            Params.Input[
            pManager.AddTextParameter("Path", "P", "File path or directory of shapefile as string.", GH_ParamAccess.list)
            ].Optional = true;

            Params.Input[
            pManager.AddNumberParameter("South West Point", "SW", "South West extents as lat long coordinate", GH_ParamAccess.list)
            ].Optional = true;

            Params.Input[
            pManager.AddNumberParameter("North East Point", "NE", "North East extents as lat long coordinate", GH_ParamAccess.list)
            ].Optional = true;


        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Crop extents", "Pts", "Max and min points to WGS84", GH_ParamAccess.list);
        }

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


            // Setup default inputs
            List<double> SW = new List<double>();
            DA.GetDataList(2, SW);
            if (SW.Count == 0)
            {
                SW.Add(0);
                SW.Add(0);
            }

            List<double> NE = new List<double>();
            DA.GetDataList(3, NE);
            if (NE.Count == 0)
            {
                NE.Add(0);
                NE.Add(0);
            }


            if (runIt)

            {

                List<CropProperties> properties = new List<CropProperties>();

                foreach (string path in paths)
                {
                    // Open shapefile from path
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

                    Lats.Add(SW[0]);
                    Lats.Add(NE[0]);

                    Lngs.Add(SW[1]);
                    Lngs.Add(NE[1]);

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

                    // Create crop properties
                    CropProperties crop = new CropProperties(minExtent, maxExtent, minLatLng, maxLatLng, uiCrop, shp);
                    properties.Add(crop);



                    

                }

                // Display form
                helpers_UI.DisplayForm(properties);


                // Set output 
                CropProperties mainCrop = properties[0];
                List<Point3d> extents = new List<Point3d>();
                Point3d min = new Point3d(mainCrop.minCrop.Lng, mainCrop.minCrop.Lat, 0);
                Point3d max = new Point3d(mainCrop.maxCrop.Lng, mainCrop.maxCrop.Lat, 0);
                extents.Add(min);
                extents.Add(max);

                DA.SetDataList(0, extents);



            }

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
                return null;
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