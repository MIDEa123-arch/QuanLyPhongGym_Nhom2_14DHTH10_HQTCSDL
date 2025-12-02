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
    public partial class QLHA : Form
    {
        DataHelper db;
        private int maHinhSelected = 0;

        public QLHA()
        {
            InitializeComponent();
            db = new DataHelper();
            LoadMaSP();
            LoadTatCaHinhAnh();
            dgvHinhAnh.CellClick += dgvHinhAnh_CellClick;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void LoadTatCaHinhAnh()
        {
            string sql = "SELECT MaHinh, MaSP, Url, IsMain FROM HINHANH ORDER BY MaSP, IsMain DESC";

            DataTable dt = db.ExecuteQuery(sql);

            dgvHinhAnh.DataSource = dt;
        }

        private void dgvHinhAnh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                maHinhSelected = Convert.ToInt32(dgvHinhAnh.Rows[e.RowIndex].Cells["MaHinh"].Value);

                cbMaSP.SelectedValue = dgvHinhAnh.Rows[e.RowIndex].Cells["MaSP"].Value;
                textURL.Text = dgvHinhAnh.Rows[e.RowIndex].Cells["Url"].Value.ToString();
                checkIsMain.Checked = Convert.ToBoolean(dgvHinhAnh.Rows[e.RowIndex].Cells["IsMain"].Value);
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void LoadMaSP()
        {
            string sql = "SELECT MaSP, TenSP FROM SanPham ORDER BY MaSP";
            DataTable dt = db.ExecuteQuery(sql);
            cbMaSP.DataSource = dt;
            cbMaSP.DisplayMember = "MaSP";
            cbMaSP.ValueMember = "MaSP";

        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textURL.Text))
            {
                MessageBox.Show("Vui lòng nhập URL hình!");
                return;
            }

            int maSP = Convert.ToInt32(cbMaSP.SelectedValue);
            bool isMain = checkIsMain.Checked;

            // CHỈ CẦN INSERT – Trigger tự xử lý ảnh chính
            string procName = "sp_HinhAnh_Them";

            SqlParameter[] prms =
            {
                new SqlParameter("@MaSP", maSP),
                new SqlParameter("@Url", textURL.Text.Trim()),
                new SqlParameter("@IsMain", isMain)
            };

            object result = db.ExecuteStoredProcedureScalar(procName, prms);

            if (result != null)
            {
                MessageBox.Show("Thêm hình thành công!");
            }
            else
            {
                MessageBox.Show("Lỗi thêm hình!");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (maHinhSelected == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 hình để sửa!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textURL.Text))
            {
                MessageBox.Show("URL không được để trống!");
                return;
            }

            int maSP = Convert.ToInt32(cbMaSP.SelectedValue);
            string url = textURL.Text.Trim();
            bool isMain = checkIsMain.Checked;

            string procName = "sp_HinhAnh_Sua";

            SqlParameter[] prms =
            {
                new SqlParameter("@MaHinh", maHinhSelected),
                new SqlParameter("@MaSP", maSP),
                new SqlParameter("@Url", url),
                new SqlParameter("@IsMain", isMain)
            };

            object result = db.ExecuteStoredProcedureScalar(procName, prms);

            if (result != null)
            {
                MessageBox.Show("Sửa hình thành công!");
                LoadTatCaHinhAnh();  
                maHinhSelected = 0;   // reset lại ID
            }
            else
            {
                MessageBox.Show("Sửa thất bại!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (maHinhSelected == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 hình để xóa!");
                return;
            }

            // Hỏi xác nhận
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa hình này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.No)
                return;

            string procName = "sp_HinhAnh_Xoa";

            SqlParameter[] prms =
            {
                new SqlParameter("@MaHinh", maHinhSelected)
            };

            object result = db.ExecuteStoredProcedureScalar(procName, prms);

            if (result != null)
            {
                MessageBox.Show("Xóa hình thành công!");
                LoadTatCaHinhAnh();   // load lại
                maHinhSelected = 0;   // reset
                textURL.Clear();
                checkIsMain.Checked = false;
            }
            else
            {
                MessageBox.Show("Xóa thất bại!");
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            LoadTatCaHinhAnh();
        }
    }
}
