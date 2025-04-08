using System;
using Gtk;
using Pango;

class GenerarServicios : Gtk.Window {

    ArbolServicios arbolServicios = ArbolServicios.ObtenerInstancia();
     ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();
    ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia();

    ArbolB arbolFacturas = ArbolB.ObtenerInstancia(); // Instancia del árbol B para facturas

    public GenerarServicios() : base("GENERAR SERVICIOS") {
        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Label titulo = new Label("GENERAR SERVICIOS");
        titulo.ModifyFont(Pango.FontDescription.FromString("Arial 26"));
        Fixed fix = new Fixed();

        //================================LABELS==========================
        Label id = new Label("ID:");
        id.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(id, 50, 50);

        Label idrepuesto = new Label("ID Repuesto:");
        idrepuesto.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(idrepuesto, 50, 100);

        Label idvehiculo = new Label("ID Vehiculo:");
        idvehiculo.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(idvehiculo, 50, 150);

        Label detalles = new Label("Detalles:");
        detalles.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(detalles, 50, 200);

        Label costo = new Label("Costo:");
        costo.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(costo, 50, 250);

        //================================ENTRIES==========================
        Entry entryId = new Entry();
        fix.Put(entryId, 200, 50);

        Entry entryIdRepuesto = new Entry();
        fix.Put(entryIdRepuesto, 200, 100);

        Entry entryIdVehiculo = new Entry();
        fix.Put(entryIdVehiculo, 200, 150);

        Entry entryDetalles = new Entry();
        fix.Put(entryDetalles, 200, 200);

        Entry entryCosto = new Entry();
        fix.Put(entryCosto, 200, 250);

        //================================BOTONES==========================
        Button guardar = new Button("Guardar");
        guardar.SetSizeRequest(80, 50);
        fix.Put(guardar, 50, 300);

            guardar.Clicked += (sender, e) => {
    try {
        int id = int.Parse(entryId.Text);
        int idRepuesto = int.Parse(entryIdRepuesto.Text);
        int idVehiculo = int.Parse(entryIdVehiculo.Text);
        string detalles = entryDetalles.Text;
        double costo = double.Parse(entryCosto.Text);

        // Validar si el ID del vehículo existe
        if (!listaVehiculos.ExisteId(idVehiculo)) {
            Console.WriteLine($"Error: El ID del vehículo {idVehiculo} no existe.");
            return;
        }

        // Validar si el ID del repuesto existe
        if (arbolRepuestos.BuscarPorId(idRepuesto) == null) {
            Console.WriteLine($"Error: El ID del repuesto {idRepuesto} no existe.");
            return;
        }

        // Si ambos IDs existen, agregar el servicio
        arbolServicios.Agregar(id, idRepuesto, idVehiculo, detalles, costo);

        // Generar y almacenar la factura en el árbol B
        int idFactura = id; // Usar el ID del servicio como ID de la factura
        arbolFacturas.Insertar(idFactura, idVehiculo, idRepuesto, costo);

        // Limpiar los campos
        entryId.Text = "";
        entryIdRepuesto.Text = "";
        entryIdVehiculo.Text = "";
        entryDetalles.Text = "";
        entryCosto.Text = "";

        Console.WriteLine("Servicio y factura generados exitosamente.");
    } catch (Exception ex) {
        Console.WriteLine($"Error: {ex.Message}");
    }
};

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 500, 40);
        button.Clicked += (sender, e) => {
            generarmain ventana1 = new generarmain();
            ventana1.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}