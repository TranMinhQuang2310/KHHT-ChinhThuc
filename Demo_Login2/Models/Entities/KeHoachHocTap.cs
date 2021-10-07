using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo_Login2.Models.Entities
{
    public class KeHoachHocTap
    {
        public int ID { get; set; }
        public int IDKeHoachDaoTao { get; set; }
        public bool TrangThai { get; set; }
        public bool HocLai { get; set; }
        public bool HocVuot { get; set; }
        public bool CaiThien { get; set; }
        public string GhiChu { get; set; }
    }
}