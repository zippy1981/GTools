using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3.Services
{
    public class ServiceActionTemplate
    {
        public readonly string ID;
        public string Name;
        public SC_START_TYPE StartType;

        public ServiceActionTemplate(string id)
        {
            ID = id;
            StartType = SC_START_TYPE.SERVICE_NO_CHANGE;
            Name = "";
        }
    }
}
