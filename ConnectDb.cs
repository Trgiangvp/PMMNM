using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUẢN_LÝ_KHÁCH_SẠN
{
    class ConnectDb
    {
        private string connection;

        public ConnectDb()
        {
            connection = "Data Source=DESKTOP-0OPNQDB\\SQLEXPRESS;Initial Catalog=QUANLYKHACHSAN;Integrated Security=True";
        }
        public DataTable ReadData(string sql)
        {
            DataTable dt = new DataTable();//Tạo đối tượng DataTable
            SqlConnection con = new SqlConnection(connection);//Tạo đối tượng kết nối
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);//Đọc dữ liệu từ CSDL
            try
            {
                con.Open();
                adapter.Fill(dt); // Nạp dữ liệu vào DataTable
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối "+ex.Message);
                con.Close();
            }
            return dt;
        }
        public int WriteData(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(sql, con);//Thực thi câu lệnh SQL
            int roweffect = -1;
            try
            {
                con.Open();
                //Thực hiện ghi dữ liệu
                roweffect = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối " + ex.Message);
                con.Close();
                return -1;
            }
            return roweffect;
        }
    }
}
