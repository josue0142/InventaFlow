using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaInventario.Models;

namespace SistemaInventario.Controllers
{
    public class AlmacenesController : Controller
    {
        private SistemaInventarioDbContext db = new SistemaInventarioDbContext();

        [Authorize(Roles = "Administrador,Almacenista")]
        // GET: Almacenes
        public ActionResult Index(string Criterio = null)
        {
            return View(db.Almacenes.Where(p => Criterio == null || 
            p.Descripcion.Contains(Criterio)).ToList());
        }

        [Authorize(Roles = "Administrador,Almacenista")]
        // GET: Almacenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Almacenes almacenes = db.Almacenes.Find(id);
            if (almacenes == null)
            {
                return HttpNotFound();
            }
            return View(almacenes);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Almacenes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Almacenes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado")] Almacenes almacenes)
        {
            if (ModelState.IsValid)
            {
                db.Almacenes.Add(almacenes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(almacenes);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Almacenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Almacenes almacenes = db.Almacenes.Find(id);
            if (almacenes == null)
            {
                return HttpNotFound();
            }
            return View(almacenes);
        }

        // POST: Almacenes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado")] Almacenes almacenes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(almacenes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(almacenes);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Almacenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Almacenes almacenes = db.Almacenes.Find(id);
            if (almacenes == null)
            {
                return HttpNotFound();
            }
            return View(almacenes);
        }

        // POST: Almacenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Almacenes almacenes = db.Almacenes.Find(id);
            db.Almacenes.Remove(almacenes);
            db.SaveChanges();
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
        public ActionResult exportaExcel()
        {
            string filename = "Almacenes.csv";
            string filepath = @"c:\Inventario\Almacen\" + filename;
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine("sep=,"); //Separador en Excel 
            sw.WriteLine("Id,Almacenes"); //Encabezado 
            foreach (var i in db.Almacenes.ToList())
            {
                sw.WriteLine(i.Id + "," + i.Descripcion);
            }
            sw.Close();
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
    }
}
