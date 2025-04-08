using System;
using Gtk;
using Pango;

class GeneracionFacturas : Gtk.Window
{
    private Entry entryIdFactura;
    private Entry entryIdUsuario;
    private Entry entryIdServicio;
    private Entry entryTotal;

    private ListaUsuarios listaUsuarios = ListaUsuarios.ObtenerInstancia(); // Invocar el método correctamente
    private ArbolServicios arbolServicios = ArbolServicios.ObtenerInstancia();
    private ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia();
    private ArbolB arbolFacturas = ArbolB.ObtenerInstancia(); // Instancia del árbol B para facturas

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

        Label idFacturaLabel = new Label("ID Factura:");
        idFacturaLabel.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(idFacturaLabel, 50, 50);

        Label idUsuarioLabel = new Label("ID Usuario:");
        idUsuarioLabel.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(idUsuarioLabel, 50, 100);

        Label idServicioLabel = new Label("ID Servicio:");
        idServicioLabel.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(idServicioLabel, 50, 150);

        Label totalLabel = new Label("Total:");
        totalLabel.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(totalLabel, 50, 200);

        // ============================ ENTRIES ============================

        entryIdFactura = new Entry();
        fix.Put(entryIdFactura, 200, 50);

        entryIdUsuario = new Entry();
        fix.Put(entryIdUsuario, 200, 100);

        entryIdServicio = new Entry();
        fix.Put(entryIdServicio, 200, 150);

        entryTotal = new Entry();
        entryTotal.Sensitive = false; // El total no se puede editar manualmente
        fix.Put(entryTotal, 200, 200);

        // ============================ BOTONES ============================

        Button btnGenerar = new Button(" VER ");
        btnGenerar.SetSizeRequest(80, 50);
        fix.Put(btnGenerar, 50, 300);

        btnGenerar.Clicked += (sender, e) => BuscarFactura();

        Button btnVolver = new Button("Volver");
        btnVolver.SetSizeRequest(80, 50);
        fix.Put(btnVolver, 500, 40);

        btnVolver.Clicked += (sender, e) => {
            generarmain ventana2 = new generarmain(); // Assuming MainWindow is the previous window class
            ventana2.ShowAll();
            this.Hide();
        };

        // Agregar contenedor a la ventana
        Add(fix);
        ShowAll();
    }

        private void BuscarFactura()
{
    try
    {
        // Obtener el ID de la factura desde el Entry
        int idFactura = int.Parse(entryIdFactura.Text);

        // Buscar la factura en el árbol B
        var factura = arbolFacturas.Buscar(idFactura);
        if (factura == null)
        {
            Console.WriteLine($"Error: La factura con ID {idFactura} no existe.");
            return;
        }

        // Rellenar los campos con los datos de la factura
        entryIdUsuario.Text = factura.Id_Usuario.ToString();
        entryIdServicio.Text = factura.Id_Servicio.ToString();
        entryTotal.Text = factura.Total.ToString();

        Console.WriteLine("Factura encontrada y datos cargados.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

   private void GenerarFactura()
{
    try
    {
        // Obtener datos de los entries
        int idFactura = int.Parse(entryIdFactura.Text);
        int idUsuario = int.Parse(entryIdUsuario.Text);
        int idServicio = int.Parse(entryIdServicio.Text);

        // Validar que el ID de usuario exista
        if (listaUsuarios.ObtenerUsuarioPorId(idUsuario) == null)
        {
            Console.WriteLine($"Error: El ID de usuario {idUsuario} no existe.");
            return;
        }

        // Validar que el ID de servicio exista
        var servicio = arbolServicios.BuscarPorId(idServicio);
        if (servicio == null)
        {
            Console.WriteLine($"Error: El ID de servicio {idServicio} no existe.");
            return;
        }

        // Validar que el ID de repuesto asociado al servicio exista
        var repuesto = arbolRepuestos.BuscarPorId(servicio.Id_Repuesto);
        if (repuesto == null)
        {
            Console.WriteLine($"Error: El ID del repuesto asociado al servicio no existe.");
            return;
        }

        // Calcular el total (costo del servicio + costo del repuesto)
        double total = servicio.Costo + repuesto.Objeto.Costo;

        // Verificar los valores de costo
        Console.WriteLine($"Costo del servicio: {servicio.Costo}");
        Console.WriteLine($"Costo del repuesto: {repuesto.Objeto.Costo}");
        Console.WriteLine($"Total calculado: {total}");

        // Mostrar el total en el Entry correspondiente
        entryTotal.Text = total.ToString();

        // Insertar la factura en el árbol B
        arbolFacturas.Insertar(idFactura, idUsuario, idServicio, total);
        Console.WriteLine("Factura generada exitosamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
}