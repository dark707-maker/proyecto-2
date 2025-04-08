using Gtk;
using Gdk;
using System;
using System.IO;
using System.Collections.Generic;

//=============================================================================
//===============================INTERFAZ GRAFICA===============================

class Graficas : Gtk.Window {
    private ListaUsuarios listaUsuarios = ListaUsuarios.Instancia;
    private ListaVehiculos listaVehiculos = ListaVehiculos.Instancia;
    ListaRepuestos listaRepuestos = ListaRepuestos.Instancia;
    ColaServicios cola = ColaServicios.Instancia;
    PilaFacturas pilaFacturas = PilaFacturas.Instancia;
    MatrizDispersa matrizDispersa = MatrizDispersa.GetInstance();

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
            listaUsuarios.GenerarReporte();
            Console.WriteLine("Reporte generado");
        };

        //---------
        Button boton2 = new Button(" VEHICULOS ");
        boton2.SetSizeRequest(100, 60);
        fix.Put(boton2, 250, 100);

        boton2.Clicked += (sender, e) => {
            listaVehiculos.GraficoVehiculos();
            Console.WriteLine("Reporte generado");
        };

        //---------
        Button boton3 = new Button(" REPUESTOS ");
        boton3.SetSizeRequest(100, 60);
        fix.Put(boton3, 390, 100);

        boton3.Clicked += (sender, e) => {
            listaRepuestos.GraficoRepuestos();
            Console.WriteLine("Reporte generado");
        };

        //---------
        Button boton4 = new Button(" SERVICIOS ");
        boton4.SetSizeRequest(100, 60);
        fix.Put(boton4, 80, 250);

        boton4.Clicked += (sender, e) => {
            cola.GraficaCola();
            Console.WriteLine("Reporte generado");
        };
        //---------
        Button boton5 = new Button(" FACTURACION ");
        boton5.SetSizeRequest(100, 60);
        fix.Put(boton5, 250, 250);

        boton5.Clicked += (sender, e) => {
            pilaFacturas.GraficoFacturas();
            Console.WriteLine("Reporte generado");
        };

        //---------
        Button Botongraficas = new Button(" BITACORA ");
        Botongraficas.SetSizeRequest(100, 60);
        fix.Put(Botongraficas, 400, 250);

        Botongraficas.Clicked += (sender, e) => {

                try
            {
             matrizDispersa.GraficaMatriz();
        Console.WriteLine("Reporte generado");
                }
            catch (Exception ex)
            {
        Console.WriteLine($"Error al generar la bitácora: {ex.Message}");
                    }
                };

                //------------TOPS-----------------

        Button Botontops = new Button(" TOPS ");
        Botontops.SetSizeRequest(100, 60);
        fix.Put(Botontops, 250, 350);

        Botontops.Clicked += (sender, e) => {
            
             listaVehiculos.TopAntiguo();
             Console.WriteLine("-===============================");
             Console.WriteLine("================================");
            listaVehiculos.VehiculosMasServicios(ColaServicios.Instancia);

        };
        //----------------volver---------------------

        Button button = new Button("Volver");
        button.SetSizeRequest(100, 30);
        fix.Put(button, 450, 25);
        button.Clicked += (sender, e) => {
            Principal principal = new Principal();
            principal.ShowAll();
            this.Hide();
        };

        Add(fix);
        ShowAll();
    }
}