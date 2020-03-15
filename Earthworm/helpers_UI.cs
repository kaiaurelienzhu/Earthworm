using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            //form.Show();
        }
    }

    // Create properties which must be met for each shapefile
    public class CropProperties
    {
        private double _minLat;
        private double _minLng;
        private double _maxLat;
        private double _maxLng;
        private double _minCropLat;
        private double _minCropLng;
        private double _maxCropLat;
        private double _maxCropLng;
        public List<PointLatLng> _uiCrop;


        public CropProperties(double minLat, double minLng, double maxLat, double maxLng, double minCropLat, double minCropLng, double maxCropLat, double maxCropLng, List<PointLatLng> uiCrop)
        {
            _minLat = minLat;
            _minLng = minLng;
            _maxLat = maxLat;
            _maxLng = maxLng;
            _minCropLat = minCropLat;
            _minCropLng = minCropLng;
            _maxCropLat = maxCropLat;
            _maxCropLng = maxCropLng;
            _uiCrop = uiCrop;
        }


        public double minLat
        {
            get
            {
                return _minLat;
            }

            set
            {
                _minLat = value;
            }
        }

        public double minLng
        {
            get
            {
                return _minLng;
            }

            set
            {
                _minLng = value;
            }
        }

        public double maxLat
        {
            get
            {
                return _maxLat;
            }

            set
            {
                _maxLat = value;
            }
        }

        public double maxLng
        {
            get
            {
                return _maxLng;
            }

            set
            {
                _maxLng = value;
            }
        }

        public double minCropLat
        {
            get
            {
                return _minCropLat;
            }

            set
            {
                _minCropLat = value;
            }
        }

        public double minCropLng
        {
            get
            {
                return _minCropLng;
            }

            set
            {
                _minCropLng = value;
            }
        }

        public double maxCropLat
        {
            get
            {
                return _maxCropLat;
            }

            set
            {
                _maxCropLat = value;
            }
        }

        public double maxCropLng
        {
            get
            {
                return _maxCropLng;
            }

            set
            {
                _maxCropLng = value;
            }
        }

    }



}
