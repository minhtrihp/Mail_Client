using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace Client2Mail
{
    public partial class FormSendMail : Form
    {
        Attachment attach = null; //khởi tạo 1 file attachment có tên attach và gán giá trị là null
        public FormSendMail()
        {
            InitializeComponent();
        }

        private void FormSendMail_Load(object sender, EventArgs e)
        {

        }


        private void FormSendMail_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        } //đóng ứng dụng khi đóng form sendmail

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Return To Your Mail Box ?", "Announcement !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                FormListMail loginForm = new FormListMail();
                loginForm.Show();
            }   //khi bấm button Quit thì sẽ quay về form List mail
        }
        void guiMail(string from, string to, string subject, string message, Attachment file = null)
        {
            MailMessage m = new MailMessage(from, to, subject, message); 
            //tạo 1 đối tượng MailMessage và truyền vào các tham số "gửi từ đâu, gửi đến đâu, tiêu đề, nội dung" 

            if (attach != null) //nếu file đính kèm đã được thêm thì đưa vào mailmessage
            {
                m.Attachments.Add(attach);
            }
            SmtpClient client = new SmtpClient(txtHost.Text, Convert.ToInt32(txtPort.Text));
            //tạo mới 1 đối tượng SmtpCLient với host và port do mình nhập vào ở textbox
            client.EnableSsl = true; //bật bảo mật SSL cho SmtpClient
            client.Credentials = new NetworkCredential(txtUser.Text, txtPass.Text); //kiểm tra đăng nhập bằng tài khoản mail
            client.Send(m); //gửi mailmessage
            MessageBox.Show("Đã gửi tin nhắn thành công!", "Thành Công", MessageBoxButtons.OK);
            //gửi thông báo khi đã gửi
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            
            attach = null; //gán file đính kèm là rỗng
            try
            {
                FileInfo file = new FileInfo(txbAttachFile.Text); //tạo 1 FileInfo để lưu đường dẫn file
                attach = new Attachment(txbAttachFile.Text); //truyền đường dẫn vào file attach ban đầu rỗng
            }
            catch
            {

            }
            guiMail(txtUser.Text, txtTo.Text, txtSj.Text, txtMess.Text, attach); //khi bấm gửi thì gọi hàm guiMail
        }

        private void btAttach_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog(); //tạo mới 1 dialog để đính kèm file
            if (dialog.ShowDialog() == DialogResult.OK) //khi đã chọn file xong và bấm ok thì đường dẫn file sẽ được thêm vào textbox
            {
                txbAttachFile.Text = dialog.FileName;
            }

        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            txtUser.ResetText();
            txtPass.ResetText();
            txtTo.ResetText();
            txtSj.ResetText();
            txtMess.ResetText();
            chkPassShow.Checked = false;
            txtPort.ResetText();
            txtHost.ResetText();
            txbAttachFile.ResetText();

            //khi button Reset được bấm sẽ làm rỗng các textbox và checkbox
        }

        private void chkPassShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPassShow.Checked)
            {
                if (txtPass.Text != "")
                {
                    txtPass.UseSystemPasswordChar = false;
                }
            } //khi checkbox Password được check thì password sẽ hiện rõ
            else
            {
                if (txtPass.Text != "")
                {
                    txtPass.UseSystemPasswordChar = true;
                }
                else
                {
                    txtPass.UseSystemPasswordChar = false;
                }
            }//khi checkbox Password không được check thì password sẽ thể hiện bằng kí tự che giấu của hệ thống
        }
    }
}
