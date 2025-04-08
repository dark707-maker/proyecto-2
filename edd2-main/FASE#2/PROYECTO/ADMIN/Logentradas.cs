using System;
using Gtk;
using Pango;

class Entradas : Window
{
    public Entradas() : base("ENTRADAS .JSON")
    {
        SetDefaultSize(700, 600);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        // Título
        Label titulo = new Label("ENTRADAS .JSON");
        titulo.ModifyFont(new Pango.FontDescription { Family = "Arial", Size = 26000 });
        fix.Put(titulo, 200, 50);

        // Botón Guardar
        Button guardar = new Button(" Generar ARCHIVO");
        guardar.SetSizeRequest(100, 50);
        fix.Put(guardar, 50, 300);

        // Botón Volver
        Button button = new Button("Volver");
        button.SetSizeRequest(100, 50);
        fix.Put(button, 500, 100);

        button.Clicked += (sender, e) =>
        {
            Principaladmin manual = new Principaladmin();
            manual.ShowAll();
            this.Hide();
        };

        // Agregar a la ventana
        Add(fix);
        ShowAll();
    }

   
}
