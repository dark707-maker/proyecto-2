using System;
using Gtk;
using Pango;

class VisualizarMain : Gtk.Window
{
    public VisualizarMain() : base("VISUALIZAR")
    {
        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Button boton1 = new Button("Visualizar Repuestos");
        boton1.SetSizeRequest(150, 60);
        fix.Put(boton1, 250, 200);

        Button boton2 = new Button("Visualizar Servicios");
        boton2.SetSizeRequest(150, 60);
        fix.Put(boton2, 250, 300);

        Button button = new Button("Volver");
        button.SetSizeRequest(80, 50);
        fix.Put(button, 600, 40);
        button.Clicked += (sender, e) => {

            Principalus manual = new Principalus();
            manual.ShowAll();
            this.Hide();
            
        };

        Add(fix);
        ShowAll();
    }
}
