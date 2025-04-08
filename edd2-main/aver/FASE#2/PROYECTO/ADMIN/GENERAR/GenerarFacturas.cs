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

        Label lblId = new Label("ID:");
        lblId.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(lblId, 50, 50);

        Label lblIdCliente = new Label("ID Cliente:");
        lblIdCliente.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(lblIdCliente, 50, 100);

        Label lblIdServicio = new Label("ID Servicio:");
        lblIdServicio.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(lblIdServicio, 50, 150);

        Label lblTotal = new Label("Total:");
        lblTotal.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(lblTotal, 50, 200);

        // ============================ ENTRADAS ============================
        Entry txtId = new Entry();
        txtId.SetSizeRequest(200, 30);
        fix.Put(txtId, 200, 50);

        Entry txtIdCliente = new Entry();
        txtIdCliente.SetSizeRequest(200, 30);
        fix.Put(txtIdCliente, 200, 100);

        Entry txtIdServicio = new Entry();
        txtIdServicio.SetSizeRequest(200, 30);
        fix.Put(txtIdServicio, 200, 150);

        Entry txtTotal = new Entry();
        txtTotal.SetSizeRequest(200, 30);
        fix.Put(txtTotal, 200, 200);

        // ============================ BOTONES ============================

        Button btnVer = new Button("Ver");
        btnVer.SetSizeRequest(80, 50);
        fix.Put(btnVer, 50, 300);

        btnVer.Clicked += (sender, e) => {
            // Obtener el ID ingresado por el usuario
            int idFactura;
            if (!int.TryParse(txtId.Text, out idFactura))
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
}