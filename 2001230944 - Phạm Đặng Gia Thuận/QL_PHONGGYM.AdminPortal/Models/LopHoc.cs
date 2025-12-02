using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("LopHoc")]
    public class LopHoc
    {
        [Key]
        public int MaLop { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        [StringLength(50)]
        [Display(Name = "Tên Lớp")]
        public string TenLop { get; set; }

        [Display(Name = "Chuyên Môn")]
        public int MaCM { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Học Phí")]
        public decimal HocPhi { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Bắt Đầu")]
        public DateTime NgayBatDau { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Kết Thúc")]
        public DateTime NgayKetThuc { get; set; }

        [Required]
        [Display(Name = "Số Buổi")]
        public int? SoBuoi { get; set; }

        [Display(Name = "Sĩ Số Tối Đa")]
        public int? SiSoToiDa { get; set; }

        // --- CỘT MỚI: GIÁO VIÊN CHỦ NHIỆM ---
        [Display(Name = "Giáo Viên Đảm Nhận")]
        public int? MaNV { get; set; }

        // ==========================================================
        // == CÁC THUỘC TÍNH "ẢO" (KHÔNG CÓ TRONG CSDL) ==
        // == Dùng để nhập giờ học hàng tuần khi tạo lớp ==
        // ==========================================================

        [NotMapped]
        [Display(Name = "Giờ học bắt đầu (Hàng tuần)")]
        [DataType(DataType.Time)]
        public TimeSpan GioBatDau { get; set; }

        [NotMapped]
        [Display(Name = "Giờ học kết thúc (Hàng tuần)")]
        [DataType(DataType.Time)]
        public TimeSpan GioKetThuc { get; set; }

        // ==========================================================


        // --- Navigation Properties ---

        [ForeignKey("MaCM")]
        public virtual ChuyenMon ChuyenMon { get; set; }

        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        public virtual ICollection<DangKyLop> DangKyLops { get; set; }
        public virtual ICollection<LichLop> LichLops { get; set; }

        public LopHoc()
        {
            this.DangKyLops = new HashSet<DangKyLop>();
            this.LichLops = new HashSet<LichLop>();
        }
    }
}