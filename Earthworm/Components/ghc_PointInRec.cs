using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Earthworm.Components
{
    public class ghc_PointInRec : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_PointInRec class.
        /// </summary>
        public ghc_PointInRec()
          : base("WIP-Point in rec", "PointInRec",
              "Test point in rec function",
              "Earthworm", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("Rectangle", "R", "Input Rectangle", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "P", "Input point", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Interior", "Boolean", "True = interior", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d pt = new Point3d();
            if (!DA.GetData("Point", ref pt)) return;

            Rectangle3d rec = new Rectangle3d();
            if (!DA.GetData("Rectangle", ref rec)) return;

            bool result = helpers_Conversions.PointInRect(pt, rec);
            DA.SetData(0, result);
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
            get { return new Guid("225de0c0-3834-49ae-81e3-a41550a6b458"); }
        }
    }
}