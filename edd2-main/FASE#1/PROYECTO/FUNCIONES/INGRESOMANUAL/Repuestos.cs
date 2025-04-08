using Gtk;
using System;
using System.Collections.Generic;
using Pango;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

//===============================CLASE REPUESTO==============================
class Repuesto {
    public int ID { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public double Costo { get; set; }
}

//===============================NODO REPUESTOS==============================

public unsafe class NodoRepuesto {
    public int ID;
    public IntPtr Nombre;
    public IntPtr Descripcion;
    public double Costo;
    public NodoRepuesto* Siguiente;

    public NodoRepuesto(int id, string nombre, string descripcion, double costo) {
        ID = id;
        Nombre = Marshal.StringToHGlobalAnsi(nombre);
        Descripcion = Marshal.StringToHGlobalAnsi(descripcion);
        Costo = costo;
        Siguiente = null;
    }
}

//===============================LISTA REPUESTOS==============================
public unsafe class ListaRepuestos {
    private NodoRepuesto* cabeza;
    private static ListaRepuestos instancia;

    private ListaRepuestos() {
        cabeza = null;
    }

    public static ListaRepuestos Instancia {
        get {
            if (instancia == null) {
                instancia = new ListaRepuestos();
            }
            return instancia;
        }
    }

    //===============================METODO AGREGAR==============================
    public void Agregar(int id, string nombre, string descripcion, double costo) {
        if (ExisteID(id)) {
            Console.WriteLine("Error: El ID ya existe en la lista.");
            return;
        }

        NodoRepuesto* nuevo = (NodoRepuesto*)Marshal.AllocHGlobal(sizeof(NodoRepuesto));
        *nuevo = new NodoRepuesto(id, nombre, descripcion, costo);

        if (cabeza == null) {
            cabeza = nuevo;
            cabeza->Siguiente = cabeza;
        } else {
            NodoRepuesto* temp = cabeza;
            while (temp->Siguiente != cabeza) {
                temp = temp->Siguiente;
            }
            temp->Siguiente = nuevo;
            nuevo->Siguiente = cabeza;
        }
        Console.WriteLine("Repuesto agregado con éxito.");
    }

    //===============================METODO EXISTE ID==============================
    public bool ExisteID(int id) {
        if (cabeza == null) return false;
        NodoRepuesto* temp = cabeza;
        do {
            if (temp->ID == id) return true;
            temp = temp->Siguiente;
        } while (temp != cabeza);
        return false;
    }

    //===============================METODO OBTENER COSTO POR ID==============================
    public double ObtenerCostoPorID(int id) {
        if (cabeza == null) return -1;
        NodoRepuesto* temp = cabeza;
        do {
            if (temp->ID == id) return temp->Costo;
            temp = temp->Siguiente;
        } while (temp != cabeza);
        return -1; // Retorna -1 si no se encuentra el ID
    }

    //===============================METODO IMPRIMIR==============================
    public void Imprimir() {
        if (cabeza == null) {
            Console.WriteLine("Lista vacía.");
            return;
        }
        NodoRepuesto* actual = cabeza;
        do {
            Console.WriteLine($"ID: {actual->ID}");
            Console.WriteLine($"Nombre: {Marshal.PtrToStringAnsi(actual->Nombre)}");
            Console.WriteLine($"Descripción: {Marshal.PtrToStringAnsi(actual->Descripcion)}");
            Console.WriteLine($"Costo: {actual->Costo}");
            Console.WriteLine("-----------------------------");
            actual = actual->Siguiente;
        } while (actual != cabeza);
    }

    //===============================METODO CARGAR DESDE JSON==============================
    public void CargarDesdeJson(string rutaArchivo) {
        try {
            string json = File.ReadAllText(rutaArchivo);
            var repuestos = JsonConvert.DeserializeObject<List<Repuesto>>(json);

            foreach (var repuesto in repuestos) {
                Agregar(repuesto.ID, repuesto.Nombre, repuesto.Descripcion, repuesto.Costo);
            }
            Console.WriteLine("Repuestos cargados correctamente.");
        }
        catch (Exception ex) {
            Console.WriteLine("Error al cargar el archivo JSON: " + ex.Message);
        }
    }

    ~ListaRepuestos() {
        LiberarMemoria();
    }

    //===============================METODO LIBERAR MEMORIA==============================
    public void LiberarMemoria() {
        if (cabeza == null) return;
        NodoRepuesto* temp = cabeza;
        do {
            NodoRepuesto* siguiente = temp->Siguiente;
            Marshal.FreeHGlobal(temp->Nombre);
            Marshal.FreeHGlobal(temp->Descripcion);
            Marshal.FreeHGlobal((IntPtr)temp);
            temp = siguiente;
        } while (temp != cabeza);
        cabeza = null;
        Console.WriteLine("Memoria liberada correctamente.");
    }

