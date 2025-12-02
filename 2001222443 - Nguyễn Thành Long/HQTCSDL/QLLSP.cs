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

namespace HQTCSDL
{
    public partial class QLLSP : Form
    {
        DataHelper db;

        public QLLSP()
        {
            InitializeComponent();
            db = new DataHelper();
        }

        private void QLLSP_Load(object sender, EventArgs e)
        {
            LoadLoaiSanPham();
            dgvLoaiSanPham.CellClick += dgvLoaiSanPham_CellClick;
            textMa.Enabled = false;
        }

        private void LoadLoaiSanPham()
        {
            try
            {
                string sql = "SELECT * FROM LoaiSanPham";
                DataTable dtLoaiSanPham = db.ExecuteQuery(sql);
                dgvLoaiSanPham.DataSource = dtLoaiSanPham;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvLoaiSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click vào header hoặc dòng rỗng
            if (e.RowIndex < 0)
                return;

            // Lấy dòng hiện tại
            DataGridViewRow row = dgvLoaiSanPham.Rows[e.RowIndex];

            textMa.Text = row.Cells["MaLoaiSP"].Value?.ToString();
            textTenLoai.Text = row.Cells["TenLoaiSP"].Value?.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string tenLoai = textTenLoai.Text.Trim();

                if (string.IsNullOrEmpty(tenLoai))
                {
                    MessageBox.Show("Vui lòng nhập tên loại sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenLoaiSP", tenLoai)
                };

                // Gọi Stored Procedure (dùng hàm ExecuteStoredProcedureScalar bạn đã có)
                object newId = db.ExecuteStoredProcedureScalar("sp_ThemLoaiSanPham", parameters);

                if (newId != null)
                {
                    MessageBox.Show($"Thêm loại sản phẩm thành công! Mã loại mới: {newId}", "Thông báo");
                    LoadLoaiSanPham(); 
                    textTenLoai.Clear();
                }
                else
                {
                    MessageBox.Show("Không thêm được loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ form
                if (string.IsNullOrEmpty(textMa.Text))
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maLoai = Convert.ToInt32(textMa.Text);
                string tenLoai = textTenLoai.Text.Trim();

                if (string.IsNullOrEmpty(tenLoai))
                {
                    MessageBox.Show("Vui lòng nhập tên loại sản phẩm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaLoaiSP", maLoai),
                    new SqlParameter("@TenLoaiSP", tenLoai)
                };

                // Gọi SP cập nhật
                int affected = db.ExecuteStoredProcedureScalarInt("sp_SuaLoaiSanPham", parameters);

                if (affected == -1)
                {
                    MessageBox.Show("Mã loại sản phẩm không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (affected == 0)
                {
                    MessageBox.Show("Không có gì thay đổi!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (affected > 0)
                {
                    MessageBox.Show("Cập nhật loại sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLoaiSanPham();
                }
                else if (affected == -99)
                {
                    MessageBox.Show("Lỗi hệ thống hoặc lỗi SQL (CATCH). Không thể cập nhật!",
                                    "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi không xác định!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                if (string.IsNullOrEmpty(textMa.Text))
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm cần xóa!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maLoai = Convert.ToInt32(textMa.Text);

                // Hỏi xác nhận trước khi xóa
                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa loại sản phẩm này không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No)
                    return;

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaLoaiSP", maLoai)
                };

                int result = db.ExecuteStoredProcedureScalarInt("sp_XoaLoaiSanPham", parameters);

                if (result > 0)
                {
                    MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo");
                    LoadLoaiSanPham();
                    textMa.Clear();
                    textTenLoai.Clear();
                }
                else if (result == -1)
                {
                    MessageBox.Show("Mã loại sản phẩm không tồn tại!", "Thông báo");
                }
                else if (result == -2)
                {
                    MessageBox.Show("Không thể xóa! Vẫn còn sản phẩm thuộc loại này.", "Cảnh báo");
                }
                else
                {
                    MessageBox.Show("Không có loại sản phẩm nào bị xóa.", "Thông báo");
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
    }
}
