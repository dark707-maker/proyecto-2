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
    private static int contadorFacturas = 1; 

    public ArbolServicios() {
        root = null;
    }

    public static ArbolServicios ObtenerInstancia() {
        if (instancia == null) {
            instancia = new ArbolServicios();
        }
        return instancia;
    }

    public void Agregar(int id, int idRepuesto, int idVehiculo, string detalles, double costo) {
        NodoServicio nuevoNodo = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);
        if (root == null) {
            root = nuevoNodo;
        } else {
            AgregarRecursivo(root, nuevoNodo);
        }


    }

       private void GenerarFacturaAutomatica(int idServicio, int idVehiculo, double costoServicio, int idRepuesto)
{
    // Validar que el vehículo exista y obtener el usuario asociado
    ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();
    var vehiculo = listaVehiculos.BuscarPorId(idVehiculo);
    if (vehiculo == null)
    {
        Console.WriteLine($"Error: El ID del vehículo {idVehiculo} no existe.");
        return;
    }

    int idUsuario = vehiculo.Vehiculo.IdUsuario; // Obtener el usuario asociado al vehículo

    // Validar que el repuesto exista y obtener su costo
    ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia();
    var nodoRepuesto = arbolRepuestos.BuscarPorId(idRepuesto);
    if (nodoRepuesto == null)
    {
        Console.WriteLine($"Error: El ID del repuesto {idRepuesto} no existe.");
        return;
    }

    double costoRepuesto = nodoRepuesto.Objeto.Costo; // Obtener el costo del repuesto

    // Calcular el total como la suma del costo del servicio y el costo del repuesto
    double total = costoServicio + costoRepuesto;

    // Verificar los valores de costoServicio y costoRepuesto
    Console.WriteLine($"Costo del servicio: {costoServicio}");
    Console.WriteLine($"Costo del repuesto: {costoRepuesto}");
    Console.WriteLine($"Total calculado: {total}");

    // Generar la factura
    ArbolB arbolFacturas = ArbolB.ObtenerInstancia(); // Usar el método Singleton
    arbolFacturas.Insertar(contadorFacturas, idUsuario, idServicio, total);

    Console.WriteLine($"Factura generada automáticamente:");
    Console.WriteLine($"ID Factura: {contadorFacturas}, ID Usuario: {idUsuario}, ID Servicio: {idServicio}, Total: {total}");

    // Incrementar el contador de facturas
    contadorFacturas++;
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

    public NodoServicio? BuscarPorId(int id)
{
    return BuscarRecursivo(root, id);
}

private NodoServicio? BuscarRecursivo(NodoServicio? nodo, int id)
{
    if (nodo == null) return null;

    if (nodo.Id == id) return nodo;

    if (id < nodo.Id)
    {
        return BuscarRecursivo(nodo.Izquierda, id);
    }
    else
    {
        return BuscarRecursivo(nodo.Derecha, id);
    }
}
    // Recorrido In-Orden
   public void PreOrden(List<NodoServicio> servicios)
{
    PreOrdenRecursivo(root, servicios);
}

private void PreOrdenRecursivo(NodoServicio? actual, List<NodoServicio> servicios)
{
    if (actual == null) return;
    servicios.Add(actual);
    PreOrdenRecursivo(actual.Izquierda, servicios);
    PreOrdenRecursivo(actual.Derecha, servicios);
}

public void InOrden(List<NodoServicio> servicios)
{
    InOrdenRecursivo(root, servicios);
}

private void InOrdenRecursivo(NodoServicio? actual, List<NodoServicio> servicios)
{
    if (actual == null) return;
    InOrdenRecursivo(actual.Izquierda, servicios);
    servicios.Add(actual);
    InOrdenRecursivo(actual.Derecha, servicios);
}

public void PostOrden(List<NodoServicio> servicios)
{
    PostOrdenRecursivo(root, servicios);
}

private void PostOrdenRecursivo(NodoServicio? actual, List<NodoServicio> servicios)
{
    if (actual == null) return;
    PostOrdenRecursivo(actual.Izquierda, servicios);
    PostOrdenRecursivo(actual.Derecha, servicios);
    servicios.Add(actual);
}

    public void Imprimir()
{
    ImprimirRecursivo(root);
}

private void ImprimirRecursivo(NodoServicio? nodo)
{
    if (nodo == null) return;

    // Recorrido In-Orden para imprimir los servicios
    ImprimirRecursivo(nodo.Izquierda);
    Console.WriteLine($"ID: {nodo.Id}");
    Console.WriteLine($"ID Repuesto: {nodo.Id_Repuesto}");
    Console.WriteLine($"ID Vehículo: {nodo.Id_Vehiculo}");
    Console.WriteLine($"Detalles: {nodo.Detalles}");
    Console.WriteLine($"Costo: {nodo.Costo}");
    Console.WriteLine("------------------------------");
    ImprimirRecursivo(nodo.Derecha);
}
}


