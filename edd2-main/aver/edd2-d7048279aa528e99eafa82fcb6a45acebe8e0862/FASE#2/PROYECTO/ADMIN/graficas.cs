using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;

//=============================================================================
//===============================INTERFAZ GRAFICA===============================

class Graficas : Gtk.Window {
 
 ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();
    ListaUsuarios listaUsuarios = ListaUsuarios.ObtenerInstancia();
    ArbolServicios arbolServicios = ArbolServicios.ObtenerInstancia();
    ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia();
    ArbolB arbolFacturas = ArbolB.ObtenerInstancia();

    public Graficas() : base(" GRAFICAS ") {

        SetDefaultSize(600, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        //-----------------------------------BOTONES------------------------------------------------
        Button boton1 = new Button(" USUARIOS ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 90, 100);

        boton1.Clicked += (sender, e) => {
           listaUsuarios= ListaUsuarios.ObtenerInstancia();
            string dot = listaUsuarios.GenerarGraphviz();
            string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "usuarios.dot");
            File.WriteAllText(rutaArchivo, dot);
            Console.WriteLine("Archivo DOT generado en el escritorio.");
            // Mostrar el archivo DOT en una ventana emergente o abrirlo con un visor de archivos DOT
            // Aquí puedes agregar el código para abrir el archivo o mostrarlo en una ventana emergente

        };

        //---------
        Button boton2 = new Button(" VEHICULOS ");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 250, 100);

        boton2.Clicked += (sender, e) => {
           
            
        };

        //---------
        Button boton3 = new Button(" REPUESTOS ");
        boton3.SetSizeRequest(100, 60);
        fix.Put(boton3, 390, 100);

        boton3.Clicked += (sender, e) => {
            
            
        };

        //---------
        Button boton4 = new Button(" SERVICIOS ");
        boton4.SetSizeRequest(100, 60);
        fix.Put(boton4, 80, 250);

        boton4.Clicked += (sender, e) => {
            
            
        };
        //---------
        Button boton5 = new Button(" FACTURACION ");
        boton5.SetSizeRequest(100, 60);
        fix.Put(boton5, 250, 250);

        boton5.Clicked += (sender, e) => {
            
        };

        //----------------volver---------------------

        Button button = new Button("Volver");
        button.SetSizeRequest(100, 30);
        fix.Put(button, 450, 25);
        button.Clicked += (sender, e) => {

            Principaladmin manual = new Principaladmin();
            manual.ShowAll();
            this.Hide();

            
        };

        Add(fix);
        ShowAll();
    }
}