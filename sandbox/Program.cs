using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hypermedia;

namespace sandbox
{
  class Program
  {
    static void Main(string[] args)
    {
      var descriptor = new Descriptor("/Root/");

      var entity = new[] { new { id = 1, name = "Ten" } };
      var json = entity.ToEntity(descriptor).ToSiren();
      System.Diagnostics.Debug.WriteLine(json);

      return;
    }
  }
}

namespace Hypermedia
{
  public interface IDescriptor
  {
    string GetPath(string ancestorPath, string entityType);
    string GetPath(string ancestorPath, string entityType, string id);
  }

  public class Descriptor : IDescriptor
  {
    public Descriptor(string route)
    {
      if (route.EndsWith("/"))
      {
        route = route.Substring(0, route.Length - 1);
      }
      this.Route = route;
    }

    public string Route { get; }

    public string GetPath(string ancestorPath, string entityType)
    {
      return string.Join("/", ancestorPath ?? Route, entityType);
    }

    public string GetPath(string ancestorPath, string entityType, string id)
    {
      return string.Join("/", ancestorPath ?? Route, id);
    }

    public static implicit operator string(Descriptor context)
    {
      return context.Route;
    }

    public static implicit operator Descriptor(string route)
    {
      return new Descriptor(route);
    }
  }

  public static class EntityExtensions
  {
    public static Entity ToEntity(this object graph, IDescriptor context)
    {
      var entity = CreateEntity(context, null, graph, 1, null);
      return entity;
    }

    private static Entity CreateEntity(IDescriptor descriptor, string parent, object graph, int depth, string suggestedName)
    {
      var entity = new Entity();

      var entityType = graph.GetType().Name;
      if (entityType.Contains("AnonymousType"))
      {
        entityType = suggestedName ?? "entity";
      }

      string selfPath;

      var idProperty = (
        from property in graph.GetType().GetProperties()
        where property.Name == "Id" || property.Name == "id"
        select property
      ).SingleOrDefault();

      if (idProperty != null)
      {
        var id = idProperty.GetValue(graph, null)?.ToString() ?? "";
        selfPath = descriptor.GetPath(parent, entityType, id);
      }
      else
      {
        selfPath = descriptor.GetPath(parent, entityType);
      }

      entity.TypeInfo = new TypeNameCollection();
      entity.TypeInfo.Add(entityType);

      entity.Links = new LinkCollection();
      entity.Links.Add("self", selfPath);

      if (graph is IEnumerable)
      {
        var collection = ((IEnumerable)graph).Cast<object>();

        entity.TypeInfo.Add(TypeName.DefaultTypeNameForCollection);
        entity.Properties = new PropertyCollection();
        entity.Properties.Add("count", collection.Count());

        if (depth > 0)
        {
          entity.Entities = new EntityCollection();
          foreach (var item in collection)
          {
            var relationship = CreateEntity(descriptor, selfPath, item, depth - 1, null);
            entity.Entities.Add(relationship);
          }
        }
      }
      else
      {
        var properties = graph.GetType().GetProperties();
        var simpleProperties =
          from property in properties
          where IsSimpleType(property.PropertyType)
          select property;
        var complexProperties = properties.Except(simpleProperties);

        if (simpleProperties.Any())
        {
          entity.Properties = new PropertyCollection();
          foreach (var property in simpleProperties)
          {
            var value = property.GetValue(graph, null);
            entity.Properties.Add(property.Name, value);
          }
        }

        if (complexProperties.Any() && depth > 0)
        {
          entity.Entities = new EntityCollection();
          foreach (var property in complexProperties)
          {
            var value = property.GetValue(graph, null);
            var relationship = CreateEntity(descriptor, selfPath, value, depth - 1, property.Name);
            entity.Entities.Add(relationship);
          }
        }
      }
      return entity;
    }

    private static bool IsSimpleType(Type type)
    {
      return type == typeof(string)
          || type == typeof(short)
          || type == typeof(int)
          || type == typeof(long)
          || type == typeof(float)
          || type == typeof(double)
          || type == typeof(decimal)
          || type == typeof(bool)
          || type == typeof(DateTime);
    }
  }

  public static class EntitySerializationExtensions
  {
    public static string ToSiren(this Entity entity)
    {
      return Serialize(entity);
    }

    private static string Serialize(Entity entity)
    {
      var builder = new StringBuilder();
      builder.Append("{");

      string[] tokens = {
        Serialize(entity.TypeInfo),
        Serialize(entity.Properties),
        Serialize(entity.Links),
        Serialize(entity.Entities)
      };

      builder.Append(string.Join(",", tokens.Where(x => x != null)));

      builder.Append("}");
      return builder.ToString();
    }

    private static string Serialize(EntityCollection collection)
    {
      if (collection == null || collection.Count == 0)
        return null;

      var builder = new StringBuilder();
      builder.Append("\"entities\":[");

      var emitComma = false;
      foreach (var item in collection)
      {
        if (emitComma)
          builder.Append(",");
        else
          emitComma = true;

        builder.Append(Serialize(item));
      }

      builder.Append("]");
      return builder.ToString();
    }

