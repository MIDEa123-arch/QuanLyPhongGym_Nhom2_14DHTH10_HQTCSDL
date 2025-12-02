using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HQTCSDL
{
    public partial class Form1 : Form
    {
        private DataHelper db; // Sử dụng class helper
        private DataTable dtSanPham;
        private DataTable dtLoaiSanPham;

        public Form1()
        {
            InitializeComponent();
            db = new DataHelper();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLoaiSanPham();
            LoadSanPham();

            textMaSP.Enabled = false;
            dgvSanPham.CellClick += dgvSanPham_CellClick;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadSanPham();
        }

        private void LoadSanPham()
        {
            try
            {
                string sql = "SELECT * FROM SanPham";
                dtSanPham = db.ExecuteQuery(sql);
                dgvSanPham.DataSource = dtSanPham;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy dữ liệu từ các control trên form
                string tenSP = textTenSP.Text.Trim();
                int maLoaiSP = Convert.ToInt32(textLSP.SelectedValue);
                decimal donGia = Convert.ToDecimal(textDonGia.Text);
                int soLuongTon = string.IsNullOrEmpty(textSL.Text) ? 0 : Convert.ToInt32(textSL.Text);
                decimal? giaKM = string.IsNullOrEmpty(textGKM.Text) ? (decimal?)null : Convert.ToDecimal(textGKM.Text);
                string hang = textHSX.Text.Trim();
                string xuatXu = textXuatXu.Text.Trim();
                string baoHanh = textBaoHanh.Text.Trim();
                string moTaChung = textMTC.Text.Trim();
                string moTaChiTiet = textMTCT.Text.Trim();

                //Tạo danh sách tham số cho SP
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenSP", tenSP),
                    new SqlParameter("@MaLoaiSP", maLoaiSP),
                    new SqlParameter("@DonGia", donGia),
                    new SqlParameter("@SoLuongTon", soLuongTon),
                    new SqlParameter("@GiaKhuyenMai", giaKM.HasValue ? (object)giaKM.Value : DBNull.Value),
                    new SqlParameter("@Hang", string.IsNullOrEmpty(hang) ? (object)DBNull.Value : hang),
                    new SqlParameter("@XuatXu", string.IsNullOrEmpty(xuatXu) ? (object)DBNull.Value : xuatXu),
                    new SqlParameter("@BaoHanh", string.IsNullOrEmpty(baoHanh) ? (object)DBNull.Value : baoHanh),
                    new SqlParameter("@MoTaChung", string.IsNullOrEmpty(moTaChung) ? (object)DBNull.Value : moTaChung),
                    new SqlParameter("@MoTaChiTiet", string.IsNullOrEmpty(moTaChiTiet) ? (object)DBNull.Value : moTaChiTiet)
                };

                //Gọi SP và lấy mã sản phẩm mới
                object newId = db.ExecuteStoredProcedureScalar("sp_ThemSanPham", parameters);

                if (newId != null)
                {
                    MessageBox.Show($"Thêm sản phẩm thành công! Mã SP mới: {newId}");
                    LoadSanPham(); // Tải lại danh sách
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được thêm.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void LoadLoaiSanPham()
        {
            try
            {
                string sql = "SELECT MaLoaiSP, TenLoaiSP FROM LoaiSanPham";
                dtLoaiSanPham = db.ExecuteQuery(sql);

                textLSP.DataSource = dtLoaiSanPham;
                textLSP.DisplayMember = "TenLoaiSP"; // Tên hiển thị ra combobox
                textLSP.ValueMember = "MaLoaiSP";    // Giá trị thực (ID loại)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.Message);
            }
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click vào header hoặc dòng rỗng
            if (e.RowIndex < 0)
                return;

            // Lấy dòng hiện tại
            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            textMaSP.Text = row.Cells["MaSP"].Value?.ToString();
            textTenSP.Text = row.Cells["TenSP"].Value?.ToString();
            textDonGia.Text = row.Cells["DonGia"].Value?.ToString();
            textSL.Text = row.Cells["SoLuongTon"].Value?.ToString();
            textGKM.Text = row.Cells["GiaKhuyenMai"].Value?.ToString();
            textHSX.Text = row.Cells["Hang"].Value?.ToString();
            textXuatXu.Text = row.Cells["XuatXu"].Value?.ToString();
            textBaoHanh.Text = row.Cells["BaoHanh"].Value?.ToString();
            textMTC.Text = row.Cells["MoTaChung"].Value?.ToString();
            textMTCT.Text = row.Cells["MoTaChiTiet"].Value?.ToString();

            if (row.Cells["MaLoaiSP"].Value != null)
            {
                textLSP.SelectedValue = Convert.ToInt32(row.Cells["MaLoaiSP"].Value);
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy mã sản phẩm cần sửa(bắt buộc)
                if (string.IsNullOrEmpty(textMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.");
                    return;
                }

                int maSP = Convert.ToInt32(textMaSP.Text.Trim());
                string tenSP = textTenSP.Text.Trim();
                int? maLoaiSP = (textLSP.SelectedValue != null) ? Convert.ToInt32(textLSP.SelectedValue) : (int?)null;
                decimal? donGia = string.IsNullOrEmpty(textDonGia.Text) ? (decimal?)null : Convert.ToDecimal(textDonGia.Text);
                int? soLuongTon = string.IsNullOrEmpty(textSL.Text) ? (int?)null : Convert.ToInt32(textSL.Text);
                decimal? giaKM = string.IsNullOrEmpty(textGKM.Text) ? (decimal?)null : Convert.ToDecimal(textGKM.Text);
                string hang = textHSX.Text.Trim();
                string xuatXu = textXuatXu.Text.Trim();
                string baoHanh = textBaoHanh.Text.Trim();
                string moTaChung = textMTC.Text.Trim();
                string moTaChiTiet = textMTCT.Text.Trim();

                // Chuẩn bị tham số
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaSP", maSP),
                    new SqlParameter("@TenSP", string.IsNullOrEmpty(tenSP) ? (object)DBNull.Value : tenSP),
                    new SqlParameter("@MaLoaiSP", maLoaiSP.HasValue ? (object)maLoaiSP.Value : DBNull.Value),
                    new SqlParameter("@DonGia", donGia.HasValue ? (object)donGia.Value : DBNull.Value),
                    new SqlParameter("@SoLuongTon", soLuongTon.HasValue ? (object)soLuongTon.Value : DBNull.Value),
                    new SqlParameter("@GiaKhuyenMai", giaKM.HasValue ? (object)giaKM.Value : DBNull.Value),
                    new SqlParameter("@Hang", string.IsNullOrEmpty(hang) ? (object)DBNull.Value : hang),
                    new SqlParameter("@XuatXu", string.IsNullOrEmpty(xuatXu) ? (object)DBNull.Value : xuatXu),
                    new SqlParameter("@BaoHanh", string.IsNullOrEmpty(baoHanh) ? (object)DBNull.Value : baoHanh),
                    new SqlParameter("@MoTaChung", string.IsNullOrEmpty(moTaChung) ? (object)DBNull.Value : moTaChung),
                    new SqlParameter("@MoTaChiTiet", string.IsNullOrEmpty(moTaChiTiet) ? (object)DBNull.Value : moTaChiTiet)
                };

                // Gọi stored procedure qua DataHelper
                int rows = db.ExecuteStoredProcedureScalarInt("sp_SuaSanPham", parameters);

                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                    LoadSanPham(); // Refresh lại DataGridView
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm nào được cập nhật.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = textSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadSanPham();
                    return;
                }

                string sql;

                if (int.TryParse(keyword, out int maSP))
                {
                    sql = $"SELECT * FROM dbo.fn_TimKiemSanPham({maSP}, NULL)";
                }
                else
                {
                    sql = $"SELECT * FROM dbo.fn_TimKiemSanPham(NULL, N'{keyword.Replace("'", "''")}')";
                }

                DataTable dt = db.ExecuteQuery(sql);

                if (dt.Rows.Count > 0)
                {
                    dgvSanPham.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp.");
                    dgvSanPham.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        private void btnLSP_Click(object sender, EventArgs e)
        {
            QLLSP qlsp = new QLLSP();
            qlsp.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QLHA qLHA = new QLHA();
            qLHA.Show();
        }
    }
}
