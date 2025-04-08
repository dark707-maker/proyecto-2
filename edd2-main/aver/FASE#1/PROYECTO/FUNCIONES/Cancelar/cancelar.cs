using Gtk;
using System;
using Pango;
using System.Diagnostics;
using System.IO;

public class FACTURACION
{
    public int ID { get; set; }
    public int IdOrden { get; set; }
    public double Total { get; set; }

    public FACTURACION(int id, int idOrden, double total)
    {
        ID = id;
        IdOrden = idOrden;
        Total = total;
    }
}

//==============================NODO FACTURA===============================================
public class NodoFactura
{
    public FACTURACION Factura { get; set; }
    public NodoFactura Siguiente { get; set; }

    public NodoFactura(FACTURACION factura)
    {
        Factura = factura;
        Siguiente = null;
    }
}

//==============================PILA FACTURAS===============================================
public class PilaFacturas
{
    private NodoFactura tope;
    private static PilaFacturas _instancia;

    private PilaFacturas()
    {
        tope = null;
    }

    public static PilaFacturas Instancia
    {
        get
        {
            if (_instancia == null)
            {
                _instancia = new PilaFacturas();
            }
            return _instancia;
        }
    }

//==============================METODOS PILA FACTURAS===============================================
    public void Apilar(FACTURACION factura)
    {
        NodoFactura nuevo = new NodoFactura(factura);
        nuevo.Siguiente = tope;
        tope = nuevo;
    }

    public FACTURACION Desapilar()
    {
        if (tope == null)
        {
            return null;
        }

        FACTURACION factura = tope.Factura;
        tope = tope.Siguiente;
        return factura;
    }

    public bool EstaVacia()
    {
        return tope == null;
    }

    public FACTURACION ObtenerFacturaPorID(int id)
    {
        NodoFactura actual = tope;
        while (actual != null)
        {
            if (actual.Factura.ID == id)
            {
                return actual.Factura;
            }
            actual = actual.Siguiente;
        }
        return null;
    }

    public int ObtenerSiguienteIdOrden()
    {
        int maxIdOrden = 0;
        NodoFactura actual = tope;
        while (actual != null)
        {
            if (actual.Factura.IdOrden > maxIdOrden)
            {
                maxIdOrden = actual.Factura.IdOrden;
            }
            actual = actual.Siguiente;
        }
        return maxIdOrden + 1;
    }

//==============================GRAFICO FACTURAS===============================================
    public void GraficoFacturas()
    {
        string rutaDirectorio = "/tmp/Graficaspng";
        string rutaArchivoDot = Path.Combine(rutaDirectorio, "reporte.dot");
        string rutaImagen = Path.Combine(rutaDirectorio, "Facturas.png");

        // Asegurar que el directorio existe
        if (!Directory.Exists(rutaDirectorio))
        {
            Directory.CreateDirectory(rutaDirectorio);
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivoDot))
            {
                writer.WriteLine("digraph G {");
                writer.WriteLine("    rankdir=TB;"); // La pila se representa de arriba hacia abajo
                writer.WriteLine("    node [shape=record];");

                NodoFactura actual = tope; // Empezar desde el tope de la pila
                while (actual != null)
                {
                    writer.WriteLine($"    Factura{actual.Factura.ID} [label=\"Factura {actual.Factura.ID}\\nID: {actual.Factura.ID}\\nIdOrden: {actual.Factura.IdOrden}\\nTotal: {actual.Factura.Total}\"];");

                    if (actual.Siguiente != null)
                    {
                        writer.WriteLine($"    Factura{actual.Factura.ID} -> Factura{actual.Siguiente.Factura.ID};");
                    }

                    actual = actual.Siguiente;
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

    //==============================IMPRIMIR FACTURAS===============================================
    public void IMPRIMIR()
    {
        NodoFactura actual = tope;
        while (actual != null)
        {
            Console.WriteLine($"--------------------------------------------------");
            Console.WriteLine($"ID: {actual.Factura.ID}");
            Console.WriteLine($"IdOrden: {actual.Factura.IdOrden}");
            Console.WriteLine($"Total: {actual.Factura.Total}");
            Console.WriteLine();
            actual = actual.Siguiente;
        }
    }
}

//-----------------------------------------------------------------------------
//-----------------------------------INTERFAZ GRAFICA-------------------------- 
//-----------------------------------------------------------------------------

class Cancelar : Gtk.Window
{
    private PilaFacturas pilaFacturas;

    public Cancelar() : base("CANCELAR")
    {
        pilaFacturas = PilaFacturas.Instancia;

        SetDefaultSize(600, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("FACTURACION");
        label1.ModifyFont(FontDescription.FromString("Arial 30"));
        fix.Put(label1, 170, 40);

        Label label2 = new Label("ID");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 180, 150);

        Label label3 = new Label("Id_Orden");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 170, 220);

        Label label4 = new Label("Total");
        label4.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label4, 170, 300);

        Entry entry1 = new Entry();
        entry1.SetSizeRequest(200, 30);
        fix.Put(entry1, 250, 150);

        Entry entry2 = new Entry();
        entry2.SetSizeRequest(200, 30);
        fix.Put(entry2, 250, 220);

        Entry entry3 = new Entry();
        entry3.SetSizeRequest(200, 30);
        fix.Put(entry3, 250, 300);

        Button buttonBuscar = new Button("Buscar");
        buttonBuscar.SetSizeRequest(80, 50);
        fix.Put(buttonBuscar, 400, 350);
        buttonBuscar.Clicked += (sender, e) =>
{
    if (int.TryParse(entry1.Text, out int id))
    {
        var factura = pilaFacturas.ObtenerFacturaPorID(id);
        if (factura != null)
        {
            entry2.Text = factura.IdOrden.ToString();
            entry3.Text = factura.Total.ToString();
        }
        else
        {
            Console.WriteLine("Factura no encontrada.");
        }
    }
    else
    {
        Console.WriteLine("ID inválido.");
    }
};

        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 500, 40);

        buttonVolver.Clicked += (sender, e) =>
        {
            Principal manual = new Principal();
            manual.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}