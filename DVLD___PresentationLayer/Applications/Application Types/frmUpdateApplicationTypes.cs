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

namespace DVLDWinForms___Presentation_Layer.ApplicationTypes
{
    public partial class frmUpdateApplicationTypes : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;
        public frmUpdateApplicationTypes(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }

        private void _FillApplicationTypeInfo()
        {
            lblID.Text = _ApplicationType.ID.ToString();
            txtTitle.Text = _ApplicationType.Title;
            txtFees.Text = _ApplicationType.Fees.ToString();
        }
        private void frmUpdateApplicationTypes_Load(object sender, EventArgs e)
        {
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if(_ApplicationType == null)
            {
                MessageBox.Show("No Application Type with ID = [" + _ApplicationTypeID + "] exists");
                this.Close();
                return;
            }

            _FillApplicationTypeInfo();

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if(String.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "This Field Can not be empty");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            // Validate non int values and negative numbers
            if(!float.TryParse(txtFees.Text.Trim(), out float Fees) || Fees < 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
                return;
            }

            errorProvider1.SetError(txtFees, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _SaveApplicationType()
        {
            _ApplicationType.Title = txtTitle.Text.Trim();
            _ApplicationType.Fees = float.Parse(txtFees.Text.Trim());

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Data is Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _SaveApplicationType();

        }

    }
}
