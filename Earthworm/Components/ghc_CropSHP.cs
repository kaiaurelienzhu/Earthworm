using DotSpatial.Data;
using GMap.NET;
using Grasshopper.Kernel;
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
            List<double> min = new List<double>();
            min.Add(0);
            min.Add(0);

            // Compulsory inputs
            pManager.AddBooleanParameter("Run", "R", "Press to run", GH_ParamAccess.item);
            
            // Options inputs
            Params.Input[
            pManager.AddTextParameter("Path", "P", "File path or directory of shapefile as string.", GH_ParamAccess.list)
            ].Optional = true;

            Params.Input[
            pManager.AddNumberParameter("South West Point", "SW", "South West extents as lat long coordinate", GH_ParamAccess.list, 0)
            ].Optional = true;

            Params.Input[
            pManager.AddNumberParameter("North East Point", "NE", "North East extents as lat long coordinate", GH_ParamAccess.list, 0)
            ].Optional = true;


        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool runIt = false;
            if (!DA.GetData("Run", ref runIt)) return;

            

            if (runIt)

            {

                // Setup variables
                List<string> paths = new List<string>();
                if (!DA.GetDataList(1, paths)) return;

                List<double> SW = new List<double>();
                if (!DA.GetDataList(2, SW)) return;


                List<double> NE = new List<double>();
                if (!DA.GetDataList(3, NE)) return;


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



                    // Reference inputs must be valid lat long
                    var minCropLng = SW[1];
                    var minCropLat = SW[0];
                    var maxCropLng = NE[1];
                    var maxCropLat = NE[0];

                    // Convert shp XY vals to Lat Lng and pass into Form properties
                    helpers_Projection.UTMToLatLongDSP(shp.Extent.MinX, shp.Extent.MinY, prjStr, out minLat, out minLng);
                    helpers_Projection.UTMToLatLongDSP(shp.Extent.MaxX, shp.Extent.MaxY, prjStr, out maxLat, out maxLng);

                    PointLatLng minExtent = new PointLatLng(minLat, minLng);
                    PointLatLng maxExtent = new PointLatLng(maxLat, maxLng);
                    PointLatLng minCrop = new PointLatLng(minCropLat, minCropLng);
                    PointLatLng maxCrop = new PointLatLng(maxCropLat, maxCropLng);
                    List<PointLatLng> uiCrop = new List<PointLatLng>();

                    // Create crop properties
                    CropProperties crop = new CropProperties(minExtent, maxExtent, minCrop, maxCrop, uiCrop);
                    properties.Add(crop);
                }

                // Display form
                helpers_UI.DisplayForm(properties);
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