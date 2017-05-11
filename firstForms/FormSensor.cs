using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace firstForms
{
    public partial class FormSensor : Form
    {
        public Point ShiftWindow;
        private void FormSensor_Paint(object sender, PaintEventArgs e)
        {
            this.Location = new Point(Owner.Location.X + ShiftWindow.X , Owner.Location.Y + ShiftWindow.Y );
            pictureBack.Location = new Point(this.Size.Width / 2 - pictureBack.Size.Width / 2, this.Size.Height/ 2 - pictureBack.Size.Height / 2);
        }

        public FormSensor()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.TransparencyKey = this.BackColor;
        }
        public FormSensor(int x, int y)
        {
            InitializeComponent();
            ShiftWindow = new Point(x, y);
        }
        public void SetShift(int x, int y) {
            ShiftWindow = new Point(x, y);
        }

        private void FormSensor_Activated(object sender, EventArgs e)
        {
            //Owner.Focus();
        }

        private void FormSensor_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBack_Click(object sender, EventArgs e)
        {
            FormSensor_Click(sender, e);
            Owner.Focus();
        }
    }
}
