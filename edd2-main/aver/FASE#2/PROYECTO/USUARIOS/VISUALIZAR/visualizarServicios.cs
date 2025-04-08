using System;
using Gtk;
using Pango;

class VisualizarServicios : Window
{
    public VisualizarServicios() : base("Visualizar Servicios")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        //============================LABELS==============================
        Label label1 = new Label("Visualizar Servicios");
        label1.ModifyFont(FontDescription.FromString("Arial 20"));
        fix.Put(label1, 300, 50);

        //============================ComboBox==============================
        ComboBoxText comboBox = new ComboBoxText();
        comboBox.AppendText("PRE-ORDEN");
        comboBox.AppendText("POST-ORDEN");
        comboBox.AppendText("IN-ORDEN");
        fix.Put(comboBox, 300, 100);

        //============================Botón==============================
        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(100, 40);
        fix.Put(btnVer, 450, 95);
        btnVer.Clicked += (sender, e) =>
        {
            Console.WriteLine($"Orden seleccionado: {comboBox.ActiveText}");
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
        tree.AppendColumn("Repuestos", new CellRendererText(), "text", 1);
        tree.AppendColumn("Vehículos", new CellRendererText(), "text", 2);
        tree.AppendColumn("Detalles", new CellRendererText(), "text", 3);
        tree.AppendColumn("Costo", new CellRendererText(), "text", 4);

        listStore.AppendValues("1", "Llanta", "Toyota Corolla", "Cambio de llanta", "100");
        listStore.AppendValues("2", "Batería", "Honda Civic", "Reemplazo de batería", "150");
        listStore.AppendValues("3", "Aceite", "Ford Focus", "Cambio de aceite", "50");

        scrollTree.Add(tree);
        fix.Put(scrollTree, 100, 200);

        Add(fix);
        ShowAll();
    }

    
}
