using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Earthworm.Components
{
    public class ghc_LatLong : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_LatLong class.
        /// </summary>
        public ghc_LatLong()
          : base("XYZ to Latitude Longitude", "ToLatLong",
              "Converts xyz to lat long",
              "Earthworm", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Points to convert to lat long", GH_ParamAccess.item);
            pManager.AddTextParameter("Projection", "PrjStr", "Projection of shapefile", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Latitude", "Lat", "Converted latitude value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Longitude", "Long", "Converted longitude value", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // Setup inputs
            Point3d pt = new Point3d();
            if (!DA.GetData("Point", ref pt)) return;

            string prj = "";
            if (!DA.GetData("Projection", ref prj)) return;

            double lat;
            double lon;

            helpers_Projection.UTMToLatLongDSP(pt.X, pt.Y, prj, out lat, out lon);

            DA.SetData(0, lat);
            DA.SetData(1, lon);
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
            get { return new Guid("8d5f5cbc-992e-4282-9ac0-28e823c3a509"); }
        }
    }
}