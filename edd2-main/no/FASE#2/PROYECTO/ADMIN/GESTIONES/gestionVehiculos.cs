using Gtk;
using System;
using Pango;

//===============================VEHICULOS===============================


//=============================================================================
//===============================INTERFAZ GRAFICAS==============================
//=============================================================================
class GestionVehiculoss : Gtk.Window 
{
    // Instancia de la lista de vehículos
    private ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();
    public GestionVehiculoss  () : base("EDITOR DE VeHICULOS") 
    {
        
        SetDefaultSize(800, 700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("Editar un vehiculo:");
        label1.ModifyFont(FontDescription.FromString("Arial 28"));
        fix.Put(label1, 240, 50);

        Label label2 = new Label("ID:");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 190, 160);

        Label label3 = new Label("ID_nombre:");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 170, 260);

        Label label4 = new Label("Marca:");
        label4.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label4, 170, 360);

        Label label5 = new Label("Modelo:");
        label5.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label5, 170, 460);

        Label label6 = new Label("Placa:");
        label6.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label6, 170, 510);


        // ============================== CAMPOS DE ENTRADA ==============================
        Entry id = new Entry();
        id.SetSizeRequest(200, 30);
        fix.Put(id, 300, 160);

        Entry IDnombre = new Entry();
        IDnombre.SetSizeRequest(180, 30);
        fix.Put(IDnombre, 300, 260);

        Entry Marca = new Entry();
        Marca.SetSizeRequest(180, 30);
        fix.Put(Marca, 300, 360);

        Entry Modelo = new Entry();
        Modelo.SetSizeRequest(180, 30);
        fix.Put(Modelo, 300, 460);

        Entry Placa = new Entry();
        Placa.SetSizeRequest(180, 30);
        fix.Put(Placa, 300, 550);


        // ============================== BOTONES ==============================
        Button buttonBuscar = new Button("BUSCAR");
        buttonBuscar.SetSizeRequest(100, 60);
        fix.Put(buttonBuscar, 550, 150);

        buttonBuscar.Clicked += (sender, e) =>
            {
                int idBuscado;
                if (int.TryParse(id.Text, out idBuscado)) // Verifica que el ID sea un número válido
                {
                    NodoVehiculo nodoEncontrado = listaVehiculos.BuscarPorId(idBuscado);
                    if (nodoEncontrado != null)
                    {
                        // Llena los campos con los datos del vehículo encontrado
                        Vehiculo vehiculo = nodoEncontrado.Vehiculo;
                        IDnombre.Text = vehiculo.IdUsuario.ToString();
                        Marca.Text = vehiculo.Marca;
                        Modelo.Text = vehiculo.Modelo;
                        Placa.Text = vehiculo.Placa;

                        Console.WriteLine($"Vehículo con ID {idBuscado} encontrado.");
                    }
                    else
                    {
                        // Mensaje si no se encuentra el vehículo
                        Console.WriteLine($"Vehículo con ID {idBuscado} no encontrado.");
                    }
                }
                else
                {
                    // Mensaje si el ID no es válido
                    Console.WriteLine("Por favor, ingrese un ID válido.");
                }
            };

        Button buttonEliminar = new Button("ELIMINAR");
        buttonEliminar.SetSizeRequest(120, 60);
        fix.Put(buttonEliminar, 500, 600);

        buttonEliminar.Clicked += (sender, e) =>
        {
            int idEliminar;
            if (int.TryParse(id.Text, out idEliminar)) // Verifica que el ID sea un número válido
            {
                bool eliminado = listaVehiculos.EliminarPorId(idEliminar);
                if (eliminado)
                {
                    // Limpia los campos si el vehículo fue eliminado
                    id.Text = "";
                    IDnombre.Text = "";
                    Marca.Text = "";
                    Modelo.Text = "";
                    Placa.Text = "";

                    Console.WriteLine($"Vehículo con ID {idEliminar} eliminado exitosamente.");
                }
                else
                {
                    // Mensaje si no se encuentra el vehículo
                    Console.WriteLine($"Vehículo con ID {idEliminar} no encontrado.");
                }
            }
            else
            {
                // Mensaje si el ID no es válido
                Console.WriteLine("Por favor, ingrese un ID válido.");
            }
        };


        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 650, 40);

        buttonVolver.Clicked += (sender, e) => {

            gestionesmainA principal = new gestionesmainA();
            principal.ShowAll();
            this.Hide();
          
        };

        Add(fix);
        ShowAll();
    }


}