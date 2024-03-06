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
    public class TransaccionesController : Controller
    {
        private SistemaInventarioDbContext db = new SistemaInventarioDbContext();

        [Authorize(Roles = "Administrador,Vendedor")]
        // GET: Transacciones
        public ActionResult Index(string Criterio = null)
        {
            var transacciones = db.Transacciones.Include(t => t.Articulos);
            transacciones = transacciones.Where(p => Criterio == null ||
            p.TipoTrasaccion.Contains(Criterio) ||
            p.Articulos.Descripcion.Contains(Criterio) ||
            p.Fecha.ToString().Contains(Criterio) ||
            p.Cantidad.ToString().Contains(Criterio) ||
            p.Monto.ToString().Contains(Criterio));
            return View(transacciones.ToList());
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        // GET: Transacciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transacciones transacciones = db.Transacciones.Find(id);
            if (transacciones == null)
            {
                return HttpNotFound();
            }
            return View(transacciones);
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        // GET: Transacciones/Create
        public ActionResult Create()
        {
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion");
            return View();
        }

        // POST: Transacciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoTrasaccion,IdArticulo,Fecha,Cantidad,Monto")] Transacciones transacciones)
        {
            if (ModelState.IsValid)
            {
                db.Transacciones.Add(transacciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion", transacciones.IdArticulo);
            return View(transacciones);
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        // GET: Transacciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transacciones transacciones = db.Transacciones.Find(id);
            if (transacciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion", transacciones.IdArticulo);
            return View(transacciones);
        }

        // POST: Transacciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoTrasaccion,IdArticulo,Fecha,Cantidad,Monto")] Transacciones transacciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transacciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdArticulo = new SelectList(db.Articulos, "Id", "Descripcion", transacciones.IdArticulo);
            return View(transacciones);
        }


        [Authorize(Roles = "Administrador")]
        // GET: Transacciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transacciones transacciones = db.Transacciones.Find(id);
            if (transacciones == null)
            {
                return HttpNotFound();
            }
            return View(transacciones);
        }

        // POST: Transacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transacciones transacciones = db.Transacciones.Find(id);
            db.Transacciones.Remove(transacciones);
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
            string filename = "Transacciones.csv";
            string filepath = @"c:\Transacciones1\" + filename;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filepath);
            sw.WriteLine("sep=,"); //Separador en Excel 
            sw.WriteLine("Tipo Transaccion, Fecha, Cantidad, Monto"); //Encabezado 
            foreach (var i in db.Transacciones.ToList())
            {
                sw.WriteLine(i.Articulos + "," + i.TipoTrasaccion + "," + i.Fecha + "," + i.Cantidad + "," + i.Monto);
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
