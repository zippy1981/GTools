using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    /// <summary>
    /// An IServiceController is used to control access to a collection of IServiceObjects
    /// </summary>
    public abstract class IServiceController
    {
        protected readonly List<IServiceColumn> Columns = new List<IServiceColumn>();
        protected readonly ListView Control;
        protected IServiceView ServiceView;

        public IServiceController(ListView listView)
        {
            Control = listView;
        }

        /// <summary>
        /// Ask the underlying data source to refresh its content
        /// </summary>
        public abstract void Refresh();

        public virtual void CancelRefresh()
        {
            // default: cancel is not possible
        }

        public virtual void RefreshUsingLongTaskDialog(LongTaskDialog dialog)
        {
            // default: not implemented
        }

        /// <summary>
        /// Get the items this container currently holds
        /// </summary>
        /// <returns>A list of ServiceObject implementations</returns>
        public abstract IEnumerable<IServiceObject> GetObjects();
        
        public virtual List<IServiceColumn> GetColumns()
        {
            return Columns;
        }

        public abstract ContextMenu CreateContextMenu();

        public virtual void SetView(IServiceView view)
        {
            ServiceView = view;
        }

        /// <summary>
        /// Context Menu / Item Handling
        /// </summary>
        public virtual void OnContextRestart()
        {
            // default: do nothing
        }

        public virtual void OnContextStart()
        {
            // default: do nothing
        }

        public virtual void OnContextStop()
        {
            // default: do nothing
        }

        public virtual void OnContextPause()
        {
            // default: do nothing
        }

        public virtual void OnContextContinue()
        {
            // default: do nothing
        }


        /// <summary>
        /// Call this function to show the properties of the currently selected items
        /// </summary>
        public virtual void OnShowProperties()
        {
            // default: not implemented
        }

        public virtual bool IsContextRestartEnabled()
        {
            // default: not supported
            return false;
        }

        public virtual bool IsContextStartEnabled()
        {
            // default: not supported
            return false;
        }

        public virtual bool IsContextStopEnabled()
        {
            // default: not supported
            return false;
        }

        public virtual bool IsContextPauseEnabled()
        {
            // default: not supported
            return false;
        }

        public virtual bool IsContextContinueEnabled()
        {
            // default: not supported
            return false;
        }

        public abstract void SaveAsXML(string filename);        
        
        /// <summary>
        /// Call this function to apply a template XML to the current situation
        /// </summary>
        /// <param name="filename"></param>
        public virtual void ApplyTemplate(string filename)
        {
            // default: templates are not supported
        }

        public abstract string CreatePrintDocument();
        public abstract string GetCaption();        

        public virtual bool CanConnectToRemoteMachine()
        {
            return false;
        }

        /// <summary>
        /// Check if the items returned by this controller are cached. If so, then two assumptions will be made:
        /// a) 
        /// </summary>
        /// <returns>true if items are cached, false if they are not cached</returns>
        public virtual bool ItemDataIsCached()
        {
            return false;
        }
    }
}
