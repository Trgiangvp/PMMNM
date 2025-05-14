using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QUẢN_LÝ_KHÁCH_SẠN
{
    public partial class Bookings: Form
    {
        public Bookings()
        {
            InitializeComponent();
        }
        ConnectDb Connect = new ConnectDb();
        private void LoaddataGrigBooking()
        {
            string sql = "SELECT * FROM Booking "+
                "WHERE TrangThai=N'Chưa thanh toán'";
                
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }
        private void LoadRooms()
        {
            string sql = "select * from Rooms WHERE TinhTrang LIKE 'Tr%' ";
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            if (dt != null)
            {
                cboPhong.DataSource = dt;
                cboPhong.DisplayMember = "TenPhong";
                cboPhong.ValueMember = "MaPhong";
            }
        }
        private void LoadService()
        {
            string sql = "SELECT * FROM Service";
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            if (dt != null)
            {
               lstDichvu.DataSource = dt;
                lstDichvu.DisplayMember = "TenDV";
                lstDichvu.ValueMember = "MaDV";
            }
        }
        private void LoadCustomer()
        {
            string sql = "select * from Customers";
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            if (dt != null)
            {
                cboCustomer.DataSource = dt;
                cboCustomer.DisplayMember = "TenKH";
                cboCustomer.ValueMember = "CCCD";
            }
        }
        private void Bookings_Load(object sender, EventArgs e)
        {
            
            LoaddataGrigBooking();
            LoadRooms();
            LoadService();
            LoadCustomer();
        }
     
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Mở form thêm khách hàng
            AddCustomers addCustomers = new AddCustomers();
            addCustomers.ShowDialog();
        }
        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
           string sql = "select * FROM Rooms";
            if (cboPhong.SelectedIndex < 0) return; // Kiểm tra nếu không có mục nào được chọn
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            txtCostRoom.Text = dt.Rows[cboPhong.SelectedIndex]["GiaPhong"].ToString(); 
        }
        
       public void Songay()
        {
            DateTime startdate = dtpIn.Value;
            DateTime enddate = dtpOut.Value;
            TimeSpan interval = enddate - startdate;//Tính khoảng cách thời gian giữa 2 ngày
            int days = interval.Days+1;
            guna2HtmlLabel1.Text = days.ToString();
        }

         private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           Songay();
        }
       

        private void btnTong_Click(object sender, EventArgs e)
        {
         txtAmount.Text = (Convert.ToInt32(txtCostRoom.Text) * Convert.ToInt32(guna2HtmlLabel1.Text) + Convert.ToInt32(txtCostService.Text)).ToString();
        }
       
        private void lstDichvu_SelectedValueChanged(object sender, EventArgs e)//khi bấm chọn dịch vụ thì sẽ hiển thị giá tiền
        {
            if (lstDichvu.SelectedItems.Count == 0)
            {
                txtCostService.Text = "0";
            }
            // Nếu không có dịch vụ nào được chọn

            int sotien = 0;

            foreach (DataRowView selectedItem in lstDichvu.SelectedItems)
            {
                string maDV = selectedItem["MaDV"].ToString(); // Lấy mã dịch vụ được chọn
                string sql = $"SELECT GiaTien FROM Service WHERE MaDV = '{maDV}'";
                DataTable dt = Connect.ReadData(sql);

                if (dt.Rows.Count > 0)
                {
                    sotien += Convert.ToInt32(dt.Rows[0]["GiaTien"]); // Cộng dồn giá dịch vụ
                }
            }

            txtCostService.Text = sotien.ToString(); // Hiển thị tổng giá dịch vụ

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboCustomer.SelectedIndex < 0 || cboPhong.SelectedIndex < 0 || txtAmount.Text == "")
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin");
                return;
            }

            string cccd = cboCustomer.SelectedValue.ToString();
            string maphong = cboPhong.SelectedValue.ToString();
            string ngaynhan = dtpIn.Value.ToString("yyyy-MM-dd");
            string ngaytra = dtpOut.Value.ToString("yyyy-MM-dd");
            string tongtien = txtAmount.Text;

            List<string> maDVList = new List<string>();
            foreach (DataRowView selectedItem in lstDichvu.SelectedItems)
            {
                maDVList.Add(selectedItem["MaDV"].ToString().Trim());
            }

            string maSDDV = string.Join(",", maDVList);

            // Thêm vào bảng Booking
            string sqlBooking = $"INSERT INTO Booking (CCCD, MaPhong, NgayDen, NgayDi, MaSDDV, ThanhTien,TrangThai ) " +
                                $"VALUES ('{cccd}', '{maphong}', '{ngaynhan}', '{ngaytra}', '{maSDDV}', '{tongtien}',N'Chưa thanh toán');";
             sqlBooking+= $"UPDATE Rooms SET TinhTrang =N'Có khách' WHERE MaPhong ='{maphong}'";
            int kqBooking = Connect.WriteData(sqlBooking);

            if (kqBooking > 0)
            {
                // Lấy MaHD vừa thêm trước đó theo giảm dần
                string sqlGetMaHD = "SELECT TOP 1 MaHD FROM Booking ORDER BY MaHD DESC";
                DataTable dt = Connect.ReadData(sqlGetMaHD);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int maHD = Convert.ToInt32(dt.Rows[0]["MaHD"]);

                    // Thêm hóa đơn ngay khi đặt phòng thành công
                    string sqlHoaDon = $"INSERT INTO HoaDon (MaHD, CCCD, MaPhong, MaSDDV , TrangThai, ThanhTien) " +
                                       $"VALUES ('{maHD}', '{cccd}', '{maphong}','{maSDDV}', N'Chưa thanh toán','{tongtien}')";
                  

                    int kqHoaDon = Connect.WriteData(sqlHoaDon);
                    if (kqHoaDon > 0)
                    {
                        MessageBox.Show("Thêm thành công vào Booking và tạo hóa đơn!");
                        LoaddataGrigBooking();
                        LoadRooms();
                    }
                    else
                    {
                        MessageBox.Show("Thêm Booking thành công nhưng tạo hóa đơn thất bại.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Thêm thất bại vào bảng Booking");
            }
        }

  
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtMaHD.Text=="")
            {
                MessageBox.Show("Vui lòng chọn một đặt phòng để sửa.");
                return;
            }

            string maHD = txtMaHD.Text;
            string cccd = cboCustomer.SelectedValue.ToString();
            string maphong = cboPhong.Text;
            string ngaynhan = dtpIn.Value.ToString("yyyy-MM-dd");
            string ngaytra = dtpOut.Value.ToString("yyyy-MM-dd");
            string tongtien = txtAmount.Text;
            // Lấy danh sách dịch vụ đã chọn từ ListBox
            List<string> MaDVlist = new List<string>();
            foreach (DataRowView selectedItem in lstDichvu.SelectedItems)
            {
               MaDVlist.Add(selectedItem["MaDV"].ToString().Trim());
            }
            string maSDDV = string.Join(",",MaDVlist); // Ghép thành chuỗi "1,2,3"

            // Cập nhật dữ liệu trong bảng Booking
            string sqlUpdateBooking = $"UPDATE Booking SET CCCD = '{cccd}', MaPhong = '{maphong}', NgayDen = '{ngaynhan}', " +
                                      $"NgayDi = '{ngaytra}', MaSDDV = '{maSDDV}', ThanhTien='{tongtien}',TrangThai=N'Chưa thanh toán' WHERE MaHD = {maHD}";

            int kqBooking = Connect.WriteData(sqlUpdateBooking);

            if (kqBooking > 0)
            {
                string sqlUpdateHoaDon = $"UPDATE HoaDon SET CCCD = '{cccd}', MaPhong = '{maphong}', MaSDDV='{maSDDV}',TrangThai=N'Chưa thanh toán', ThanhTien='{tongtien}'  WHERE MaHD = {maHD}";

                int kqHoaDon = Connect.WriteData(sqlUpdateHoaDon);

                if (kqHoaDon > 0)
                {
                    MessageBox.Show("Cập nhật thành công Booking và Hóa Đơn!");
                    LoaddataGrigBooking();
                    LoadRooms();
                }
                else
                {
                    MessageBox.Show("Cập nhật Booking thành công nhưng cập nhật hóa đơn thất bại.");
                }

                LoaddataGrigBooking();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }


        private void btnDel_Click(object sender, EventArgs e)
        {
            if (txtMaHD.Text.Trim()=="")
            {
                MessageBox.Show("Vui lòng chọn một đơn đặt phòng để xóa.");
                return;
            }
            string mahd = txtMaHD.Text;
            string map = cboPhong.SelectedValue.ToString();
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string sqlDelBk = $"DELETE FROM Booking WHERE MaHD = '{mahd}'";
                sqlDelBk += $"DELETE FROM HoaDon WHERE MaHD = '{mahd}'"; // Xóa hóa đơn liên quan
                sqlDelBk += $"UPDATE Rooms SET TinhTrang = N'Trống' WHERE MaPhong = '{map}'"; // Cập nhật lại tình trạng phòng
                sqlDelBk += $"DELETE FROM CtietHD WHERE MaHD='{mahd}'";


                int kq = Connect.WriteData(sqlDelBk);
                if (kq > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoaddataGrigBooking();
                    LoadRooms();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuTK = txtSearch.Text.Trim();
            string sql = $"SELECT * FROM Booking " +
                $"WHERE MaPhong LIKE '%{tuTK}%' OR CCCD LIKE '%{tuTK}%'";
            DataTable dt = Connect.ReadData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                LoaddataGrigBooking(); // Tải lại dữ liệu gốc nếu không tìm thấy
            }
        
    }

       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            txtMaHD.Text = dataGridView1.CurrentRow.Cells["MaHD"].Value?.ToString();
            cboCustomer.SelectedValue = dataGridView1.CurrentRow.Cells["CCCD"].Value;
            cboPhong.Text = dataGridView1.CurrentRow.Cells["MaPhong"].Value?.ToString();
            dtpIn.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["NgayDen"].Value);
            dtpOut.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["NgayDi"].Value);

        }

        private void dtpIn_ValueChanged(object sender, EventArgs e)
        {
            Songay();
        }

        private void cboCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            string sql = "select * from Customers";
            if (cboCustomer.SelectedIndex < 0) return; // Kiểm tra nếu không có mục nào được chọn
            DataTable dt = new DataTable();
            dt = Connect.ReadData(sql);
            txtCCCD.Text = dt.Rows[cboCustomer.SelectedIndex]["CCCD"].ToString();
        }
    }
}
