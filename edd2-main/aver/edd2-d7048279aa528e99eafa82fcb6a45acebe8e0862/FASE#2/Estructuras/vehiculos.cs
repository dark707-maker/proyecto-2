using System;
using Newtonsoft.Json;

public class Vehiculo
{
    [JsonProperty("ID")]
    public int Id { get; set; }

    [JsonProperty("ID_Usuario")]
    public int IdUsuario { get; set; }

    [JsonProperty("Marca")]
    public string Marca { get; set; }

    [JsonProperty("Modelo")]
    public string Modelo { get; set; }

    [JsonProperty("Placa")]
    public string Placa { get; set; }
}

public class NodoVehiculo
{
    public Vehiculo Vehiculo { get; set; }
    public NodoVehiculo Siguiente { get; set; }
    public NodoVehiculo Anterior { get; set; }

    public NodoVehiculo(Vehiculo vehiculo)
    {
        Vehiculo = vehiculo;
        Siguiente = null;
        Anterior = null;
    }
}

public class ListaDobleVehiculos
{
    private NodoVehiculo cabeza;
    private NodoVehiculo cola;

    // Instancia única para el patrón Singleton
    private static ListaDobleVehiculos instancia;

    // Constructor privado para evitar instanciación directa
    private ListaDobleVehiculos()
    {
        cabeza = null;
        cola = null;
    }

    // Método para obtener la instancia única
    public static ListaDobleVehiculos ObtenerInstancia()
    {
        if (instancia == null)
        {
            instancia = new ListaDobleVehiculos();
        }
        return instancia;
    }

    public void Agregar(Vehiculo vehiculo)
    {
        NodoVehiculo nuevoNodo = new NodoVehiculo(vehiculo);

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
            cola = nuevoNodo;
        }
        else
        {
            cola.Siguiente = nuevoNodo;
            nuevoNodo.Anterior = cola;
            cola = nuevoNodo;
        }

        Console.WriteLine($"Vehículo agregado: {vehiculo.Marca} {vehiculo.Modelo}, Placa: {vehiculo.Placa}");
    }

    public bool ExisteId(int id)
{
    NodoVehiculo actual = cabeza;

    while (actual != null)
    {
        if (actual.Vehiculo.Id == id)
        {
            return true; // El ID ya existe
        }
        actual = actual.Siguiente;
    }

    return false; // El ID no existe
}


public string GenerarGraphviz()
{
    string dot = "digraph Vehiculos {\n";
    dot += "    rankdir=LR;\n";
    dot += "    node [shape=record, style=filled, fillcolor=lightyellow];\n";

    NodoVehiculo actual = cabeza;
    int contador = 0;

    while (actual != null)
    {
        string nombreNodo = $"vehiculo{contador}";
        Vehiculo v = actual.Vehiculo;

        dot += $"    {nombreNodo} [label=\"{{ID: {v.Id} | Usuario: {v.IdUsuario} | {v.Marca} {v.Modelo} | Placa: {v.Placa}}}\"];\n";

        if (actual.Siguiente != null)
        {
            dot += $"    {nombreNodo} -> vehiculo{contador + 1} [dir=both];\n";
        }

        actual = actual.Siguiente;
        contador++;
    }

    dot += "}";
    return dot;
}

public void GraphvizVehiculo(string rutaDot, string rutaImagen)
{
    string dot = GenerarGraphviz();
    File.WriteAllText(rutaDot, dot);

    var process = new System.Diagnostics.Process();
    process.StartInfo.FileName = "dot";
    process.StartInfo.Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaImagen}\"";
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardError = true;

    process.Start();
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    if (!string.IsNullOrEmpty(error))
    {
        Console.WriteLine("Error Graphviz: " + error);
    }
    else
    {
        Console.WriteLine("Gráfico generado: " + rutaImagen);
    }
}



public bool EliminarPorId(int id)
{
    NodoVehiculo actual = cabeza;

    while (actual != null)
    {
        if (actual.Vehiculo.Id == id)
        {
            if (actual == cabeza)
            {
                cabeza = actual.Siguiente;
                if (cabeza != null)
                {
                    cabeza.Anterior = null;
                }
            }
            else if (actual == cola)
            {
                cola = actual.Anterior;
                if (cola != null)
                {
                    cola.Siguiente = null;
                }
            }
            else
            {
                actual.Anterior.Siguiente = actual.Siguiente;
                actual.Siguiente.Anterior = actual.Anterior;
            }
            return true; // Nodo eliminado
        }
        actual = actual.Siguiente;
    }
    return false; // Nodo no encontrado
}

public NodoVehiculo BuscarPorId(int id)
{
    NodoVehiculo actual = cabeza;
    while (actual != null)
    {
        if (actual.Vehiculo.Id == id)
        {
            return actual; // Nodo encontrado
        }
        actual = actual.Siguiente;
    }
    return null; // Nodo no encontrado
}

public bool ExisteUsuario(int idUsuario)
{
    ListaUsuarios listaUsuarios = ListaUsuarios.ObtenerInstancia();
    return listaUsuarios.UsuarioExiste(idUsuario);
}

    public void Imprimir()
    {
        NodoVehiculo actual = cabeza;

        if (actual == null)
        {
            Console.WriteLine("La lista está vacía.");
            return;
        }

        Console.WriteLine("Vehículos en la lista:");
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual.Vehiculo.Id}, Usuario: {actual.Vehiculo.IdUsuario}, Marca: {actual.Vehiculo.Marca}, Modelo: {actual.Vehiculo.Modelo}, Placa: {actual.Vehiculo.Placa}");
            actual = actual.Siguiente;
        }
    }
}