using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ProTechTiveGear.Models;

namespace ProTechTiveGear.Controllers
{
    public class BlogsController : Controller
    {
        private ProTechTiveGearEntities db = new ProTechTiveGearEntities();

        // Check if user is admin
        private bool IsAdminLoggedIn()
        {
            return Session["Account"] != null;
        }

        // GET: Blogs - Admin Management View with Search and Filter
        public ActionResult Index(string searchString, DateTime? fromDate, DateTime? toDate, string sortOrder)
        {
            // Check admin authentication
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var blogs = from b in db.Blogs select b;

            // Search functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(b => b.Title.Contains(searchString) 
                                      || b.ShortTitle.Contains(searchString)
                                      || b.Describe.Contains(searchString));
            }

            // Date range filter
            if (fromDate.HasValue)
            {
                blogs = blogs.Where(b => b.DateImport >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                blogs = blogs.Where(b => b.DateImport <= toDate.Value);
            }

            // Sorting
            switch (sortOrder)
            {
                case "title_desc":
                    blogs = blogs.OrderByDescending(b => b.Title);
                    break;
                case "Date":
                    blogs = blogs.OrderBy(b => b.DateImport);
                    break;
                case "date_desc":
                    blogs = blogs.OrderByDescending(b => b.DateImport);
                    break;
                default:
                    blogs = blogs.OrderBy(b => b.Title);
                    break;
            }

            // Statistics for dashboard
            ViewBag.TotalBlogs = db.Blogs.Count();
            ViewBag.ThisMonthBlogs = db.Blogs.Count(b => b.DateImport.HasValue && 
                                                    b.DateImport.Value.Month == DateTime.Now.Month &&
                                                    b.DateImport.Value.Year == DateTime.Now.Year);
            ViewBag.TodayBlogs = db.Blogs.Count(b => b.DateImport.HasValue && 
                                                 DbFunctions.TruncateTime(b.DateImport.Value) == DbFunctions.TruncateTime(DateTime.Now));

            return View(blogs.ToList());
        }

        // GET: Blogs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        // POST: Blogs/Create with Image Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Picture,Title,DateImport,Describe,ShortTitle")] Blog blog, HttpPostedFileBase imageFile)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string newFileName = "blog_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    string path = Path.Combine(Server.MapPath("~/img/Item/"), newFileName);
                    
                    imageFile.SaveAs(path);
                    blog.Picture = newFileName;
                }

                // Set default date if not provided
                if (!blog.DateImport.HasValue)
                {
                    blog.DateImport = DateTime.Now;
                }

                db.Blogs.Add(blog);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Tin tức đã được tạo thành công!";
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5 with Image Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Picture,Title,DateImport,Describe,ShortTitle")] Blog blog, HttpPostedFileBase imageFile)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(blog.Picture))
                    {
                        string oldImagePath = Path.Combine(Server.MapPath("~/img/Item/"), blog.Picture);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string newFileName = "blog_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    string path = Path.Combine(Server.MapPath("~/img/Item/"), newFileName);
                    
                    imageFile.SaveAs(path);
                    blog.Picture = newFileName;
                }

                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Tin tức đã được cập nhật thành công!";
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Admin");
            }

            Blog blog = db.Blogs.Find(id);
            
            // Delete associated image
            if (!string.IsNullOrEmpty(blog.Picture))
            {
                string imagePath = Path.Combine(Server.MapPath("~/img/Item/"), blog.Picture);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            db.Blogs.Remove(blog);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Tin tức đã được xóa thành công!";
            return RedirectToAction("Index");
        }

        // AJAX: Quick Delete
        [HttpPost]
        public JsonResult QuickDelete(long id)
        {
            if (!IsAdminLoggedIn())
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                Blog blog = db.Blogs.Find(id);
                if (blog != null)
                {
                    // Delete associated image
                    if (!string.IsNullOrEmpty(blog.Picture))
                    {
                        string imagePath = Path.Combine(Server.MapPath("~/img/Item/"), blog.Picture);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    db.Blogs.Remove(blog);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Xóa thành công!" });
                }
                return Json(new { success = false, message = "Không tìm thấy tin tức!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
