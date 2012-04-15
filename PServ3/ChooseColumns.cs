using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pserv3
{
    public partial class ChooseColumns : Form
    {
        public readonly List<IServiceColumn> VisibleColumns;
        public readonly List<IServiceColumn> HiddenColumns;

        private class TempColumn : IServiceColumn
        {
            public readonly IServiceColumn Wrapper;

            public override string ToString()
            {
                return Wrapper.GetName();
            }

            public TempColumn(IServiceColumn sc)
            {
                Wrapper = sc;
            }

            public HorizontalAlignment GetTextAlign()
            {
                return Wrapper.GetTextAlign();
            }
            
            public string GetName()
            {
                return Wrapper.GetName();
            }
            public int GetID()
            {
                return Wrapper.GetID();
            }
            public int Compare(IServiceObject a, IServiceObject b)
            {
                return Wrapper.Compare(a, b);
            }
        }

        private readonly List<TempColumn> TempVisibleColumns;
        private readonly List<TempColumn> TempHiddenColumns;

        // create list of all columns, to be modified only if the user does press on OK
        public ChooseColumns(List<IServiceColumn> visible, List<IServiceColumn> hidden)
        {
            InitializeComponent();

            VisibleColumns = visible;
            TempVisibleColumns = CopyList(visible, lbVisibleColumns);
            HiddenColumns = hidden;
            TempHiddenColumns = CopyList(hidden, lbAvailableColumns);
        }

        private List<TempColumn> CopyList(List<IServiceColumn> source, ListBox control)
        {
            List<TempColumn> result = new List<TempColumn>();
            foreach (IServiceColumn vc in source)
            {
                result.Add(new TempColumn(vc));
                control.Items.Add(vc.GetName());
            }
            return result;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            HiddenColumns.Clear();
            VisibleColumns.Clear();

            foreach(TempColumn tc in TempVisibleColumns )
            {
                VisibleColumns.Add(tc.Wrapper);
            }

            foreach(TempColumn tc in TempHiddenColumns )
            {
                HiddenColumns.Add(tc.Wrapper);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btShow_Click(object sender, EventArgs e)
        {
            MoveItems(lbAvailableColumns, TempHiddenColumns, lbVisibleColumns, TempVisibleColumns);
        }

        private void btHide_Click(object sender, EventArgs e)
        {
            MoveItems(lbVisibleColumns, TempVisibleColumns, lbAvailableColumns, TempHiddenColumns);
        }

        private List<int> GetSelectedIndices(ListBox control)
        {
            List<int> SelectedIndices = new List<int>();
            foreach (int index in control.SelectedIndices)
                SelectedIndices.Add(index);
            return SelectedIndices;
        }

        private void MoveItems(
                ListBox lbSource, List<TempColumn> lSource,
                ListBox lbTarget, List<TempColumn> lTarget)
        {
            if (lbSource.SelectedIndices.Count > 0)
            {
                List<int> SelectedIndices = GetSelectedIndices(lbSource);

                SelectedIndices.Sort();
                SelectedIndices.Reverse();

                int insertPos = lbTarget.Items.Count;
                lbTarget.SelectedIndices.Clear();
                foreach(int index in SelectedIndices)
                {
                    lbSource.Items.RemoveAt(index);
                    TempColumn tc = lSource[index];
                    lTarget.Insert(insertPos, tc);
                    lSource.RemoveAt(index);
                    lbTarget.Items.Insert(insertPos, tc.GetName());
                    lbTarget.SelectedIndices.Add(insertPos);
                }
            }
        }

        private void btTop_Click(object sender, EventArgs e)
        {
            if (lbVisibleColumns.SelectedIndices.Count > 0)
            {
                List<TempColumn> temp = RemoveSelectedVisibles();
                for (int index = 0; index < temp.Count; ++index)
                {
                    TempColumn tc = temp[index];
                    lbVisibleColumns.Items.Insert(index, tc.GetName());
                    TempVisibleColumns.Insert(index, tc);
                    lbVisibleColumns.SelectedIndices.Add(index);
                }
                temp.Clear();
            }
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            List<int> SelectedIndices = GetSelectedIndices(lbVisibleColumns);

            SelectedIndices.Sort();

            List<int> NewSelection = new List<int>();
            foreach (int index in SelectedIndices)
            {
                int targetIndex = index - 1;
                if (targetIndex < 0)
                    return;

                lbVisibleColumns.Items.RemoveAt(index);
                TempColumn tc = TempVisibleColumns[index];
                TempVisibleColumns.RemoveAt(index);
                TempVisibleColumns.Insert(targetIndex, tc);
                lbVisibleColumns.Items.Insert(targetIndex, tc.GetName());
                NewSelection.Add(targetIndex);
            }
            lbVisibleColumns.SelectedIndices.Clear();
            foreach (int index in NewSelection)
                lbVisibleColumns.SelectedIndices.Add(index);
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            List<int> SelectedIndices = GetSelectedIndices(lbVisibleColumns);

            SelectedIndices.Sort();
            SelectedIndices.Reverse();

            List<int> NewSelection = new List<int>();
            foreach (int index in SelectedIndices)
            {
                int targetIndex = index + 1;
                if (targetIndex == lbVisibleColumns.Items.Count)
                    return;

                lbVisibleColumns.Items.RemoveAt(index);
                TempColumn tc = TempVisibleColumns[index];                    
                TempVisibleColumns.RemoveAt(index);
                TempVisibleColumns.Insert(targetIndex, tc);
                lbVisibleColumns.Items.Insert(targetIndex, tc.GetName());
                NewSelection.Add(targetIndex);
            }
            lbVisibleColumns.SelectedIndices.Clear();
            foreach (int index in NewSelection)
                lbVisibleColumns.SelectedIndices.Add(index);
        }

        private List<TempColumn> RemoveSelectedVisibles()
        {
            List<int> SelectedIndices = GetSelectedIndices(lbVisibleColumns);

            SelectedIndices.Sort();
            SelectedIndices.Reverse();

            List<TempColumn> temp = new List<TempColumn>();
            foreach (int index in SelectedIndices)
            {
                lbVisibleColumns.Items.RemoveAt(index);
                TempColumn tc = TempVisibleColumns[index];
                temp.Insert(0, tc);
                TempVisibleColumns.RemoveAt(index);
            }

            lbVisibleColumns.SelectedIndices.Clear();
            return temp;
        }

        private void btBottom_Click(object sender, EventArgs e)
        {
            if (lbVisibleColumns.SelectedIndices.Count > 0)
            {
                List<TempColumn> temp = RemoveSelectedVisibles();

                int insertPos = lbVisibleColumns.Items.Count;
                for (int index = 0; index < temp.Count; ++index)
                {
                    TempColumn tc = temp[index];
                    lbVisibleColumns.Items.Insert(insertPos, tc.GetName());
                    TempVisibleColumns.Insert(insertPos, tc);
                    lbVisibleColumns.SelectedIndices.Add(insertPos);
                    ++insertPos;
                }
                temp.Clear();
            }
        }
    }
}
