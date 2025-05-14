using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QUẢN_LÝ_KHÁCH_SẠN
{
    public partial class Customer: Form
    {
        public Customer()
        {
            InitializeComponent();
        }
        ConnectDb Connect = new ConnectDb();
        private void LoaddataGrigCustomer()
        {
            string sql = "select * from Customers";
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Customer_Load(object sender, EventArgs e)
        {
            LoaddataGrigCustomer();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txtCCCD.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtTen.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtDiaChi.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            cboGt.SelectedItem = dataGridView1.CurrentRow.Cells[3].Value;
            txtSDT.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string cccd = txtCCCD.Text;
            string ten = txtTen.Text;
            string sdt = txtSDT.Text;
            string diachi = txtDiaChi.Text;
            string gioitinh = cboGt.SelectedItem.ToString();
            string sql = $"INSERT INTO Customers VALUES ('{cccd}',N'{ten}',N'{diachi}',N'{gioitinh}','{sdt}')";
            int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Thêm thành công 👌👌👌");
                LoaddataGrigCustomer();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string cccd = txtCCCD.Text;
            string ten = txtTen.Text;
            string sdt = txtSDT.Text;
            string diachi = txtDiaChi.Text;
            string gioitinh = cboGt.SelectedItem.ToString();
            string sql = $"UPDATE Customers SET TenKH=N'{ten}', DiaChi=N'{diachi}', GioiTinh=N'{gioitinh}', Sdt='{sdt}' WHERE CCCD='{cccd}'";
            int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Thành công 👌👌👌");
                LoaddataGrigCustomer();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string cccd = txtCCCD.Text;
            string sql = $"DELETE FROM Customers WHERE CCCD='{cccd}'";
            int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Xóa thành công 👌👌👌");
                LoaddataGrigCustomer();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuTK = txtSearch.Text;
            string sql = $"SELECT * FROM Customers " +
                $"WHERE TenKH LIKE '%{tuTK}%' OR CCCD LIKE '%{tuTK}%'";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                LoaddataGrigCustomer();
            }
        }
    }
}
