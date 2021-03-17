using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogBookTask
{
    public partial class CommentForm : Form
    {
        public string Comment { get; set; }
        public CommentForm()
        {
            InitializeComponent();
        }

        private void CommentForm_Load(object sender, EventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CommentRchTxtBx.Text))
                return;

            Comment = CommentRchTxtBx.Text;

            this.DialogResult = DialogResult.OK;
        }
    }
}
