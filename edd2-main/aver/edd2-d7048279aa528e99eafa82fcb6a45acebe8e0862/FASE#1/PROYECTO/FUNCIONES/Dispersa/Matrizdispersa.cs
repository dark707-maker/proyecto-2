using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

//===============================MATRIZ DISPERSA===============================
//constructor
public unsafe struct NodoMatriz
{
    public int IdVehiculo { get; set; }
    public int IdRepuesto { get; set; }
    public int Cantidad { get; set; }
    public NodoMatriz* Siguiente { get; set; }
    public NodoMatriz* Abajo { get; set; }

    public NodoMatriz(int idVehiculo, int idRepuesto)
    {
        IdVehiculo = idVehiculo;
        IdRepuesto = idRepuesto;
        Cantidad = 1;
        Siguiente = null;
        Abajo = null;
    }
}

//===============================NODOS===============================

public unsafe class MatrizDispersa
{
    private static MatrizDispersa instancia;
    private Hashtable filas;
    private Hashtable columnas;
    
    private MatrizDispersa()
    {
        filas = new Hashtable();
        columnas = new Hashtable();
    }
    
    public static MatrizDispersa GetInstance()
    {
        if (instancia == null)
        {
            instancia = new MatrizDispersa();
        }
        return instancia;
    }

//===============================AGREGAR===============================
    public void Agregar(int idVehiculo, int idRepuesto)
    {
        if (!filas.ContainsKey(idVehiculo))
        {
            NodoMatriz* nuevoNodo = (NodoMatriz*)Marshal.AllocHGlobal(sizeof(NodoMatriz));
            *nuevoNodo = new NodoMatriz(idVehiculo, idRepuesto);
            filas[idVehiculo] = (IntPtr)nuevoNodo;
        }

        if (!columnas.ContainsKey(idRepuesto))
        {
            NodoMatriz* nuevoNodo = (NodoMatriz*)Marshal.AllocHGlobal(sizeof(NodoMatriz));
            *nuevoNodo = new NodoMatriz(idVehiculo, idRepuesto);
            columnas[idRepuesto] = (IntPtr)nuevoNodo;
        }

        NodoMatriz* nodoFila = (NodoMatriz*)(IntPtr)filas[idVehiculo];
        NodoMatriz* nodoColumna = (NodoMatriz*)(IntPtr)columnas[idRepuesto];

        while (nodoFila->Siguiente != null && nodoFila->Siguiente->IdRepuesto != idRepuesto)
        {
            nodoFila = nodoFila->Siguiente;
        }

        while (nodoColumna->Abajo != null && nodoColumna->Abajo->IdVehiculo != idVehiculo)
        {
            nodoColumna = nodoColumna->Abajo;
        }

        if (nodoFila->Siguiente == null)
        {
            nodoFila->Siguiente = (NodoMatriz*)Marshal.AllocHGlobal(sizeof(NodoMatriz));
            *nodoFila->Siguiente = new NodoMatriz(idVehiculo, idRepuesto);
        }
        else
        {
            nodoFila->Siguiente->Cantidad++;
        }

        if (nodoColumna->Abajo == null)
        {
            nodoColumna->Abajo = (NodoMatriz*)Marshal.AllocHGlobal(sizeof(NodoMatriz));
            *nodoColumna->Abajo = new NodoMatriz(idVehiculo, idRepuesto);
        }
        else
        {
            nodoColumna->Abajo->Cantidad++;
        }
    }

//==============================IMPRIMIR===============================
    public void Imprimir()
    {
        foreach (DictionaryEntry fila in filas)
        {
            NodoMatriz* nodo = (NodoMatriz*)(IntPtr)fila.Value;
            while (nodo != null)
            {
                Console.WriteLine($"Vehículo ID: {nodo->IdVehiculo}, Repuesto ID: {nodo->IdRepuesto}, Cantidad: {nodo->Cantidad}");
                nodo = nodo->Siguiente;
            }
        }
    }

//===============================GRAFICAR===============================

   public void GraficaMatriz()
{
    string rutaDirectorio = "/tmp/Graficaspng";
    string rutaArchivoDot = Path.Combine(rutaDirectorio, "reporte.dot");
    string rutaImagen = Path.Combine(rutaDirectorio, "MatrizDispersa.png");

    if (!Directory.Exists(rutaDirectorio))
    {
        Directory.CreateDirectory(rutaDirectorio);
    }

    try
    {
        using (StreamWriter writer = new StreamWriter(rutaArchivoDot))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=TB;");
            writer.WriteLine("    node [shape=rect, style=filled, fillcolor=lightgray];");

            // Definir los vehículos (filas)
            foreach (DictionaryEntry fila in filas)
            {
                NodoMatriz* nodo = (NodoMatriz*)(IntPtr)fila.Value;
                writer.WriteLine($"    V{nodo->IdVehiculo} [label=\"Vehículo {nodo->IdVehiculo}\", shape=box, fillcolor=lightblue];");
            }

            // Definir los repuestos (columnas)
            foreach (DictionaryEntry columna in columnas)
            {
                NodoMatriz* nodo = (NodoMatriz*)(IntPtr)columna.Value;
                writer.WriteLine($"    R{nodo->IdRepuesto} [label=\"Repuesto {nodo->IdRepuesto}\", shape=ellipse, fillcolor=lightcoral];");
            }

            // Conectar vehículos con sus respectivos servicios y servicios con repuestos
            foreach (DictionaryEntry fila in filas)
            {
                NodoMatriz* nodo = (NodoMatriz*)(IntPtr)fila.Value;
                string vehiculoId = $"V{nodo->IdVehiculo}";

                while (nodo != null)
                {
                    string repuestoId = $"R{nodo->IdRepuesto}";
                    string nodoId = $"S{nodo->IdVehiculo}_{nodo->IdRepuesto}";  // Nodo intermedio (servicio)

                    // Nodo servicio
                    writer.WriteLine($"    {nodoId} [label=\"Servicio\\nCantidad: {nodo->Cantidad}\", shape=diamond, fillcolor=lightyellow];");

                    // Conectar Vehículo -> Servicio
                    writer.WriteLine($"    {vehiculoId} -> {nodoId} [color=blue];");

                    // Conectar Servicio -> Repuesto
                    writer.WriteLine($"    {nodoId} -> {repuestoId} [color=red];");

                    nodo = nodo->Siguiente;
                }
            }

            // Conectar la estructura de matriz
            foreach (DictionaryEntry fila in filas)
            {
                NodoMatriz* nodo = (NodoMatriz*)(IntPtr)fila.Value;
                writer.Write("    { rank=same; ");
                while (nodo != null)
                {
                    writer.Write($"S{nodo->IdVehiculo}_{nodo->IdRepuesto} ");
                    nodo = nodo->Siguiente;
                }
                writer.WriteLine("}");
            }

            writer.WriteLine("}");
        }

        Console.WriteLine("Archivo DOT generado correctamente.");
        EjecutarGraphviz(rutaArchivoDot, rutaImagen);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al generar el archivo DOT: {ex.Message}");
    }
}


//===============================.dot a PNG===============================
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
}