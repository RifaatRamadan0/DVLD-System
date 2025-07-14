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
    public partial class frmDrivingLicenseApplicationInfo : Form
    {

        private int _LocalLicenseApplicationID = -1;

        public frmDrivingLicenseApplicationInfo(int LocalLicenseApplicationID)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
        }

        private void frmDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrlDivingLicenseApplicationInfo1.LoadDrivingLicenseApplicationInfo(_LocalLicenseApplicationID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
