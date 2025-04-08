﻿using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

//===============================INTERFAZ GRAFICA===============================
class Principal : Gtk.Window {

    public Principal() : base("MENU PRINCIPAL") {
        ListaUsuarios.Instancia.Imprimir();
        ListaRepuestos.Instancia.Imprimir();
        ListaVehiculos.Instancia.Imprimir();
        ColaServicios.Instancia.Imprimir();
        PilaFacturas.Instancia.IMPRIMIR();
       
        
        SetDefaultSize(800,700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };


        Fixed fix = new Fixed();

//-----------------------------------BOTONES------------------------------------------------
        Button boton1 = new Button(" Cargas Masivas ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 325, 80);

        boton1.Clicked += (sender, e) => {
            Cargas cargasMasivas = new Cargas();
            cargasMasivas.ShowAll();
            this.Hide();
        };
        //------------------------------

        Button boton2 = new Button("Ingreso Individual");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 320, 180);

        boton2.Clicked += (sender, e) => {
            Ingresomanual ingresoIndividual = new Ingresomanual();
            ingresoIndividual.ShowAll();
            this.Hide();
        };

         //------------------------------

        Button boton3 = new Button(" Gestion de Usuarios ");
        boton3.SetSizeRequest(100, 60);
        fix.Put(boton3, 310, 270);

         boton3.Clicked += (sender, e) => {
            Editor ingresoIndividual = new Editor();
            ingresoIndividual.ShowAll();
            this.Hide();
        };
        //------------------------------

        Button boton4 = new Button(" Generar Servicio ");
        boton4.SetSizeRequest(100, 60);
        fix.Put(boton4, 320, 370);

         boton4.Clicked += (sender, e) => {
            ServicioIngreso ingresoIndividual = new ServicioIngreso();
            ingresoIndividual.ShowAll();
            this.Hide();
        };

         //------------------------------
        Button boton5 = new Button(" Cancelar Factura" );
        boton5.SetSizeRequest(100, 60);
        fix.Put(boton5, 320, 480);

         boton5.Clicked += (sender, e) => {
            Cancelar ingresoIndividual = new Cancelar();
            ingresoIndividual.ShowAll();
            this.Hide();
        };

         //------------------------------
        Button Botongraficas  = new Button(" Graficas ");
        Botongraficas .SetSizeRequest(100, 60);
        fix.Put(Botongraficas , 100, 280);

        Botongraficas.Clicked += (sender, e) => {
            Graficas graficas = new Graficas();
            graficas.ShowAll();
            this.Hide();

        };
         //------------------------------

         Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 650, 40);
        button.Clicked += (sender, e) => {
            Login manual = new Login();
            manual.ShowAll();
            this.Hide();
        };

         
        
        Add(fix);
        ShowAll();


    }

  
  }
