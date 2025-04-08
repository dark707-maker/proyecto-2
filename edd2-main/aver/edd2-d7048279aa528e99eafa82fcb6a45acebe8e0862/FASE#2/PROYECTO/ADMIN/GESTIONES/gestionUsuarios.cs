using Gtk;
using System;
using Pango;

class GestionUsuarios : Gtk.Window 
{
   private ListaUsuarios listaUsuarios;

    public GestionUsuarios() : base("EDITOR DE USUARIOS") 
    {
        listaUsuarios = ListaUsuarios.ObtenerInstancia();
        
        SetDefaultSize(800, 700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();
        // ============================== ETIQUETAS ==============================
        Label label1 = new Label("Editar un usuario:");
        label1.ModifyFont(FontDescription.FromString("Arial 28"));
        fix.Put(label1, 240, 50);

        Label label2 = new Label("ID:");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 190, 160);

        Label label3 = new Label("NOMBRES:");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 170, 260);

        Label label4 = new Label("APELLIDOS:");
        label4.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label4, 170, 360);

        Label label5 = new Label("CORREO:");
        label5.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label5, 170, 460);

        Label label6 = new Label("EDAD:");
        label6.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label6, 170, 510);

        Label label7 = new Label("CONTRASEÑA:");
        label7.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label7, 170, 560);

        // ============================== CAMPOS DE ENTRADA ==============================
        Entry id = new Entry();
        id.SetSizeRequest(200, 30);
        fix.Put(id, 300, 160);

        Entry nombres = new Entry();
        nombres.SetSizeRequest(180, 30);
        fix.Put(nombres, 300, 260);

        Entry apellidos = new Entry();
        apellidos.SetSizeRequest(180, 30);
        fix.Put(apellidos, 300, 360);

        Entry correo = new Entry();
        correo.SetSizeRequest(180, 30);
        fix.Put(correo, 300, 460);

        Entry edad = new Entry();
        edad.SetSizeRequest(180, 30);
        fix.Put(edad, 300, 510);

        Entry contrasenia = new Entry();
        contrasenia.SetSizeRequest(180, 30);
        fix.Put(contrasenia, 300, 560);

        // ============================== BOTONES ==============================
        Button Buscar = new Button("BUSCAR");
        Buscar.SetSizeRequest(100, 60);
        fix.Put(Buscar, 550, 150);

        Button Eliminar = new Button("ELIMINAR");
        Eliminar.SetSizeRequest(100, 60);
        fix.Put(Eliminar, 550, 250);

        Buscar.Clicked += (sender, e) => {
        int idBuscado;
        if (int.TryParse(id.Text, out idBuscado)) // Verifica que el ID sea un número válido
        {
            Usuario usuarioEncontrado = listaUsuarios.BuscarUsuario(idBuscado);
            if (usuarioEncontrado != null)
            {
                // Llena los campos con los datos del usuario encontrado
                nombres.Text = usuarioEncontrado.Nombres;
                apellidos.Text = usuarioEncontrado.Apellidos;
                correo.Text = usuarioEncontrado.Correo;
                edad.Text = usuarioEncontrado.Edad.ToString();
                contrasenia.Text = usuarioEncontrado.Contrasenia;

                Console.WriteLine($"Usuario con ID {idBuscado} encontrado.");
            }
            else
            {
                // Mensaje si no se encuentra el usuario
                Console.WriteLine($"Usuario con ID {idBuscado} no encontrado.");
            }
        }
        else
        {
        // Mensaje si el ID no es válido
                Console.WriteLine("Por favor, ingrese un ID válido.");
            }
        };

        Eliminar.Clicked += (sender, e) => {
            int idEliminar;
            if (int.TryParse(id.Text, out idEliminar)) // Verifica que el ID sea un número válido
            {
                bool eliminado = listaUsuarios.EliminarUsuario(idEliminar);
                if (eliminado)
                {
                    // Limpia los campos si el usuario fue eliminado
                    id.Text = "";
                    nombres.Text = "";
                    apellidos.Text = "";
                    correo.Text = "";
                    edad.Text = "";
                    contrasenia.Text = "";

                    Console.WriteLine($"Usuario con ID {idEliminar} eliminado exitosamente.");
                }
                else
                {
                    // Mensaje si no se encuentra el usuario
                    Console.WriteLine($"Usuario con ID {idEliminar} no encontrado.");
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
            gestionesmainA gestionesmain = new gestionesmainA();
            gestionesmain.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}