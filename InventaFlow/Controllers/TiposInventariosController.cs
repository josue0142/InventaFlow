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
    public class TiposInventariosController : Controller
    {
        private SistemaInventarioDbContext db = new SistemaInventarioDbContext();

        [Authorize(Roles = "Administrador,Almacenista")]
        // GET: TiposInventarios
        public ActionResult Index(string Criterio = null)
        {
            return View(db.TipoInventarios.Where(p => Criterio == null ||  
            p.Descripcion.Contains(Criterio)).ToList());
        }

        [Authorize(Roles = "Administrador,Almacenista")]
        // GET: TiposInventarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposInventarios tiposInventarios = db.TipoInventarios.Find(id);
            if (tiposInventarios == null)
            {
                return HttpNotFound();
            }
            return View(tiposInventarios);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TiposInventarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposInventarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado")] TiposInventarios tiposInventarios)
        {
            if (ModelState.IsValid)
            {
                db.TipoInventarios.Add(tiposInventarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposInventarios);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TiposInventarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposInventarios tiposInventarios = db.TipoInventarios.Find(id);
            if (tiposInventarios == null)
            {
                return HttpNotFound();
            }
            return View(tiposInventarios);
        }

        // POST: TiposInventarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado")] TiposInventarios tiposInventarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposInventarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposInventarios);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TiposInventarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposInventarios tiposInventarios = db.TipoInventarios.Find(id);
            if (tiposInventarios == null)
            {
                return HttpNotFound();
            }
            return View(tiposInventarios);
        }

        // POST: TiposInventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposInventarios tiposInventarios = db.TipoInventarios.Find(id);
            db.TipoInventarios.Remove(tiposInventarios);
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
            string filename = "TiposInventarios.csv";
            string filepath = @"c:\TipoInventario1\" + filename;
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine("sep=,"); //Separador en Excel 
            sw.WriteLine("Descripcion, Estado"); //Encabezado 
            foreach (var i in db.TipoInventarios.ToList())
            {
                sw.WriteLine(i.Descripcion + "," + i.Estado);
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
