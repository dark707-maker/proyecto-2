using System;
using Gtk;
using Pango;

class InsertarVehiculos : Gtk.Window
{
    private int idUsuarioActual; // ID del usuario actual

    public InsertarVehiculos(int idUsuario) : base("INSERTAR VEHÍCULOS")
    {
        idUsuarioActual = idUsuario; // Guardar el ID del usuario actual

        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        // ================================ LABELS ==============================
        Label titulo = new Label("INSERTAR VEHÍCULOS");
        titulo.ModifyFont(Pango.FontDescription.FromString("Arial 26"));
        fix.Put(titulo, 50, 10);

        Label id = new Label("ID:");
        id.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(id, 50, 50);

        Label marca = new Label("Marca:");
        marca.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(marca, 50, 100);

        Label modelo = new Label("Modelo:");
        modelo.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(modelo, 50, 150);

        Label placa = new Label("Placa:");
        placa.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(placa, 50, 200);

        // ================================ ENTRIES ==============================
        Entry entryid = new Entry();
        entryid.SetSizeRequest(200, 30);
        fix.Put(entryid, 200, 50);

        Entry entrymarca = new Entry();
        entrymarca.SetSizeRequest(200, 30);
        fix.Put(entrymarca, 200, 100);

        Entry entrymodelo = new Entry();
        entrymodelo.SetSizeRequest(200, 30);
        fix.Put(entrymodelo, 200, 150);

        Entry entryplaca = new Entry();
        entryplaca.SetSizeRequest(200, 30);
        fix.Put(entryplaca, 200, 200);

        // ================================ BOTONES ==============================
        Button guardar = new Button("Guardar");
        guardar.SetSizeRequest(80, 50);
        fix.Put(guardar, 50, 300);

        guardar.Clicked += (sender, e) =>
        {
            // Obtener los datos ingresados
            int idVehiculo;
            if (!int.TryParse(entryid.Text, out idVehiculo))
            {
                Console.WriteLine("El ID debe ser un número.");
                return;
            }

            string marca = entrymarca.Text;
            string modelo = entrymodelo.Text;
            string placa = entryplaca.Text;

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(placa))
            {
                Console.WriteLine("Todos los campos son obligatorios.");
                return;
            }

            // Obtener la instancia de la lista doble enlazada
            ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();

            // Verificar si el ID del vehículo ya existe
            if (listaVehiculos.ExisteId(idVehiculo))
            {
                Console.WriteLine("El ID del vehículo ya existe.");
                return;
            }

            // Crear un nuevo vehículo y agregarlo a la lista
            Vehiculo nuevoVehiculo = new Vehiculo
            {
                Id = idVehiculo,
                IdUsuario = idUsuarioActual,
                Marca = marca,
                Modelo = modelo,
                Placa = placa
            };

            listaVehiculos.Agregar(nuevoVehiculo);
            Console.WriteLine("Vehículo agregado correctamente.");
        };

        Button button = new Button("Volver");
        button.SetSizeRequest(100, 30);
        fix.Put(button, 450, 25);
        button.Clicked += (sender, e) =>
        {
            Principalus manual = new Principalus();
            manual.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}