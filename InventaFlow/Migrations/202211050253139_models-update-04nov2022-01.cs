namespace SistemaInventario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelsupdate04nov202201 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Almacenes", "Descripcion", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Articulos", "Descripcion", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TiposInventarios", "Descripcion", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Transacciones", "TipoTrasaccion", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.ExistenciasXAlmacenes", "IdAlmacen");
            CreateIndex("dbo.ExistenciasXAlmacenes", "IdArticulo");
            CreateIndex("dbo.Articulos", "IdTipoInventario");
            CreateIndex("dbo.Transacciones", "IdArticulo");
            AddForeignKey("dbo.ExistenciasXAlmacenes", "IdArticulo", "dbo.Articulos", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Articulos", "IdTipoInventario", "dbo.TiposInventarios", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transacciones", "IdArticulo", "dbo.Articulos", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExistenciasXAlmacenes", "IdAlmacen", "dbo.Almacenes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            /*DropForeignKey("dbo.ExistenciasXAlmacenes", "IdAlmacen", "dbo.Almacenes");
            DropForeignKey("dbo.Transacciones", "IdArticulo", "dbo.Articulos");
            DropForeignKey("dbo.Articulos", "IdTipoInventario", "dbo.TiposInventarios");
            DropForeignKey("dbo.ExistenciasXAlmacenes", "IdArticulo", "dbo.Articulos");
            DropIndex("dbo.Transacciones", new[] { "IdArticulo" });
            DropIndex("dbo.Articulos", new[] { "IdTipoInventario" });
            DropIndex("dbo.ExistenciasXAlmacenes", new[] { "IdArticulo" });
            DropIndex("dbo.ExistenciasXAlmacenes", new[] { "IdAlmacen" });
            AlterColumn("dbo.Transacciones", "TipoTrasaccion", c => c.String());
            AlterColumn("dbo.TiposInventarios", "Descripcion", c => c.String());
            AlterColumn("dbo.Articulos", "Descripcion", c => c.String());
            AlterColumn("dbo.Almacenes", "Descripcion", c => c.String());*/
        }
    }
}
