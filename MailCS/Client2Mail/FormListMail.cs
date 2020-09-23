using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Limilabs.Mail;//Gọi lớp thư viên hỗ trợ qua namespace của nó
using Limilabs.Client.IMAP;

namespace Client2Mail
{
    public partial class FormListMail : Form
    {
        private long idmail;
        private Imap imap; // Đối tượng dùng để kết nối với servermail của gmail
        private IMail imail;// đối tượng dùng để xử lý thông tin mail
        private DataTable table; // tạo bảng chứa thông tin mail

        public FormListMail()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            imap = new Imap();
            imap.ConnectSSL("imap.gmail.com", 993);// điền thông tin host của gmail, kết nối qua cổng 993
            try
            {
                imap.Login(txtTaiKhoan.Text, txtMatKhau.Text);//Đăng nhập vào tk gmail
                //khởi tạo bảng với các thuộc tính
                table = new DataTable(); // tạo bảng với các cột thuộc tính
                table.Columns.Add("IDMail", typeof(string));
                table.Columns.Add("Subject", typeof(string));
                table.Columns.Add("Date", typeof(string));
                table.Columns.Add("From", typeof(string));
                MessageBox.Show("Success"); // xuất ra thông báo đăng nhập thành công

            }
            catch
            {
                MessageBox.Show("Fail"); // xuất ra thông báo đăng nhập thất bại
            }
        }

        private void btnInbox_Click(object sender, EventArgs e) // xử lý sự kiện click inbox
        {
            imap.SelectInbox(); // lấy tất cả thư trong hộp thư
            List<long> uids = imap.SearchFlag(Flag.All); //Lấy danh sách thư mới
            int i = int.Parse(txtSoLuongMail.Text); // gán thuộc tính SoluongMail
            if (i > uids.Count)
            {
                i = uids.Count;
            }
            int j = 0;
            foreach (long uid in uids)// Duyệt  các mail
            {
                if (j < i)
                {
                    try
                    {
                        byte[] eml = imap.GetHeadersByUID(uid);// tạo mảng byte  
                        imail = new MailBuilder().CreateFromEml(eml);//lấy thông tin mail trong imap đưa vào imail để xử lý
                        // tính khoảng cách từ ngày mình muốn đến ngày nhận mail
                        TimeSpan t = dateTimePicker1.Value - imail.Date.Value;
                        TimeSpan t2 = dateTimePicker2.Value - imail.Date.Value;
                        if (t.Days <= 0 && t2.Days >= 0)
                        {
                            DataRow row = table.NewRow();
                            row["IDMail"] = uid.ToString();
                            row["Subject"] = imail.Subject;
                            row["Date"] = imail.Date.Value.ToString("dd/MM/yyyy");
                            row["From"] = imail.From.ToString();
                            
                            table.Rows.Add(row);
                            table.AcceptChanges();// thêm 1 dòng vào table
                        }
                        else
                        {
                            j--;
                        }
                        j++;
                    }
                    catch
                    {
                        j++;
                    }
                }
                else
                    break;
            }
            dataGridView1.DataSource = table;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)// xử lý sự kiện khi click trên data này
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null) // kiểm tra xem mình có lấy giá trị không
            {
                if (e.ColumnIndex == 0) // lấy đúng giá trị đầu tiên của datagrid
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    byte[] eml = imap.GetMessageByUID(Convert.ToInt64(id));// lấy nội dung mail
                    imail = new MailBuilder().CreateFromEml(eml); //lấy thông tin mail trong imap đưa vào imail để xử lý
                    richTextBox1.Text = imail.Text;
                    idmail = long.Parse(id);
                }
                else
                {
                    idmail = 0;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (idmail != 0)
            {
                imap.DeleteMessageByUID(idmail);
                foreach (DataRow row in table.Rows)
                {
                    if (idmail == long.Parse(row["IDMail"].ToString()))
                    {
                        for (idmail = 0; idmail == 3; idmail++)
                        {
                            table.Rows.Remove(row);
                            table.AcceptChanges();
                        }
                        break;
                    }
                }
                
                dataGridView1.DataSource = table;
            }
        }

        private void chkPassShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPassShow.Checked)
            {
                if (txtMatKhau.Text != "")
                {
                    txtMatKhau.UseSystemPasswordChar = true;
                }
            }
            else
            {
                if (txtMatKhau.Text != "")
                {
                    txtMatKhau.UseSystemPasswordChar = false;
                }
                else
                {
                    txtMatKhau.UseSystemPasswordChar = true;
                }
            }
        }

        private void FormListMail_Load(object sender, EventArgs e)
        {

        }

        private void FormListMail_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            
        }

        private void FormListMail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do You Want To Close Mail Exchange ?", "Announcement !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Log Out ?", "Announcement !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                FormDangNhap loginForm = new FormDangNhap();
                loginForm.Show();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtTaiKhoan.ResetText();
            txtMatKhau.ResetText();
            txtSoLuongMail.ResetText();
            chkPassShow.Checked = false;
            dateTimePicker1.ResetText();
            dateTimePicker2.ResetText();
            dataGridView1.DataSource = "";
            richTextBox1.ResetText();
        } //làm mới các textbox

        private void btnSoanMail_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSendMail loginForm = new FormSendMail();
            loginForm.Show();
        } //khi bấm Compose để chuyển qua form SendMail thì form hiện tại sẽ ẩn đi

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSendMail a = new FormSendMail();
            a.Show();
            //
            string b = txtTaiKhoan.Text;
            string c = txtMatKhau.Text;
        }
    }
}
