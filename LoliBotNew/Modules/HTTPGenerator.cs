using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;

namespace LoliBotNew.Modules{
  
  public static class HTTPGenerator{
    public static void Start(){
      Thread BackgroundServer=new Thread(new ThreadStart(HttpServerLaunch));
      BackgroundServer.Start();
    }
    private static void HttpServerLaunch()
  {
    var l = new TcpListener(IPAddress.Loopback, 0);
    l.Start();
    int port = ((IPEndPoint)l.LocalEndpoint).Port;
    l.Stop();
    
    var listener = new HttpListener();
    listener.Prefixes.Add("http://*:" + port + "/");
    listener.Start();
    while (true)
    {
      try
      {
        var context = listener.GetContext();
       MainClass.Process(context);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
  }
  }

  public class MainClass {
  private static Random _random = new Random();

  public static object GetResponseData(HttpListenerRequest request)
  {
    var name = request.QueryString["name"] ?? "World";

    var plants = new[] { "Rose", "Moss", "Rhododendron" };
    
      
    
    return new { 
      Message = $"Hello {name}!", 
      Items = new[] { 1, 2, _random.Next(255) },
      RemoteEndPoint = request.RemoteEndPoint.ToString(),
      Date = DateTime.Now,
      
    };
  }

  public static void Process(HttpListenerContext context)
  {
    context.Response.StatusCode = 200;
    context.Response.ContentType = "application/json";

    var data = GetResponseData(context.Request);
    var buffer = Encoding.UTF8.GetBytes(data.ToJson());

    context.Response.ContentLength64 = buffer.Length;
    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    context.Response.OutputStream.Flush();
    context.Response.OutputStream.Close();
  }

    
}
  public static class JsonExtensions
  {
    private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
    {
        typeof(int),  typeof(double),  typeof(decimal),
        typeof(long), typeof(short),   typeof(sbyte),
        typeof(byte), typeof(ulong),   typeof(ushort),  
        typeof(uint), typeof(float)
    };

    private static bool IsNumeric(Type myType)
    {
      return NumericTypes.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
    }
    
    static public string ToJson(this object o) {
      if (o == null)
        return string.Empty;
      
      var t = o.GetType();
      if (typeof(string).IsAssignableFrom(t))
        return $"\"{o}\"";
      else if (IsNumeric(t))
        return o.ToString();
      else if (typeof(char).IsAssignableFrom(t))
        return $"'{o}'";
      else if (typeof(DateTime).IsAssignableFrom(t))
        return $"\"{((DateTime)o).ToString("s")}\"";
      else if (o is ICollection<object>)
        return $"[{string.Join(",", (o as ICollection<object>).Select(i => i.ToJson()))}]";
      else
        return $"something";
    
      return $"{{{ string.Join(",", t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(pi => $"\"{pi.Name}\":{ToJson(pi.GetValue(o))}"))}}}";
    }

    static T FromJson<T>(string text)
    {
      return default(T);
    }
  }
}