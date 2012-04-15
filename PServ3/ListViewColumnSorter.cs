using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace pserv3
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer<IServiceObject>
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;

        private IServiceColumn SortColumn;

        private readonly MainForm MainForm;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter(MainForm mainForm)
        {
            MainForm = mainForm;

            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.Ascending;
            MainForm.listView1.ColumnClick += OnColumnClick;
        }

        public void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e != null)
            {
                // Determine if clicked column is already the column that is being sorted.
                if (e.Column == ColumnToSort)
                {
                    // Reverse the current sort direction for this column.
                    if (OrderOfSort == SortOrder.Ascending)
                    {
                        OrderOfSort = SortOrder.Descending;
                    }
                    else
                    {
                        OrderOfSort = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    ColumnToSort = e.Column;
                    OrderOfSort = SortOrder.Ascending;
                }
            }

            SortColumn = MainForm.CurrentController.GetColumns()[ColumnToSort];

            MainForm.CurrentObjects.Sort(this);
            MainForm.UpdateDisplay();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(IServiceObject soX, IServiceObject soY)
        {
            int compareResult = 0;

            // Compare the two items
            if( SortColumn != null )
                compareResult = SortColumn.Compare(soX, soY);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

    }
}
