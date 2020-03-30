using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using DotSpatial.Data;
using DotSpatial.Projections;

namespace Earthworm.Components
{
    public class ghc_ParsePRJ : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_ParsePRJ class.
        /// </summary>
        public ghc_ParsePRJ()
          : base("Parse a PRJ file", "ParsePRJ",
              "Parse a projection file for in grasshopper projections",
              "Earthworm", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Compulsory inputs
            pManager.AddTextParameter("Path", "P", "File path or directory of shapefile as string.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Projection String", "PrjStr", "The projection string of prj. Used for projections.", GH_ParamAccess.item);
            pManager.AddTextParameter("Projection", "Prj", "Human readable projection of .prj", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Setup inputs
            string path = "";
            if (!DA.GetData("Path", ref path)) return;

            ProjectionInfo prjFile = ProjectionInfo.Open(path);
            string prjStr = prjFile.ToProj4String();
            string prj = prjFile.ToString();

            DA.SetData(0, prjStr);
            DA.SetData(1, prj);
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
            get { return new Guid("200f792e-7d26-45b5-befe-1ab1e7e3de9b"); }
        }
    }
}