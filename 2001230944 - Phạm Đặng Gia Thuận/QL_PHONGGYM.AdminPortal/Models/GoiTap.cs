using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) class C# này tới bảng "GoiTap" trong CSDL
    [Table("GoiTap")]
    public class GoiTap
    {
        // Đánh dấu đây là Khóa chính
        [Key]
        public int MaGoiTap { get; set; }

        [Required(ErrorMessage = "Tên gói không được để trống")]
        [StringLength(50, ErrorMessage = "Tên gói không được vượt quá 50 ký tự")]
        public string TenGoi { get; set; }

        [Required(ErrorMessage = "Thời hạn không được để trống")]
        [Range(1, 120, ErrorMessage = "Thời hạn phải là một số (tháng) hợp lệ")]
        public int ThoiHan { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [DataType(DataType.Currency)]
        public decimal Gia { get; set; }

        // Cho phép null (không bắt buộc)
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }

        // ======================================================
        // Tạo mối quan hệ (Navigation Property)
        // "Một Gói Tập có thể có Nhiều Lượt Đăng Ký"
        // ======================================================
        public virtual ICollection<DangKyGoiTap> DangKyGoiTaps { get; set; }

        // (Tùy chọn) Khởi tạo danh sách để tránh lỗi null
        public GoiTap()
        {
            this.DangKyGoiTaps = new HashSet<DangKyGoiTap>();
        }
    }
}
