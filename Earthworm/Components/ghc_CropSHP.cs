using System;
using System.Collections.Generic;
using DotSpatial.Data.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Topology;

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
            pManager.AddBooleanParameter("Run", "R", "Press to run", GH_ParamAccess.item);
            
            // Compulsory inputs
            Params.Input[
            pManager.AddTextParameter("Path", "P", "File path or directory of shapefile as string.", GH_ParamAccess.list)
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
            // Setup inputs
            bool runIt = false;
            if (!DA.GetData("Run", ref runIt)) return;

            if (runIt)

            {

                // Setup inputs
                List<string> paths = new List<string>();
                if (!DA.GetDataList(1, paths)) return;

                List<CropProperties> properties = new List<CropProperties>();
                foreach (string path in paths)
                {
                    // Open shapefile from path
                    Shapefile shp = Shapefile.OpenFile(path);
                    string prjStr = shp.ProjectionString;
                    string prj = shp.Projection.ToString();

                    // Find extents & centre of shapefile
                    double minLng = shp.Extent.MinX;
                    double minLat = shp.Extent.MinY;

                    double maxLng = shp.Extent.MaxX;
                    double maxLat = shp.Extent.MaxY;

                    double centreLng = shp.Extent.Center.X;
                    double centreLat = shp.Extent.Center.Y;

                    helpers_Projection.UTMToLatLongDSP(shp.Extent.Center.X, shp.Extent.Center.Y, prjStr, out centreLng, out centreLat);

                    // Convert XY vals to Lat Lng and pass into Form properties
                    helpers_Projection.UTMToLatLongDSP(shp.Extent.MinX, shp.Extent.MinY, prjStr, out minLat, out minLng);
                    helpers_Projection.UTMToLatLongDSP(shp.Extent.MaxX, shp.Extent.MaxY, prjStr, out maxLat, out maxLng);


                    CropProperties crop = new CropProperties(minLat, minLng, maxLat, maxLng, centreLat, centreLng);

                    properties.Add(crop);
                }
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