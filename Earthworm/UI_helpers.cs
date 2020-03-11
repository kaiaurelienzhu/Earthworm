using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Earthworm
{
    class UI_helpers
    {
        public static void DisplayForm()
        {
            mapBrowser form = new mapBrowser();
            Grasshopper.GUI.GH_WindowsFormUtil.CenterFormOnCursor(form, true);
            form.ShowDialog();
        }
    }
}
