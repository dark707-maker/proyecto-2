using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Pango;
using System.Runtime.InteropServices; // Agregar esta directiva

//===============================CLASE SERVICIO==============================
public class Servicio
{
    public int ID { get; set; }
    public string IdRepuesto { get; set; }
    public string IdVehiculo { get; set; }
    public string Detalles { get; set; }
    public double Costo { get; set; }

    public Servicio(int id, string idRepuesto, string idVehiculo, string detalles, double costo)
    {
        ID = id;
        IdRepuesto = idRepuesto;
        IdVehiculo = idVehiculo;
        Detalles = detalles;
        Costo = costo;
    }
}

//===============================NODO SERVICIO==============================

public unsafe class NodoServicio
{
    public Servicio Servicio { get; set; }
    public NodoServicio* Siguiente;

    public NodoServicio(Servicio servicio)
    {
        Servicio = servicio;
        Siguiente = null;
    }
}

//===============================COLA SERVICIOS==============================

public unsafe class ColaServicios
{
    private static readonly ColaServicios instancia = new ColaServicios();
    private NodoServicio* frente;
    private NodoServicio* fin;

    private ColaServicios()
    {
        frente = null;
        fin = null;
    }

    public static ColaServicios Instancia => instancia;

    public void Encolar(Servicio servicio)
    {
        NodoServicio* nuevoNodo = (NodoServicio*)Marshal.AllocHGlobal(sizeof(NodoServicio));
        *nuevoNodo = new NodoServicio(servicio);

        if (fin == null)
        {
            frente = fin = nuevoNodo;
        }
        else
        {
            fin->Siguiente = nuevoNodo;
            fin = nuevoNodo;
        }
    }

    //===============================OBTENER FRENTE==============================

    public NodoServicio* ObtenerFrente()
    {
        return frente;
    }

    public Servicio Desencolar()
    {
        if (frente == null) return null;

        Servicio servicio = frente->Servicio;
        NodoServicio* temp = frente;
        frente = frente->Siguiente;

        if (frente == null)
        {
            fin = null;
        }

        Marshal.FreeHGlobal((IntPtr)temp);
        return servicio;
    }

    //===============================GRAFICA COLA==============================

    public void GraficaCola()
    {
        string rutaDirectorio = "/tmp/Graficaspng";
        string rutaArchivoDot = Path.Combine(rutaDirectorio, "reporte.dot");
        string rutaImagen = Path.Combine(rutaDirectorio, "Servicio.png");

        if (!Directory.Exists(rutaDirectorio))
        {
            Directory.CreateDirectory(rutaDirectorio);
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivoDot))
            {
                writer.WriteLine("digraph G {");
                writer.WriteLine("    rankdir=LR;");
                writer.WriteLine("    node [shape=rect];");

                NodoServicio* actual = this.ObtenerFrente();
                while (actual != null)
                {
                    writer.WriteLine($"    Servicio{actual->Servicio.ID} [label=\"Servicio {actual->Servicio.ID}\\nID: {actual->Servicio.ID}\\nId_Repuesto: {actual->Servicio.IdRepuesto}\\nId_Vehiculo: {actual->Servicio.IdVehiculo}\\nDetalles: {actual->Servicio.Detalles}\\nCosto: {actual->Servicio.Costo}\"];");

                    if (actual->Siguiente != null)
                    {
                        writer.WriteLine($"    Servicio{actual->Servicio.ID} -> Servicio{actual->Siguiente->Servicio.ID};");
                    }

                    actual = actual->Siguiente;
                }

                writer.WriteLine("}");
            }

            EjecutarGraphviz(rutaArchivoDot, rutaImagen);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el archivo DOT: {ex.Message}");
        }
    }

    //===============================EJECUTAR GRAPHVIZ==============================
    private static void EjecutarGraphviz(string rutaDot, string rutaImagen)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dot",
            Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaImagen}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using (Process proceso = Process.Start(psi))
            {
                proceso.WaitForExit();
                Console.WriteLine($"Imagen generada: {rutaImagen}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al ejecutar Graphviz: {ex.Message}");
        }
    }

    //

    public void Imprimir()
    {
        NodoServicio* actual = frente;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual->Servicio.ID}");
            Console.WriteLine($"Repuesto: {actual->Servicio.IdRepuesto}");
            Console.WriteLine($"Vehículo: {actual->Servicio.IdVehiculo}");
            Console.WriteLine($"Detalles: {actual->Servicio.Detalles}");
            Console.WriteLine($"Costo: {actual->Servicio.Costo}");
            Console.WriteLine("-----------------------------");

            actual = actual->Siguiente;
        }
    }

    public bool EstaVacia()
    {
        return frente == null;
    }
}

//-----------------------------------------------------------------------------
//-----------------------------------INTERFAZ GRAFICA-------------------------- 
//-----------------------------------------------------------------------------
unsafe class ServicioIngreso : Gtk.Window{
    ListaRepuestos listaRepuestos;
    ListaVehiculos listaVehiculos;
    ColaServicios colaServicios;
    PilaFacturas pilaFacturas;
    private HashSet<int> idsExistentes;

