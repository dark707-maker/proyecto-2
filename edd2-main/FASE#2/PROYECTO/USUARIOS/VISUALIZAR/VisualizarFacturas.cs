using System;
using Gtk;
using Pango;

public class VisualizarFacturas : Window
{
    public VisualizarFacturas() : base("Visualizar Facturas")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        //============================LABELS==============================
        Label label1 = new Label("Visualizar Facturas");
        label1.ModifyFont(FontDescription.FromString("Arial 20"));
        fix.Put(label1, 300, 50);


        //============================Botón==============================
        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(100, 40);
        fix.Put(btnVer, 450, 95);
        btnVer.Clicked += (sender, e) =>
        {
            
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

        //============================TreeView en ScrolledWindow==============================
        ScrolledWindow scrollTree = new ScrolledWindow();
        scrollTree.SetSizeRequest(600, 250);
        scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        TreeView tree = new TreeView();
        ListStore listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
        tree.Model = listStore;

        tree.AppendColumn("ID", new CellRendererText(), "text", 0);
        tree.AppendColumn("Orden", new CellRendererText(), "text", 1);
        tree.AppendColumn("Total", new CellRendererText(), "text", 2);
       

        listStore.AppendValues("1", "Llanta", "Toyota Corolla");
        listStore.AppendValues("2", "Batería", "Honda Civic");
        listStore.AppendValues("3", "Aceite", "Ford Focus");

        scrollTree.Add(tree);
        fix.Put(scrollTree, 100, 200);

        Add(fix);
        ShowAll();
    }

    
}
