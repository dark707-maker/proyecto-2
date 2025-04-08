using System;

public class Usuario
{
    public int Id { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public int Edad { get; set; }
    public string Contrasenia { get; set; }
    public Usuario Siguiente { get; set; }

    public Usuario(int id, string nombres, string apellidos, string correo, int edad, string contrasenia)
    {
        Id = id;
        Nombres = nombres;
        Apellidos = apellidos;
        Correo = correo;
        Edad = edad;
        Contrasenia = contrasenia;
        Siguiente = null;
    }
}

public class ListaUsuarios
{
    private static ListaUsuarios instancia;
    private Usuario cabeza;

    private ListaUsuarios()
    {
        cabeza = null;
    }

    public static ListaUsuarios ObtenerInstancia()
    {
        if (instancia == null)
        {
            instancia = new ListaUsuarios();
        }
        return instancia;
    }

  public Usuario ObtenerUsuarioPorCorreo(string correo)
{
    Usuario actual = cabeza;
    while (actual != null)
    {
        if (actual.Correo == correo)
        {
            return actual; 
        }
        actual = actual.Siguiente;
    }
    return null; 
}

    private bool EsIdUnico(int id)
    {
        Usuario actual = cabeza;
        while (actual != null)
        {
            if (actual.Id == id) return false;
            actual = actual.Siguiente;
        }
        return true;
    }

    private bool EsCorreoUnico(string correo)
    {
        Usuario actual = cabeza;
        while (actual != null)
        {
            if (actual.Correo == correo) return false;
            actual = actual.Siguiente;
        }
        return true;
    }

    public bool UsuarioExiste(int idUsuario)
    {
        return BuscarUsuario(idUsuario) != null;
    }

    public bool AgregarUsuario(Usuario nuevoUsuario)
    {
        if (!EsIdUnico(nuevoUsuario.Id))
        {
            Console.WriteLine("Error: El ID ya está en uso.");
            return false;
        }

        if (!EsCorreoUnico(nuevoUsuario.Correo))
        {
            Console.WriteLine("Error: El correo ya está en uso.");
            return false;
        }

        if (cabeza == null)
        {
            cabeza = nuevoUsuario;
        }
        else
        {
            Usuario actual = cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoUsuario;
        }

        Console.WriteLine("Usuario agregado correctamente.");
        return true;
    }

    public bool EliminarUsuario(int id)
    {
        if (cabeza == null) return false;

        if (cabeza.Id == id)
        {
            cabeza = cabeza.Siguiente;
            return true;
        }

        Usuario actual = cabeza;
        while (actual.Siguiente != null && actual.Siguiente.Id != id)
        {
            actual = actual.Siguiente;
        }

        if (actual.Siguiente == null) return false;

        actual.Siguiente = actual.Siguiente.Siguiente;
        return true;
    }

   public Usuario BuscarUsuario(int id)
{
    Usuario actual = cabeza;
    while (actual != null)
    {
        if (actual.Id == id)
        {
            return actual; // Usuario encontrado
        }
        actual = actual.Siguiente;
    }
    return null; // Usuario no encontrado
}
public Usuario ObtenerUsuarioPorId(int id)
{
    Usuario actual = cabeza;
    while (actual != null)
    {
        if (actual.Id == id)
        {
            return actual; // Usuario encontrado
        }
        actual = actual.Siguiente;
    }
    return null; // Usuario no encontrado
}

    public void Imprimir()
    {
        Usuario actual = cabeza;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual.Id}");
            Console.WriteLine($"Nombres: {actual.Nombres}");
            Console.WriteLine($"Apellidos: {actual.Apellidos}");
            Console.WriteLine($"Correo: {actual.Correo}");
            Console.WriteLine($"Edad: {actual.Edad}");
            Console.WriteLine($"Contraseña: {actual.Contrasenia}");
            Console.WriteLine("-----------------------------");
            actual = actual.Siguiente;
        }
    }
}
