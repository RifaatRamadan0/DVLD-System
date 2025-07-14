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

namespace DVLDWinForms___Presentation_Layer.Users
{
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID = -1;
        private clsUser _User;

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void _FillUserCard()
        {
            ctrlPersonCard1.LoadPersonCard(_User.PersonID);

            lblUserID.Text = _UserID.ToString();
            lblUsername.Text = _User.UserName;
            lblIsActive.Text = (_User.IsActive ? "Yes" : "No");
        }
        private void _ResetUserCard()
        {
            ctrlPersonCard1.ResetPersonCard();

            lblUserID.Text = "???";
            lblUsername.Text = "???";
            lblIsActive.Text = "???";
        }
        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                _ResetUserCard();
                MessageBox.Show("User With ID = " + _UserID + " Does not Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillUserCard();
        }
        
    }
}
