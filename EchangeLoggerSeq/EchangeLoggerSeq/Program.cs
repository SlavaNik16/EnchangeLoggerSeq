
using System;
using Serilog;
using System.Net;
using System.Text;

class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.Unicode;
        var assembly = System.Reflection.Assembly.GetCallingAssembly();
        var version = assembly.GetName().Version.ToString();

        String host = Dns.GetHostName();
        IPHostEntry ipEntry = Dns.GetHostByName(host);
        IPAddress[] addr = ipEntry.AddressList;

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341", apiKey: "W7QuX5V3DkiAtLgq2iQZ")
            .Enrich.WithProperty("Version", version)
            .Enrich.WithProperty("IP", addr[6])
            .CreateLogger();
        var dollar = 0m;
        var course = 60;
       
        Log.Information($"Привет,{Environment.UserName}!");
        Log.Information("Ваши IP: ");
        for (int i = 0; i < ipEntry.AddressList.Length; i++)
        {
            Log.Information($"{i} - {addr[i]}");
        }
        Log.Information($"Курс доллара в перевод в рублях 1:{course}");
        while (true)
        {
            Log.Information($"Пользователь вводит число: ");
            if (decimal.TryParse(Console.ReadLine(),out dollar) && dollar > 0)
            {
                Log.Information($"Удачно! Пользователь ввел: {dollar}$");
                break;
            }
            else
            {
                Log.Error($"Не удачно! Повторите попытку");
            }
        }
        var rub = dollar * course;
        var proc = 0.37d;
        if (dollar < 500)
        {
            Log.Information($"Взята коммисия 8 рублей");
            Console.WriteLine($"Взято {8} руб и получается: {(rub - 8):C2}");
        }
        else
        {
            Log.Information($"Взята коммисия {proc}% рублей");
            Console.WriteLine($"Взято {proc}% и получается: {(rub - (rub * (decimal)proc / 100)):C2}");
        }
        Log.Information($"{Environment.UserName} успешно перевел(a) средства!");

        Log.CloseAndFlush();

        Console.ReadKey(true);
    }
    
}