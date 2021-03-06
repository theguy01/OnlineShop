﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyModel.EF;
using MyModel.DAO;
using OnlineShop.Common;
using MyTools;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.ParentCategory);
            return View(categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescription,Status,ShowOnHome")] Category category)
        {
            if (ModelState.IsValid)
            {
                //Set CreateDate,CreateBy
                category.CreatedDate = DateTime.Now;
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                category.CreatedBy = session.UserName;

                //Set MetaTitle
                category.MetaTitle = category.Name.ToLower().ConvertToUnSign();
                if (new CategoryDao().Insert(category)>0)
                {
                    return RedirectToAction("Index");
                }               
            }

            SetViewBag();
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentID = new SelectList(db.Categories, "ID", "Name", category.ParentID);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescription,Status,ShowOnHome")] Category category)
        {
            if (ModelState.IsValid)
            {
                //Set ModifiedDate,ModifiedBy
                category.ModifiedDate = DateTime.Now;
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                category.ModifiedBy = session.UserName;

                //Set MetaTitle
                category.MetaTitle = category.Name.ToLower().ConvertToUnSign();
                if (new CategoryDao().Update(category))
                {
                    return RedirectToAction("Index");
                }              
            }
            ViewBag.ParentID = new SelectList(db.Categories, "ID", "Name", category.ParentID);
            return View(category);
        }


        // POST: Admin/Categories/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            if (!new CategoryDao().Delete(id))
            {
                ModelState.AddModelError("", "Xóa danh mục thất bại");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.ParentID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);

        }
    }
}
