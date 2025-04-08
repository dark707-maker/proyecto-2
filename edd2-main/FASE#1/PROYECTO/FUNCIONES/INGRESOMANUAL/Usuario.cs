using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using Gtk;
using Pango;

//=============================NODO USUARIOS==============================================

public unsafe class NodoUsuario
{
    public int ID { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public string Contrasenia { get; set; }
    public NodoUsuario* Siguiente;

    public NodoUsuario(int id, string nombres, string apellidos, string correo, string contrasenia)
    {
        ID = id;
        Nombres = nombres;
        Apellidos = apellidos;
        Correo = correo;
        Contrasenia = contrasenia;
        Siguiente = null;
    }
}

//=============================LISTA USUARIOS==============================================

public unsafe class ListaUsuarios
{
    private static ListaUsuarios instancia; 
    private static readonly object bloqueo = new object(); 
    private NodoUsuario* cabeza; 
    
    private ListaUsuarios()
    {
        cabeza = null;
    }

   
    public static ListaUsuarios Instancia
    {
        get
        {
            
            if (instancia == null)
            {
                lock (bloqueo)
                {
                    if (instancia == null) 
                    {
                        instancia = new ListaUsuarios();
                    }
                }
            }
            return instancia;
        }
    }

//===============================METODO OBTENER==============================================
     public NodoUsuario* ObtenerUsuarioPorID(int id)
    {
        NodoUsuario* temp = cabeza;
        while (temp != null)
        {
            if (temp->ID == id)
            {
                return temp;
            }
            temp = temp->Siguiente;
        }
        return null;
    }

    // ================================METODO AGREGAR==============================================

public void AgregarUsuario(int id, string nombres, string apellidos, string correo, string contrasenia)
    {
        if (ObtenerUsuarioPorID(id) != null)
        {
            Console.WriteLine("Error: El usuario con este ID ya existe.");
            return;
        }

        NodoUsuario* nuevoNodo = (NodoUsuario*)Marshal.AllocHGlobal(sizeof(NodoUsuario));
        *nuevoNodo = new NodoUsuario(id, nombres, apellidos, correo, contrasenia);

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
        }
        else
        {
            NodoUsuario* temp = cabeza;
            while (temp->Siguiente != null)
            {
                temp = temp->Siguiente;
            }
            temp->Siguiente = nuevoNodo;
            ListaUsuarios.Instancia.GenerarReporte();
        }
    }


    //===============================METODO GENERAR REPORTE==============================================

    public unsafe void GenerarReporte()
{
    string rutaDirectorio = "/tmp/Graficaspng";
    string rutaArchivoDot = Path.Combine(rutaDirectorio, "reporte.dot");
    string rutaImagen = Path.Combine(rutaDirectorio, "USUARIO.png");

    if (!Directory.Exists(rutaDirectorio))
    {
        Directory.CreateDirectory(rutaDirectorio);
    }

    using (StreamWriter writer = new StreamWriter(rutaArchivoDot))
    {
        writer.WriteLine("digraph G {");
        writer.WriteLine("    rankdir=LR;");
        writer.WriteLine("    node [shape=record];");

        NodoUsuario* actual = cabeza;
        while (actual != null)
        {
            string id = actual->ID.ToString();
            string nombre = actual->Nombres;
            string correo = actual->Correo;
            writer.WriteLine($"    nodo{id} [label=\"ID: {id} | Nombre: {nombre} | Correo: {correo}\"];");

            if (actual->Siguiente != null)
            {
                string idNext = actual->Siguiente->ID.ToString();
                writer.WriteLine($"    nodo{id} -> nodo{idNext};");
            }

            actual = actual->Siguiente;
        }

        writer.WriteLine("}");
    }

    // Usar el método más robusto para ejecutar Graphviz
    EjecutarGraphviz(rutaArchivoDot, rutaImagen);
    Console.WriteLine($"Reporte generado en: {rutaImagen}");
}


//===============================DOT A PNG===========================================================
private void EjecutarGraphviz(string rutaDot, string rutaSalida)
{
    try
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dot",
            Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaSalida}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.Start();

            // Leer salidas del proceso
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine($"Graphviz Output: {output}");
            }
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Graphviz Error: {error}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al ejecutar Graphviz: {ex.Message}");
    }
}

