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

namespace DVLDWinForms___Presentation_Layer.Applications.Local_Driving_License
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int _LocalLicenseApplicationID = -1;
        private clsLocalLicenseApplication _LocalLicenseApplication;

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadDrivingLicenseApplicationInfo(int LocalLicenseApplicationID)
        {
            _LocalLicenseApplicationID = LocalLicenseApplicationID;
            _LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);

            if(_LocalLicenseApplication == null)
            {
                MessageBox.Show("Local License Application with ID = [" + LocalLicenseApplicationID + "] Does not exist", "Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalLicenseApplicationID.Text = LocalLicenseApplicationID.ToString();
            lblLicenseClass.Text = _LocalLicenseApplication.LicenseClassInfo.ClassName;
            lblPassedTests.Text = _LocalLicenseApplication.GetPassedTestCount() + "/" + clsTestType.GetAllTestTypes().Rows.Count;

            ctrlApplicationBasicInfo1.LoadApplicationBasicInfo(_LocalLicenseApplication.ApplicationID);
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
                
        }
    }
}
