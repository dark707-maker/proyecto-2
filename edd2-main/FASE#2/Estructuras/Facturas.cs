using System;
using System.Collections.Generic;
using System.Text;

public class Elemento
{
    public int Id;
    public int Id_Cliente;
    public int Id_Servicio;
    public double Total;

    public Elemento(int id, int idCliente, int idServicio, double total)
    {
        Id = id;
        Id_Cliente = idCliente;
        Id_Servicio = idServicio;
        Total = total;
    }
}

public class NodoArbolB
{
    private const int ORDEN = 5;
    private const int MAX_CLAVES = ORDEN - 1;
    private const int MIN_CLAVES = (ORDEN / 2) - 1;

    public List<Elemento> Claves;
    public List<NodoArbolB> Hijos;
    public bool EsHoja;

    public NodoArbolB()
    {
        Claves = new List<Elemento>(MAX_CLAVES);
        Hijos = new List<NodoArbolB>(ORDEN);
        EsHoja = true;
    }

    public bool EstaLleno()
    {
        return Claves.Count >= MAX_CLAVES;
    }

    public bool TieneMinimoClaves()
    {
        return Claves.Count >= MIN_CLAVES;
    }
}

public class ArbolB
{
    private NodoArbolB raiz;
    private const int ORDEN = 5;
    private const int MAX_CLAVES = ORDEN - 1;
    private const int MIN_CLAVES = (ORDEN / 2) - 1;

    // Instancia única para el patrón Singleton
    private static ArbolB instancia;

    // Constructor privado para evitar instanciación directa
    private ArbolB()
    {
        raiz = new NodoArbolB();
    }

    // Método para obtener la instancia única
    public static ArbolB ObtenerInstancia()
    {
        if (instancia == null)
        {
            instancia = new ArbolB();
        }
        return instancia;
    }

    public void Insertar(int id, int idCliente, int idServicio)
    {
        // Validar que el ID de la factura sea único
        if (Buscar(id) != null)
        {
            Console.WriteLine($"Error: El ID {id} ya existe en el árbol.");
            return;
        }

        // Validar que el cliente exista
        ListaUsuarios listaUsuarios = ListaUsuarios.ObtenerInstancia();
        if (!listaUsuarios.UsuarioExiste(idCliente))
        {
            Console.WriteLine($"Error: El cliente con ID {idCliente} no existe.");
            return;
        }

        // Validar que el servicio exista
        ArbolServicios arbolServicios = ArbolServicios.ObtenerInstancia();
        NodoServicio servicio = arbolServicios.BuscarNodo(idServicio);
        if (servicio == null)
        {
            Console.WriteLine($"Error: El servicio con ID {idServicio} no existe.");
            return;
        }

        // Calcular el total (suma del costo del servicio y del repuesto)
        ListaRepuesto listaRepuestos = ListaRepuesto.ObtenerInstancia();
        NodoAVL repuesto = listaRepuestos.BuscarPorId(servicio.Id_Repuesto);
        if (repuesto == null)
        {
            Console.WriteLine($"Error: No se encontró el repuesto asociado al servicio.");
            return;
        }

        double total = servicio.Costo + repuesto.Objeto.Costo;

        // Crear el nuevo elemento
        Elemento nuevoElemento = new Elemento(id, idCliente, idServicio, total);

        // Insertar en el árbol
        if (raiz.EstaLleno())
        {
            NodoArbolB nuevaRaiz = new NodoArbolB();
            nuevaRaiz.EsHoja = false;
            nuevaRaiz.Hijos.Add(raiz);
            DividirHijo(nuevaRaiz, 0);
            raiz = nuevaRaiz;
        }

        InsertarNoLleno(raiz, nuevoElemento);
    }

    public void InsertarNoLleno(NodoArbolB nodo, Elemento elemento)
    {
        int i = nodo.Claves.Count - 1;

        if (nodo.EsHoja)
        {
            // Insertamos la clave
            while (i >= 0 && elemento.Id < nodo.Claves[i].Id)
            {
                i--;
            }
            nodo.Claves.Insert(i + 1, elemento);
        }
        else
        {
            // Encuentra el hijo donde debe estar el elemento
            while (i >= 0 && elemento.Id < nodo.Claves[i].Id)
            {
                i--;
            }
            i++;

            // Si el hijo está lleno
            if (nodo.Hijos[i].EstaLleno())
            {
                DividirHijo(nodo, i);
                if (elemento.Id > nodo.Claves[i].Id)
                {
                    i++;
                }
            }

            InsertarNoLleno(nodo.Hijos[i], elemento);
        }
    }

    public void DividirHijo(NodoArbolB padre, int indiceHijo)
    {
        NodoArbolB hijoCompleto = padre.Hijos[indiceHijo];
        NodoArbolB nuevoHijo = new NodoArbolB();

        nuevoHijo.EsHoja = hijoCompleto.EsHoja;

        // Elemento del medio que se promoverá al padre
        Elemento elementoMedio = hijoCompleto.Claves[MIN_CLAVES];

        // Mover la mitad de las claves
        for (int i = MIN_CLAVES + 1; i < MAX_CLAVES; i++)
        {
            nuevoHijo.Claves.Add(hijoCompleto.Claves[i]);
        }

        if (!hijoCompleto.EsHoja)
        {
            for (int i = (ORDEN / 2); i < ORDEN; i++)
            {
                nuevoHijo.Hijos.Add(hijoCompleto.Hijos[i]);
            }
            hijoCompleto.Hijos.RemoveRange((ORDEN / 2), hijoCompleto.Hijos.Count - (ORDEN / 2));
        }

        hijoCompleto.Claves.RemoveRange(MIN_CLAVES, hijoCompleto.Claves.Count - MIN_CLAVES);

        padre.Hijos.Insert(indiceHijo + 1, nuevoHijo);

        int j = 0;
        while (j < padre.Claves.Count && padre.Claves[j].Id < elementoMedio.Id)
        {
            j++;
        }

        padre.Claves.Insert(j, elementoMedio);
    }

    public Elemento Buscar(int id)
    {
        return BuscarRecursivo(raiz, id);
    }

    private Elemento BuscarRecursivo(NodoArbolB nodo, int id)
    {
        int i = 0;
        while (i < nodo.Claves.Count && id > nodo.Claves[i].Id)
        {
            i++;
        }

        if (i < nodo.Claves.Count && id == nodo.Claves[i].Id)
        {
            return nodo.Claves[i];
        }

        if (nodo.EsHoja)
        {
            return null;
        }

        return BuscarRecursivo(nodo.Hijos[i], id);
    }
}