//=====================================IMPRIMIR========================================================
    public void Imprimir()
    {
        NodoUsuario* actual = cabeza;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual->ID}");
            Console.WriteLine($"Nombres: {actual->Nombres}");
            Console.WriteLine($"Apellidos: {actual->Apellidos}");
            Console.WriteLine($"Correo: {actual->Correo}");
            Console.WriteLine($"Contraseña: {actual->Contrasenia}");
            Console.WriteLine("-----------------------------");

            actual = actual->Siguiente;
        }
    }

    //================================ELIMINAR USUARIO====================================================

    public void EliminarUsuario(int id)
    {
        if (cabeza == null)
        {
            Console.WriteLine("Error: La lista está vacía.");
            return;
        }
        if (cabeza->ID == id)
        {
            NodoUsuario* temp = cabeza;
            cabeza = cabeza->Siguiente;  
            Marshal.FreeHGlobal((IntPtr)temp);  
            Console.WriteLine($"Usuario con ID {id} eliminado.");
            return;
        }

        NodoUsuario* actual = cabeza;
        while (actual->Siguiente != null && actual->Siguiente->ID != id)
        {
            actual = actual->Siguiente;
        }

        if (actual->Siguiente == null)
        {
            Console.WriteLine($"Error: No se encontró un usuario con el ID {id}.");
            return;
        }

        NodoUsuario* nodoEliminar = actual->Siguiente;
        actual->Siguiente = actual->Siguiente->Siguiente;  
        Marshal.FreeHGlobal((IntPtr)nodoEliminar);  
        Console.WriteLine($"Usuario con ID {id} eliminado.");
        ListaUsuarios.Instancia.Imprimir();
        ListaUsuarios.Instancia.GenerarReporte();
    }

//=====================================IMPORTAR USUARIOS DESDE JSON========================================================
    public void ImportarUsuariosDesdeJson(string filePath)
    {
        string json = System.IO.File.ReadAllText(filePath);
        var usuarios = JsonSerializer.Deserialize<List<NodoUsuario>>(json);
        foreach (var usuario in usuarios)
        {
            AgregarUsuario(usuario.ID, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Contrasenia);
        }
        GenerarReporte();
    }
}


//===============================INTERFAZ GRAFICA===============================
//==============================================================================
class Usuarios : Gtk.Window
{
    private ListaUsuarios listaUsuarios = ListaUsuarios.Instancia;

    public Usuarios() : base("USUARIOS")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        // Titulo
        Label labelTitulo = new Label("Ingreso de Usuario");
        labelTitulo.ModifyFont(FontDescription.FromString("Arial 30"));
        fix.Put(labelTitulo, 230, 50);

        // Etiquetas
        Label label1 = new Label("ID");
        label1.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(label1, 270, 150);

        Label label2 = new Label("Nombres");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 270, 200);

        Label label3 = new Label("Apellido");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 270, 250);

        Label label4 = new Label("Correo");
        label4.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label4, 270, 300);

        Label label5 = new Label("Contraseña");
        label5.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label5, 270, 350);

        // Entradas de texto
        Entry entry1 = new Entry();
        entry1.SetSizeRequest(200, 30);
        fix.Put(entry1, 400, 150);

        Entry entry2 = new Entry();
        entry2.SetSizeRequest(200, 30);
        fix.Put(entry2, 400, 200);

        Entry entry3 = new Entry();
        entry3.SetSizeRequest(200, 30);
        fix.Put(entry3, 400, 250);

        Entry entry4 = new Entry();
        entry4.SetSizeRequest(200, 30);
        fix.Put(entry4, 400, 300);

        Entry entry5 = new Entry();
        entry5.SetSizeRequest(200, 30);
        fix.Put(entry5, 400, 350);

        // Botón Volver
        Button botonVolver = new Button("Volver");
        botonVolver.SetSizeRequest(80, 50);
        fix.Put(botonVolver, 650, 40);

        botonVolver.Clicked += (sender, e) =>
        {
            Ingresomanual manual = new Ingresomanual();
            manual.ShowAll();
            this.Hide();
        };

        //----------------- Botón Guardar
        Button botonGuardar = new Button("Guardar");
        botonGuardar.SetSizeRequest(100, 60);
        fix.Put(botonGuardar, 340, 450);

        botonGuardar.Clicked += (sender, e) =>
        {
            int id;
            if (int.TryParse(entry1.Text, out id)) // Verifica si el ID es un número entero
            {
              
                if (string.IsNullOrEmpty(entry2.Text) || string.IsNullOrEmpty(entry3.Text) || string.IsNullOrEmpty(entry4.Text) || string.IsNullOrEmpty(entry5.Text))
                {
                    Console.WriteLine("Error: Todos los campos deben ser completados.");
                    return;
                }

               
                listaUsuarios.AgregarUsuario(id, entry2.Text, entry3.Text, entry4.Text, entry5.Text);
                Console.WriteLine("Usuario guardado con éxito.");
                listaUsuarios.Imprimir(); 

                entry1.Text = "";
                entry2.Text = "";
                entry3.Text = "";
                entry4.Text = "";
                entry5.Text = "";

                
            }
            else
            {
                Console.WriteLine("Error: ID debe ser un número entero.");
            }
        };

        Add(fix);
        ShowAll();
    }
}
