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

namespace DVLDWinForms___Presentation_Layer.Tests
{
    public partial class frmScheduleTest : Form
    {

        private int _LocalLicenseApplicationID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private int _TestAppointmentID = -1;

        public frmScheduleTest(int LocalLicenseApplicationID, clsTestType.enTestType TestType, int TestAppointmentID = -1)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
            _TestTypeID = TestType;
            _TestAppointmentID = TestAppointmentID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {

            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.OnSave += btnClose_Click;
            ctrlScheduleTest1.LoadScheduleTestInfo(_LocalLicenseApplicationID, _TestAppointmentID);
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
