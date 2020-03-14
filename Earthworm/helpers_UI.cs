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
        public static void DisplayForm()
        {
            form_mapBrowser form = new form_mapBrowser();
            Grasshopper.GUI.GH_WindowsFormUtil.CenterFormOnCursor(form, true);
            //form.ShowDialog();
            form.Show();
        }
    }
}
