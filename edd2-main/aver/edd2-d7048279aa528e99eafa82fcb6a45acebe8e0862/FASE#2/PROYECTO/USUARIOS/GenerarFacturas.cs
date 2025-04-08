using System;
using Gtk;
using Pango;


//-----------------------------------------------------------------------------
//-----------------------------------INTERFAZ GRAFICA-------------------------- 
//-----------------------------------------------------------------------------

class Cancelar : Gtk.Window
{
    public Cancelar() : base("CANCELAR")
    {
        SetDefaultSize(600, 500);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("FACTURACION");
        label1.ModifyFont(FontDescription.FromString("Arial 30"));
        fix.Put(label1, 170, 40);

        Label label2 = new Label("ID");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 180, 150);

        Label label3 = new Label("Id_Orden");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 170, 220);

        Label label4 = new Label("Total");
        label4.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label4, 170, 300);

        Entry entry1 = new Entry();
        entry1.SetSizeRequest(200, 30);
        fix.Put(entry1, 250, 150);

        Entry entry2 = new Entry();
        entry2.SetSizeRequest(200, 30);
        fix.Put(entry2, 250, 220);

        Entry entry3 = new Entry();
        entry3.SetSizeRequest(200, 30);
        fix.Put(entry3, 250, 300);

        Button buttonBuscar = new Button("Buscar");
        buttonBuscar.SetSizeRequest(80, 50);
        fix.Put(buttonBuscar, 400, 350);
        buttonBuscar.Clicked += (sender, e) =>
{
    
};

        Button buttonVolver = new Button("Volver");
        buttonVolver.SetSizeRequest(80, 50);
        fix.Put(buttonVolver, 500, 40);

        buttonVolver.Clicked += (sender, e) =>
        {
            Principalus principalus = new Principalus();
            principalus.ShowAll();
            this.Destroy();
        };

        Add(fix);
        ShowAll();
    }

   
}