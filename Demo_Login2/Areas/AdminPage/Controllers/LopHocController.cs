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
    public class LopHocController : Controller
    {
        //Get : LayDanhSachLopHoc
        public ActionResult Index()
        {
            var lstlophoc = this.LayDanhSachLopHocTheoKhoaDaoTao(0);
            ViewBag.khoadaotao = LayDanhSachKhoaDaoTao();

            var listkhoaDT = LayDanhSachKhoaDaoTao();
            listkhoaDT.Insert(0, new KhoaDaoTaoDTO
            {
                ID = 0,
                TenKhoaDaoTao = "Tất cả khóa"
            });
            ViewData["khoaDT"] = new SelectList(listkhoaDT, "ID", "TenKhoaDaoTao");
            return View(lstlophoc);
        }
        [HttpPost]
        //Post : LayDanhSachLopHoc
        public ActionResult Index(int id)
        {
            var lstlophoc = this.LayDanhSachLopHocTheoKhoaDaoTao(id);
            ViewBag.khoadaotao = LayDanhSachKhoaDaoTao();

            var listkhoaDT = LayDanhSachKhoaDaoTao();
            listkhoaDT.Insert(0, new KhoaDaoTaoDTO
            {
                ID = 0,
                TenKhoaDaoTao = "Tất cả khóa"
            });
            ViewData["khoaDT"] = new SelectList(listkhoaDT, "ID", "TenKhoaDaoTao");
            return View(lstlophoc);
        }
        public List<LopHocDTO> LayDanhSachLopHocTheoKhoaDaoTao(int id)
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.LayDanhSachLopHocTheoKhoaDaoTao(id);
            }
        }

        //Get : TaoLopHoc
        public ActionResult Create()
        {
            ViewData["khoa"] = new SelectList(LayDanhSachKhoaDaoTao(), "ID", "TenKhoaDaoTao");
            return View();
        }

        //Post : TaoLopHoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LopHocDTO lophoc)
        {
            var id = LayLopHocDaTonTai(lophoc.TenLop);
            if (id > 0)
            {
                ViewBag.Message = " Lớp Học đã tồn tại";
                ViewData["khoa"] = new SelectList(LayDanhSachKhoaDaoTao(), "ID", "TenKhoaDaoTao");
                return View();
            }
            else
            {
                ThemLopHoc(lophoc);
                return RedirectToAction("Index");
            }
        }

        public int LayLopHocDaTonTai(string tenlop)
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.LayLopHocDaTonTai(tenlop);
            }
        }

        public List<KhoaDaoTaoDTO> LayDanhSachKhoaDaoTao()
        {
            using (KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
            {
                return bs.LayDanhSachKhoaDaoTao();
            }
        }

        public bool ThemLopHoc(LopHocDTO lophoc)
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.ThemLopHoc(lophoc);
            }
        }

        //Get:SuaLopHoc
        public ActionResult Edit(int id)
        {
            ViewData["khoa"] = new SelectList(LayDanhSachKhoaDaoTao(), "ID", "TenKhoaDaoTao");
            LopHocDTO lophoc = LayLopHoc(id);
            return View(lophoc);
        }

        //Post:SuaLopHoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LopHocDTO lophoc)
        {
            var id = LayLopHocDaTonTai(lophoc.TenLop);
            if (id == lophoc.ID || id == 0)
            {
                SuaLopHoc(lophoc);
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Message = "Lớp Học đã tồn tại!!";
                ViewData["khoa"] = new SelectList(LayDanhSachKhoaDaoTao(), "ID", "TenKhoaDaoTao");
                return View();
            }
        }

        public LopHocDTO LayLopHoc(int id)
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.LayLopHoc(id);
            }
        }

        public bool SuaLopHoc(LopHocDTO lophoc)
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.SuaLopHoc(lophoc);
            }
        }

        //XoaLopHoc
        public async Task<ActionResult> Delete(int id)
        {
            var findlophoc = CheckLoiLopHocDaTonTai(id);
            if (findlophoc > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.khoadaotao = LayDanhSachKhoaDaoTao();
                return RedirectToAction("Index");

            }
            else
            {
                var output = XoaLopHoc(id);
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

        public bool XoaLopHoc(int id)
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.XoaLopHoc(id);
            }
        }

        public int CheckLoiLopHocDaTonTai(int? id)
        {
            using(LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.CheckLoiLopHocDaTonTai(id);
            }
        }

        //XemChiTietLopHoc
        public ActionResult Details(int id)
        {
            var lophoc = LayLopHoc(id);
            ViewBag.khoadaotao = LayDanhSachKhoaDaoTao();
            return View(lophoc);
        }
    }
}