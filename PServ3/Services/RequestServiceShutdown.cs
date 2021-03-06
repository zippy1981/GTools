﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3.Services
{
    class RequestServiceShutdown : ServiceStateRequest
    {
        #region ServiceStateRequest Members

        public ACCESS_MASK GetServiceAccessMask()
        {
            return ACCESS_MASK.SERVICE_STOP;
        }

        public bool Request(ServiceStatus ss)
        {
            return ss.Control(SC_CONTROL_CODE.SERVICE_CONTROL_STOP);
        }

        public bool HasSuccess(SC_RUNTIME_STATUS state)
        {
            return state == SC_RUNTIME_STATUS.SERVICE_STOPPED;
        }

        public bool HasFailed(SC_RUNTIME_STATUS state)
        {
            return false;
        }

        #endregion
    }
}
