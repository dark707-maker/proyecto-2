using System;
using Gtk;
using Pango;

class GeneracionFacturas : Gtk.Window
{
    public GeneracionFacturas() : base("FACTURAS")
    {
        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        // ============================ LABELS ============================

        Label titulo = new Label("GENERAR FACTURAS");
        titulo.ModifyFont(FontDescription.FromString("Arial 26"));
        fix.Put(titulo, 200, 10);

        Label id = new Label("ID:");
        id.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(id, 50, 50);

        Label idRepuesto = new Label("ID Cliente:");
        idRepuesto.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(idRepuesto, 50, 100);

        Label idRepuesto = new Label("ID Servicio:");
        idRepuesto.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(idRepuesto, 50, 100);

        Label idVehiculo = new Label("Total:");
        idVehiculo.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(idVehiculo, 50, 150);

        // ============================ ENTRADAS ============================
        Entry id = new Entry();
        id.SetSizeRequest(200, 30);
        fix.Put(id, 200, 50);

        Entry idCliente = new Entry();
        idCliente.SetSizeRequest(200, 30);
        fix.Put(idCliente, 200, 100);

        Entry idServicio = new Entry();
        idServicio.SetSizeRequest(200, 30);
        fix.Put(idServicio, 200, 150);

        Entry total = new Entry();
        total.SetSizeRequest(200, 30);
        fix.Put(total, 200, 200);


        // ============================ BOTONES ============================

        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(80, 50);
        fix.Put(btnVer, 50, 300);

        btnVer.Clicked += (sender, e) => {
    // Obtener el ID ingresado por el usuario
        int idFactura;
        if (!int.TryParse(id.Text, out idFactura))
        {
            Console.WriteLine("Por favor, ingrese un ID válido.");
            return;
        }

        // Buscar la factura en el árbol B
        ArbolB arbolFacturas = ArbolB.ObtenerInstancia();
        Elemento factura = arbolFacturas.Buscar(idFactura);

        if (factura == null)
        {
            // Mostrar mensaje si no se encuentra la factura
            Console.WriteLine($"No se encontró una factura con el ID {idFactura}.");
        }
        else
        {
            // Mostrar los datos de la factura en la consola
            Console.WriteLine("Factura encontrada:");
            Console.WriteLine($"ID: {factura.Id}");
            Console.WriteLine($"ID Cliente: {factura.Id_Cliente}");
            Console.WriteLine($"ID Servicio: {factura.Id_Servicio}");
            Console.WriteLine($"Total: {factura.Total:C}");
        }
    };
        Button btnVolver = new Button("Volver");
        btnVolver.SetSizeRequest(80, 50);
        fix.Put(btnVolver, 500, 40);

        // Event handler for btnVolver
        btnVolver.Clicked += (sender, e) => {
            generarmain ventana2 = new generarmain(); 
            ventana2.ShowAll();
            this.Hide();
        };

        // Agregar contenedor a la ventana
        Add(fix);
        ShowAll();
    }

    // Main method to start the application
    
}
