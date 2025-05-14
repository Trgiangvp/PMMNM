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
    public partial class CheckOut : Form
    {
        public CheckOut()
        {
            InitializeComponent();
        }
        //Tìm kiếm dữ liệu theo Mã phòng hoặc CCCD
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string tuTK=txtSearch.Text;
            string sql = "SELECT * FROM HoaDon " +
                $"WHERE MaPhong LIKE '%{tuTK}%' OR CCCD LIKE '%{tuTK}%' OR MaHD LIKE '%{tuTK}%'";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                LoaddataGrigCheckout();
            }

        }
        ConnectDb Connect = new ConnectDb();
        //Load dữ liệu lên DataGridView
        private void LoaddataGrigCheckout()
        {


            //// Câu lệnh SELECT để hiển thị dữ liệu lên DataGridView
            string sql = "SELECT * FROM HoaDon WHERE TrangThai =N'Chưa thanh toán' ";
            DataTable dt = Connect.ReadData(sql);

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }

        }//Load dữ liệu lên ComboBox
        private void LoadNhanvien()
        {
            lblNhanvien.Text = UserLogin.TenNV;
            lblNhanvien.Enabled = false; // Nếu muốn không cho sửa
        }
        private void CheckOut_Bill_Load(object sender, EventArgs e)
        {

            LoaddataGrigCheckout();
            LoadNhanvien();

        }

       

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.SuspendLayout();
            lblMaHD.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            lblMaKH.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            lblMaP.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            lblSDDV.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            lblTongtien.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            dataGridView1.ResumeLayout();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (lblMaHD.Text.Trim() == ".......")
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần thanh toán");
                return;
            }
            string mahd = lblMaHD.Text;
            string cccd = lblMaKH.Text;
            string masddv = lblSDDV.Text;
            string map = lblMaP.Text;
            string tongtien = lblTongtien.Text;
            string ngaythanhtoan = dtpNgaytt.Value.ToString("yyyy-MM-dd");
            string manv = UserLogin.MaNV;

            string  sqlCtiet= $"INSERT INTO CtietHD ( MaHD, CCCD, MaSDDV, MaPhong, TongTien, NgayThanhToan, TrangThai , MaNV )   " +
                $"VALUES ( '{mahd}','{cccd}','{masddv}','{map}','{tongtien}','{ngaythanhtoan}',N'Đã thanh toán','{manv}')";
            sqlCtiet += "UPDATE HoaDon SET TrangThai = N'Đã thanh toán' WHERE MaHD = '" + lblMaHD.Text + "'";
            sqlCtiet += $"UPDATE Booking SET TrangThai=N'Đã thanh toán' WHERE MaHD = '{mahd}' ";
            sqlCtiet += $"UPDATE Rooms SET TinhTrang =N'Trống' WHERE MaPhong = '{map}'";
            int result = Connect.WriteData(sqlCtiet);
            if (result > 0)
            {
                DialogResult r = MessageBox.Show("Thanh toán thành công, Bạn có muốn in hóa đơn?","Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question );
                if (r == DialogResult.Yes)
                {
                    Xuathoadon xhd = new Xuathoadon(mahd);
                    xhd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Đã thanh toán thành công");
                }
                LoaddataGrigCheckout();
            }
            else
            {
                MessageBox.Show("Thanh toán thất bại");
            }
        }

        private void dtpNgaytt_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            string mahd = lblMaHD.Text;
            string sql= "DELETE FROM HoaDon  WHERE MaHD = '" + lblMaHD.Text + "'";
            sql += $"DELETE FROM Booking WHERE MaHD = '{mahd}' ";
            sql += $"UPDATE Rooms SET TinhTrang =N'Trống' WHERE MaPhong = '{lblMaP.Text}'";
            int kq = Connect.WriteData(sql);
            if (kq > 0)
            {
                MessageBox.Show("Hủy hóa đơn thành công");
                LoaddataGrigCheckout();
            }
            else
            {
                MessageBox.Show("Hủy hóa đơn thất bại");
            }
        }

        private void lblNhanvien_Click(object sender, EventArgs e)
        {

        }
    }
}
