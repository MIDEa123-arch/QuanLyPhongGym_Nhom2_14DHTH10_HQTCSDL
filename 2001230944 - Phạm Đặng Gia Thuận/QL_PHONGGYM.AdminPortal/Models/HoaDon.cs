using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public int MaHD { get; set; }

        public int MaKH { get; set; }

        [Display(Name = "Ngày Lập")]
        public DateTime NgayLap { get; set; }

        [Display(Name = "Tổng Tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal TongTien { get; set; }

        [Display(Name = "Giảm Giá")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal GiamGia { get; set; }

        [Display(Name = "Thành Tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal ThanhTien { get; set; }

        [StringLength(20)]
        [Display(Name = "Trạng Thái")]
        public string TrangThai { get; set; }

        // Quan hệ: Thuộc về 1 Khách hàng
        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        // Quan hệ: Có nhiều Chi tiết
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}