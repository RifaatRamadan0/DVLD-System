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

namespace DVLDWinForms___Presentation_Layer.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _PersonID = -1;

        public frmShowPersonLicenseHistory()
        {
            InitializeComponent();
        }

        public frmShowPersonLicenseHistory(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {

            if(_PersonID == -1) // When the Person ID is not sent by constructor
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                
            }
            else
            {
                ctrlPersonCardWithFilter1.LoadPerson(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;
            }

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;

            if(_PersonID == -1)
            {
                ctrlDriverLicenses1.Clear();
            }
            else
            {
                // This method is called whether the person ID is sent by constructor or entered manually
                ctrlDriverLicenses1.LoadDriverLicenses(_PersonID);
            }

        }
    }
}
