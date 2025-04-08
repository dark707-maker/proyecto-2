using System;
using Gtk;
using Pango;
using System.Collections.Generic;

class VisualizarServicios : Window
{
    private ArbolServicios arbolServicios = ArbolServicios.ObtenerInstancia(); // Instancia del árbol de servicios

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
        comboBox.AppendText("IN-ORDEN");
        comboBox.AppendText("POST-ORDEN");
        fix.Put(comboBox, 300, 100);

        //============================TreeView en ScrolledWindow==============================
        ScrolledWindow scrollTree = new ScrolledWindow();
        scrollTree.SetSizeRequest(600, 250);
        scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        TreeView tree = new TreeView();
        ListStore listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
        tree.Model = listStore;

        tree.AppendColumn("ID", new CellRendererText(), "text", 0);
        tree.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
        tree.AppendColumn("Vehículo", new CellRendererText(), "text", 2);
        tree.AppendColumn("Detalles", new CellRendererText(), "text", 3);
        tree.AppendColumn("Costo", new CellRendererText(), "text", 4);

        scrollTree.Add(tree);
        fix.Put(scrollTree, 100, 200);

        //============================Botón==============================
        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(100, 40);
        fix.Put(btnVer, 450, 95);

        btnVer.Clicked += (sender, e) =>
        {
            string opcion = comboBox.ActiveText; // Obtener la opción seleccionada
            if (string.IsNullOrEmpty(opcion))
            {
                Console.WriteLine("Por favor, selecciona un orden para visualizar.");
                return;
            }

            // Limpiar el modelo de datos del TreeView
            listStore.Clear();

            // Lista para almacenar los datos del recorrido
            List<NodoServicio> servicios = new List<NodoServicio>();

            // Realizar el recorrido según la opción seleccionada
            switch (opcion)
            {
                case "PRE-ORDEN":
                    arbolServicios.PreOrden(servicios);
                    break;
                case "IN-ORDEN":
                    arbolServicios.InOrden(servicios);
                    break;
                case "POST-ORDEN":
                    arbolServicios.PostOrden(servicios);
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    return;
            }

            // Agregar los datos al TreeView
            foreach (var servicio in servicios)
            {
                listStore.AppendValues(
                    servicio.Id.ToString(),
                    servicio.Id_Repuesto.ToString(),
                    servicio.Id_Vehiculo.ToString(),
                    servicio.Detalles,
                    servicio.Costo.ToString("F2")
                );
            }
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

        Add(fix);
        ShowAll();
    }
}