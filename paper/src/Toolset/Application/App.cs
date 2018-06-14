using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toolset.Collections;

namespace Toolset.Application
{
  public static class App
  {
    [ThreadStatic]
    private static ClaimsPrincipal _claimsPrincipal;

    [ThreadStatic]
    private static IAppUser _user;

    [ThreadStatic]
    private static IAppSettings _settings;

    [ThreadStatic]
    private static IAppConnections _connections;

    private static string _name;
    private static string _version;
    private static string _appDataPath;
    private static string _path;
    private static string[] _domains;

    /// <summary>
    /// Nome do fabricando do aplicativo.
    /// </summary>
    public static string Manufacturer { get; set; }

    /// <summary>
    /// Nome do aplicativo em execução.
    /// Em geral correspodende ao nome do executável sem extensão.
    /// </summary>
    public static string Name
    {
      get
      {
        if (_name == null)
        {
          _name =
            Assembly.GetEntryAssembly()?.GetName()?.Name
            ?? Process.GetCurrentProcess().ProcessName;
        }
        return _name;
      }
      set { _name = value; }
    }

    /// <summary>
    /// Determina a versão do aplicativo a partir dos padrões de versionamento conhecidos.
    /// </summary>
    /// <value>
    /// A versão do aplicativo.
    /// </value>
    public static string Version
    {
      get
      {
        if (_version == null)
        {
          var revision = DetectRevision();

          _version =
            revision
            ?? Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString()
            ?? Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        return _version;
      }
      set { _version = value; }
    }

    /// <summary>
    /// Credenciais do usuário logado na thread.
    /// </summary>
    public static ClaimsPrincipal ClaimsPrincipal
    {
      get
      {
        IdentifyUser();
        return _claimsPrincipal;
      }
    }

    /// <summary>
    /// Coleção dos domínios conhecidos no sistema.
    /// </summary>
    public static string[] Domains
    {
      get
      {
        if (_domains == null)
        {
          var config = AppConfig.GetDefaultConfiguration();
          var value = config["host:domains"] ?? "";
          var domains =
            value
              .Split(',')
              .Select(x => x.Trim())
              .NonEmpty()
              .Distinct()
              .ToArray();

          _domains = domains.Any() ? domains : new[] { AppUser.DefaultDomain };
        }
        return _domains;
      }
    }

    /// <summary>
    /// Informações sobre o usuário atualmente logado no aplicativo.
    /// </summary>
    public static IAppUser User
    {
      get
      {
        IdentifyUser();
        return _user ?? (_user = new AppUser(_claimsPrincipal.Identity.Name));
      }
    }

    /// <summary>
    /// Configurações do aplicativo corrente.
    /// </summary>
    public static IAppSettings Settings
    {
      get
      {
        IdentifyUser();
        return _settings ?? (_settings = new AppSettings(User.Domain));
      }
    }

    /// <summary>
    /// Configurações de conexão do aplicativo corrente.
    /// </summary>
    public static IAppConnections Connections
    {
      get
      {
        IdentifyUser();
        return _connections ?? (_connections = new AppConnections(User.Domain));
      }
    }

    /// <summary>
    /// Pasta a partir da qual o aplicativo está sendo executado.
    /// </summary>
    public static string Path
    {
      get
      {
        if (_path == null)
        {
          var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
          var local = assembly.Location;
          _path = System.IO.Path.GetDirectoryName(local);
        }
        return _path;
      }
      set { _path = value; }
    }

    /// <summary>
    /// Pasta do sistema onde os dados de aplicativos são armazenados.
    /// Em geral, em dispositivos móveis a pasta de dados de aplicativo é mapeada
    /// para um SDCard e em computadores é mapeada para a variável de ambiente
    /// %AppData%.
    /// </summary>
    public static string AppDataPath
    {
      get
      {
        if (_appDataPath == null)
        {
          var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

          if (Manufacturer != null)
            path = System.IO.Path.Combine(path, Manufacturer);

          path = System.IO.Path.Combine(path, Name);

          if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);

          _appDataPath = path;
        }
        return _appDataPath;
      }
      set { _appDataPath = value; }
    }

    /// <summary>
    /// Identifica o usuário da Thread e reseta informações caso tenha mudado.
    /// </summary>
    private static void IdentifyUser()
    {
      if (_claimsPrincipal != AppContext.ClaimsPrincipal)
      {
        _claimsPrincipal = AppContext.ClaimsPrincipal;

        // o usuario foi recem definido, entao,
        // vamos resetar a informação armazenada em cache
        _user = null;
        _settings = null;
        _connections = null;
      }
    }

    private static string DetectRevision()
    {
      string revision;

      revision = LoadRevisionFromManifest(Assembly.GetEntryAssembly());
      if (revision == null)
      {
        revision = LoadRevisionFromManifest(Assembly.GetExecutingAssembly());
      }

      return revision;
    }

    private static string LoadRevisionFromManifest(Assembly assembly)
    {
      try
      {
        if (assembly == null)
          return null;

        var manifest = (
          from m in assembly.GetManifestResourceNames()
          where m.EndsWith("REVISION.txt")
          select m
        ).FirstOrDefault();

        if (manifest == null)
          return null;

        using (var stream = assembly.GetManifestResourceStream(manifest))
        {
          var reader = new StreamReader(stream);
          var version = "";

          string line;
          while ((line = reader.ReadLine()) != null)
          {
            version += line.Split('#').First().Trim();
          }

          return version;
        }
      }
      catch (Exception ex)
      {
        ex.TraceWarning("Não foi possível ler informação de revisão do arquivo embarcado REVISION.txt.");
        return null;
      }
    }
  }
}