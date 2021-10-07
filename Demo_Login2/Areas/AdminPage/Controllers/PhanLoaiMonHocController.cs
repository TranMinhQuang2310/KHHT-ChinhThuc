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
    public class PhanLoaiMonHocController : Controller
    {
        //LayPhanLoaiMonHoc
        public ActionResult Index()
        {
            var lstphanloaimh = this.LayDanhSachPhanLoaiMonHoc();
            return View(lstphanloaimh);
        }

        public List<PhanLoaiMonHocDTO> LayDanhSachPhanLoaiMonHoc()
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.LayDanhSachPhanLoaiMonHoc();
            }
        }

        //Get : TaoPhanLoaiMonHoc
        public ActionResult Create()
        {
            return View();
        }

        //Post : TaoPhanLoaiMonHoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PhanLoaiMonHocDTO phanloaimh)
        {
            var id = LayPhanLoaiMonHocDaTonTai(phanloaimh.LoaiMonHoc);
            if(id > 0)
            {
                ViewBag.Error = "Lọai Môn Học đã tồn tại";
                return View();
            }
            else
            {
                ThemPhanLoaiMonHoc(phanloaimh);
                return RedirectToAction("Index");
            }
        }

        public int LayPhanLoaiMonHocDaTonTai(string loaimonhoc)
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.LayPhanLoaiMonHocDaTonTai(loaimonhoc);
            }
        }
        public bool ThemPhanLoaiMonHoc(PhanLoaiMonHocDTO phanloaimh)
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.ThemPhanLoaiMonHoc(phanloaimh);
            }
        }

        //Get : SuaPhanLoaiMonHoc
        public ActionResult Edit(int id)
        {
            PhanLoaiMonHocDTO phanloaimh = LayPhanLoaiMonHoc(id);
            return View(phanloaimh);
        }

        //Post : SuaPhanLoaiMonHoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PhanLoaiMonHocDTO phanloaimh)
        {
            var id = LayPhanLoaiMonHocDaTonTai(phanloaimh.LoaiMonHoc);
            if (id == phanloaimh.ID || id == 0)
            {
                SuaPhanLoaiMonHoc(phanloaimh);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Loại Môn Học đã tồn tại";
                return View();
            }
        }

        public PhanLoaiMonHocDTO LayPhanLoaiMonHoc(int id)
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.LayPhanLoaiMonHoc(id);
            }
        }

        public bool SuaPhanLoaiMonHoc(PhanLoaiMonHocDTO phanloaimh)
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.SuaPhanLoaiMonHoc(phanloaimh);
            }
        }

        //XoaPhanLoaiMonHoc
        public async Task<ActionResult> Delete(int id)
        {
            var findphanloaimh = CheckLoiPhanLoaiMonHocDaTonTai(id);
            if(findphanloaimh > 0)
            {
                TempData["error"] = "lỗi";
                return RedirectToAction("Index");
            }
            else
            {
                var output = XoaPhanLoaiMonHoc(id);
                if (output)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Fail");
                }
            }
            
        }

        public bool XoaPhanLoaiMonHoc(int id)
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.XoaPhanLoaiMonHoc(id);
            }
        }

        public int CheckLoiPhanLoaiMonHocDaTonTai(int? id)
        {
            using(PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.CheckLoiPhanLoaiMonHocDaTonTai(id);
            }
        }

        //XemChiTietPhanLoaiMonHoc
        public ActionResult Details(int id)
        {
            var lstphanloaimh = LayPhanLoaiMonHoc(id);
            return View(lstphanloaimh);
        }
    }
}