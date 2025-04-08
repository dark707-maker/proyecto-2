using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

//=============================================================================
//===============================INTERFAZ GRAFICA===============================
class Cargas : Gtk.Window
{
    private ListaVehiculos listaVehiculos = ListaVehiculos.Instancia;
    private ListaRepuestos listaRepuestos = ListaRepuestos.Instancia;
    private ListaUsuarios listaUsuarios = ListaUsuarios.Instancia;

    public Cargas() : base("MENU CARGA MASIVA")
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
                Console.WriteLine("Debe seleccionar un tipo de carga.");
            }
        };

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 400, 40);
        button.Clicked += (sender, e) =>
        {
            Principal manual = new Principal();
            manual.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }

//===============================CARGA DE ARCHIVOS===============================
    
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
                //--------------USUARIOS----------
                {
                    case "Usuarios":
                     var usuarios = JsonConvert.DeserializeObject<List<NodoUsuario>>(json);
                     if (usuarios != null)
                    {
                  foreach (var usuario in usuarios)
                         {
                  listaUsuarios.AgregarUsuario(usuario.ID, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Contrasenia);
                     }
                     Console.WriteLine("Usuarios cargados correctamente.");
                        }
                    else
                     {
                 Console.WriteLine("Error: El archivo JSON de usuarios está vacío o tiene un formato incorrecto.");
                        }
                   break;
                   
                //----------------VEHICULOS----------
                    case "Vehículos":
                        listaVehiculos.VehiculosDesdeJson(filePath);
                        Console.WriteLine("Vehículos cargados correctamente.");
                        break;

                    //-------------REPUESTOS----------
                    case "Repuestos":
                        var repuestos = JsonConvert.DeserializeObject<List<Repuesto>>(json);
                        if (repuestos != null)
                        {
                            foreach (var repuesto in repuestos)
                            {
                                listaRepuestos.Agregar(repuesto.ID, repuesto.Nombre, repuesto.Descripcion, repuesto.Costo);
                            }
                            Console.WriteLine("Repuestos cargados correctamente.");
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
