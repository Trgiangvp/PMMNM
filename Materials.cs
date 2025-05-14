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
    public partial class Materials: Form
    {
        public Materials()
        {
            InitializeComponent();
        }
        ConnectDb Connect = new ConnectDb();
        private void Loaddata()
        {   

            string sql = "SELECT * FROM Materials";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }
        private void Materials_Load(object sender, EventArgs e)
        {
            Loaddata();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string mavt = txtMaVT.Text;
            string tenvt = txtTenVT.Text;
            string giatien = txtGiaTien.Text;
            string sql = $"INSERT INTO Materials VALUES ('{mavt}',N'{tenvt}','{giatien}')";
            int kq=Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Thêm thành công");
                Loaddata();
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string mavt = txtMaVT.Text;
            string tenvt = txtTenVT.Text;
            string giatien = txtGiaTien.Text;
            string sql = $"UPDATE Materials SET TenVT=N'{tenvt}',GiaTien='{giatien}' WHERE MaVT='{mavt}'";
            int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Sửa thành công");
                Loaddata();
            }
            else
            {
                MessageBox.Show("Sửa thất bại");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {   
           
            txtGiaTien.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtMaVT.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtTenVT.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string mavt = txtMaVT.Text;
            string sql = $"DELETE FROM Materials WHERE MaVT='{mavt}'";
            int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Xóa thành công");
                Loaddata();
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuTK = txtSearch.Text;
            string sql = "SELECT * FROM Materials " +
                $"WHERE MaVT LIKE '%{tuTK}%' OR TenVT LIKE N'%{tuTK}%' ";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }else
            {
                Loaddata();
            }

        }
    }
}
