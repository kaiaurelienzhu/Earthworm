using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using DotSpatial.Data;
using DotSpatial.Serialization;
using Grasshopper;
using Grasshopper.Kernel.Data;
using System.Data;
using Earthworm.Properties;


// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Earthworm.Components
{
    public class ghc_ParseSHP : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ghc_ParseSHP()
          : base("Parse SHP", "ParseSHP",
              "Parses a shapefile into the Grasshopper environment",
              "Earthworm", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            // Compulsory inputs
            pManager.AddTextParameter("File Path", "P", "File path or directory of shapefile as string.", GH_ParamAccess.item);

            // Optional inputs
            Params.Input[
            pManager.AddVectorParameter("Vector", "V", "Optional translation vector to working origin", GH_ParamAccess.item)
            ].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            
            pManager.AddTextParameter("Key", "K ", "Keys of feature attributes", GH_ParamAccess.tree);
            pManager.AddTextParameter("Value", "V ", "Values of feature attributes", GH_ParamAccess.tree);
            pManager.AddPointParameter("Points", "Pts", "Feature geometry as point", GH_ParamAccess.tree);
            pManager.AddTextParameter("Projection String", "PrjStr", "The projection string of shapefile. Used for projections.", GH_ParamAccess.item);
            pManager.AddTextParameter("Projection", "Prj", "Human readable projection of shapefile", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Setup inputs
            string path = "";
            if (!DA.GetData("File Path", ref path)) return;
            Vector3d vec = new Vector3d(0, 0, 0);
            DA.GetData("Vector", ref vec);

            // Setup output
            DataTree<Point3d> ptTree = new DataTree<Point3d>();
            DataTree<string> keysTree = new DataTree<string>();
            DataTree<string> valsTree = new DataTree<string>();

            // Open shapefile from path
            Shapefile shp = Shapefile.OpenFile(path);
            string prjStr = shp.ProjectionString;
            string prj = shp.Projection.ToString();

            //Read features in the shapefile 
            int pathCount = 0;
            foreach (Feature f in shp.Features)
            {

                valsTree = GetFeatureValues(f, pathCount, valsTree);
                keysTree = GetFeatureKeys(f, pathCount, keysTree);
                ptTree = GetFeaturePts(f, pathCount, ptTree, vec);
                // Increment path
                pathCount++;
            }

            //Output the data
            DA.SetDataTree(0, keysTree);
            DA.SetDataTree(1, valsTree);
            DA.SetDataTree(2, ptTree);
            DA.SetData(3, prjStr);
            DA.SetData(4, prj);
        }

        private DataTree<Point3d> GetFeaturePts(Feature f, int pathCount, DataTree<Point3d> ptTree, Vector3d vec)
        {
            List<Point3d> pts = new List<Point3d>();
            GH_Path p = new GH_Path(pathCount);
            IList<DotSpatial.Topology.Coordinate> coords = f.Coordinates;
            foreach (DotSpatial.Topology.Coordinate coord in coords)
            {
                Point3d pt = new Point3d(coord.X + vec.X, coord.Y + vec.Y, 0 + vec.Z);
                pts.Add(pt);
            }
            ptTree.AddRange(pts, p);
            return ptTree;
        }
        private DataTree<string> GetFeatureKeys(Feature f, int pathCount, DataTree<string> keysTree)
        {
            DataRow dataRow = f.DataRow;
            GH_Path p = new GH_Path(pathCount);
            DataTable table = dataRow.Table;
            DataColumnCollection columns = table.Columns;
            DataColumn[] dca = new DataColumn[columns.Count];
            columns.CopyTo(dca, 0);
            List<string> keys = new List<string>();
            foreach (DataColumn col in dca)
            {

                keys.Add(col.ColumnName.ToString());
            }
            keysTree.AddRange(keys, p);
            return keysTree;
        }
        private DataTree<string> GetFeatureValues(Feature f, int pathCount, DataTree<string> valsTree)
        {
            DataRow dataRow = f.DataRow;
            GH_Path p = new GH_Path(pathCount);
            List<string> vals = new List<string>();
            foreach (object item in dataRow.ItemArray)
            {
                vals.Add(item.ToString());
            }
            valsTree.AddRange(vals, p);
            return valsTree;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resources.Earthworm_ParseSHP;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("112907d2-b9fc-45f5-9c06-677b42d95349"); }
        }
    }
}
