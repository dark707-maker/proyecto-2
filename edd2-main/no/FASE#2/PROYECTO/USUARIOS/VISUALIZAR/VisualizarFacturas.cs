using System;
using Gtk;
using Pango;
using System.Collections.Generic;

public class VisualizarFacturas : Window
{
    private int idUsuarioActual; // ID del usuario actual
    private ArbolB arbolFacturas = ArbolB.ObtenerInstancia(); // Instancia del árbol B

    public VisualizarFacturas(int idUsuario) : base("Visualizar Facturas")
    {
        idUsuarioActual = idUsuario; // Guardar el ID del usuario actual

        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        //============================LABELS==============================
        Label label1 = new Label("Visualizar Facturas");
        label1.ModifyFont(FontDescription.FromString("Arial 20"));
        fix.Put(label1, 300, 50);

        //============================TreeView en ScrolledWindow==============================
        ScrolledWindow scrollTree = new ScrolledWindow();
        scrollTree.SetSizeRequest(600, 250);
        scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        TreeView tree = new TreeView();
        ListStore listStore = new ListStore(typeof(string), typeof(string), typeof(string));
        tree.Model = listStore;

        tree.AppendColumn("ID Factura", new CellRendererText(), "text", 0);
        tree.AppendColumn("ID Servicio", new CellRendererText(), "text", 1);
        tree.AppendColumn("Total", new CellRendererText(), "text", 2);

        scrollTree.Add(tree);
        fix.Put(scrollTree, 100, 200);

        //============================Botón Ver==============================
        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(100, 40);
        fix.Put(btnVer, 450, 95);

        btnVer.Clicked += (sender, e) =>
        {
            // Limpiar el modelo de datos del TreeView
            listStore.Clear();

            // Obtener las facturas del usuario actual
            List<Elemento> facturasUsuario = arbolFacturas.ObtenerFacturasPorUsuario(idUsuarioActual);

            // Agregar las facturas al TreeView
            foreach (var factura in facturasUsuario)
            {
                listStore.AppendValues(
                    factura.Id_Factura.ToString(),
                    factura.Id_Servicio.ToString(),
                    factura.Total.ToString("F2")
                );
            }

            if (facturasUsuario.Count == 0)
            {
                Console.WriteLine("No se encontraron facturas para este usuario.");
            }
        };

        //============================Botón Volver==============================
        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 500, 40);

        buttonVolver.Clicked += (sender, e) =>
        {
            VisualizarMain principalus = new VisualizarMain();
            principalus.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}