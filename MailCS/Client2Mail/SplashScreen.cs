using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client2Mail
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        int n = 1;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(n==8)
            {
                timer1.Stop();
                this.Hide();
                new FormDangNhap().Show();
            }
            n++;
        }
    }
}
