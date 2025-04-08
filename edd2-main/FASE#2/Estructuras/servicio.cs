using System;

class NodoServicio {
    public int Id;
    public int Id_Repuesto;
    public int Id_Vehiculo;
    public string Detalles;
    public double Costo;
    public NodoServicio? Izquierda;
    public NodoServicio? Derecha;

    public NodoServicio(int id, int idRepuesto, int idVehiculo, string detalles, double costo) {
        Id = id;
        Id_Repuesto = idRepuesto;
        Id_Vehiculo = idVehiculo;
        Detalles = detalles;
        Costo = costo;
        Izquierda = null;
        Derecha = null;
    }
}

class ArbolServicios {
    private NodoServicio root;
    private static ArbolServicios instancia;

    // Constructor privado para el patrón Singleton
    private ArbolServicios() {
        root = null;
    }

    // Método para obtener la única instancia del árbol
    public static ArbolServicios ObtenerInstancia() {
        if (instancia == null) {
            instancia = new ArbolServicios();
        }
        return instancia;
    }

    // Método para agregar un servicio al árbol
    public void Agregar(int id, int idRepuesto, int idVehiculo, string detalles, double costo) {
        NodoServicio nuevoNodo = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);
        if (root == null) {
            root = nuevoNodo;
        } else {
            AgregarRecursivo(root, nuevoNodo);
        }
    }

    private void AgregarRecursivo(NodoServicio actual, NodoServicio nuevoNodo) {
        if (nuevoNodo.Id < actual.Id) {
            if (actual.Izquierda == null) {
                actual.Izquierda = nuevoNodo;
            } else {
                AgregarRecursivo(actual.Izquierda, nuevoNodo);
            }
        } else if (nuevoNodo.Id > actual.Id) {
            if (actual.Derecha == null) {
                actual.Derecha = nuevoNodo;
            } else {
                AgregarRecursivo(actual.Derecha, nuevoNodo);
            }
        } else {
            Console.WriteLine("El servicio con el ID especificado ya existe.");
        }
    }

    // Recorrido In-Orden
    public void InOrden() {
        Console.WriteLine("Recorrido In-Orden:");
        InOrdenRecursivo(root);
    }

    private void InOrdenRecursivo(NodoServicio? actual) {
        if (actual == null) return;
        InOrdenRecursivo(actual.Izquierda);
        Imprimir(actual);
        InOrdenRecursivo(actual.Derecha);
    }

    // Recorrido Pre-Orden
    public void PreOrden() {
        Console.WriteLine("Recorrido Pre-Orden:");
        PreOrdenRecursivo(root);
    }

    private void PreOrdenRecursivo(NodoServicio? actual) {
        if (actual == null) return;
        Imprimir(actual);
        PreOrdenRecursivo(actual.Izquierda);
        PreOrdenRecursivo(actual.Derecha);
    }

    // Recorrido Post-Orden
    public void PostOrden() {
        Console.WriteLine("Recorrido Post-Orden:");
        PostOrdenRecursivo(root);
    }

    private void PostOrdenRecursivo(NodoServicio? actual) {
        if (actual == null) return;
        PostOrdenRecursivo(actual.Izquierda);
        PostOrdenRecursivo(actual.Derecha);
        Imprimir(actual);
    }

    public void ImprimirNodo(NodoServicio nodo) {
    Imprimir(nodo);
}

    // Método para imprimir los detalles de un servicio
    private void Imprimir(NodoServicio servicio) {
        Console.WriteLine($"ID: {servicio.Id}");
        Console.WriteLine($"ID Repuesto: {servicio.Id_Repuesto}");
        Console.WriteLine($"ID Vehículo: {servicio.Id_Vehiculo}");
        Console.WriteLine($"Detalles: {servicio.Detalles}");
        Console.WriteLine($"Costo: {servicio.Costo}");
        Console.WriteLine("------------------------------");
    }
}