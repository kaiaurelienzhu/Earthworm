using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Earthworm
{
    class helpers_UI
    {
        public static void DisplayForm(double _minLng, double _minLat, double _maxLng, double _maxLat)
        {
            // Define inputs for properties
            CropProperties properties = new CropProperties();
            properties.minLat = _minLat;
            properties.minLng = _minLng;
            properties.maxLat = _maxLat;
            properties.maxLng = _maxLng;

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
        private double _minLat = 0;
        private double _minLng = 0;
        private double _maxLat = 0;
        private double _maxLng = 0;

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


    }



}
