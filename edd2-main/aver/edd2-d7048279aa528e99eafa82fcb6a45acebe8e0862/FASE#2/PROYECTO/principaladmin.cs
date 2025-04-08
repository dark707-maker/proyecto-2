using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

//===============================INTERFAZ GRAFICA===============================
class Principaladmin : Gtk.Window {

    public ListaUsuarios listaUsuarios;
    public ListaDobleVehiculos listaVehiculos;

    public ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia(); 
    
    public ArbolServicios arbolServicios = ArbolServicios.ObtenerInstancia(); 

    public ArbolB arbolFacturas = ArbolB.ObtenerInstancia(); 

    public Principaladmin() : base("MENU PRINCIPAL") {


        listaUsuarios = ListaUsuarios.ObtenerInstancia();
        listaUsuarios.Imprimir();

        listaVehiculos = ListaDobleVehiculos.ObtenerInstancia();
        listaVehiculos.Imprimir();

        ArbolAVL arbolRepuestos = ArbolAVL.ObtenerInstancia();
        arbolRepuestos.Imprimirmetodo();

        arbolFacturas = ArbolB.ObtenerInstancia();
        arbolFacturas.Imprimir();

        arbolServicios.Imprimir();

        //==============================================================
        SetDefaultSize(800, 700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        //-----------------------------------BOTONES------------------------------------------------
        
        // Botón Cargas Masivas
        Button boton1 = new Button(" Cargas Masivas ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 325, 80);

        boton1.Clicked += (sender, e) => {
            CargaMasiva carga = new CargaMasiva();
            carga.ShowAll();
            this.Destroy();
        };

        // Botón GESTIONES
        Button boton2 = new Button("GESTIONES");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 320, 180);

        boton2.Clicked += (sender, e) => {
            gestionesmainA gestion = new gestionesmainA();
            gestion.ShowAll();
            this.Destroy();
        };

        // Botón Actualizar Repuestos
        Button boton3 = new Button(" Actualizar Repuestos ");
        boton3.SetSizeRequest(100, 60);
        fix.Put(boton3, 310, 270);

        boton3.Clicked += (sender, e) => {
            Actualizar actualizar = new Actualizar();
            actualizar.ShowAll();
            this.Destroy();
        };

        // Botón Visualizar Repuestos
        Button boton4 = new Button(" Visualizar Repuestos");
        boton4.SetSizeRequest(100, 60);
        fix.Put(boton4, 320, 370);

        boton4.Clicked += (sender, e) => {
            Visualizar visualizar = new Visualizar();
            visualizar.ShowAll();
            this.Destroy();
        };

        // Botón Generar Servicios
        Button boton5 = new Button(" Generar servicios" );
        boton5.SetSizeRequest(100, 60);
        fix.Put(boton5, 320, 480);

        boton5.Clicked += (sender, e) => {
            generarmain  generar = new generarmain();
            generar.ShowAll();
            this.Destroy();
        };

        // Botón Log Entradas
        Button Botonentradas  = new Button(" Log entradas ");
        Botonentradas .SetSizeRequest(100, 60);
        fix.Put(Botonentradas , 320, 550);

        Botonentradas.Clicked += (sender, e) => {
            Entradas entradas = new Entradas();
            entradas.ShowAll();
            this.Destroy();
        };

        // Botón Reportes
        Button Botongraficas  = new Button(" Reportes ");
        Botongraficas.SetSizeRequest(100, 60);
        fix.Put(Botongraficas, 100, 280);

        Botongraficas.Clicked += (sender, e) => {
            Graficasvarias graficas = new Graficasvarias();
            graficas.ShowAll();
            this.Destroy();
        };

        // Botón Volver
        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 650, 40);

        button.Clicked += (sender, e) => {
            Login manual = new Login();
            manual.ShowAll();
            this.Destroy();
        };

        
        Add(fix);
        ShowAll();
    }
}
