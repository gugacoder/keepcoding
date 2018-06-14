using System;
using System.Linq;
using System.Collections.Generic;
using Paper.Media;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Toolset;

namespace Paper.Media.Serialization
{
  /// <summary>
  /// Seriaizador de hipermídia para o formato Siren+JSON.
  /// </summary>
  public class SirenSerializer : ISerializer
  {
    #region Variações de Serialize()

    /// <summary>
    /// Serializa a entidade para a saída indicada.
    /// </summary>
    /// <param name="entity">Entidade a ser serializada.</param>
    /// <param name="output">Stream de saída.</param>
    public void Serialize(Entity entity, TextWriter output)
    {
      Write(output, entity);
    }

    /// <summary>
    /// Serializa a entidade para a saída indicada.
    /// </summary>
    /// <param name="entity">Entidade a ser serializada.</param>
    /// <param name="output">Stream de saída.</param>
    public void Serialize(Entity entity, Stream output)
    {
      TextWriter writer = new StreamWriter(output);
      Serialize(entity, writer);
    }

    /// <summary>
    /// Serializa a entidade para string.
    /// </summary>
    /// <param name="entity">Entidade a ser serializada.</param>
    public string Serialize(Entity entity)
    {
      using (var writer = new StringWriter())
      {
        Serialize(entity, writer);
        return writer.ToString();
      }
    }

    #endregion

    #region Funções de serialização

    private void WriteProperty(TextWriter writer, string property, object value, ref int count)
    {
      if (value == null)
        return;

      if (count > 0)
        writer.Write(",");

      var name = MakeCompatibleName(property);

      writer.Write("\"");
      writer.Write(name);
      writer.Write("\":");

      Write(writer, value);

      count += 1;
    }

    private void Write(TextWriter writer, object value)
    {
      if (value is CaseVariantString)
      {
        value = MakeCompatibleName(value.ToString());
        // apenas ajusta o valor de acordo com a caixa e prossegue...
      }

      if (IsNull(value))
      {
        writer.Write("null");
        return;
      }

      if (IsNumeric(value))
      {
        writer.Write(value);
        return;
      }

      if (value is string)
      {
        writer.Write("\"");
        writer.Write(Toolset.Json.Escape((string)value));
        writer.Write("\"");
        return;
      }

      if (value is bool)
      {
        writer.Write((bool)value ? 1 : 0);
        return;
      }

      if (value is DateTime)
      {
        writer.Write("\"");
        writer.Write(((DateTime)value).ToString("yyyy-MM-ddTHH:mm:sszzz"));
        writer.Write("\"");
        return;
      }

      if (value is NameCollection)
      {
        var items = (NameCollection)value;

        writer.Write("[");

        var comma = false;
        foreach (var item in items)
        {
          if (comma) writer.Write(',');
          comma = true;

          writer.Write("\"");
          writer.Write(Toolset.Json.Escape(item));
          writer.Write("\"");
        }

        writer.Write("]");
        return;
      }

      if (value is PropertyCollection)
      {
        var items = (PropertyCollection)value;
        
        writer.Write("{");

        int count = 0;
        foreach (var item in items)
        {
          WriteProperty(writer, item.Name, item.Value, ref count);
        }

        writer.Write("}");
        return;
      }

      if (value is IList)
      {
        var items = (IList)value;

        writer.Write("[");

        bool comma = false;
        foreach (var item in items)
        {
          if (comma) writer.Write(",");
          comma = true;

          Write(writer, item);
        }

        writer.Write("]");
        return;
      }

      writer.Write("{");

      var properties = (
        from property in value.GetType().GetProperties()
        where !property.GetIndexParameters().Any()
        select property
      ).ToArray();

      int propertyCount = 0;
      foreach (var property in properties)
      {
        var propertyValue = property.GetValue(value);
        if (IsEmpty(propertyValue))
          continue;

        // Se a string for CaseVariantStringAttribute temos que converte-la para camelCase
        if (property.GetCustomAttributes().OfType<CaseVariantStringAttribute>().Any())
        {
          propertyValue = (CaseVariantString)propertyValue.ToString();
        }
        // Se estamos renderizando um link a sua coleção "Rel" precisa ser convertida para camelCase
        else if (value is Link)
        {
          if (property.Name == "Rel")
          {
            propertyValue = 
              ((NameCollection)propertyValue)
                .Select(x => (CaseVariantString)x)
                .ToArray();
          }
        }

        var propertyName = MakeCompatibleName(property.Name);

        WriteProperty(writer, propertyName, propertyValue, ref propertyCount);
      }

      writer.Write("}");
    }

    private bool IsEmpty(object value)
    {
      if (value == null) return true;
      if (value is IEnumerable)
      {
        var any = ((IEnumerable)value).Cast<object>().Any();
        return !any;
      }
      return false;
    }

    private bool IsNull(object value)
    {
      return value == null || value == DBNull.Value;
    }

    private bool IsNumeric(object value)
    {
      if (value is short
       || value is int
       || value is long
       || value is float
       || value is double
       || value is decimal
      )
      {
        return true;
      }

      if (value is string)
      {
        var text = (string)value;
        return text != "" && text.Length <= 9 && text.All(c => char.IsDigit(c) || char.IsNumber(c));
      }

      return false;
    }

    private string MakeCompatibleName(string originalName)
    {
      var tokens = originalName.Split('.');
      var parts =
        from token in tokens
        select Json.Escape(token.ChangeCase(TextCase.CamelCase));
      var name = string.Join(".", parts);
      return name;
    }

    #endregion
  }
}