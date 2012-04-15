using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace pserv3.Services
{
    public class DevicesController : ServicesController
    {
        public DevicesController(ListView listView)
            :   base(listView)
        {
            ServicesType = SC_SERVICE_TYPE.SERVICE_DRIVER;
        }

        public override string GetCaption()
        {
            return string.Format("Devices on \\\\{0} (this machine)", Environment.MachineName);
        }
    }
}
