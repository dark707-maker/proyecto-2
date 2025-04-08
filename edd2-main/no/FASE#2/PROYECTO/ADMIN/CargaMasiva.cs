using Gtk;
using System;
using System.IO;
using Newtonsoft.Json;

class CargaMasiva : Gtk.Window
{
    private ListaUsuarios listaUsuarios = ListaUsuarios.ObtenerInstancia(); // Instancia única de la lista de usuarios
    private ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia(); // Instancia única de la lista de vehículos
    private ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia(); // Usar el método Singleton para obtener la instancia del árbol AVL

    public CargaMasiva() : base("MENU CARGA MASIVA")
    {
        SetDefaultSize(600, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("Selecciona:");
        label1.ModifyFont(Pango.FontDescription.FromString("Arial 20"));
        fix.Put(label1, 220, 60);

        ComboBoxText combo = new ComboBoxText();
        combo.AppendText("Usuarios");
        combo.AppendText("Vehículos");
        combo.AppendText("Repuestos");
        fix.Put(combo, 250, 160);

        Button boton1 = new Button("Cargar");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 240, 270);

        boton1.Clicked += (sender, e) =>
        {
            string tipoCarga = combo.ActiveText;
            if (!string.IsNullOrEmpty(tipoCarga))
            {
                CargarArchivo(tipoCarga);
            }
            else
            {
                Console.WriteLine("Por favor, selecciona una opción.");
            }
        };

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 400, 40);
        button.Clicked += (sender, e) =>
        {
            Principaladmin manual = new Principaladmin();
            manual.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }

    private void CargarArchivo(string tipoCarga)
    {
        Gtk.FileChooserDialog fileChooser = new Gtk.FileChooserDialog(
            "Selecciona el archivo JSON",
            this,
            FileChooserAction.Open,
            "Cancelar", ResponseType.Cancel,
            "Abrir", ResponseType.Accept);

        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            string filePath = fileChooser.Filename;
            try
            {
                string json = File.ReadAllText(filePath);

                switch (tipoCarga)
                {
                    case "Usuarios":
                        var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                        if (usuarios != null)
                        {
                            foreach (var usuario in usuarios)
                            {
                                listaUsuarios.AgregarUsuario(usuario); // Agregar cada usuario a la lista
                            }

                            Console.WriteLine("Usuarios cargados correctamente.");
                            listaUsuarios.Imprimir(); // Imprimir usuarios cargados para verificar
                        }
                        else
                        {
                            Console.WriteLine("Error: El archivo JSON de usuarios está vacío o tiene un formato incorrecto.");
                        }
                        break;

                    case "Vehículos":
                        Console.WriteLine("Usuarios en la lista antes de cargar vehículos:");
                        listaUsuarios.Imprimir(); // Imprimir usuarios cargados para verificar

                        var vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(json);
                        if (vehiculos != null)
                        {
                            foreach (var vehiculo in vehiculos)
                            {
                                Console.WriteLine($"Verificando vehículo con ID: {vehiculo.Id}, IdUsuario: {vehiculo.IdUsuario}");

                                // Validar si el IdUsuario existe en la lista de usuarios
                                if (listaVehiculos.ExisteUsuario(vehiculo.IdUsuario))
                                {
                                    // Validar si el ID del vehículo ya existe
                                    if (!listaVehiculos.ExisteId(vehiculo.Id))
                                    {
                                        listaVehiculos.Agregar(vehiculo);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"El vehículo con ID {vehiculo.Id} ya existe y no será agregado.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"El usuario con ID {vehiculo.IdUsuario} no existe. El vehículo no será agregado.");
                                }
                            }
                            listaVehiculos.Imprimir(); // Imprimir los vehículos cargados
                        }
                        else
                        {
                            Console.WriteLine("Error: El archivo JSON de vehículos está vacío o tiene un formato incorrecto.");
                        }
                        break;

                    case "Repuestos":
                    var repuestos = JsonConvert.DeserializeObject<List<ListaRepuesto>>(json); // Cambiado de Repuesto a ListaRepuesto
                    if (repuestos != null)
                    {
                        foreach (var repuesto in repuestos)
                        {
                            arbolRepuestos.Insert(repuesto);
                            Console.WriteLine($"Repuesto agregado: {repuesto.Repuesto}, Detalles: {repuesto.Detalles}, Costo: {repuesto.Costo}");
                        }
                        Console.WriteLine("Repuestos cargados correctamente.");
                        Console.WriteLine("Repuestos en el árbol AVL:");
                        arbolRepuestos.Imprimirmetodo(); // Llama al método público
                    }
                    else
                    {
                        Console.WriteLine("Error: El archivo JSON de repuestos está vacío o tiene un formato incorrecto.");
                    }
                    break;

                    default:
                        Console.WriteLine("Tipo de carga no reconocido.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el archivo JSON: {ex.Message}");
            }
        }

        fileChooser.Destroy();
    }
}