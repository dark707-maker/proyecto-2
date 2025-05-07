using System;

class NodoServicio
{
    public int Id;
    public int Id_Repuesto;
    public int Id_Vehiculo;
    public string Detalles;
    public double Costo;
    public NodoServicio? Izquierda;
    public NodoServicio? Derecha;

    public NodoServicio(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
    {
        Id = id;
        Id_Repuesto = idRepuesto;
        Id_Vehiculo = idVehiculo;
        Detalles = detalles;
        Costo = costo;
        Izquierda = null;
        Derecha = null;
    }
}

class ArbolServicios
{
    private NodoServicio root;
    private static ArbolServicios instancia;

    // Constructor privado para el patrón Singleton
    private ArbolServicios()
    {
        root = null;
    }

    // Método para obtener la única instancia del árbol
    public static ArbolServicios ObtenerInstancia()
    {
        if (instancia == null)
        {
            instancia = new ArbolServicios();
        }
        return instancia;
    }

    // Método para agregar un servicio al árbol
    public void Agregar(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
    {
        NodoServicio nuevoNodo = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);
        if (root == null)
        {
            root = nuevoNodo;
        }
        else
        {
            AgregarRecursivo(root, nuevoNodo);
        }
    }

    public void GenerarGraphviz()
    {
        string rutaDot = @"temp\servicios.dot";
        string rutaImagen = @"temp\servicios.png";

        using (StreamWriter writer = new StreamWriter(rutaDot))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("node [shape=record];");
            GenerarNodosGraphviz(root, writer);
            writer.WriteLine("}");
        }

        // Generar la imagen usando Graphviz
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng {rutaDot} -o {rutaImagen}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            Console.WriteLine("Archivo Graphviz generado correctamente en /temp.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar la imagen: {ex.Message}");
        }
    }

    private void GenerarNodosGraphviz(NodoServicio? nodo, StreamWriter writer)
    {
        if (nodo == null) return;

        writer.WriteLine($"node{nodo.Id} [label=\"{{ID: {nodo.Id} | ID Repuesto: {nodo.Id_Repuesto} | ID Vehículo: {nodo.Id_Vehiculo} | Detalles: {nodo.Detalles} | Costo: {nodo.Costo}}}\"];");

        if (nodo.Izquierda != null)
        {
            writer.WriteLine($"node{nodo.Id} -> node{nodo.Izquierda.Id};");
            GenerarNodosGraphviz(nodo.Izquierda, writer);
        }

        if (nodo.Derecha != null)
        {
            writer.WriteLine($"node{nodo.Id} -> node{nodo.Derecha.Id};");
            GenerarNodosGraphviz(nodo.Derecha, writer);
        }
    }

    private void AgregarRecursivo(NodoServicio actual, NodoServicio nuevoNodo)
    {
        if (nuevoNodo.Id < actual.Id)
        {
            if (actual.Izquierda == null)
            {
                actual.Izquierda = nuevoNodo;
            }
            else
            {
                AgregarRecursivo(actual.Izquierda, nuevoNodo);
            }
        }
        else if (nuevoNodo.Id > actual.Id)
        {
            if (actual.Derecha == null)
            {
                actual.Derecha = nuevoNodo;
            }
            else
            {
                AgregarRecursivo(actual.Derecha, nuevoNodo);
            }
        }
        else
        {
            Console.WriteLine("El servicio con el ID especificado ya existe.");
        }
    }

    // Recorrido In-Orden
    public void InOrden()
    {
        Console.WriteLine("Recorrido In-Orden:");
        InOrdenRecursivo(root);
    }

    private void InOrdenRecursivo(NodoServicio? actual)
    {
        if (actual == null) return;
        InOrdenRecursivo(actual.Izquierda);
        Imprimir(actual);
        InOrdenRecursivo(actual.Derecha);
    }

    // Recorrido Pre-Orden
    public void PreOrden()
    {
        Console.WriteLine("Recorrido Pre-Orden:");
        PreOrdenRecursivo(root);
    }

    private void PreOrdenRecursivo(NodoServicio? actual)
    {
        if (actual == null) return;
        Imprimir(actual);
        PreOrdenRecursivo(actual.Izquierda);
        PreOrdenRecursivo(actual.Derecha);
    }

    // Recorrido Post-Orden
    public void PostOrden()
    {
        Console.WriteLine("Recorrido Post-Orden:");
        PostOrdenRecursivo(root);
    }

    private void PostOrdenRecursivo(NodoServicio? actual)
    {
        if (actual == null) return;
        PostOrdenRecursivo(actual.Izquierda);
        PostOrdenRecursivo(actual.Derecha);
        Imprimir(actual);
    }

    public void ImprimirNodo(NodoServicio nodo)
    {
        Imprimir(nodo);
    }

    // Método para imprimir los detalles de un servicio
    private void Imprimir(NodoServicio servicio)
    {
        Console.WriteLine($"ID: {servicio.Id}");
        Console.WriteLine($"ID Repuesto: {servicio.Id_Repuesto}");
        Console.WriteLine($"ID Vehículo: {servicio.Id_Vehiculo}");
        Console.WriteLine($"Detalles: {servicio.Detalles}");
        Console.WriteLine($"Costo: {servicio.Costo}");
        Console.WriteLine("------------------------------");
    }
}