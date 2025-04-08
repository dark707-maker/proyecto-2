using Gtk;
using System;
using Pango;

//===============================USUARIOS===============================

public unsafe class GestorUsuarios
{
    private ListaUsuarios listaUsuarios;
    private Entry idEntry, nombresEntry, apellidosEntry, correoEntry;

    public GestorUsuarios(ListaUsuarios lista, Entry id, Entry nombres, Entry apellidos, Entry correo)
    {
        listaUsuarios = lista;
        idEntry = id;
        nombresEntry = nombres;
        apellidosEntry = apellidos;
        correoEntry = correo;
    }

    // ===============================BUSCAR USUARIOS POR ID===============================
    public void BuscarUsuario()
    {
        int id;
        if (int.TryParse(idEntry.Text, out id))
        {
            NodoUsuario* usuario = listaUsuarios.ObtenerUsuarioPorID(id);
            if (usuario != null)
            {
                nombresEntry.Text = usuario->Nombres;
                apellidosEntry.Text = usuario->Apellidos;
                correoEntry.Text = usuario->Correo;
                Console.WriteLine("Usuario encontrado.");
            }
            else
            {
                Console.WriteLine("Error: Usuario no encontrado.");
            }
        }
        else
        {
            Console.WriteLine("Error: ID debe ser un número entero.");
        }
    }

    // ===============================EDITAR USUARIOS===============================
    public void EditarUsuario()
    {
        int id;
        if (int.TryParse(idEntry.Text, out id))
        {
            NodoUsuario* usuario = listaUsuarios.ObtenerUsuarioPorID(id);
            if (usuario != null)
            {
                usuario->Nombres = nombresEntry.Text;
                usuario->Apellidos = apellidosEntry.Text;
                usuario->Correo = correoEntry.Text;
                Console.WriteLine("Usuario actualizado con éxito.");
                listaUsuarios.GenerarReporte(); // Actualiza el reporte
            }
            else
            {
                Console.WriteLine("Error: Usuario no encontrado.");
            }
        }
        else
        {
            Console.WriteLine("Error: ID debe ser un número entero.");
        }
    }

    // ===============================ELIMINAR USUARIOS===============================
    public void EliminarUsuario()
    {
        int id;
        if (int.TryParse(idEntry.Text, out id))
        {
            listaUsuarios.EliminarUsuario(id); // 
        }
        else
        {
            Console.WriteLine("Error: ID debe ser un número entero.");
        }
    }

}

//=============================================================================
//===============================INTERFAZ GRAFICAS==============================
//=============================================================================
class Editor : Gtk.Window 
{
    ListaUsuarios listaUsuarios = ListaUsuarios.Instancia;

    public Editor() : base("EDITOR DE USUARIOS") 
    {
        SetDefaultSize(800, 700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

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

//==============================BOTONES===============================
        Button button1 = new Button("ACTUALIZAR");
        button1.SetSizeRequest(100, 60);
        fix.Put(button1, 200, 550);

        Button button2 = new Button("ELIMINAR");
        button2.SetSizeRequest(120, 60);
        fix.Put(button2, 500, 550);

        Button button3 = new Button("BUSCAR");
        button3.SetSizeRequest(100, 60);        
        fix.Put(button3, 550, 150);

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
        fix.Put(correo, 500, 460);
        
//==============================FUNCIONES===============================
        GestorUsuarios gestor = new GestorUsuarios(listaUsuarios, id, nombres, apellidos, correo);

        button1.Clicked += (sender, e) => gestor.EditarUsuario();
        button2.Clicked += (sender, e) => gestor.EliminarUsuario(); 
        button3.Clicked += (sender, e) => gestor.BuscarUsuario();

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 650, 40);
        button.Clicked += (sender, e) => {
            Principal manual = new Principal();
            manual.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}
