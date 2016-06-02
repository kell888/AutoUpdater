using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KellAutoUpdater
{
    public partial class DownloadConfirm : Form
    {
        #region The constructor of DownloadConfirm
        public DownloadConfirm(string appName, string remote)
        {
            InitializeComponent();
            lbl_AppName.Text = appName;
            lbl_Remote.Text = remote;
        }
        #endregion

        #region The private method
        private void OnLoad(object sender, EventArgs e)
        {
            this.Activate();
            this.Focus();
        }
        #endregion
    }
}