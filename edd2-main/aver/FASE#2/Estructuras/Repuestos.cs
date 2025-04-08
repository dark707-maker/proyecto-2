using System;
using Gtk;

using System;
using Gtk;
using Newtonsoft.Json;

public class ListaRepuesto
{
    [JsonProperty("ID")] // Mapea la propiedad "ID" del JSON a "Id" en la clase
    public int Id { get; set; } // Identificador único del repuesto

    [JsonProperty("Repuesto")] // Mapea la propiedad "Repuesto" del JSON
    public string Repuesto { get; set; } // Nombre del repuesto

    [JsonProperty("Detalles")] // Mapea la propiedad "Detalles" del JSON
    public string Detalles { get; set; } // Descripción del repuesto

    [JsonProperty("Costo")] // Mapea la propiedad "Costo" del JSON
    public double Costo { get; set; } // Costo del repuesto

    public ListaRepuesto(int id, string repuesto, string detalles, double costo)
    {
        Id = id;
        Repuesto = repuesto;
        Detalles = detalles;
        Costo = costo;
    }
}

public class NodoAVL
{
    public ListaRepuesto Objeto { get; set; }
    public NodoAVL Izquierda { get; set; }
    public NodoAVL Derecha { get; set; }
    public int Altura { get; set; }

    public NodoAVL(ListaRepuesto item)
    {
        Objeto = item;
        Izquierda = null;
        Derecha = null;
        Altura = 1;
    }
}

public class ArbolAVL
{
    private NodoAVL root;

    // Instancia única para el patrón Singleton
    private static ArbolAVL instancia;

    // Constructor privado para evitar instanciación directa
    private ArbolAVL()
    {
        root = null;
    }

    // Método para obtener la instancia única
    public static ArbolAVL ObtenerInstancia()
    {
        if (instancia == null)
        {
            instancia = new ArbolAVL();
        }
        return instancia;
    }

    public void Insert(ListaRepuesto item)
    {
        root = Recursividad(root, item);
    }

    public NodoAVL ObtenerRaiz()
{
    return root;
}

    private NodoAVL Recursividad(NodoAVL node, ListaRepuesto item)
    {
        if (node == null)
        {
            return new NodoAVL(item);
        }

        if (item.Id < node.Objeto.Id)
        {
            node.Izquierda = Recursividad(node.Izquierda, item);
        }
        else if (item.Id > node.Objeto.Id)
        {
            node.Derecha = Recursividad(node.Derecha, item);
        }
        else
        {
            Console.WriteLine($"Error: El repuesto con ID {item.Id} ya existe.");
            return node;
        }

        node.Altura = 1 + Math.Max(ObtenerAltura(node.Izquierda), ObtenerAltura(node.Derecha));
        return Balance(node);
    }

    private int ObtenerAltura(NodoAVL node)
    {
        return node == null ? 0 : node.Altura;
    }

    private int ObtenerBalance(NodoAVL node)
    {
        return node == null ? 0 : ObtenerAltura(node.Izquierda) - ObtenerAltura(node.Derecha);
    }

    private NodoAVL Balance(NodoAVL node)
    {
        int balance = ObtenerBalance(node);

        if (balance > 1)
        {
            if (ObtenerBalance(node.Izquierda) < 0)
            {
                node.Izquierda = RotacionIzquierda(node.Izquierda);
            }
            return RotacionDerecha(node);
        }

        if (balance < -1)
        {
            if (ObtenerBalance(node.Derecha) > 0)
            {
                node.Derecha = RotacionDerecha(node.Derecha);
            }
            return RotacionIzquierda(node);
        }

        return node;
    }


    private NodoAVL RotacionDerecha(NodoAVL y)
    {
        NodoAVL x = y.Izquierda;
        NodoAVL T2 = x.Derecha;

        x.Derecha = y;
        y.Izquierda = T2;

        y.Altura = Math.Max(ObtenerAltura(y.Izquierda), ObtenerAltura(y.Derecha)) + 1;
        x.Altura = Math.Max(ObtenerAltura(x.Izquierda), ObtenerAltura(x.Derecha)) + 1;

        return x;
    }

    private NodoAVL RotacionIzquierda(NodoAVL x)
    {
        NodoAVL y = x.Derecha;
        NodoAVL T2 = y.Izquierda;

        y.Izquierda = x;
        x.Derecha = T2;

        x.Altura = Math.Max(ObtenerAltura(x.Izquierda), ObtenerAltura(x.Derecha)) + 1;
        y.Altura = Math.Max(ObtenerAltura(y.Izquierda), ObtenerAltura(y.Derecha)) + 1;

        return y;
    }


    public bool Actualizar(int id, string nuevoNombre, string nuevosDetalles, double nuevoCosto)
{
    NodoAVL nodo = BuscarNodo(root, id);
    if (nodo != null)
    {
        nodo.Objeto.Repuesto = nuevoNombre;
        nodo.Objeto.Detalles = nuevosDetalles;
        nodo.Objeto.Costo = nuevoCosto;
        return true; // Actualización exitosa
    }
    return false; // No se encontró el nodo
}

    public NodoAVL BuscarPorId(int id)
        {
    return BuscarNodo(root, id);
        }

    private NodoAVL BuscarNodo(NodoAVL node, int id)
{
    if (node == null || node.Objeto.Id == id)
    {
        return node;
    }

    if (id < node.Objeto.Id)
    {
        return BuscarNodo(node.Izquierda, id);
    }
    else
    {
        return BuscarNodo(node.Derecha, id);
    }
}

      public void Imprimirmetodo()
    {
        Imprimir(root);
    }

    private void Imprimir(NodoAVL node)
    {
        if (node == null) return;

        Imprimir(node.Izquierda);
        Console.WriteLine($"ID:{node.Objeto.Id}");
        Console.WriteLine($"Nombre: {node.Objeto.Repuesto}");
        Console.WriteLine($"Detalles: {node.Objeto.Detalles}");
        Console.WriteLine($"Costo: {node.Objeto.Costo}");
        Imprimir(node.Derecha);
        Console.WriteLine();
        
    }
}


