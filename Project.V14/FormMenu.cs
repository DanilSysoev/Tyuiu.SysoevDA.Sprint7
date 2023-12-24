using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComponentFactory.Krypton.Toolkit;

namespace Project.V14
{
    public partial class FormMenu : KryptonForm
    {
        public FormMenu()
        {
            InitializeComponent();
            openFileDialog_SDA.Filter = "Значения, разделенные запятыми(*.csv)|*.csv|Все файлы(*.*)|*.*";
        }

        private void ButtonAbout_SDA_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog();
        }

        private void ButtonRules_SDA_Click(object sender, EventArgs e)
        {
            FormRules formRules = new FormRules();
            formRules.ShowDialog();
        }

        private void ButtonOpenFile_SDA_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                FormFunc formFunc = new FormFunc();
                formFunc.filePath = filePath;
                this.Hide();
                formFunc.ShowDialog();
                this.Show();
            }
        }
    }
}
