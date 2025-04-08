using System;
using Gtk;
using Pango;
using System.Collections.Generic;

public class VisualizarFacturas : Window
{
    private ComboBoxText comboOrden;
    private TreeView tree;
    private ListStore listStore;

    private ArbolB arbolFacturas = ArbolB.ObtenerInstancia(); // Asegúrate que esta instancia es la misma usada para insertar

    public VisualizarFacturas() : base("Visualizar Facturas")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("Visualizar Facturas");
        label1.ModifyFont(FontDescription.FromString("Arial 20"));
        fix.Put(label1, 300, 50);

        Label labelOrden = new Label("Tipo de Orden:");
        fix.Put(labelOrden, 100, 105);

        comboOrden = new ComboBoxText();
        comboOrden.AppendText("InOrden");
        comboOrden.AppendText("PreOrden");
        comboOrden.AppendText("PostOrden");
        comboOrden.Active = 0;
        fix.Put(comboOrden, 200, 100);

        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(100, 40);
        fix.Put(btnVer, 450, 95);
        btnVer.Clicked += (sender, e) =>
        {
            MostrarFacturas();
        };

        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 500, 40);
        buttonVolver.Clicked += (sender, e) =>
        {
            VisualizarMain principalus = new VisualizarMain();
            principalus.ShowAll();
            this.Hide();
        };

        ScrolledWindow scrollTree = new ScrolledWindow();
        scrollTree.SetSizeRequest(600, 250);
        scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        tree = new TreeView();
        listStore = new ListStore(typeof(string), typeof(string), typeof(string));
        tree.Model = listStore;

        tree.AppendColumn("ID", new CellRendererText(), "text", 0);
        tree.AppendColumn("Orden", new CellRendererText(), "text", 1);
        tree.AppendColumn("Total", new CellRendererText(), "text", 2);

        scrollTree.Add(tree);
        fix.Put(scrollTree, 100, 200);

        Add(fix);
        ShowAll();
    }

    private void MostrarFacturas()
    {
        listStore.Clear();
        string tipo = comboOrden.ActiveText;

        List<Factura> facturas = new List<Factura>();

        if (tipo == "InOrden")
        {
            facturas = arbolFacturas.RecorridoInOrden(); // Este método debe devolver List<Factura>
        }
        else if (tipo == "PreOrden")
        {
            facturas = arbolFacturas.RecorridoPreOrden();
        }
        else if (tipo == "PostOrden")
        {
            facturas = arbolFacturas.RecorridoPostOrden();
        }

        foreach (var factura in facturas)
        {
            listStore.AppendValues(
                factura.Id_Factura.ToString(),
                tipo,
                "Q" + factura.Total.ToString("F2")
            );
        }
    }
}
