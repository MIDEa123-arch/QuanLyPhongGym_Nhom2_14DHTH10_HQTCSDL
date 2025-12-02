using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("ChucVu")]
    public class ChucVu
    {
        [Key]
        public int MaChucVu { get; set; }

        [Display(Name = "Tên Chức Vụ")]
        [Required(ErrorMessage = "Tên chức vụ không được để trống")]
        [StringLength(50)]
        public string TenChucVu { get; set; }

        // Mối quan hệ: Một Chức Vụ có nhiều Nhân Viên
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}