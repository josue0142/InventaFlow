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
    public class ArticulosController : Controller
    {
        private SistemaInventarioDbContext db = new SistemaInventarioDbContext();

        [Authorize(Roles = "Administrador,Vendedor,Almacenista")]
        // GET: Articulos
        public ActionResult Index(string Criterio = null)
        {
            return View(db.Articulos.Include(a => a.TiposInventarios)
            .Where(p => Criterio == null ||
            p.Descripcion.Contains(Criterio) ||
            p.TiposInventarios.Descripcion.Contains(Criterio) ||
            p.CostoUnitario.ToString().StartsWith(Criterio)).ToList());
        }

        [Authorize(Roles = "Administrador,Vendedor,Almacenista")]
        // GET: Articulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos articulos = db.Articulos.Find(id);
            if (articulos == null)
            {
                return HttpNotFound();
            }
            return View(articulos);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Articulos/Create
        public ActionResult Create()
        {
            ViewBag.IdTipoInventario = new SelectList(db.TipoInventarios, "Id", "Descripcion");
            return View();
        }

        // POST: Articulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Existencia,IdTipoInventario,Estado,CostoUnitario")] Articulos articulos)
        {
            if (ModelState.IsValid)
            {
                db.Articulos.Add(articulos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTipoInventario = new SelectList(db.TipoInventarios, "Id", "Descripcion", articulos.IdTipoInventario);
            return View(articulos);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Articulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos articulos = db.Articulos.Find(id);
            if (articulos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTipoInventario = new SelectList(db.TipoInventarios, "Id", "Descripcion", articulos.IdTipoInventario);
            return View(articulos);
        }

        // POST: Articulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Existencia,IdTipoInventario,Estado,CostoUnitario")] Articulos articulos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articulos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTipoInventario = new SelectList(db.TipoInventarios, "Id", "Descripcion", articulos.IdTipoInventario);
            return View(articulos);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Articulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos articulos = db.Articulos.Find(id);
            if (articulos == null)
            {
                return HttpNotFound();
            }
            return View(articulos);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articulos articulos = db.Articulos.Find(id);
            db.Articulos.Remove(articulos);
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
            string filename = "Articulos.csv";
            string filepath = @"c:\Art1\" + filename;
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine("sep=,"); //Separador en Excel 
            sw.WriteLine("Tipos Inventarios,Descripcion,Existencia,Costo Unitario"); //Encabezado 
            foreach (var i in db.Articulos.ToList())
            {
                sw.WriteLine(i.TiposInventarios + "," + i.Descripcion + "," + i.Existencia + "," + i.CostoUnitario);
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
