using DVLD___BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Licenses.Local_License.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        public event Action<int> OnLicenseSelected;

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public bool FilterEnabled
        {
            get { return gbFilterLocalLicense.Enabled; }
            set
            {
                gbFilterLocalLicense.Enabled = value;
            }
        }

        private int _LicenseID = -1;
        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }
        
        private clsLicense _License;
        public clsLicense SelectedLicenseInfo
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtLocalLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadLicenseInfo(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            if(OnLicenseSelected != null)
                OnLicenseSelected?.Invoke(_LicenseID);
        }
        private void btnFilterLocalLicense_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LicenseID = int.Parse(txtLocalLicenseID.Text);
            LoadLicenseInfo(_LicenseID);

        }

        private void txtLocalLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar);
            
            if(e.KeyChar == (char)13)
            {
                btnFilterLocalLicense.PerformClick();
            }

        }

        private void txtLocalLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtLocalLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLocalLicenseID, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txtLocalLicenseID, "");
            }
        }
    }
}
 