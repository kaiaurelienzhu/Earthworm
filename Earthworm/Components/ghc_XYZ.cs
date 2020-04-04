using System;
using System.Collections.Generic;
using Earthworm.Properties;
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
          : base("Lat Long to Point", "toXYZ",
              "Description",
              "Earthworm", "Util")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Latitude", "Lat", "Latitude value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Longitude", "Long", "Longitude value", GH_ParamAccess.item);
            pManager.AddTextParameter("Projection", "PrjStr", "Projection of shapefile", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Converted point", GH_ParamAccess.item);
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

            double lng = 0;
            if (!DA.GetData("Latitude", ref lng)) return;

            string prj = "";
            if (!DA.GetData("Projection", ref prj)) return;

            double X;
            double Y;

            helpers_Conversions.DefaultProjectPts(new Point3d(lat, lng, 0), prj, out X, out Y);
            Point3d pt = new Point3d(X, Y, 0);
            DA.SetData(0, pt);
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
                return Resources.LatLong_24x24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4f0206ed-a878-4597-aecc-85fdbcc70c2c"); }
        }
    }
}