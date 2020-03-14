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
        private double _centreLat;
        private double _centreLng;

        public CropProperties(double minLat, double minLng, double maxLat, double maxLng, double centreLat, double centreLng)
        {
            _minLat = minLat;
            _minLng = minLng;
            _maxLat = maxLat;
            _maxLng = maxLng;
            _centreLat = centreLat;
            _centreLng = centreLng;
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

        public double centreLat
        {
            get
            {
                return _centreLat;
            }

            set
            {
                _centreLat = value;
            }
        }

        public double centreLng
        {
            get
            {
                return _centreLng;
            }

            set
            {
                _centreLng = value;
            }
        }

    }



}
