using System;
using System.Collections.Generic;

public class Elemento
{
    public int Id_Factura;
    public int Id_Usuario;
    public int Id_Servicio;
    public double Total;

    public Elemento(int idFactura, int idUsuario, int idServicio, double total)
    {
        Id_Factura = idFactura;
        Id_Usuario = idUsuario;
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
    private static ArbolB instancia; // Instancia única de la clase
    private NodoArbolB raiz;
    private const int ORDEN = 5;
    private const int MAX_CLAVES = ORDEN - 1;
    private const int MIN_CLAVES = (ORDEN / 2) - 1;

    // Constructor privado para evitar instanciación directa
    private ArbolB()
    {
        raiz = new NodoArbolB();
    }

    // Método estático para obtener la instancia única
    public static ArbolB ObtenerInstancia()
    {
        if (instancia == null)
        {
            instancia = new ArbolB();
        }
        return instancia;
    }

    public void Insertar(int idFactura, int idUsuario, int idServicio, double total)
    {
        Elemento nuevoElemento = new Elemento(idFactura, idUsuario, idServicio, total);

        // Validación: Verificar si el ID de la factura ya existe
        if (Buscar(idFactura) != null)
        {
            Console.WriteLine($"El ID de la factura {idFactura} ya existe.");
            return;
        }

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
            while (i >= 0 && elemento.Id_Factura < nodo.Claves[i].Id_Factura)
            {
                i--;
            }
            nodo.Claves.Insert(i + 1, elemento);
        }
        else
        {
            while (i >= 0 && elemento.Id_Factura < nodo.Claves[i].Id_Factura)
            {
                i--;
            }
            i++;

            if (nodo.Hijos[i].EstaLleno())
            {
                DividirHijo(nodo, i);
                if (elemento.Id_Factura > nodo.Claves[i].Id_Factura)
                {
                    i++;
                }
            }

            InsertarNoLleno(nodo.Hijos[i], elemento);
        }
    }

    public void Imprimir()
    {
        Console.WriteLine("Facturas almacenadas en el árbol B:");
        ImprimirRecursivo(raiz);
    }

    private void ImprimirRecursivo(NodoArbolB nodo)
    {
        if (nodo == null) return;

        for (int i = 0; i < nodo.Claves.Count; i++)
        {
            // Imprimir los hijos izquierdos antes de la clave actual
            if (!nodo.EsHoja)
            {
                ImprimirRecursivo(nodo.Hijos[i]);
            }

            // Imprimir la clave actual
            var factura = nodo.Claves[i];
            Console.WriteLine($"ID Factura: {factura.Id_Factura}, ID Usuario: {factura.Id_Usuario}, ID Servicio: {factura.Id_Servicio}, Total: {factura.Total}");
        }

        // Imprimir el último hijo derecho
        if (!nodo.EsHoja)
        {
            ImprimirRecursivo(nodo.Hijos[nodo.Claves.Count]);
        }
    }

    public Elemento Buscar(int idFactura)
    {
        return BuscarRecursivo(raiz, idFactura);
    }

    private Elemento BuscarRecursivo(NodoArbolB nodo, int idFactura)
    {
        int i = 0;
        while (i < nodo.Claves.Count && idFactura > nodo.Claves[i].Id_Factura)
        {
            i++;
        }

        if (i < nodo.Claves.Count && idFactura == nodo.Claves[i].Id_Factura)
        {
            return nodo.Claves[i];
        }

        if (nodo.EsHoja)
        {
            return null;
        }

        return BuscarRecursivo(nodo.Hijos[i], idFactura);
    }

    public List<Elemento> ObtenerFacturasPorUsuario(int idUsuario)
{
    List<Elemento> facturasUsuario = new List<Elemento>();
    ObtenerFacturasPorUsuarioRecursivo(raiz, idUsuario, facturasUsuario);
    return facturasUsuario;
}

private void ObtenerFacturasPorUsuarioRecursivo(NodoArbolB nodo, int idUsuario, List<Elemento> facturasUsuario)
{
    if (nodo == null) return;

    foreach (var clave in nodo.Claves)
    {
        if (clave.Id_Usuario == idUsuario)
        {
            facturasUsuario.Add(clave);
        }
    }

    if (!nodo.EsHoja)
    {
        foreach (var hijo in nodo.Hijos)
        {
            ObtenerFacturasPorUsuarioRecursivo(hijo, idUsuario, facturasUsuario);
        }
    }
}

    public void DividirHijo(NodoArbolB padre, int indiceHijo)
    {
        NodoArbolB hijoCompleto = padre.Hijos[indiceHijo];
        NodoArbolB nuevoHijo = new NodoArbolB();

        nuevoHijo.EsHoja = hijoCompleto.EsHoja;

        Elemento elementoMedio = hijoCompleto.Claves[MIN_CLAVES];

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
        while (j < padre.Claves.Count && padre.Claves[j].Id_Factura < elementoMedio.Id_Factura)
        {
            j++;
        }

        padre.Claves.Insert(j, elementoMedio);
    }
}