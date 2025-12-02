using QL_PHONGGYM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.Repositories
{
   
    public class GoiTapRepository
    {
        private readonly QL_PHONGGYMEntities _context;

        public GoiTapRepository(QL_PHONGGYMEntities context)
        {
            _context = context;
        }
        
        public List<GoiTap> goiTaps()
        {
            return _context.GoiTaps.ToList();
        }

        public GoiTap ThongTinGoiTap(int id)
        {
            return _context.GoiTaps.FirstOrDefault(gt => gt.MaGoiTap == id);
        }
    }
}