    private static string Serialize(TypeNameCollection collection)
    {
      if (collection == null || collection.Count == 0)
        return null;

      var builder = new StringBuilder();
      builder.Append("\"class\":[");

      var emitComma = false;
      foreach (var item in collection)
      {
        if (emitComma)
          builder.Append(",");
        else
          emitComma = true;

        builder.AppendInQuotes(item.Name);
      }

      builder.Append("]");
      return builder.ToString();
    }

    private static string Serialize(PropertyCollection collection)
    {
      if (collection == null || collection.Count == 0)
        return null;

      var builder = new StringBuilder();
      builder.Append("\"properties\":{");

      var emitComma = false;
      foreach (var item in collection)
      {
        if (emitComma)
          builder.Append(",");
        else
          emitComma = true;

        builder.AppendInQuotes(item.Name);
        builder.Append(":");
        builder.AppendInQuotesIfNeeded(item.Value);
      }

      builder.Append("}");
      return builder.ToString();
    }

    private static string Serialize(LinkCollection collection)
    {
      if (collection == null || collection.Count == 0)
        return null;

      var builder = new StringBuilder();
      builder.Append("\"links\":[");

      var emitComma = false;
      foreach (var item in collection)
      {
        if (emitComma)
          builder.Append(",");
        else
          emitComma = true;

        builder.Append("{");

        builder.AppendInQuotes("rel");
        builder.Append(":[");
        builder.AppendInQuotes(item.Rel);
        builder.Append("]");

        builder.Append(",");

        builder.AppendInQuotes("href");
        builder.Append(":");
        builder.AppendInQuotes(item.HRef);

        builder.Append("}");
      }

      builder.Append("]");
      return builder.ToString();
    }

    private static void AppendInQuotes(this StringBuilder builder, object value)
    {
      builder.Append('"');
      builder.Append(value);
      builder.Append('"');
    }

    private static void AppendInQuotesIfNeeded(this StringBuilder builder, object value)
    {
      if (value == null)
      {
        builder.Append("null");
        return;
      }

      if (value is bool)
      {
        builder.Append((bool)value ? "true" : "false");
        return;
      }

      if (value is short
      || value is int
      || value is long
      || value is float
      || value is double
      || value is decimal)
      {
        builder.Append(value);
        return;
      }

      if (value is DateTime)
      {
        builder.Append('"');
        builder.Append(((DateTime)value).ToString("o"));
        builder.Append('"');
        return;
      }

      builder.Append('"');
      builder.Append(value);
      builder.Append('"');
    }
  }

  public class Entity
  {
    public TypeNameCollection TypeInfo { get; set; }
    public PropertyCollection Properties { get; set; }
    public EntityCollection Entities { get; set; }
    public LinkCollection Links { get; set; }
  }

  public class TypeName
  {
    public const string DefaultTypeNameForCollection = "collection";

    public string Name { get; set; }

    public static implicit operator string(TypeName typeInfo)
    {
      return typeInfo.Name;
    }

    public static implicit operator TypeName(string typeInfo)
    {
      return new TypeName { Name = typeInfo };
    }
  }

  public class TypeNameCollection : List<TypeName>
  {
    public TypeNameCollection()
    {
    }

    public TypeNameCollection(IEnumerable<TypeName> items)
      : base(items)
    {
    }

    public static implicit operator string(TypeNameCollection collection)
    {
      return string.Join(",", collection.Select(x => x.Name));
    }

    public static implicit operator TypeNameCollection(string collection)
    {
      var types =
        collection
          .Split(',')
          .Where(x => !string.IsNullOrWhiteSpace(x))
          .Select(x => (TypeName)x.Trim());
      return new TypeNameCollection(types);
    }
  }

  public class EntityCollection : List<Entity>
  {
    public EntityCollection()
    {
    }

    public EntityCollection(IEnumerable<Entity> items)
      : base(items)
    {
    }
  }

  public class Link
  {
    public string Rel { get; set; }
    public string HRef { get; set; }

    public Link()
    {
    }

    public Link(string rel, string href)
    {
      this.Rel = rel;
      this.HRef = href;
    }
  }

  public class LinkCollection : List<Link>
  {
    public LinkCollection()
    {
    }

    public LinkCollection(IEnumerable<Link> items)
      : base(items)
    {
    }

    public void Add(string rel, string href)
    {
      this.Add(new Link(rel, href));
    }
  }

  public class Property
  {
    public string Name { get; set; }
    public object Value { get; set; }

    public Property()
    {
    }

    public Property(string name, object value)
    {
      this.Name = name;
      this.Value = value;
    }
  }

  public class PropertyCollection : List<Property>
  {
    public PropertyCollection()
    {
    }

    public PropertyCollection(IEnumerable<Property> items)
      : base(items)
    {
    }

    public void Add(string name, object value)
    {
      this.Add(new Property(name, value));
    }
  }
}
