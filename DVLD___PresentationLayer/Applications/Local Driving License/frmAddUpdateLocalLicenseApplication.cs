using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
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
    public partial class frmAddUpdateLocalLicenseApplication : Form
    {
        enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _LocalLicenseApplicationID = -1;
        private clsLocalLicenseApplication _LocalLicenseApplication;
        private int _SelectedPeronID = -1;

        public frmAddUpdateLocalLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalLicenseApplication(int LocalLicenseApplicationID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _LocalLicenseApplicationID = LocalLicenseApplicationID;
        }


        private void _FillLicenseClassesInComboBox()
        {
            DataTable LicenseClasses = clsLicenseClass.GetAllLicenseClasses();
            foreach (DataRow Class in LicenseClasses.Rows)
            {
                cmbLicenseClass.Items.Add(Class["ClassName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();
            
            if(_Mode == enMode.AddNew)
            {
                _LocalLicenseApplication = new clsLocalLicenseApplication();
                lblTitle.Text = "Add New Local Driving License Application";
                this.Text = "Add New Local Driving License Application";
                ctrlPersonCardWithFilter1.FilterFocus();
                tbApplicationInfo.Enabled = false;
                btnSave.Enabled = false;

                lblLocalLicenseApplicationID.Text = "[???]";
                lblApplicationDate.Text = DateTime.Now.ToString("dd-MMMM-yy");
                cmbLicenseClass.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalLicense).Fees.ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                tbApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
                ctrlPersonCardWithFilter1.FilterEnabled = false;
            }
        }
        private void LoadApplicationData()
        {
            _LocalLicenseApplication = clsLocalLicenseApplication.Find(_LocalLicenseApplicationID);

            if(_LocalLicenseApplication == null)
            {
                MessageBox.Show($"This Local Driving License Application With Id = {_LocalLicenseApplication.LocalLicenseApplicationID}" +
                    $" Does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPerson(_LocalLicenseApplication.ApplicantPersonID);

            lblLocalLicenseApplicationID.Text = _LocalLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LocalLicenseApplication.ApplicationDate.ToString("dd-MMMM-yy");
            cmbLicenseClass.SelectedIndex = cmbLicenseClass.FindString(_LocalLicenseApplication.LicenseClassInfo.ClassName);
            lblApplicationFees.Text = _LocalLicenseApplication.ApplicationTypeInfo.Fees.ToString();
            lblCreatedBy.Text = _LocalLicenseApplication.CreatedByUserInfo.UserName;
        }
        private void frmAddUpdateLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
            {
                LoadApplicationData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(ctrlPersonCardWithFilter1.SelectedPerson == null)
            {
                MessageBox.Show("Please Select a Person", "Select Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            tbApplicationInfo.Enabled = true;
            btnSave.Enabled = true;
            tbPages.SelectedTab = tbApplicationInfo;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            int LicenseClassID = clsLicenseClass.Find(cmbLicenseClass.Text).LicenseClassID;
            int ApplicantPersonID = _SelectedPeronID;
            int ActiveApplicationID = clsLocalLicenseApplication.GetActiveApplicationIDForLicenseClass(ApplicantPersonID, 
                clsApplication.enApplicationType.NewLocalLicense, LicenseClassID);

            bool isSameClassDuringUpdate = (_Mode == enMode.Update && _LocalLicenseApplication.LicenseClassID == LicenseClassID);
            if (ActiveApplicationID != -1 && !isSameClassDuringUpdate)
            {
                MessageBox.Show("This Person Has already an active application for this License Class, choose another license class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;    
            }

            if(clsLicense.IsLicenseExistByPersonID(ApplicantPersonID, LicenseClassID))
            {
                MessageBox.Show("This Person Has already issued a license with this License Class, choose another license class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check for the age of person if it meets the minimum required age
            DateTime PersonAge = (clsPerson.Find(ApplicantPersonID).DateOfBirth);
            byte MinimumAllowedAge = clsLicenseClass.Find(LicenseClassID).MinimumAllowedAge;

            DateTime MinimumAllowedBirthDate = DateTime.Now.AddYears(-MinimumAllowedAge);
            if (DateTime.Compare(PersonAge, MinimumAllowedBirthDate) > 0 )
            {
                MessageBox.Show("The Age of the person doesn't meet the minimum requirement of " + cmbLicenseClass.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalLicenseApplication.LicenseClassID = LicenseClassID;
            _LocalLicenseApplication.ApplicantPersonID = ApplicantPersonID;
            _LocalLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalLicenseApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.NewLocalLicense;
            _LocalLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalLicenseApplication.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalLicense).Fees;
            _LocalLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            if(_LocalLicenseApplication.Save())
            {
                lblLocalLicenseApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                this.Text = "Update Local Driving License Application";
                lblTitle.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPeronID = obj;
        }

        private void frmAddUpdateLocalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
