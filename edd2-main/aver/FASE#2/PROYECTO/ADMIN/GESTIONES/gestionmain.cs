using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

//===============================INTERFAZ GRAFICA===============================
class gestionesmainA: Gtk.Window {

    public gestionesmainA() : base(" GESTIONES ") {
        
        SetDefaultSize(700,600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };


        Fixed fix = new Fixed();

        //-----------------------------------BOTONES------------------------------------------------
        
        // Botón Gestión Usuarios
        Button boton1 = new Button(" Gestion Usuarios ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 325, 80);

        boton1.Clicked += (sender, e) => {
        GestionUsuarios gestionusu = new GestionUsuarios(); // Cambiado a GestionUsuarios
        gestionusu.ShowAll();   
        this.Hide();
    };

        //------------------------------

        // Botón Gestión Vehículos
        Button boton2 = new Button(" Gestion Vehiculos");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 320, 180);

        boton2.Clicked += (sender, e) => {
            GestionVehiculoss gestionVehiculos = new GestionVehiculoss();
            gestionVehiculos.ShowAll();
            this.Hide();
        };

        //------------------------------

        // Botón Volver
        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 620, 40);
        button.Clicked += (sender, e) => {
            Principaladmin principal = new Principaladmin();
            principal.ShowAll();  // Fixed the typo here
            this.Hide();
        };

        // Agregar contenedor a la ventana
        Add(fix);
        ShowAll();
    }
}
