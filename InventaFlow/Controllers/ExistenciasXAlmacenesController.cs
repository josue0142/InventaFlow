using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaInventario.Models;

namespace SistemaInventario.Controllers
{
    public class ExistenciasXAlmacenesController : Controller
    {
        private SistemaInventarioDbContext db = new SistemaInventarioDbContext();

        [Authorize(Roles = "Administrador,Vendedor,Almacenista")]
        // GET: ExistenciasXAlmacenes
        public ActionResult Index(string Criterio = null)
        {
            return View(db.ExistenciaXAlmacenes.Include(e => e.Almacenes).Include(e => e.Articulos)
                .Where(p => Criterio == null ||
            p.Almacenes.Descripcion.Contains(Criterio) ||
            p.Articulos.Descripcion.Contains(Criterio)).ToList());
        }

        [Authorize(Roles = "Administrador,Vendedor,Almacenista")]
        // GET: ExistenciasXAlmacenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExistenciasXAlmacenes existenciasXAlmacenes = db.ExistenciaXAlmacenes.Find(id);
            if (existenciasXAlmacenes == null)
            {
                return HttpNotFound();
            }
            return View(existenciasXAlmacenes);
        }

        [Authorize(Roles = "Administrador,Almacenista")]
        // GET: ExistenciasXAlmacenes/Create
        public ActionResult Create()
        {
            ViewBag.IdAlmacen = new SelectList(db.Almacenes, "Id", "Descripcion");
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion");
            return View();
        }

        // POST: ExistenciasXAlmacenes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdAlmacen,IdArticulo,Cantidad")] ExistenciasXAlmacenes existenciasXAlmacenes)
        {
            if (ModelState.IsValid)
            {
                db.ExistenciaXAlmacenes.Add(existenciasXAlmacenes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAlmacen = new SelectList(db.Almacenes, "Id", "Descripcion", existenciasXAlmacenes.IdAlmacen);
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion", existenciasXAlmacenes.IdArticulo);
            return View(existenciasXAlmacenes);
        }

        [Authorize(Roles = "Administrador,Almacenista")]
        // GET: ExistenciasXAlmacenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExistenciasXAlmacenes existenciasXAlmacenes = db.ExistenciaXAlmacenes.Find(id);
            if (existenciasXAlmacenes == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlmacen = new SelectList(db.Almacenes, "Id", "Descripcion", existenciasXAlmacenes.IdAlmacen);
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion", existenciasXAlmacenes.IdArticulo);
            return View(existenciasXAlmacenes);
        }

        // POST: ExistenciasXAlmacenes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdAlmacen,IdArticulo,Cantidad")] ExistenciasXAlmacenes existenciasXAlmacenes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(existenciasXAlmacenes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlmacen = new SelectList(db.Almacenes, "Id", "Descripcion", existenciasXAlmacenes.IdAlmacen);
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion", existenciasXAlmacenes.IdArticulo);
            return View(existenciasXAlmacenes);
        }

        [Authorize(Roles = "Administrador")]
        // GET: ExistenciasXAlmacenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExistenciasXAlmacenes existenciasXAlmacenes = db.ExistenciaXAlmacenes.Find(id);
            if (existenciasXAlmacenes == null)
            {
                return HttpNotFound();
            }
            return View(existenciasXAlmacenes);
        }

        // POST: ExistenciasXAlmacenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExistenciasXAlmacenes existenciasXAlmacenes = db.ExistenciaXAlmacenes.Find(id);
            db.ExistenciaXAlmacenes.Remove(existenciasXAlmacenes);
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
            string filename = "ExistenciaXAlmacenes.csv";
            string filepath = @"c:\Inventario\ExistenciaXAlmacenes\" + filename;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filepath);
            sw.WriteLine("sep=,"); //Separador en Excel 
            sw.WriteLine("Almacenes, Articulos, Cantidad"); //Encabezado 
            foreach (var i in db.ExistenciaXAlmacenes.Include(e => e.Almacenes).Include(e => e.Articulos).ToList())
            {
                sw.WriteLine(i.Almacenes.Descripcion + "," + i.Articulos.Descripcion + "," + i.Cantidad);
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
