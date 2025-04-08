using System;
using Gtk;
using Pango;

class InsertarVehiculos : Gtk.Window
{
    public InsertarVehiculos() : base("INSERTAR VEHÍCULOS")
    {
        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        // ================================ LABELS ==============================
        Label titulo = new Label("INSERTAR VEHÍCULOS");
        titulo.ModifyFont(Pango.FontDescription.FromString("Arial 26"));
        fix.Put(titulo, 50, 10);

        Label id = new Label("ID:");
        id.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(id, 50, 50);

        Label marca = new Label("Marca:");
        marca.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(marca, 50, 100);

        Label modelo = new Label("Modelo:");
        modelo.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(modelo, 50, 150);

        Label placa = new Label("Placa:");
        placa.ModifyFont(Pango.FontDescription.FromString("Arial 16"));
        fix.Put(placa, 50, 200);

        // ================================ ENTRIES ==============================
        Entry entryid = new Entry();
        entryid.SetSizeRequest(200, 30);
        fix.Put(entryid, 200, 50);

        Entry entrymarca = new Entry();
        entrymarca.SetSizeRequest(200, 30);
        fix.Put(entrymarca, 200, 100);

        Entry entrymodelo = new Entry();
        entrymodelo.SetSizeRequest(200, 30);
        fix.Put(entrymodelo, 200, 150);

        Entry entryplaca = new Entry();
        entryplaca.SetSizeRequest(200, 30);
        fix.Put(entryplaca, 200, 200);

        // ================================ BOTONES ==============================
        Button guardar = new Button("Guardar");
        guardar.SetSizeRequest(80, 50);
        fix.Put(guardar, 50, 300);

        Button button = new Button("Volver");
        button.SetSizeRequest(100, 30);
        fix.Put(button, 450, 25);
        button.Clicked += (sender, e) =>
        {
            Principalus manual = new Principalus();
            manual.ShowAll();
            this.Hide();

        };

        Add(fix);
        ShowAll();
    }
}