    public ServicioIngreso() : base("SERVICIOS")
    {
        pilaFacturas = PilaFacturas.Instancia;
        colaServicios = ColaServicios.Instancia;
        listaRepuestos = ListaRepuestos.Instancia;
        listaVehiculos = ListaVehiculos.Instancia;
        MatrizDispersa matriz = MatrizDispersa.GetInstance();

        idsExistentes = new HashSet<int>();


        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label6 = new Label("Ingreso de Servicios");
        label6.ModifyFont(FontDescription.FromString("Arial 30"));
        fix.Put(label6, 210, 50);

        Label label1 = new Label("ID");
        fix.Put(label1, 305, 155);
        Entry entry1 = new Entry();
        fix.Put(entry1, 400, 150);

        Label label2 = new Label("ID_Repuestos");
        fix.Put(label2, 270, 208);
        Entry entry2 = new Entry();
        fix.Put(entry2, 400, 200);

        Label label3 = new Label("ID_Vehiculos");
        fix.Put(label3, 275, 255);
        Entry entry3 = new Entry();
        fix.Put(entry3, 400, 250);

        Label label4 = new Label("DETALLES");
        fix.Put(label4, 280, 310);
        Entry entry4 = new Entry();
        fix.Put(entry4, 400, 300);

        Label label5 = new Label("Costo");
        fix.Put(label5, 265, 355);
        Entry entry5 = new Entry();
        fix.Put(entry5, 400, 350);

        Button botonfactura = new Button(" Factura ");
        botonfactura.SetSizeRequest(150, 50);
        fix.Put(botonfactura, 650, 150);

        botonfactura.Clicked += (sender, e) =>
        {
           Cancelar cancelar = new Cancelar();
           cancelar.ShowAll();
           this.Hide();
        };

        Button botonVolver = new Button("Volver");
        botonVolver.SetSizeRequest(80, 50);
        fix.Put(botonVolver, 650, 40);

        botonVolver.Clicked += (sender, e) =>
        {
            Ingresomanual manual = new Ingresomanual();
            manual.ShowAll();
            this.Hide();
        };

        //====================================FUNCIONALIDAD GUARDAR====================================

        Button boton1 = new Button("Guardar");
        fix.Put(boton1, 340, 450);
        boton1.Clicked += (sender, e) =>
{
    if (!string.IsNullOrWhiteSpace(entry1.Text) &&
        !string.IsNullOrWhiteSpace(entry2.Text) &&
        !string.IsNullOrWhiteSpace(entry3.Text) &&
        !string.IsNullOrWhiteSpace(entry5.Text))
    {
        if (int.TryParse(entry1.Text, out int id) && !idsExistentes.Contains(id))
        {
            if (int.TryParse(entry2.Text, out int idRepuesto) &&
                int.TryParse(entry3.Text, out int idVehiculo) &&
                double.TryParse(entry5.Text, out double costo))
            {
                if (listaRepuestos.ExisteID(idRepuesto) && listaVehiculos.ObtenerVehiculoPorID(idVehiculo) != null)
                {
                    Servicio servicio = new Servicio(id, entry2.Text, entry3.Text, entry4.Text, costo);
                    colaServicios.Encolar(servicio);
                    idsExistentes.Add(id);
                    Console.WriteLine("Servicio agregado con éxito.");

                    double costoRepuesto = listaRepuestos.ObtenerCostoPorID(idRepuesto);
                    double total = costo + costoRepuesto;
                    FACTURACION factura = new FACTURACION(id, pilaFacturas.ObtenerSiguienteIdOrden(), total);
                    pilaFacturas.Apilar(factura);
                    Console.WriteLine("Factura generada con éxito.");

                    matriz.Agregar(idVehiculo, idRepuesto);
                    matriz.Imprimir();

                    entry1.Text = "";
                    entry2.Text = "";
                    entry3.Text = "";
                    entry4.Text = "";
                    entry5.Text = "";

                    colaServicios.Imprimir();
                }
                else
                {
                    Console.WriteLine("Error: ID de repuesto o vehículo no existente.");
                }
            }
            else
            {
                Console.WriteLine("Error: Formato inválido en ID de repuesto, ID de vehículo o costo.");
            }
        }
        else
        {
            Console.WriteLine("ID inválido o ya existente.");
        }
    }
    else
    {
        Console.WriteLine("Error: Todos los campos deben estar llenos.");
    }
};

//-----------------------------------BOTON PAGAR-----------------------------------

        Button botonPagado = new Button("Pagado");
        botonPagado.SetSizeRequest(80, 50);
        fix.Put(botonPagado, 340, 520);
        botonPagado.Clicked += (sender, e) =>
        {
            var factura = pilaFacturas.Desapilar();
            if (factura != null)
            {
                Console.WriteLine($"Factura con ID {factura.ID} y Total {factura.Total} ha sido pagada y removida.");
            }
            else
            {
                Console.WriteLine("No hay facturas para pagar.");
            }
        };
        Add(fix);
        ShowAll();
    }
}