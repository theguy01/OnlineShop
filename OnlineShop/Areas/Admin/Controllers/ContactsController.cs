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

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ContactsController : BaseController
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        // GET: Admin/Contacts
        public ActionResult Index()
        {
            var model = new ContactDao().ListAll();
            return View(model);
        }

        // GET: Admin/Contacts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Admin/Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Content,Status")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (new ContactDao().Insert(contact)>0)
                {
                    SetAlert("Thêm mới contact thành công", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CreateFailed", "Thêm mới contact thất bại");
                }
                
            }
            return View(contact);
        }

        // GET: Admin/Contacts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Admin/Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Content,Status")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (new ContactDao().Update(contact))
                {
                    SetAlert("Cập nhật contact thành công", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("UpdateFailed", "Cập nhật contact thất bại");
                }
                
            }
            return View(contact);
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            if (!new ContactDao().Delete(id))
            {
                ModelState.AddModelError("DeleteFailed", "Xóa contact thất bại");
            }
            else
            {
                SetAlert("Xóa contact thành công", AlertType.Success);
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
    }
}
