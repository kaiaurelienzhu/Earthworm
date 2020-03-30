using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using DotSpatial.Data;
using DotSpatial.Projections;

namespace Earthworm.Components
{
    public class ghc_Reproject : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_Reproject class.
        /// </summary>
        public ghc_Reproject()
          : base("Reproject pts", "Reproject",
              "Reprojects features from one coordinate system to another",
              "Earthworm", "Util")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("From Projection", "FromPrjStr", "The projection string of used for projections.", GH_ParamAccess.item);

            Params.Input[
            pManager.AddTextParameter("To Projection", "ToPrjStr", "The projection string of used for projections.", GH_ParamAccess.item)
            ].Optional = true;

            pManager.AddPointParameter("Points", "Pt", "The points to be transformed by new projection.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "The points to be transformed by new projection.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Setup inputs
            string fromPrj = "";
            if(!DA.GetData(0, ref fromPrj)) return;

            string toPrj = "";
            DA.GetData(1, ref toPrj);


            List<Point3d> pts = new List<Point3d>();
            if (!DA.GetDataList(2, pts)) return;

            // Setup outputs
            List<Point3d> outPts = new List<Point3d>();

            foreach (Point3d pt in pts)
            {
                // Set up temp inputs
                double x = 0;
                double y = 0;

                // Reproject pts 
                if (toPrj == "")
                {
                    helpers_Conversions.DefaultProjectPts(pt, fromPrj, out x, out y);
                }

                else
                {
                    helpers_Conversions.ProjectPts(pt, fromPrj, toPrj, out x, out y);
                }
                
                Point3d outPt = new Point3d(x, y, 0);
                outPts.Add(outPt);
            }

            DA.SetDataList(0, outPts);
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
            get { return new Guid("7d20f68a-0c2a-4ecf-9ad0-f0d5bfbeb83b"); }
        }
    }
}