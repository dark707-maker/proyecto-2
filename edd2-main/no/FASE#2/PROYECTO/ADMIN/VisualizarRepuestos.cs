using System;
using Gtk;
using Pango;
using System.Collections.Generic;

class Visualizar : Gtk.Window
{
    private ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia(); // Instancia del árbol AVL

    public Visualizar() : base("VISUALIZAR REPUESTOS")
    {
        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        //===============================LABELS===============================
        Fixed fix = new Fixed();

        Label label1 = new Label("Visualizar Repuestos:");
        label1.ModifyFont(FontDescription.FromString("Arial 18"));
        fix.Put(label1, 240, 50);

        //===============================Combobox===============================
        ComboBoxText combobox = new ComboBoxText();
        combobox.AppendText("PRE-ORDEN");
        combobox.AppendText("IN-ORDEN");
        combobox.AppendText("POST-ORDEN");
        fix.Put(combobox, 300, 100);

        //===============================BOTONES===============================
        Button button1 = new Button("VISUALIZAR");
        button1.SetSizeRequest(100, 60);
        fix.Put(button1, 200, 200);

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 650, 40);
        button.Clicked += (sender, e) => 
        {
            Principaladmin manual = new Principaladmin();
            manual.ShowAll();
            this.Hide();
        };

        //===============================TABLA===============================
        TreeView treeView = new TreeView();

        // Crear el modelo de datos
        ListStore listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
        treeView.Model = listStore;

        // Agregar columnas
        treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeView.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
        treeView.AppendColumn("Detalles", new CellRendererText(), "text", 2);
        treeView.AppendColumn("Costo", new CellRendererText(), "text", 3);

        fix.Put(treeView, 100, 300);
        treeView.SetSizeRequest(500, 200); // Ajuste del tamaño del TreeView

        //===============================EVENTO VISUALIZAR===============================
        button1.Clicked += (sender, e) =>
        {
            string opcion = combobox.ActiveText; // Obtener la opción seleccionada
            if (string.IsNullOrEmpty(opcion))
            {
                Console.WriteLine("Por favor, selecciona un orden para visualizar.");
                return;
            }

            // Limpiar el modelo de datos del TreeView
            listStore.Clear();

            // Lista para almacenar los datos del recorrido
            List<ListaRepuesto> repuestos = new List<ListaRepuesto>();

            // Realizar el recorrido según la opción seleccionada
            switch (opcion)
            {
                case "PRE-ORDEN":
                    RecorridoPreOrden(arbolRepuestos.ObtenerRaiz(), repuestos);
                    break;
                case "IN-ORDEN":
                    RecorridoInOrden(arbolRepuestos.ObtenerRaiz(), repuestos);
                    break;
                case "POST-ORDEN":
                    RecorridoPostOrden(arbolRepuestos.ObtenerRaiz(), repuestos);
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    return;
            }

            // Agregar los datos al TreeView
            foreach (var repuesto in repuestos)
            {
                listStore.AppendValues(repuesto.Id.ToString(), repuesto.Repuesto, repuesto.Detalles, repuesto.Costo.ToString("F2"));
            }
        };

        Add(fix); 
        ShowAll();
    }

    // Métodos para los recorridos del árbol AVL
    private void RecorridoPreOrden(NodoAVL nodo, List<ListaRepuesto> repuestos)
    {
        if (nodo == null) return;

        repuestos.Add(nodo.Objeto); // Agregar el nodo actual
        RecorridoPreOrden(nodo.Izquierda, repuestos); // Recorrer subárbol izquierdo
        RecorridoPreOrden(nodo.Derecha, repuestos); // Recorrer subárbol derecho
    }

    private void RecorridoInOrden(NodoAVL nodo, List<ListaRepuesto> repuestos)
    {
        if (nodo == null) return;

        RecorridoInOrden(nodo.Izquierda, repuestos); // Recorrer subárbol izquierdo
        repuestos.Add(nodo.Objeto); // Agregar el nodo actual
        RecorridoInOrden(nodo.Derecha, repuestos); // Recorrer subárbol derecho
    }

    private void RecorridoPostOrden(NodoAVL nodo, List<ListaRepuesto> repuestos)
    {
        if (nodo == null) return;

        RecorridoPostOrden(nodo.Izquierda, repuestos); // Recorrer subárbol izquierdo
        RecorridoPostOrden(nodo.Derecha, repuestos); // Recorrer subárbol derecho
        repuestos.Add(nodo.Objeto); // Agregar el nodo actual
    }
}