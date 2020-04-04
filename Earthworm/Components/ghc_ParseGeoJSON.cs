using System;
using System.Collections.Generic;
using System.Text;
using Grasshopper.Kernel;
using Rhino.Geometry;



namespace Earthworm.Components
{
    public class ghc_ParseGeoJSON : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ghc_ParseGeoJSON class.
        /// </summary>
        public ghc_ParseGeoJSON()
          : base("ParseGeoJSON", "ParseGJSON",
              "Description",
              "Earthworm", "Util")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "File path or directory of shapefile as string.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "R", "Press to run", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "GJSON", "File path or directory of shapefile as string.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            bool runIt = false;
            if (!DA.GetData("Run", ref runIt)) return;

            string path = "";
            if (!DA.GetData("Path", ref path)) return;

            if (runIt)
            {
                StringBuilder strBuilder;
                helpers_Conversions.ConvertSHPtoGJSON(path, out strBuilder);
                string gJSON = strBuilder.ToString();
                DA.SetData(0, gJSON);
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
            get { return new Guid("f1411b45-3b00-4f3e-bb1a-7272ed45abd5"); }
        }
    }
}