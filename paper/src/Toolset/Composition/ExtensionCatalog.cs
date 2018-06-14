//using System;
//using System.Collections.Generic;
//using System.Composition;
//using System.Composition.Hosting;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.Loader;
//using System.Text;
//using Toolset.Application;

//namespace Toolset.Composition
//{
//  public class ExtensionCatalog : IExtensionCatalog
//  {
//    private readonly static ContainerConfiguration containerConfiguration;

//    private CompositionHost compositionHost;

//    static ExtensionCatalog()
//    {
//      containerConfiguration = CreateCompositionHost();
//    }

//    public ExtensionCatalog()
//    {
//      compositionHost = containerConfiguration.CreateContainer();
//    }

//    private static ContainerConfiguration CreateCompositionHost()
//    {
//      var assemblies =
//        Directory
//          .GetFiles(App.Path, "*.dll", SearchOption.TopDirectoryOnly)
//          .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
//          .ToArray();
//      var configuration = new ContainerConfiguration().WithAssemblies(assemblies);
//      return configuration;
//    }

//    #region GetExtension

//    public object GetExtension(Type contratType)
//    {
//      return compositionHost.GetExport(contratType);
//    }

//    public TExtension GetExtension<TExtension>()
//    {
//      return compositionHost.GetExport<TExtension>();
//    }

//    #endregion

//    #region GetExtensions

//    public object[] GetExtensions(Type contratType)
//    {
//      return compositionHost.GetExports(contratType).ToArray();
//    }

//    public TExtension[] GetExtensions<TExtension>()
//    {
//      return compositionHost.GetExports<TExtension>().ToArray();
//    }

//    #endregion

//    #region GetExtensionTypes

//    public Type[] GetExtensionTypes(Type contratType)
//    {
//      var instances = compositionHost.GetExports(contratType);
//      var types = instances.Select(x => x.GetType()).ToArray();
//      instances.OfType<IDisposable>().ForEach(instance =>
//      {
//        try
//        {
//          instance.Dispose();
//        }
//        catch { /* nada a fazer */ }
//      });
//      return types;
//    }

//    public Type[] GetExtensionTypes<TContract>()
//    {
//      var instances = compositionHost.GetExports<TContract>();
//      var types = instances.Select(x => x.GetType()).ToArray();
//      instances.OfType<IDisposable>().ForEach(instance =>
//      {
//        try
//        {
//          instance.Dispose();
//        }
//        catch { /* nada a fazer */ }
//      });
//      return types;
//    }

//    #endregion

//    #region GetMetadata

//    public IDictionary<string, object> GetMetadata(Type extensionType)
//    {
//      var map = new Dictionary<string, object>();
//      ExtractMetadata(extensionType, (key, value) => map[key] = value);
//      return map;
//    }

//    public IDictionary<string, object> GetMetadata(object extensionInstance)
//    {
//      return GetMetadata(extensionInstance.GetType());
//    }

//    public TMetadata GetMetadata<TMetadata>(Type extensionType) where TMetadata : new()
//    {
//      var metadata = new TMetadata();
//      FillMetadata(extensionType, metadata);
//      return metadata;
//    }

//    public TMetadata GetMetadata<TMetadata>(object extensionInstance) where TMetadata : new()
//    {
//      return GetMetadata<TMetadata>(extensionInstance.GetType());
//    }

//    #endregion

//    #region FillMetadata

//    public void FillMetadata(Type extensionType, object metadataInstance)
//    {
//      var type = metadataInstance.GetType();
//      var flags =
//          BindingFlags.IgnoreCase
//        | BindingFlags.Instance
//        | BindingFlags.Public
//        | BindingFlags.NonPublic;

//      ExtractMetadata(extensionType, (key, value) =>
//      {
//        var property = type.GetProperty(key, flags);
//        if (property != null)
//        {
//          var convertedValue = Convert.To(value, property.PropertyType);
//          property.SetValue(metadataInstance, convertedValue);
//        }
//      });
//    }

//    public void FillMetadata(object extensionInstance, object metadataInstance)
//    {
//      FillMetadata(extensionInstance.GetType(), metadataInstance);
//    }

//    #endregion

//    #region Outros...

//    public void SatisfyImports(object targetObject)
//    {
//      compositionHost.SatisfyImports(targetObject);
//    }

//    public void Dispose()
//    {
//      compositionHost.Dispose();
//      compositionHost = null;
//    }

//    private void ExtractMetadata(Type extensionType, Action<string, object> setter)
//    {
//      var exportings =
//        from a in extensionType.GetCustomAttributes(true)
//        where typeof(ExportAttribute).IsAssignableFrom(a.GetType())
//        select (ExportAttribute)a;
//      foreach (var exporting in exportings)
//      {
//        foreach (var prop in exporting.GetType().GetProperties())
//        {
//          var name = prop.Name;
//          var value = prop.GetValue(exporting);
//          setter.Invoke(name, value);
//        }
//      }

//      var metadata =
//        from a in extensionType.GetCustomAttributes(true)
//        where typeof(ExportMetadataAttribute).IsAssignableFrom(a.GetType())
//        select (ExportMetadataAttribute)a;
//      foreach (var attr in metadata)
//      {
//        setter.Invoke(attr.Name, attr.Value);
//      }
//    }

//    #endregion

//  }
//}