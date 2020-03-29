using System;
using System.Collections.Generic;
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
        public Shapefile _shp;


        public CropProperties(PointLatLng minExtent, PointLatLng maxExtent, PointLatLng minCrop, PointLatLng maxCrop, List<PointLatLng> uiCrop, Shapefile shp)
        {
            _minExtent = minExtent;
            _maxExtent = maxExtent;
            _minCrop = minCrop;
            _maxCrop = maxCrop;
            _uiCrop = uiCrop;
            _shp = shp;
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


        }

    }

    }
