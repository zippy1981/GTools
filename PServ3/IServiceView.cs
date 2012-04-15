using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3
{
    public interface IServiceView
    {
        /*
         * /// <summary>
        /// ask display to update a single item. It is assumed that the service controller
        /// has refreshed its internal status earlier
        /// </summary>
        /// <param name="o"></param>
        void Update(IServiceObject o);
        */
        void UpdateDisplay();

        /// <summary>
        /// Ask display to refresh all items
        /// </summary>
        void RefreshDisplay();
    }
}
