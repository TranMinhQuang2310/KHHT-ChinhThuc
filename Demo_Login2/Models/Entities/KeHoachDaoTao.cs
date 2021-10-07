using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo_Login2.Models.Entities
{
    public class KeHoachDaoTao
    {
        public int ID { get; set; }
        public int IDKhoaDaoTao { get; set; }
        public string TenKeHoachDaoTao { get; set; }
        public string GhiChu { get; set; }
    }
}