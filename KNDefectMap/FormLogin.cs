using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KNDefectMap.Properties;

namespace KNDefectMap
{
    public partial class FLogin : Form
    {
        public string EmployeeName = "";

        public FLogin()
        {
            InitializeComponent();
        }

        private void textBoxEmployeeNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EmployeeName = DBAccessor.getUserName(textBoxEmployeeNum.Text);
                if (EmployeeName == "" && textBoxEmployeeNum.Text.Length >= 7)
                {
                    string id = textBoxEmployeeNum.Text.Substring(1, 6);
                    EmployeeName = DBAccessor.getUserName(id);
                }
                textBoxEmployeeNum.Text = "";
                if (EmployeeName == "")
                    MessageBox.Show("查無此薪號", "系統訊息");
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }






    }
}
