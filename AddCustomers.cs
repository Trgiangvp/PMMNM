using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUẢN_LÝ_KHÁCH_SẠN
{
    public partial class AddCustomers: Form
    {
        public AddCustomers()
        {
            InitializeComponent();
        }
        ConnectDb Connect = new ConnectDb();
        private void LoaddataGrigAddCustomer()
        {
            string sql = "select * from Customers";
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }

        }
        private void AddCustomers_Load(object sender, EventArgs e)
        {
            LoaddataGrigAddCustomer();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCCCD.Text == "" || txtTenKh.Text == "" || txtSdt.Text == "" || txtDiaChi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string CCCD = txtCCCD.Text;
            string sqlcheck = "select * from Customers where CCCD='" + CCCD + "'";
            DataTable dt = Connect.ReadData(sqlcheck);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Đã có thông tin khách hàng");
                return;
            }
            string tenKH = txtTenKh.Text;
            string sdt = txtSdt.Text;
            string diachi = txtDiaChi.Text;
            string gioitinh = rdbNam.Checked ? "Nam" : "Nữ";
            string sql = "insert into Customers values('" + CCCD + "',N'" + tenKH + "',N'" + diachi + "',N'" + gioitinh + "','" + sdt + "')";
           int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Thêm thành công");
                LoaddataGrigAddCustomer();
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }

        private void rdbNam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuTK = txtSearch.Text;
            string sql = "SELECT * FROM Customers " +
                $" WHERE TenKH LIKE '%{tuTK}%' OR CCCD LIKE '%{tuTK}%' ";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                LoaddataGrigAddCustomer();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
