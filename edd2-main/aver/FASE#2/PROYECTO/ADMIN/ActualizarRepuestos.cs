using Gtk;
using System;
using Pango;

class Actualizar : Gtk.Window
{
    private ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia(); // Usar el método Singleton para obtener la instancia del árbol AVL

    public Actualizar() : base(" ACTUALIZAR ")
    {
        SetDefaultSize(800, 700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("Actualizar Repuestos:");
        label1.ModifyFont(FontDescription.FromString("Arial 28"));
        fix.Put(label1, 240, 50);

        Label label2 = new Label("ID:");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 190, 160);

        Label label3 = new Label("Repuesto:");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 170, 260);

        Label label4 = new Label("Detalles:");
        label4.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label4, 170, 360);

        Label label5 = new Label("Costo:");
        label5.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label5, 170, 460);

        // ============================== ENTRADAS ===============================
        Entry id = new Entry();
        id.SetSizeRequest(200, 30);
        fix.Put(id, 300, 160);

        Entry repuesto = new Entry();
        repuesto.SetSizeRequest(180, 30);
        fix.Put(repuesto, 300, 260);

        Entry detalles = new Entry();
        detalles.SetSizeRequest(180, 30);
        fix.Put(detalles, 300, 360);

        Entry costo = new Entry();
        costo.SetSizeRequest(180, 30);
        fix.Put(costo, 500, 460);

        // ============================== BOTONES ================================
        Button buttonBuscar = new Button("Buscar");
        buttonBuscar.SetSizeRequest(100, 60);
        fix.Put(buttonBuscar, 500, 150);

        Button buttonActualizar = new Button("Actualizar");
        buttonActualizar.SetSizeRequest(100, 60);
        fix.Put(buttonActualizar, 350, 550);

        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 650, 40);

        // ============================== EVENTOS ================================
        buttonBuscar.Clicked += (sender, e) =>
{
    if (int.TryParse(id.Text, out int repuestoId))
    {
        NodoAVL nodo = arbolRepuestos.BuscarPorId(repuestoId);
        if (nodo != null)
        {
            // Verificar valores nulos antes de asignarlos a los campos de entrada
            repuesto.Text = nodo.Objeto.Repuesto ?? ""; // Cambiado de Nombre a Repuesto
            detalles.Text = nodo.Objeto.Detalles ?? "";
            costo.Text = nodo.Objeto.Costo.ToString() ?? "";

            Console.WriteLine("Repuesto encontrado.");
        }
        else
        {
            Console.WriteLine("Repuesto no encontrado.");
        }
    }
    else
    {
        Console.WriteLine("ID inválido.");
    }
};

        buttonActualizar.Clicked += (sender, e) =>
{
    if (int.TryParse(id.Text, out int repuestoId) && double.TryParse(costo.Text, out double nuevoCosto))
    {
        bool actualizado = arbolRepuestos.Actualizar(repuestoId, repuesto.Text, detalles.Text, nuevoCosto);
        if (actualizado)
        {
            Console.WriteLine("Repuesto actualizado correctamente.");
        }
        else
        {
            Console.WriteLine("No se pudo actualizar el repuesto. Verifique el ID.");
        }
    }
    else
    {
        Console.WriteLine("Datos inválidos.");
    }
};

        buttonVolver.Clicked += (sender, e) =>
        {
            Principaladmin manual = new Principaladmin();
            manual.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}
