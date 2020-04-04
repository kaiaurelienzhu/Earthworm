using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Data;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;


namespace Earthworm
{
    class helpers_UI
    {
        public static void DisplayForm(List<CropProperties> properties)
        {
 
            // Create form instance
            form_mapBrowser form = new form_mapBrowser(properties);
            Grasshopper.GUI.GH_WindowsFormUtil.CenterFormOnCursor(form, true);
            form.ShowDialog();
            
        }

    }

    // Create properties which must be met for each shapefile
    public class CropProperties
    {
        private PointLatLng _minExtent;
        private PointLatLng _maxExtent;
        private PointLatLng _minCrop;
        private PointLatLng _maxCrop;
        private List<PointLatLng> _uiCrop;
        private Shapefile _shp;
        private string _path;
        private Color _color;


        public CropProperties(PointLatLng minExtent, PointLatLng maxExtent, PointLatLng minCrop, PointLatLng maxCrop, List<PointLatLng> uiCrop, Shapefile shp, string path, Color color)
        {
            _minExtent = minExtent;
            _maxExtent = maxExtent;
            _minCrop = minCrop;
            _maxCrop = maxCrop;
            _uiCrop = uiCrop;
            _shp = shp;
            _path = path;
            _color = color;
        }


        public PointLatLng minExtent
        {
            get
            {
                return _minExtent;
            }

            set
            {
                _minExtent = value;
            }
        }

        public PointLatLng maxExtent
        {
            get
            {
                return _maxExtent;
            }

            set
            {
                _maxExtent = value;
            }
        }

        public PointLatLng minCrop
        {
            get
            {
                return _minCrop;
            }

            set
            {
                _minCrop = value;
            }
        }

        public PointLatLng maxCrop
        {
            get
            {
                return _maxCrop;
            }

            set
            {
                _maxCrop = value;
            }
        }

        public List<PointLatLng> uiCrop
        {
            get
            {
                return _uiCrop;
            }

            set
            {
                _uiCrop = value;
            }
        }

        public Shapefile shp
        {
            get
            {
                return _shp;
            }

            set
            {
                _shp = value;
            }

        }

        public string path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }

        public Color color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

    }

    public class RandomColorGenerator
    {
        public static Color RandomColor()
        {

            Random r = new Random(2);
            byte red = (byte)r.Next(0, 255);
            byte green = (byte)r.Next(0, 255);
            byte blue = (byte)r.Next(0, 255);

            Color _opacity = Color.FromArgb(80, red, green, blue);
            Color _solid = Color.FromArgb(100, red, green, blue);

            return Color.FromArgb(80, red, green, blue);
        }
    }


}
