using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using QUẢN_LÝ_KHÁCH_SẠN;

namespace QLKS
{
    public partial class Form_Đăng_Ký: Form
    {
        public Form_Đăng_Ký()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
        public  bool checkAccount(string ac)
        {
            return Regex.IsMatch(ac, "^[a-zA-Z0-9]{6,24}");

        }
        public bool CheckEmail(string em)
        {
            return Regex.IsMatch(em, "^[a-zA-Z0-9_.]{3,20}@gmail.com(.vn|)$");
        }
        ConnectDb connect = new ConnectDb();
        string ChucVu = "Khách hàng";
        private void guna2Button3_Click(object sender, EventArgs e)
        {
           
            string  email=textBox_Email.Text;
            string  matkhau=textBox_MatKhau.Text;
            string xnmatkhau = textBox_XNMatKhau.Text;
            string tentk = textBox_TenTaiKhoan.Text;
            if (!CheckEmail(email)) 
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng");
                return;
            }
            if (!checkAccount(matkhau))
            {
                MessageBox.Show("Mật khẩu  không đủ mạnh");
                return;
            }
            if (matkhau != xnmatkhau)
            {
                MessageBox.Show("Vui lòng xác  nhận  mật khẩu chính xác");
                return;
            }
            DataTable dt=new DataTable();
            dt = connect.ReadData("Select * from TaiKhoan where Email='" + email + "'");
            if (dt.Rows.Count>0)
            {
                MessageBox.Show("Email này đã được đăng  kí.Vui lòng đăng kí email khác");

                return;
            }
           
            try
            {
                string query= "Insert into  TaiKhoan (TaiKhoan,MatKhau,Email,ChucVu ) values('"+tentk+"' ,'" + matkhau+"','" + email + "','"+ChucVu+"')";
               int kq= connect.WriteData(query);
                if (kq > 0)
                {
                    DialogResult result = MessageBox.Show("Đăng kí thành công, bạn có muốn đăng nhập không?", "Thông báo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        this.Hide();
                        Form_Đăng_Nhập dn = new Form_Đăng_Nhập();
                        dn.ShowDialog();
                    }
                    else if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Đăng kí không thành công");
                }
               
            }
            catch
            {
                MessageBox.Show("Tài khoản này đã  tồn tại.Vui  lòng đăng kí tên tài khoản khác");
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form_Đăng_Ký_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form_Đăng_Nhập dn = new Form_Đăng_Nhập();
            dn.ShowDialog();
            this.Close();
        }
    }
}

