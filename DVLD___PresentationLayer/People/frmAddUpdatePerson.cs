using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DVLDWinForms___Presentation_Layer.Global_Classes;

namespace DVLDWinForms___Presentation_Layer
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        private int _PersonID = -1;
        private clsPerson _Person;
        public enum enMode { AddNew = 0, Update = 1};
        public enum enGender { Male = 0, Female = 1};
        enMode _Mode;

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            _Mode = enMode.Update;
        }
        public frmAddUpdatePerson()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }
        private void _ResetPersonValues()
        {
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblAddUpdatePerson.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblAddUpdatePerson.Text = "Update Person";
            }

            rbMale.Checked = true;
            rbFemale.Checked = false;
            if (rbMale.Checked)
                pbImage.Image = Resources.Male_512;
            else
                pbImage.Image = Resources.Female_512;
                
            linkRemoveImg.Visible = (pbImage.ImageLocation != null);

            DateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            DateOfBirth.Value = DateOfBirth.MaxDate;
            DateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            cmbCountry.SelectedIndex = cmbCountry.FindString("Lebanon");

            lblPersonID.Text = "N/A";
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";

            txtFirstName.Focus();
        }
        private void _FillCountriesInComboBox()
        {
            DataTable Countries = clsCountry.GetAllCountries();

            foreach (DataRow Row in Countries.Rows)
            {
                cmbCountry.Items.Add(Row["CountryName"]);
            }
        }
        private void _LoadPersonData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person With ID = " + _PersonID, " Is Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalID;
            DateOfBirth.Text = _Person.DateOfBirth.ToString();

            if ((enGender)_Person.Gender == enGender.Male)
            {
                rbMale.Checked = true;
                pbImage.Image = Resources.Male_512;
            }
            else
            {
                rbFemale.Checked = true;
                pbImage.Image = Resources.Female_512;
            }


            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            cmbCountry.SelectedItem = clsCountry.Find(_Person.CountryID).CountryName;

            if(_Person.ImagePath != "")
            {
                pbImage.ImageLocation = _Person.ImagePath;
            }

            linkRemoveImg.Visible = (pbImage.ImageLocation != null);
            
        }
        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            _ResetPersonValues();

            if (_Mode == enMode.Update)
            {
                _LoadPersonData();
            }
        }

        private void _SavePersonData()
        {
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalID = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.DateOfBirth = DateOfBirth.Value;

            if (rbFemale.Checked)
                _Person.Gender = (int)enGender.Female;
            else
                _Person.Gender = (int)enGender.Male;

            _Person.CountryID = clsCountry.Find(cmbCountry.Text).CountryID;

            _Person.ImagePath = pbImage.ImageLocation;

        }
        private bool _AddPersonImage()
        {
            string SourceImagePath = pbImage.ImageLocation;

            if(clsImageHelper.CopyImageToImagesFolder(ref SourceImagePath))
            {
                pbImage.ImageLocation = SourceImagePath;
                return true;
            }

            MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private bool _HandlePersonImage()
        {

            if(_Person.ImagePath != pbImage.ImageLocation)
            {
                if(_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(IOException ex)
                    {

                    }

                }

                if(pbImage.ImageLocation != null)
                {
                    return _AddPersonImage();
                }
            }

            return true;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
            {
                return;
            }

            _SavePersonData();

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.Update;
                lblAddUpdatePerson.Text = "Update Person";
                _PersonID = _Person.PersonID;
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(_Person, _PersonID);
            }
            else
            {
                MessageBox.Show("Data is not Saved", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(pbImage.ImageLocation == null)
                pbImage.Image = Resources.Male_512;
        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbImage.ImageLocation == null)
                pbImage.Image = Resources.Female_512;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "This Field is required");
            }
            else
            {
                errorProvider1.SetError(textBox, null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This Field is required");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (clsPerson.IsPersonExist(txtNationalNo.Text.Trim()) && _Person.NationalID != txtNationalNo.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This NationalNo is used by other person");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                errorProvider1.SetError(txtEmail, null);
                return;
            }

            if (!clsValidation.ValidateEmail(txtEmail.Text.Trim()))
                    errorProvider1.SetError(txtEmail, "Incorrect Email");
            else
                errorProvider1.SetError(txtEmail, null);
        }

        private void linkSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files |*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbImage.ImageLocation = openFileDialog1.FileName;
                linkRemoveImg.Visible = true;
            }
        }

        private void linkRemoveImg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbImage.ImageLocation = null;

            if (rbFemale.Checked)
            {
                pbImage.Image = Resources.Female_512;
            }
            else
            {
                pbImage.Image = Resources.Male_512;
            }

            linkRemoveImg.Visible = false;
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