    //===============================METODO GRAFICO REPUESTOS==============================
    public unsafe void GraficoRepuestos() {
        string rutaDirectorio = "/tmp/Graficaspng";
        string rutaArchivoDot = Path.Combine(rutaDirectorio, "reporte.dot");
        string rutaImagen = Path.Combine(rutaDirectorio, "Repuestos.png");

        // Crear el directorio si no existe
        if (!Directory.Exists(rutaDirectorio)) {
            Directory.CreateDirectory(rutaDirectorio);
        }

        if (cabeza == null) {
            Console.WriteLine("Lista vacía.");
            return;
        }

        using (StreamWriter sw = new StreamWriter(rutaArchivoDot)) {
            sw.WriteLine("digraph G {");
            sw.WriteLine("rankdir=LR;");

            NodoRepuesto* temp = cabeza;
            do {
                string nombre = Marshal.PtrToStringAnsi(temp->Nombre);
                string descripcion = Marshal.PtrToStringAnsi(temp->Descripcion);
                sw.WriteLine($"N{temp->ID} [label=\"ID: {temp->ID}\\nRepuesto: {nombre}\\nDetalles: {descripcion}\\nCosto: {temp->Costo}\", shape=rect];");

                if (temp->Siguiente != null) {
                    sw.WriteLine($"N{temp->ID} -> N{temp->Siguiente->ID};");
                }

                temp = temp->Siguiente;
            } while (temp != cabeza);

            // Enlace circular
            sw.WriteLine($"N{temp->ID} -> N{cabeza->ID};");

            sw.WriteLine("}");
        }

        Console.WriteLine($"Archivo DOT generado en {rutaArchivoDot}");

        // Ejecutar Graphviz para generar la imagen
        try {
            System.Diagnostics.Process.Start("dot", $"-Tpng {rutaArchivoDot} -o {rutaImagen}");
            Console.WriteLine($"Imagen generada en {rutaImagen}");
        } catch (Exception ex) {
            Console.WriteLine($"Error al generar la imagen: {ex.Message}");
        }
    }
}

//===============================INTERFAZ GRAFICA REPUESTOS==============================
//=======================================================================================

class RepuestosInterfaz : Gtk.Window {
    ListaRepuestos listaRepuestos = ListaRepuestos.Instancia;

    public RepuestosInterfaz() : base(" REPUESTOS ") {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label titulo = new Label(" Ingreso de Repuestos ");
        titulo.ModifyFont(FontDescription.FromString("Arial 30"));
        fix.Put(titulo, 210, 50);

        Label lblID = new Label("ID");
        lblID.ModifyFont(FontDescription.FromString("Arial 16"));
        fix.Put(lblID, 305, 155);

        Label lblNombre = new Label(" Repuesto ");
        lblNombre.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(lblNombre, 270, 208);

        Label lblDescripcion = new Label(" Detalles");
        lblDescripcion.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(lblDescripcion, 275, 255);

        Label lblCosto = new Label(" Costo");
        lblCosto.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(lblCosto, 280, 310);

        Entry txtID = new Entry();
        txtID.SetSizeRequest(200, 30);
        fix.Put(txtID, 400, 150);

        Entry txtNombre = new Entry();
        txtNombre.SetSizeRequest(200, 30);
        fix.Put(txtNombre, 400, 200);

        Entry txtDescripcion = new Entry();
        txtDescripcion.SetSizeRequest(200, 30);
        fix.Put(txtDescripcion, 400, 250);
        
        Entry txtCosto = new Entry();
        txtCosto.SetSizeRequest(200, 30);
        fix.Put(txtCosto, 400, 300);

        //--------------------------------

        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 650, 40);

        buttonVolver.Clicked += (sender, e) =>
        {
            Ingresomanual ingresoManual = new Ingresomanual();
            ingresoManual.ShowAll();
            this.Hide();
        };

        //--------------------------------
        Button btnGuardar = new Button(" Guardar");
        btnGuardar.SetSizeRequest(100, 60);
        fix.Put(btnGuardar, 340, 450);
        
        btnGuardar.Clicked += (sender, e) => {
            int id;
            double costo;
            if (int.TryParse(txtID.Text, out id) && double.TryParse(txtCosto.Text, out costo)) {
                listaRepuestos.Agregar(id, txtNombre.Text, txtDescripcion.Text, costo);
                listaRepuestos.Imprimir();
            } else {
                Console.WriteLine("Error: ID debe ser un número entero y Costo debe ser un número válido.");
            }
        };

        Add(fix);
        ShowAll();
    }
}