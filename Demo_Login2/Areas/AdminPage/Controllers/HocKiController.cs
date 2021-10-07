using Demo_Login2.Areas.AdminPage.Business;
using Demo_Login2.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Demo_Login2.Areas.AdminPage.Controllers
{
    public class HocKiController : Controller
    {
        //TaoDanhSachCacThangTrongNam
        List<SelectListItem> lstThang = new List<SelectListItem>()
        {
            new SelectListItem
            {
                Text = "Tháng 1",Value = "1"
            },new SelectListItem
            {
                Text = "Tháng 2",Value = "2"
            },new SelectListItem
            {
                Text = "Tháng 3",Value = "3"
            },new SelectListItem
            {
                Text = "Tháng 4",Value = "4"
            },new SelectListItem
            {
                Text = "Tháng 5",Value = "5"
            },new SelectListItem
            {
                Text = "Tháng 6",Value = "6"
            },new SelectListItem
            {
                Text = "Tháng 7",Value = "7"
            },new SelectListItem
            {
                Text = "Tháng 8",Value = "8"
            },new SelectListItem
            {
                Text = "Tháng 9",Value = "9"
            },new SelectListItem
            {
                Text = "Tháng 10",Value = "10"
            },new SelectListItem
            {
                Text = "Tháng 11",Value = "11"
            },new SelectListItem
            {
                Text = "Tháng 12",Value = "12"
            },
        };


        //LayDanhSachHocKi
        public ActionResult Index()
        {
            var lstHocKi = this.LayDanhSachHocKi();
            ViewBag.phanloaihocki = LayDanhSachPhanLoaiHocKi();
            return View(lstHocKi);
        }

        public List<HocKiDTO> LayDanhSachHocKi()
        {
            using(HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.LayDanhSachHocKi();
            }
        }

        //Get : TaoHocKi
        public ActionResult Create()
        {
            ViewData["phanloaihocki"] = new SelectList(LayDanhSachPhanLoaiHocKi(), "ID", "LoaiHocKi");
            ViewBag.lstThangs = lstThang;
            return View();
        }

        //Post : TaoHocKi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HocKiDTO hocki)
        {
            var id = LayHocKiDaTonTai(hocki.TenHocKi);
            if(id > 0)
            {
                ViewBag.Message = "Tên Học Kì đã tồn tại";
                ViewData["phanloaihocki"] = new SelectList(LayDanhSachPhanLoaiHocKi(), "ID", "LoaiHocKi");
                return View();
            }
            else
            {
                ThemHocKi(hocki);
                return RedirectToAction("Index");
            }
        }

        public int LayHocKiDaTonTai(string hocki)
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.LayHocKiDaTonTai(hocki);
            }
        }
        public List<PhanLoaiHocKiDTO> LayDanhSachPhanLoaiHocKi()
        {
            using (PhanLoaiHocKiBusiness bs = new PhanLoaiHocKiBusiness())
            {
                return bs.LayDanhSachPhanLoaiHocKi();
            }
        }

        public bool ThemHocKi(HocKiDTO hocki)
        {
            using(HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.ThemHocKi(hocki);
            }
        }

        //Get : SuaHocKi
        public ActionResult Edit(int id)
        {
            ViewBag.lstThangs = lstThang;
            ViewData["phanloaihocki"] = new SelectList(LayDanhSachPhanLoaiHocKi(), "ID", "LoaiHocKi");
            HocKiDTO hocki = LayHocKi(id);
            return View(hocki);
        }

        //Post : SuaHocKi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HocKiDTO hocki)
        {
            var id = LayHocKiDaTonTai(hocki.TenHocKi);
            if (id == hocki.ID || id == 0)
            {
                SuaHocKi(hocki);
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Message = "Học Kì đã tồn tại!!";
                ViewData["phanloaihocki"] = new SelectList(LayDanhSachPhanLoaiHocKi(), "ID", "LoaiHocKi");
                return View();
            }
        }

        public HocKiDTO LayHocKi(int id)
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.LayHocKi(id);
            }
        }

        public bool SuaHocKi(HocKiDTO hocki)
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.SuaHocKi(hocki);
            }
        }

        //XoaHocKi
        public async Task<ActionResult> Delete(int id)
        {
            var findhocki_monhockhoaDT = CheckLoiHocKiDaTonTai_MonHocKhoaDaoTao(id);
            var findhocki_trangthaidangkimon = CheckLoiHocKiDaTonTai_TrangThaiDangKiMon(id);
            if (findhocki_monhockhoaDT > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.phanloaihocki = LayDanhSachPhanLoaiHocKi();
                return RedirectToAction("Index");
            }else if(findhocki_trangthaidangkimon > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.phanloaihocki = LayDanhSachPhanLoaiHocKi();
                return RedirectToAction("Index");
            }
            else
            {
                var output = XoaHocKi(id);
                if (output)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("fail");
                }
            }
        }
        public bool XoaHocKi(int id)
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.XoaHocKi(id);
            }
        }
        public int CheckLoiHocKiDaTonTai_MonHocKhoaDaoTao(int? id)
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.CheckLoiHocKiDaTonTai_MonHocKhoaDaoTao(id);
            }
        }
        public int CheckLoiHocKiDaTonTai_TrangThaiDangKiMon(int? id)
        {
            using(HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.CheckLoiHocKiDaTonTai_TrangThaiDangKiMon(id);
            }
        }
        //XemChiTietHocKi
        public ActionResult Details(int id)
        {
            var hocki = LayHocKi(id);
            ViewBag.phanloaihocki = LayDanhSachPhanLoaiHocKi();
            ViewBag.lstThangs = lstThang;
            return View(hocki);
        }
    }


}