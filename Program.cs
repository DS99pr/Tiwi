// TiwiOS Copyright (c) 2024, Pard. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace TiwiOS {

  public class TerminalPatterns {
    public string Help = @"^\?$";
    public string Write = @"^wrt (.*)$";
    public string SettingsHelp = @"^settings$";
    public string SettingsChangePassword = @"^settings -cp (\w+) (\w+)$";
    public string NowHelp = @"^now$";
    public string NowDate = @"^now -d$";
    public string NowTime = @"^now -t$";
    public string NowDateTime = @"^now -dt$";
    public string GetStringFromUrl = @"^gsu (.*)$";
    public string OpnHelp = @"^opn$";
    public string OpnWrite = @"^opn (\w+).(\w+) -w$";
    public string OpnRead = @"^opn (\w+).(\w+) -r$";
   };
  class Program {
   public static void Main() {
    string logo = Logo();
    if (!File.Exists("already.txt")) {
        File.WriteAllText("already.txt", "");
    }
    if (!Directory.Exists("Notepad")) {
      Directory.CreateDirectory("Notepad");
    } 
    Console.WriteLine(logo);
    DateTime TimeObjectRn = DateTime.Now;
    string TimeRn = TimeObjectRn.ToString("dd.MM.yyyy, HH:mm");
    Console.WriteLine($"at {TimeRn}");
    string already = File.ReadAllText("already.txt");
    if (already == "") {
     Console.WriteLine("Welcome in TiwiOS! Let's register your account!");
     Console.Write("First, write your username: ");
     string NameValue = Console.ReadLine()!;
     Console.Write("Now, type your password: ");
     string PasswordValue = Console.ReadLine()!;
     Register(NameValue, PasswordValue);
    } else if (already == "true") {
     string Name = File.ReadAllText("name.txt");
     string CorrectPass = File.ReadAllText("pass.txt");
     Login(Name, CorrectPass);
     // ...
    } else {
     Console.WriteLine("Something went wrong.. We're trying to solve the problem.");
     File.WriteAllText("already.txt", "");
     Thread.Sleep(1500);
     Console.WriteLine("Successfully solved the problem. Restart the program, and try again.");
    }
   }
   static string Logo() {
     return @" 
 _____  _            _   ___   ____  
|_   _|(_)__      __(_) / _ \ / ___| 
  | |  | |\ \ /\ / /| || | | |\___ \ 
  | |  | | \ V  V / | || |_| | ___) |
  |_|  |_|  \_/\_/  |_| \___/ |____/ 
"; 
   }
   public static void Register(string Name, string Password) {
      if (Name.Length < 48 && Name.Length > 1 && Password.Length < 32 && Password.Length > 8) {
       if (!Password.Contains(" ") && !Name.Contains(" ")) { 
       string AlreadyPath = "already.txt";
       string NamePath = "name.txt";
       string PasswordPath = "pass.txt";
       File.WriteAllText(NamePath, Name);
       File.WriteAllText(PasswordPath, Password);
       File.WriteAllText(AlreadyPath, "true");
       Console.WriteLine($"Successfully registered you as {Name} (Password Length: {Password.Length})");
       } else {
        Console.WriteLine("Your name/password contains space!");
        Thread.Sleep(500); // for terminals
       } 
      } else {
       Console.WriteLine("Your name/password is too long/short!");
       Thread.Sleep(500); // for terminals
      }  
   }
   public static void Login(string Name, string CorrectPassword) {
    Console.WriteLine("Welcome back! Let's login you.");
    Console.WriteLine($". . {Name} . .");
    Console.Write("Type password: ");
    string Password = Console.ReadLine()!;
    if (Password == CorrectPassword) {
     Console.WriteLine($"Successfully logged as {Name}.");
     OpenTerminal();
    } else {
     Console.WriteLine("Password is incorrect! Try again.");  
     Thread.Sleep(500); // for terminals
    }
   }
   public static void OpenTerminal() {
       Console.WriteLine("Welcome on Tiwi terminal. Type \"?\" for help. ");
       TerminalPatterns Patterns = new TerminalPatterns();
       while (true) {
        Console.Write("@ sudo ");
        string InputText = Console.ReadLine()!;
        if (Regex.Match(InputText, Patterns.Help).Success) {
         Console.WriteLine(GenerateHelp());
        } else if (Regex.Match(InputText, Patterns.Write).Success) {
         string Arg1 = Regex.Match(InputText, Patterns.Write).Groups[1].Value;
         Console.WriteLine(Arg1);   
        } else if (Regex.Match(InputText, Patterns.SettingsHelp).Success) {
         Console.WriteLine("--- Settings ---");
         Console.WriteLine("settings -cp [old_pass] [new_pass] :-: change password");
        } else if (Regex.Match(InputText, Patterns.SettingsChangePassword).Success) {
         string RealOldPass = File.ReadAllText("pass.txt");
         string TypedOldPass = Regex.Match(InputText, Patterns.SettingsChangePassword).Groups[1].Value;
         string TypedNewPass = Regex.Match(InputText, Patterns.SettingsChangePassword).Groups[2].Value;
         if (RealOldPass == TypedOldPass) {
          if (!TypedNewPass.Contains(" ")) {
           if (TypedNewPass.Length < 32 && TypedNewPass.Length > 8) {
            if (TypedNewPass != RealOldPass) {
             File.WriteAllText("pass.txt", TypedNewPass);
             Console.WriteLine("Successfully changed password. Restart OS to see changes.");
            } else {
             Console.WriteLine("New password can't be the same as old!"); 
            }
           } else {
            Console.WriteLine("New password is too long/short.");
           }
          } else {
           Console.WriteLine("New password contains space.");
          }
         } else {
          Console.WriteLine("Real old password doesn't match the typed old password.");
         } 
        } else if (Regex.Match(InputText, Patterns.NowHelp).Success) {
         Console.WriteLine("--- Now ---");
         Console.WriteLine("now -d :-: look at date");
         Console.WriteLine("now -t :-: look at time");
         Console.WriteLine("now -dt :-: look at date, and time");
        } else if (Regex.Match(InputText, Patterns.NowDate).Success) {
         DateTime Now = DateTime.Now;
         string NowDate = Now.ToString("dd.MM.yyyy");
         Console.WriteLine(NowDate);
        } else if (Regex.Match(InputText, Patterns.NowTime).Success) {
         DateTime Now = DateTime.Now;
         string NowTime = Now.ToString("HH:mm:ss");
         Console.WriteLine(NowTime);
        } else if (Regex.Match(InputText, Patterns.NowDateTime).Success) {
         DateTime Now = DateTime.Now;
         string NowDateTime = Now.ToString("dd.MM.yyyy HH:mm:ss");
         Console.WriteLine(NowDateTime);
        } else if (Regex.Match(InputText, Patterns.GetStringFromUrl).Success) {
         try { 
          string Url = Regex.Match(InputText, Patterns.GetStringFromUrl).Groups[1].Value;
          HttpClient client = new HttpClient();
          string Response = client.GetStringAsync($"http://{Url}").Result;
          Console.WriteLine(Response);
         } catch (System.AggregateException e) {
          Console.WriteLine($"HTTP error occured while using operating system; {e.Message}");
         }
         catch (Exception e) {
          Console.WriteLine($"An unexcepted error occured while using operating system; {e.Message}");
         }   
        } else if (Regex.Match(InputText, Patterns.OpnHelp).Success) {
         Console.WriteLine("--- Opn ---");
         Console.WriteLine("opn [fp] -w :-: write to the file");
         Console.WriteLine("opn [fp] -r :-: read from the file");
        } else if (Regex.Match(InputText, Patterns.OpnWrite).Success) {
         if (!Directory.Exists("Notepad")) {
          Directory.CreateDirectory("Notepad");
         }
         string FileName = Regex.Match(InputText, Patterns.OpnWrite).Groups[1].Value;
         string FileExtension = Regex.Match(InputText, Patterns.OpnWrite).Groups[2].Value;
         Console.Write("File content: ");
         string FileContent = Console.ReadLine()!;
         if (FileExtension == "tf") {
          if (!FileName.Contains(" ")) {
           File.WriteAllText($"Notepad/{FileName}.{FileExtension}", FileContent);
          } else {
           Console.WriteLine("Your file name contains a space."); 
          }
         } else {
          Console.WriteLine("Your file needs to end with '.tf' (only support for text files)");
         } 
        } else if (Regex.Match(InputText, Patterns.OpnRead).Success) {
         try {
          if (!Directory.Exists("Notepad")) {
           Directory.CreateDirectory("Notepad");
          }
          string FileName = Regex.Match(InputText, Patterns.OpnRead).Groups[1].Value;
          string FileExtension = Regex.Match(InputText, Patterns.OpnRead).Groups[2].Value;
          if (FileExtension == "tf") {
           string ContentFromFile = File.ReadAllText($"Notepad/{FileName}.{FileExtension}");
           Console.WriteLine(ContentFromFile);
          } else {
           Console.WriteLine("Your file needs to end with '.tf' (only support for text files)");
          }
         }
         catch (FileNotFoundException) {
          Console.WriteLine($"Cannot find file named like that.");
         } 
        }
        else {
         Console.WriteLine("CommandNotFound | SyntaxError | ArgError");
        }
       }
   }
   public static string GenerateHelp() {
    return @"
- ? :-: help,
- wrt [arg1] :-: writing something in console,
- settings [?cmd] :-: open the settings, or set something,
- now [?arg] :-: look at the now time,
- gsu [url] :-: get http method. without 'http' prefix,
- opn [?fp] [?arg] :-: write or read the file.
";
   }
   static void Test(string InputText) {
    // ...
   }
  }
}
