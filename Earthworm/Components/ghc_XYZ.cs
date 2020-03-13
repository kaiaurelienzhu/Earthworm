using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Earthworm.Components
{
    public class ghc_XYZ : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_XYZ class.
        /// </summary>
        public ghc_XYZ()
          : base("Latitude Longitude to XYZ", "ToXYZ",
              "Converts latitude longitude coordinates to XYZ",
              "Earthworm", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Latitude", "Lat", "Latitude to convert", GH_ParamAccess.item);
            pManager.AddNumberParameter("Longitude", "Long", "Longitude to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("Projection", "PrjStr", "Projection of shapefile", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "Converted X value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "Converted Y value", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Setup inputs
            double lat = 0;
            if (!DA.GetData("Latitude", ref lat)) return;

            double lon = 0;
            if (!DA.GetData("Longitude", ref lon)) return;

            string prj = "";
            if (!DA.GetData("Projection", ref prj)) return;

            double X;
            double Y;

            helpers_Projection.LatLongDSPToUTM(lat, lon, prj, out X, out Y);

            DA.SetData(0, X);
            DA.SetData(1, Y);
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
            get { return new Guid("afebe372-6236-4922-b7d8-dac021e920a1"); }
        }
    }
}