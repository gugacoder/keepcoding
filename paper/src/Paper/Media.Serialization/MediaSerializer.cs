using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Paper.Media.Serialization
{
  public class MediaSerializer : ISerializer
  {
    public enum Formats { Json, Xml }

    public MediaSerializer()
    {
      this.DefaultFormat = Formats.Json;
    }

    public MediaSerializer(string defaultFormat)
    {
      var isJson = defaultFormat?.Contains("json") ?? true;
      DefaultFormat = isJson ? Formats.Json : Formats.Xml;
    }

    public Formats DefaultFormat { get; }

    public void Serialize(Entity entity, string mediaType, TextWriter output)
    {
      bool isJson = this.DefaultFormat == Formats.Json;

      if (mediaType != null)
      {
        if (mediaType.Contains("json"))
        {
          isJson = true;
        }
        else if (mediaType.Contains("xml"))
        {
          isJson = false;
        }
      }

      if (isJson)
      {
        var serializer = new SirenSerializer();
        serializer.Serialize(entity, output);
      }
      else
      {
        var serializer = new DataContractSerializer(typeof(Entity), new[] { typeof(DBNull) });
        using (var writer = XmlWriter.Create(output, new XmlWriterSettings { CloseOutput = false }))
        {
          serializer.WriteObject(writer, entity);
          writer.Flush();
        }
      }

      output.Flush();
    }

    #region Outras implementações de Serialize()

    public void Serialize(Entity entity, string mediaType, Stream output)
    {
      var writer = new StreamWriter(output, Encoding.UTF8, 8 * 1024, true);
      Serialize(entity, mediaType, writer);
    }

    public void Serialize(Entity entity, Stream output)
    {
      var writer = new StreamWriter(output, Encoding.UTF8, 8 * 1024, true);
      Serialize(entity, null, writer);
    }

    public string Serialize(Entity entity, string mediaType)
    {
      using (var writer = new StringWriter())
      {
        Serialize(entity, mediaType, writer);
        return writer.ToString();
      }
    }

    public string Serialize(Entity entity)
    {
      using (var writer = new StringWriter())
      {
        Serialize(entity, null, writer);
        return writer.ToString();
      }
    }

    #endregion

    #region Implementações de SerializeToXml()

    public void SerializeToXml(Entity entity, Stream output)
    {
      Serialize(entity, "xml", output);
    }

    public void SerializeToXml(Entity entity, TextWriter output)
    {
      Serialize(entity, "xml", output);
    }

    public string SerializeToXml(Entity entity)
    {
      return Serialize(entity, "xml");
    }

    #endregion

    #region Implementações de SerializeToXml()

    public void SerializeToJson(Entity entity, Stream output)
    {
      Serialize(entity, "json", output);
    }

    public void SerializeToJson(Entity entity, TextWriter output)
    {
      Serialize(entity, "json", output);
    }

    public string SerializeToJson(Entity entity)
    {
      return Serialize(entity, "json");
    }

    #endregion

  }
}