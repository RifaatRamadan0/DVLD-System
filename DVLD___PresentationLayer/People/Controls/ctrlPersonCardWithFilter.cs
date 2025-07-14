using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        public event Action<int> OnPersonSelected;

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }

        }

        public int PersonID
        {
            get 
            {
                return ctrlPersonCard1.PersonID;
            }
        }
        public clsPerson SelectedPerson
        {
            get
            {
                return ctrlPersonCard1.Person;
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cmbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
        }

        private void FindPerson()
        {
            if (txtFilterValue.Text.Trim() == "")
                return;

            switch (cmbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonCard(int.Parse(txtFilterValue.Text.Trim()));
                    break;
                case "National No.":
                    ctrlPersonCard1.LoadPersonCard(txtFilterValue.Text.Trim());
                    break;
            }


            if(OnPersonSelected != null)
                OnPersonSelected(ctrlPersonCard1.PersonID); // The person ID could be here -1 or different number

        }
        public void LoadPerson(int PersonID)
        {
            cmbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            FindPerson();
        }      
        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            FindPerson();
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            ctrlPersonCard1.LoadPersonCard(PersonID);
            cmbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += DataBackEvent;
            frm.ShowDialog();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Check if Enter pressed, 13 is the ASCII code of Enter
            if(e.KeyChar == (char)13)
            {
                e.Handled = true;
                btnFindPerson.PerformClick();
                return;
            }

            if(cmbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar);       
            }
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
            {
              //  e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

    }
}
