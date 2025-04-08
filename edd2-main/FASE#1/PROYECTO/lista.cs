using System;

unsafe class Nodo
{
    public int valor;
    public Nodo* siguiente;
}

unsafe class ListaCircular
{
    private Nodo* cabeza = null;

    public void Insertar(int valor)
    {
        Nodo* nuevo = (Nodo*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(Nodo));
        nuevo->valor = valor;
        nuevo->siguiente = null;

        if (cabeza == null)
        {
            cabeza = nuevo;
            cabeza->siguiente = cabeza; // Se apunta a sí mismo
        }
        else
        {
            Nodo* temp = cabeza;
            while (temp->siguiente != cabeza)
            {
                temp = temp->siguiente;
            }
            temp->siguiente = nuevo;
            nuevo->siguiente = cabeza;
        }
    }

    public void Mostrar()
    {
        if (cabeza == null)
        {
            Console.WriteLine("Lista vacía.");
            return;
        }

        Nodo* temp = cabeza;
        do
        {
            Console.Write(temp->valor + " -> ");
            temp = temp->siguiente;
        } while (temp != cabeza);
        Console.WriteLine("(vuelta a inicio)");
    }

    public void Eliminar(int valor)
    {
        if (cabeza == null)
            return;

        Nodo* actual = cabeza, previo = null;
        do
        {
            if (actual->valor == valor)
            {
                if (previo != null)
                    previo->siguiente = actual->siguiente;
                else
                {
                    Nodo* temp = cabeza;
                    while (temp->siguiente != cabeza)
                        temp = temp->siguiente;
                    cabeza = cabeza->siguiente;
                    temp->siguiente = cabeza;
                }

                System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)actual);
                return;
            }
            previo = actual;
            actual = actual->siguiente;
        } while (actual != cabeza);
    }
}


