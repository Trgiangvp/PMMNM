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
    public partial class Bill: Form
    {
        public Bill()
        {
            InitializeComponent();
        }
        ConnectDb Connect = new ConnectDb();
        private void LoaddataGrigBill()
        {
           
            //// Câu lệnh SELECT để hiển thị dữ liệu lên DataGridView
            string sql = "SELECT * FROM CtietHD";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Bill_Load(object sender, EventArgs e)
        {
            LoaddataGrigBill();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.SuspendLayout();
            txtMahd.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtMaKH.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtSDDV.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtMaPhong.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtTongTien.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            dtpNgaytt.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            cboTrangThai.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
             txtNhanVien.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            dataGridView1.ResumeLayout();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row= dataGridView1.Rows[e.RowIndex];

            }
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string mahd = txtMahd.Text;
                
                Xuathoadon xhd = new Xuathoadon (mahd);
                xhd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xuất");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn vui lòng đặt phòng để có hóa đơn", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtMahd.Text == "")
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            string mahd=txtMahd.Text;
            string makh = txtMaKH.Text;
            string sddv = txtSDDV.Text;
            string maphong = txtMaPhong.Text;
            string tongtien = txtTongTien.Text;
            string ngaytt = dtpNgaytt.Value.ToString("yyyy-MM-dd");
            string trangthai = cboTrangThai.SelectedItem.ToString();
            string nhanvien = txtNhanVien.Text;
            string sql = $"UPDATE CtietHD SET  CCCD= '{makh}', MaSDDV ='{sddv}',MaPhong= '{maphong}', TongTien='{tongtien}', NgayThanhToan='{ngaytt}' ," +
                $" TrangThai= N'{trangthai}', MaNV='{nhanvien}' WHERE MaHD='{mahd}'";
            int kq= Connect.WriteData(sql);
            if(kq > 0)
            {
                MessageBox.Show("Sửa thành công");
                LoaddataGrigBill();
            }
            else
            {
                MessageBox.Show("Sửa thất bại");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có muốn xóa hóa đơn này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.No)
            {
                return;
            }
            else
            {
                string mahd = txtMahd.Text;
                string sql = $"DELETE FROM CtietHD WHERE MaHD='{mahd}'";
                sql += $"DELETE FROM HoaDon WHERE MaHD='{mahd}'";
                sql += $"DELETE FROM Booking WHERE MaHD='{mahd}'";
                int kq = Connect.WriteData(sql);
                if (kq > 0)
                {

                    MessageBox.Show("Xóa thành công");

                    LoaddataGrigBill();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuTK = txtSearch.Text;
            string sql = $"SELECT * FROM CtietHD " +
                $" WHERE MaHD LIKE '%{tuTK}%' OR CCCD LIKE '%{tuTK}%' OR MaPhong LIKE '%{tuTK}%' OR MaNV LIKE '%{tuTK}%'";
            DataTable dataTable = Connect.ReadData(sql);
            if (dataTable != null)
            {
                dataGridView1.DataSource = dataTable;
            }
            else
            {
                LoaddataGrigBill();
            }
        }
    }
}
