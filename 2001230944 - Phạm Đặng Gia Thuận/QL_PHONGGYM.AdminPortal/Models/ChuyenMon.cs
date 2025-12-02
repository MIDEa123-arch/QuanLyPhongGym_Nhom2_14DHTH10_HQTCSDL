using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("ChuyenMon")]
    public class ChuyenMon
    {
        [Key]
        public int MaCM { get; set; }

        [Display(Name = "Tên Chuyên Môn")]
        [Required(ErrorMessage = "Tên chuyên môn không được để trống")]
        [StringLength(50)]
        public string TenChuyenMon { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        // Mối quan hệ: Một Chuyên Môn có thể thuộc nhiều NhanVienChuyenMon
        public virtual ICollection<NhanVienChuyenMon> NhanVienChuyenMons { get; set; }
    }
}