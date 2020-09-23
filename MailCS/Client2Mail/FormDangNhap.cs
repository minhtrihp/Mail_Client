using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Limilabs.Mail;//Gọi lớp thư viên hỗ trợ qua namespace của nó
using Limilabs.Client.IMAP;

namespace Client2Mail
{
    public partial class FormDangNhap : Form
    {
        private Imap imap; // Đối tượng dùng để kết nối với servermail của gmail

        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            String email = txt_Email.Text; //khai báo biến email kiểu chuỗi, nhận giá trị từ dữ liệu người dùng nhập vào textbox
            String password = txt_Password.Text; //khai báo biến password kiểu chuỗi, nhận giá trị từ dữ liệu người dùng nhập vào textbox

            imap = new Imap(); //tại mới 1 đối tượng Imap
            imap.ConnectSSL("imap.gmail.com", 993);// điền thông tin host của gmail, kết nối qua cổng 993
            try
            {
                imap.Login(txt_Email.Text, txt_Password.Text);//Đăng nhập vào tài khoản gmail
                MessageBox.Show("Login Success To Your GMail !", "Congratulation !"); //Khi đăng nhập thành công sẽ có thông báo chúc mừng
                this.Hide(); //gọi phương thức Hide() để ẩn form đăng nhập
                FormListMail sendMailForm = new FormListMail(); //tạo mới 1 đối tượng FormListMail
                sendMailForm.Show(); //hiện form ListMail
            }
            catch
            {
                if (email.Trim().Equals("")) //bắt lỗi để trống textbox email
                {
                    MessageBox.Show("Please Enter Your Email To Login !", "Empty Email !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (password.Trim().Equals("")) //bắt lỗi để trống textbox password
                {
                    MessageBox.Show("Please Enter Your Password To Login !", "Empty Password !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Wrong Email Or Password !", "Wrong Data !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //xuất thông báo khi email hoặc password nhập vào không đúng
                }
            }
        }

        private void lbGoToRegister_MouseEnter(object sender, EventArgs e)
        {
            lbGoToRegister.ForeColor = Color.Red;
            //đưa trỏ chuột vào dòng label Register thì sẽ hiện lên màu đỏ
        }

        private void lbGoToRegister_MouseLeave(object sender, EventArgs e)
        {
            lbGoToRegister.ForeColor = Color.Black;
            //đưa trỏ chuột rời khỏi dòng label Register thì sẽ thành màu đen
        }

        //Xử lí đóng form
        private void FormDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Do You Want To Close Mail Exchange ?", "Announcement !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            //khi đóng form đăng nhập sẽ xuất hiện hộp thoại hỏi có thực sự muốn đóng hay không
        }

        private void FormDangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        } //tắt ứng dụng khi bấm nút X

        private void chkPassShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPassShow.Checked)
            {
                if (txt_Password.Text != "")
                {
                    txt_Password.UseSystemPasswordChar = false;
                }
            } //khi checkbox Password được check thì password sẽ hiện rõ
            else
            {
                if (txt_Password.Text != "")
                {
                    txt_Password.UseSystemPasswordChar = true;
                }
                else
                {
                    txt_Password.UseSystemPasswordChar = false;
                }
            } //khi checkbox Password không được check thì password sẽ thể hiện bằng kí tự che giấu của hệ thống
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbGoToRegister_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://accounts.google.com/");
        }
    }
}
