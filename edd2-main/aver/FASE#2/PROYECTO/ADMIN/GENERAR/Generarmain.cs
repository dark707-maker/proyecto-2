using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

//===============================INTERFAZ GRAFICA===============================
class generarmain : Gtk.Window {

    public generarmain() : base(" GESTIONES ") {
        
        SetDefaultSize(700,600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        //-----------------------------------BOTONES------------------------------------------------
        
        // Botón Generar Servicio
        Button boton1 = new Button(" Generar Servicio ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 325, 80);

        boton1.Clicked += (sender, e) => {
            GenerarServicios ventana = new GenerarServicios();
            ventana.ShowAll();  
            this.Hide();
        };

        //------------------------------

        // Botón Generar Factura
        Button boton2 = new Button(" Generar Factura");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 320, 180);

        boton2.Clicked += (sender, e) => {
            GeneracionFacturas ventana1 = new GeneracionFacturas();
            ventana1.ShowAll();
            this.Hide();
        };

        //------------------------------

        // Botón Volver
        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 620, 40);
        button.Clicked += (sender, e) => {
            Principaladmin manual = new Principaladmin();
            manual.ShowAll();
            this.Hide();
        };

        // Agregar contenedor a la ventana
        Add(fix);
        ShowAll();
    }
}
