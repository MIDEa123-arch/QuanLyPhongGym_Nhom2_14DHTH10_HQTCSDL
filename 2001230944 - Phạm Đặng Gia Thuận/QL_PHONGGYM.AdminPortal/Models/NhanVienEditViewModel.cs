using QL_PHONGGYM.AdminPortal.Models;
using System.Collections.Generic;
using System.Web.Mvc; // Cần cho SelectList

namespace QL_PHONGGYM.AdminPortal.ViewModels
{
    // Đây là một "Model" đặc biệt chỉ dùng cho giao diện Sửa
    public class NhanVienEditViewModel
    {
        // 1. Thông tin của Nhân Viên
        public NhanVien NhanVien { get; set; }

        // 2. Danh sách Dropdown cho Chức Vụ
        // (Chúng ta cần cái này để hiển thị Dropdown Chức Vụ)
        public SelectList ChucVuList { get; set; }

        // 3. Danh sách TẤT CẢ các Chuyên Môn (để tạo Checkbox)
        public List<ChuyenMonCheckBoxItem> ChuyenMonList { get; set; }
    }

    // Đây là một class "phụ" để giúp tạo Checkbox
    // Nó chứa Chuyên Môn VÀ một biến (bool) để biết nó có được tick hay không
    public class ChuyenMonCheckBoxItem
    {
        public int MaCM { get; set; }
        public string TenChuyenMon { get; set; }
        public bool IsChecked { get; set; }
    }
}
