using System;
using Gtk;
using Pango;

//===============================INTERFAZ GRAFICA===============================
class Ingresomanual : Gtk.Window {
   

    public Ingresomanual() : base("MENU PRINCIPAL") {

        SetDefaultSize(600,500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix= new Fixed();
 //-----------------------------------BOTONES------------------------------------------------
        Button boton1 = new Button(" USUARIOS");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1,240, 70);

        boton1.Clicked += (sender, e) => {
            Usuarios vehiculos = new Usuarios();
            vehiculos.ShowAll();
            this.Hide();
        };

         //------------------------------
        Button button2 = new Button(" VEHICULOS ");
        button2.SetSizeRequest(100, 60);
        fix.Put(button2,235, 160);

         button2.Clicked += (sender, e) => {
            Vehiculos vehiculos = new Vehiculos();
            vehiculos.ShowAll();
            this.Hide();
        };
         //------------------------------
        Button button3 = new Button(" REPUESTOS");
        button3.SetSizeRequest(100, 60);
        fix.Put(button3,235, 260);

        button3.Clicked += (sender, e) => {
            RepuestosInterfaz vehiculos = new RepuestosInterfaz();
            vehiculos.ShowAll();
            this.Hide();
        };

         //------------------------------
        Button button4 = new Button(" SERVICIOS ");
        button4.SetSizeRequest(100, 60);
        fix.Put(button4,235, 350);

        button4.Clicked += (sender, e) => {
            ServicioIngreso vehiculos = new ServicioIngreso();
            vehiculos.ShowAll();
            this.Hide();
        };

         //------------------------------
        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 100, 40);

        button.Clicked += (sender, e) => {
            Principal manual = new Principal();
            manual.ShowAll();
            this.Hide();
        };
            Add(fix);
            ShowAll();

        }

            

       }