using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


class Principalus : Gtk.Window {

    ListaUsuarios listaUsuarios = ListaUsuarios.ObtenerInstancia();
    ListaDobleVehiculos listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();

    public Principalus() : base("MENU PRINCIPAL") {
        
        SetDefaultSize(700,600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

//-----------------------------------BOTONES------------------------------------------------
        Button boton1 = new Button(" Insertar Vehiculos ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 325, 80);

        boton1.Clicked += (sender, e) => {
            int idUsuarioActual = 1; 
            InsertarVehiculos ventanaVehiculos = new InsertarVehiculos(idUsuarioActual);
            ventanaVehiculos.ShowAll();
            this.Destroy();
        };

        //------------------------------

        Button boton2 = new Button("Visualizar ");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 320, 180);

        boton2.Clicked += (sender, e) => {
            VisualizarMain vehiculo = new VisualizarMain();
            vehiculo.ShowAll();
            this.Destroy();
        };

        //------------------------------

        Button boton3 = new Button(" Genearar Facturas");
        boton3.SetSizeRequest(100, 60);
        fix.Put(boton3, 310, 270);

        boton3.Clicked += (sender, e) => {
            Cancelar vehiculo = new Cancelar();
            vehiculo.ShowAll();
            this.Destroy();
        };

        //------------------------------

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 620, 40);
        button.Clicked += (sender, e) => {
            RegistrarSalida(Login.UsuarioActual);
            Login manual = new Login();
            manual.ShowAll();
            this.Destroy();
        };

        Add(fix);
        ShowAll();
    }

    public void RegistrarSalida(string correo)
    {
        string horaSalida = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string log = $"{{ \"usuario\": \"{correo}\", \"salida\": \"{horaSalida}\" }}";
        System.IO.File.AppendAllText("Control_log.json", log + Environment.NewLine);
    }

}