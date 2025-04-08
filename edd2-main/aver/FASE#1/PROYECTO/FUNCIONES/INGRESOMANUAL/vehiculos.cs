using Gtk;
using System;
using Pango;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

public unsafe class NodoVehiculo
{
    public int ID { get; set; }
    public string ID_Usuario { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Placa { get; set; }
    public NodoVehiculo* Siguiente { get; set; }
    public NodoVehiculo* Anterior { get; set; }

    public NodoVehiculo(int id, string id_usuario, string marca, string modelo, string placa)
    {
        ID = id;
        ID_Usuario = id_usuario;
        Marca = marca;
        Modelo = modelo;
        Placa = placa;
        Siguiente = null;
        Anterior = null;
    }
}

public class VehiculoDTO
{
    public int ID { get; set; }
    public string ID_Usuario { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Placa { get; set; }
}

public unsafe class ListaVehiculos
{
    private static ListaVehiculos instancia;
    private NodoVehiculo* cabeza;
    private NodoVehiculo* cola;

    private ListaVehiculos()
    {
        cabeza = null;
        cola = null;
    }

    public static ListaVehiculos Instancia {
        get {
            if (instancia == null) {
                instancia = new ListaVehiculos();
            }
            return instancia;
        }
    }

//=================AGREGAR VEHICULO=================

    public void AgregarVehiculo(int id, string id_usuario, string marca, string modelo, string placa)
    {
        if (ObtenerVehiculoPorID(id) != null)
        {
            Console.WriteLine("Error: El vehículo con este ID ya existe.");
            return;
        }

        NodoVehiculo* nuevoNodo = (NodoVehiculo*)Marshal.AllocHGlobal(sizeof(NodoVehiculo));
        *nuevoNodo = new NodoVehiculo(id, id_usuario, marca, modelo, placa);

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
            cola = nuevoNodo;
        }
        else
        {
            cola->Siguiente = nuevoNodo;
            nuevoNodo->Anterior = cola;
            cola = nuevoNodo;
        }
    }

    public NodoVehiculo* ObtenerVehiculoPorID(int id)
    {
        NodoVehiculo* temp = cabeza;
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

//===================================GRAPHIZ===================================
//==========================================================================
    public void GraficoVehiculos()
    {
        string rutaDirectorio = "/tmp/Graficaspng";
        string rutaArchivoDot = Path.Combine(rutaDirectorio, "reporte.dot");
        string rutaImagen = Path.Combine(rutaDirectorio, "vehiculos.png");

        if (!Directory.Exists(rutaDirectorio))
        {
            Directory.CreateDirectory(rutaDirectorio);
        }

        using (StreamWriter sw = new StreamWriter(rutaArchivoDot))
        {
            sw.WriteLine("digraph G {");
            sw.WriteLine("rankdir=LR;");
            sw.WriteLine("node [shape=record, style=filled, fillcolor=white];");

            NodoVehiculo* actual = cabeza;
            while (actual != null)
            {
                sw.WriteLine($"node{actual->ID} [label=\"ID: {actual->ID}\\nID_Usuario: {actual->ID_Usuario}\\nMarca: {actual->Marca}\\nModelo: {actual->Modelo}\\nPlaca: {actual->Placa}\"];");

                if (actual->Siguiente != null)
                {
                    sw.WriteLine($"node{actual->ID} -> node{actual->Siguiente->ID};");
                    sw.WriteLine($"node{actual->Siguiente->ID} -> node{actual->ID};");
                }

                actual = actual->Siguiente;
            }

            sw.WriteLine("}");
        }

        // Generar la imagen PNG usando Graphviz
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng {rutaArchivoDot} -o {rutaImagen}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            Console.WriteLine("Gráfico generado en: " + rutaImagen);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al generar el gráfico: " + ex.Message);
        }
    }

 //---------------------------------------TOPS MAS ANTIGUOS ---------------------------------------
    public void TopAntiguo()
    {
        List<NodoVehiculo> vehiculos = new List<NodoVehiculo>();
        NodoVehiculo* actual = cabeza;

        while (actual != null)
        {
            vehiculos.Add(*actual);
            actual = actual->Siguiente;
        }

        vehiculos.Sort((v1, v2) =>
        {
            if (int.TryParse(v1.Modelo, out int modelo1) && int.TryParse(v2.Modelo, out int modelo2))
            {
                return modelo1.CompareTo(modelo2);
            }
            else
            {
                // Si no se pueden convertir a enteros, los tratamos como iguales
                return 0;
            }
        });

        Console.WriteLine("Top Vehículos Más Antiguos:");
        foreach (var vehiculo in vehiculos)
        {
            Console.WriteLine($"ID: {vehiculo.ID}, Modelo: {vehiculo.Modelo}, Marca: {vehiculo.Marca}, Placa: {vehiculo.Placa}");
        }
    }

//------------------------------------------TOP CON MAS SERVICIOS------------------------------------------

