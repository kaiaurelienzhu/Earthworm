using DotSpatial.Projections;
using System.Text;
using Rhino.Geometry;
using OSGeo.OGR;

namespace Earthworm
{
    class helpers_Conversions
    {
        // This method converts latitude longitude to UTM xyz pts
        public static void LatLongDSPToUTM(double latitude, double longitude, string utmStr, out double X, out double Y)
        {
            double[] latlong = new double[2];
            latlong[0] = latitude;
            latlong[1] = longitude;

            double[] Z = new double[1];
            Z[0] = 1;

            //string utmStr = "+proj=utm +zone=30 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ";


            ProjectionInfo projIn = ProjectionInfo.FromProj4String(" +proj=longlat +a=6378137 +b=6356752.31424518 +no_defs");
            ProjectionInfo projOut = ProjectionInfo.FromProj4String(utmStr);

            Reproject.ReprojectPoints(latlong, Z, projIn, projOut, 0, 1);
            Y = latlong[0];
            X = latlong[1];
        }

        // This method converts XYZ UTM pts to latitude longitude
        public static void UTMToLatLongDSP(double X, double Y, string utmStr, out double latitude, out double longitude)
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

        public static void ProjectPts(Point3d point, string inPrjStr, string outPrjStr, out double outX, out double outY)
        {
            double[] XY = new double[2];
            XY[0] = point.X;
            XY[1] = point.Y;

            double[] Z = new double[1];
            Z[0] = 1;

            //string utmStr = "+proj=utm +zone=30 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ";

            ProjectionInfo projIn = ProjectionInfo.FromProj4String(inPrjStr);
            ProjectionInfo projOut = ProjectionInfo.FromProj4String(outPrjStr);
            Reproject.ReprojectPoints(XY, Z, projIn, projOut, 0, 1);

            outX = XY[0];
            outY = XY[1];
        }

        public static void DefaultProjectPts(Point3d point, string inPrjStr, out double outX, out double outY)
        {
            double[] XY = new double[2];
            XY[0] = point.X;
            XY[1] = point.Y;

            double[] Z = new double[1];
            Z[0] = 1;

            ProjectionInfo projIn = ProjectionInfo.FromProj4String(inPrjStr);
            ProjectionInfo projOut = ProjectionInfo.FromProj4String(" +proj=longlat +a=6378137 +b=6356752.31424518 +no_defs");
            Reproject.ReprojectPoints(XY, Z, projIn, projOut, 0, 1);


            outX = XY[0];
            outY = XY[1];
        }

        public static void ConvertSHPtoGJSON(string shapefilePath, out StringBuilder gJSON)
        {
            string shpPath = shapefilePath;

            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");

            var ds = drv.Open(shpPath, 0);

            /*
            Driver geoJSONDriver = Ogr.GetDriverByName("GeoJSON");

            string geojsonfilepath = @"c:\temp\us_counties_test.json";

            if (System.IO.File.Exists(geojsonfilepath))
                System.IO.File.Delete(geojsonfilepath);

            geoJSONDriver.CreateDataSource(@"c:\temp\us_counties_test.json", null);
            */

            OSGeo.OGR.Layer layer = ds.GetLayerByIndex(0);

            OSGeo.OGR.Feature f;
            layer.ResetReading();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var options = new[]
            {
                "-f", "GeoJSON",
                "-lco", "RFC7946=YES"
            };


            while ((f = layer.GetNextFeature()) != null)
            {
                var geom = f.GetGeometryRef();
                if (geom != null)
                {
                    var geometryJson = geom.ExportToJson(options);
                    sb.AppendLine(geometryJson);
                }
            }

            gJSON = sb;
        }

    }



}
