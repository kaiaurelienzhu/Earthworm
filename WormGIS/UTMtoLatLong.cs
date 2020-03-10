using DotSpatial.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WormGIS
{
    class UTMtoLatLong
    {
        public static void UTMToLatLongDSP(double X, double Y, string utmStr,  out double latitude, out double longitude)
        {
            double[] XY = new double[2];
            XY[0] = X;
            XY[1] = Y;

            double[] Z = new double[1];
            Z[0] = 1;

            //string utmStr = "+proj=utm +zone=30 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ";

            ProjectionInfo projIn = ProjectionInfo.FromProj4String(utmStr);
            ProjectionInfo projOut = KnownCoordinateSystems.Geographic.World.WGS1984;
            Reproject.ReprojectPoints(XY, Z, projIn, projOut, 0, 1);

            longitude = XY[0];
            latitude = XY[1];
        }

    }
}

