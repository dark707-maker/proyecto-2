using System;
using Gtk;
using Pango;

//===============================INTERFAZ GRAFICA===============================
class Login : Gtk.Window {
    


    public Login() : base("WELCOME") {

        
        SetDefaultSize(800,700);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        Label label1 = new Label("LOG IN");
        label1.ModifyFont(FontDescription.FromString("Arial 36")); 
        fix.Put(label1, 315, 50);

        Label label2 = new Label("Ingresa tu usuario");
        label2.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label2, 200, 200);

        Label label3 = new Label("Ingresa tu contraseña");
        label3.ModifyFont(FontDescription.FromString("Arial 14"));
        fix.Put(label3, 180, 300);

        Entry usuario = new Entry();
        usuario.SetSizeRequest(200, 30);
        fix.Put(usuario, 400, 200);

        Entry password = new Entry();
        password.Visibility = false;
        password.SetSizeRequest(200, 30);
        fix.Put(password, 400, 300);

        //=============ADMIN====================

        Button boton1 = new Button(" Ingresar ");
        boton1.SetSizeRequest(100, 60);
        fix.Put(boton1, 340, 400);

              boton1.Clicked += (sender, e) => {
            if (usuario.Text == "root@gmail.com" && password.Text == "root123") {
                Console.WriteLine("Inicio de sesión exitoso");

                Principal principal = new Principal();
                principal.ShowAll();
                this.Hide();
            } else {
                Console.WriteLine("Usuario o contraseña incorrectos");
            }
        };

        Add(fix);
        ShowAll();

    }
//===============================MAIN===============================
    class Program
{
        
    public static void Main(string[] args)
    {
         
        Application.Init();
        new Login();
        Application.Run();
    }

    }
  }  
  //root@gmail.com
    //r