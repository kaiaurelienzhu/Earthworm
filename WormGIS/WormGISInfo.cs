using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace WormGIS
{
    public class WormGISInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "WormGIS";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "This plugin's purpose is to connect Grasshopper/Rhino to GIS data such as shapefiles.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("0a0a6f7d-e63d-4a2f-9e21-ffcc6cfec7db");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "kai.aurelien.zhu@gmail.com";
            }
        }
    }
}
