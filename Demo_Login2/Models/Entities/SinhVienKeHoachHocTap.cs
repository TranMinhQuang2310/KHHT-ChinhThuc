using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo_Login2.Models.Entities
{
    public class SinhVienKeHoachHocTap
    {
        public int ID { get; set; }
        public int IDSinhVien { get; set; }
        public int IDKeHoachHocTap { get; set; }
        public string GhiChu { get; set; }
    }
}