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

namespace DVLDWinForms___Presentation_Layer.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private int _ApplicationID = -1;
        private clsApplication _Application;

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationBasicInfo(int ApplicationID)
        {
            _ApplicationID = ApplicationID;
            _Application = clsApplication.Find(ApplicationID);

            if(_Application == null)
            {
                MessageBox.Show("Application with ID = [" + ApplicationID + "] Does not exist", "Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblApplicationStatus.Text = _Application.StatusText;
            lblApplicationFees.Text = _Application.PaidFees.ToString();
            lblApplicationType.Text = _Application.ApplicationTypeInfo.Title;
            lblApplicantFullName.Text = _Application.ApplicantInfo.FullName;
            lblApplicationDate.Text = _Application.ApplicationDate.ToString("dd-MMM-yyyy");
            lblApplicationStatusDate.Text = _Application.LastStatusDate.ToString("dd-MMM-yyyy");
            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
        }

        private void linkViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_Application.ApplicantPersonID);
            frm.ShowDialog();
            LoadApplicationBasicInfo(_ApplicationID);
        }

    }
}
