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

namespace DVLDWinForms___Presentation_Layer
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        public int PersonID
        {
            get
            {
                return _PersonID;
            }
        }

        private clsPerson _Person;
        public clsPerson Person
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void ResetPersonCard()
        {
            linkEditPersonInfo.Enabled = false;
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblPersonName.Text = "[????]";
            pbGender.Image = Resources.Man_32;
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
        }
        private void _LoadPersonImage()
        {
            if (_Person.Gender == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                else
                    MessageBox.Show("Image: " + ImagePath + " Not Found", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void _FillPersonCard()
        {
            linkEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblPersonName.Text = _Person.FirstName + " " + _Person.SecondName + " "
                + _Person.ThirdName + " " + _Person.LastName;
            lblNationalNo.Text = _Person.NationalID;
            if(_Person.Gender == 0)
            {
                lblGender.Text = "Male";
                pbGender.Image = Resources.Man_32;
            }
            else
            {
                lblGender.Text = "Female";
                pbGender.Image = Resources.Woman_32;
            }
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountry.Find(_Person.CountryID).CountryName;

            _LoadPersonImage();
        }
        public void LoadPersonCard(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person == null)
            {
                ResetPersonCard();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonCard();
        }
        public void LoadPersonCard(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if(_Person == null)
            {
                ResetPersonCard();
                MessageBox.Show("No Person with NationalNo = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonCard();
        }


        private void linkEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson UpdatePerson = new frmAddUpdatePerson(_PersonID);
            UpdatePerson.ShowDialog();

            LoadPersonCard(_PersonID);
        }
    }
}