    public void VehiculosMasServicios(ColaServicios colaServicios)
    {
        Dictionary<int, int> serviciosPorVehiculo = new Dictionary<int, int>();

        NodoServicio* actualServicio = colaServicios.ObtenerFrente();
        while (actualServicio != null)
        {
            int idVehiculo = int.Parse(actualServicio->Servicio.IdVehiculo);
            if (serviciosPorVehiculo.ContainsKey(idVehiculo))
            {
                serviciosPorVehiculo[idVehiculo]++;
            }
            else
            {
                serviciosPorVehiculo[idVehiculo] = 1;
            }
            actualServicio = actualServicio->Siguiente;
        }

        List<KeyValuePair<int, int>> listaServicios = new List<KeyValuePair<int, int>>(serviciosPorVehiculo);
        listaServicios.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        Console.WriteLine("Top Vehículos con Más Servicios:");
        foreach (var pair in listaServicios)
        {
            NodoVehiculo* vehiculo = ObtenerVehiculoPorID(pair.Key);
            if (vehiculo != null)
            {
                Console.WriteLine($"ID: {vehiculo->ID}, Modelo: {vehiculo->Modelo}, Marca: {vehiculo->Marca}, Placa: {vehiculo->Placa}, Servicios: {pair.Value}");
            }
        }
    }

    //---------------------------------------TOPS MAS ANTIGUOS CONSOLAS---------------------------------------

    public void ImprimirAntiguo()
    {
        List<NodoVehiculo> vehiculos = new List<NodoVehiculo>();
        NodoVehiculo* actual = cabeza;

        while (actual != null)
        {
            vehiculos.Add(*actual);
            actual = actual->Siguiente;
        }

        vehiculos.Sort((v1, v2) => int.Parse(v1.Modelo).CompareTo(int.Parse(v2.Modelo)));

        Console.WriteLine("Top Vehículos Más Antiguos:");
        foreach (var vehiculo in vehiculos)
        {
            Console.WriteLine($"ID: {vehiculo.ID}, Modelo: {vehiculo.Modelo}, Marca: {vehiculo.Marca}, Placa: {vehiculo.Placa}");
        }
    }

//--------------------------------VER EN CONSOLA--------------------------------
    public void Imprimir()
    {
        NodoVehiculo* actual = cabeza;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual->ID}");
            Console.WriteLine($"ID Usuario: {actual->ID_Usuario}");
            Console.WriteLine($"Marca: {actual->Marca}");
            Console.WriteLine($"Modelo: {actual->Modelo}");
            Console.WriteLine($"Placa: {actual->Placa}");
            Console.WriteLine("-----------------------------");
            actual = actual->Siguiente;
        }
    }

    //----------------------------vehiculos.json----------------------------

    public void VehiculosDesdeJson(string rutaArchivo)
    {
        try
        {
            string json = File.ReadAllText(rutaArchivo);
            var vehiculos = JsonConvert.DeserializeObject<List<VehiculoDTO>>(json);

            foreach (var vehiculo in vehiculos)
            {
                AgregarVehiculo(vehiculo.ID, vehiculo.ID_Usuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
            }
            Console.WriteLine("Vehículos cargados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al cargar el archivo JSON: " + ex.Message);
        }
    }
}

//==================================================================================
//==================================INTERFAZ GRAFICA==================================
//==================================================================================

class Vehiculos : Gtk.Window
{
    private ListaVehiculos listaVehiculos = ListaVehiculos.Instancia;

    public Vehiculos() : base("  VEHÍCULOS  ")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label6 = new Label(" Ingreso de Vehículos ");
        label6.ModifyFont(FontDescription.FromString("Arial 30"));
        fix.Put(label6, 210, 50);

        Label[] labels = {
            new Label("ID"),
            new Label("ID_Usuario"),
            new Label("Marca"),
            new Label("Modelo"),
            new Label("Placa")
        };

        int yOffset = 155;
        foreach (var label in labels)
        {
            label.ModifyFont(FontDescription.FromString("Arial 14"));
            fix.Put(label, 270, yOffset);
            yOffset += 50;
        }

        Entry[] entries = {
            new Entry(), new Entry(), new Entry(), new Entry(), new Entry()
        };

        yOffset = 150;
        foreach (var entry in entries)
        {
            entry.SetSizeRequest(200, 30);
            fix.Put(entry, 400, yOffset);
            yOffset += 50;
        }

        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 650, 40);

        buttonVolver.Clicked += (sender, e) =>
        {
            Ingresomanual ingresoManual = new Ingresomanual();
            ingresoManual.ShowAll();
            this.Hide();
        };

        Button botonGuardar = new Button("Guardar");
        botonGuardar.SetSizeRequest(100, 60);
        fix.Put(botonGuardar, 340, 450);

        botonGuardar.Clicked += (sender, e) =>
        {
            int id;
            if (int.TryParse(entries[0].Text, out id))
            {
                listaVehiculos.AgregarVehiculo(id, entries[1].Text, entries[2].Text, entries[3].Text, entries[4].Text);
                Console.WriteLine("Vehículo guardado con éxito.");
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