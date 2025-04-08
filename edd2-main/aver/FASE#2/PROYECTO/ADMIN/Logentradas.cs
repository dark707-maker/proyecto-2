using System;
using Gtk;
using Pango;
using System.IO;

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

        // Botón Generar Archivo
        Button guardar = new Button(" Generar ARCHIVO");
        guardar.SetSizeRequest(100, 50);
        fix.Put(guardar, 50, 300);

        guardar.Clicked += (sender, e) =>
        {
            GenerarArchivoJSON();
        };

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

    private void GenerarArchivoJSON()
    {
        string logPath = "Control_log.json";
        string outputPath = "entradas.json";

        if (File.Exists(logPath))
        {
            try
            {
                string contenido = File.ReadAllText(logPath);
                File.WriteAllText(outputPath, contenido);

                Console.WriteLine("Archivo JSON generado correctamente en 'entradas.json'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el archivo JSON: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No se encontró el archivo de log 'Control_log.json' para generar el JSON.");
        }
    }
}