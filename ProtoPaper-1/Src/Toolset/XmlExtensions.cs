using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Toolset
{
  public static class XmlExtensions
  {
    #region ToXmlObject<T>

    public static T ToXmlObject<T>(this object value)
    {
      return (T)ToXmlObject(value, typeof(T));
    }

    public static T ToXmlObject<T>(this Stream value)
    {
      return (T)ToXmlObject(value, typeof(T));
    }

    public static T ToXmlObject<T>(this String value)
    {
      return (T)ToXmlObject(value, typeof(T));
    }

    public static T ToXmlObject<T>(this XmlNode value)
    {
      return (T)ToXmlObject(value, typeof(T));
    }

    public static T ToXmlObject<T>(this XContainer value)
    {
      return (T)ToXmlObject(value, typeof(T));
    }

    #endregion

    #region ToXmlObject

    public static Object ToXmlObject(this object value, Type type)
    {
      if (value is XContainer)
        return ToXmlObject((XContainer)value, type);
      if (value is XmlNode)
        return ToXmlObject((XmlNode)value, type);
      if (value is Stream)
        return ToXmlObject((Stream)value, type);
      if (value is string)
        return ToXmlObject((string)value, type);

      return value;
    }

    public static Object ToXmlObject(this Stream value, Type type)
    {
      return Deserializar(type, value);
    }

    public static Object ToXmlObject(this String value, Type type)
    {
      using (var reader = new StringReader(value))
      {
        return Deserializar(type, reader);
      }
    }

    public static Object ToXmlObject(this XmlNode value, Type type)
    {
      using (var reader = value.CreateNavigator().ReadSubtree())
      {
        return Deserializar(type, reader);
      }
    }

    public static Object ToXmlObject(this XContainer value, Type type)
    {
      return Deserializar(type, value.CreateReader());
    }

    #endregion

    #region ToXmlStream

    public static Stream ToXmlStream(this Object value)
    {
      if (value is XContainer)
        return ToXmlStream((XContainer)value);
      if (value is XmlNode)
        return ToXmlStream((XmlNode)value);
      if (value is Stream)
        return (Stream)value;
      if (value is string)
        return ToXmlStream((string)value);

      var memory = new MemoryStream();
      try
      {
        using (var writer = XmlWriter.Create(memory,
          new XmlWriterSettings { Indent = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
        {
          Serializar(value, writer);

          memory.Position = 0;

          return memory;
        }
      }
      catch (Exception ex)
      {
        memory.Dispose();
        throw ex;
      }
    }

    public static Stream ToXmlStream(this String value)
    {
      var memory = new MemoryStream();
      try
      {
        var bytes = Encoding.UTF8.GetBytes(value);
        memory.Write(bytes, 0, bytes.Length);
        memory.Flush();
        memory.Position = 0;
        return memory;
      }
      catch (Exception ex)
      {
        memory.Dispose();
        throw ex;
      }
    }

    public static Stream ToXmlStream(this XmlNode value)
    {
      var memory = new MemoryStream();
      try
      {
        using (var output = XmlWriter.Create(memory,
          new XmlWriterSettings { Indent = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
        {
          value.WriteTo(output);
        }
        memory.Flush();
        memory.Position = 0;
        return memory;
      }
      catch (Exception ex)
      {
        memory.Dispose();
        throw ex;
      }
    }

    public static Stream ToXmlStream(this XContainer value)
    {
      var memory = new MemoryStream();
      try
      {
        if (value is XDocument)
        {
          ((XDocument)value).Save(memory, SaveOptions.DisableFormatting);
        }
        else
        {
          ((XElement)value).Save(memory, SaveOptions.DisableFormatting);
        }
        memory.Flush();
        memory.Position = 0;
        return memory;
      }
      catch (Exception ex)
      {
        memory.Dispose();
        throw ex;
      }
    }

    #endregion

    #region ToXmlStream para um Stream determinado

    public static void ToXmlStream(this Object value, Stream stream)
    {
      if (value is XContainer)
      {
        ToXmlStream((XContainer)value, stream);
        return;
      }
      if (value is XmlNode)
      {
        ToXmlStream((XmlNode)value, stream);
        return;
      }
      if (value is Stream)
      {
        ((Stream)value).CopyTo(stream);
        return;
      }
      if (value is string)
      {
        ToXmlStream((string)value, stream);
        return;
      }

      var stringWriter = new StringWriter();
      using (var writer = XmlWriter.Create(stringWriter,
        new XmlWriterSettings { Indent = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
      {
        Serializar(value, writer);
        var xml = stringWriter.ToString();

        var bytes = Encoding.UTF8.GetBytes(xml);
        stream.Write(bytes, 0, bytes.Length);
        stream.Flush();

        if (stream is MemoryStream)
        {
          ((MemoryStream)stream).Position = 0;
        }
      }
    }

    public static void ToXmlStream(this String value, Stream stream)
    {
      var bytes = Encoding.UTF8.GetBytes(value);
      stream.Write(bytes, 0, bytes.Length);
      stream.Flush();

      if (stream is MemoryStream)
      {
        ((MemoryStream)stream).Position = 0;
      }
    }

    public static void ToXmlStream(this XmlNode value, Stream stream)
    {
      using (var output = XmlWriter.Create(stream,
        new XmlWriterSettings { Indent = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
      {
        value.WriteTo(output);
      }
      stream.Flush();
      stream.Position = 0;

      if (stream is MemoryStream)
      {
        ((MemoryStream)stream).Position = 0;
      }
    }

    public static void ToXmlStream(this XContainer value, Stream stream)
    {
      if (value is XDocument)
      {
        ((XDocument)value).Save(stream, SaveOptions.DisableFormatting);
      }
      else
      {
        ((XElement)value).Save(stream, SaveOptions.DisableFormatting);
      }
      stream.Flush();
      stream.Position = 0;

      if (stream is MemoryStream)
      {
        ((MemoryStream)stream).Position = 0;
      }
    }

    #endregion

    #region ToXmlWriter

    public static void ToXmlWriter(this Object value, XmlWriter writer)
    {
      if (value is XContainer)
      {
        ToXmlWriter((XContainer)value, writer);
        return;
      }
      if (value is XmlNode)
      {
        ToXmlWriter((XmlNode)value, writer);
        return;
      }
      if (value is Stream)
      {
        using (var reader = XmlReader.Create((Stream)value))
        {
          writer.WriteNode(reader, true);
        }
      }
      if (value is string)
      {
        ToXmlWriter((string)value, writer);
        return;
      }

      Serializar(value, writer);
    }

    public static void ToXmlWriter(this String value, XmlWriter writer)
    {
      var xml = XElement.Parse(value);
      xml.Save(writer);
    }

    public static void ToXmlWriter(this XmlNode value, XmlWriter writer)
    {
      value.WriteTo(writer);
    }

    public static void ToXmlWriter(this XContainer value, XmlWriter writer)
    {
      if (value is XDocument)
      {
        ((XDocument)value).Save(writer);
      }
      else
      {
        ((XElement)value).Save(writer);
      }
    }

    #endregion

    #region ToXmlElement

    public static XmlElement ToXmlElement(this Object value)
    {
      if (value is XContainer)
        return ToXmlElement((XContainer)value);
      if (value is XmlNode)
        return ToXmlElement((XmlNode)value);
      if (value is Stream)
        return ToXmlElement((Stream)value);
      if (value is string)
        return ToXmlElement((string)value);

      using (var memory = new MemoryStream())
      {
        var format = new XmlWriterSettings
        {
          Encoding = Encoding.UTF8,
          Indent = false,
        };
        var writer = XmlTextWriter.Create(memory, format);

        Serializar(value, writer);

        writer.Flush();
        memory.Position = 0;

        var xml = new XmlDocument();
        xml.PreserveWhitespace = false;
        xml.Load(memory);
        return xml.DocumentElement;
      }
    }

    public static XmlElement ToXmlElement(this Stream value)
    {
      var xml = new XmlDocument();
      xml.PreserveWhitespace = false;
      xml.Load(value);
      return xml.DocumentElement;
    }

    public static XmlElement ToXmlElement(this String value)
    {
      var xml = new XmlDocument();
      xml.PreserveWhitespace = false;
      xml.LoadXml(value);
      return xml.DocumentElement;
    }

    public static XmlElement ToXmlElement(this XmlNode value)
    {
      if (value is XmlDocument)
      {
        return ((XmlDocument)value).DocumentElement;
      }
      else
      {
        return (XmlElement)value;
      }
    }

    public static XmlElement ToXmlElement(this XContainer value)
    {
      using (var reader = value.CreateReader())
      {
        var xml = new XmlDocument();
        xml.PreserveWhitespace = false;
        xml.Load(reader);
        return xml.DocumentElement;
      }
    }

    #endregion

    #region ToXmlDocument

    public static XmlDocument ToXmlDocument(this Object value)
    {
      if (value is XContainer)
        return ToXmlDocument((XContainer)value);
      if (value is XmlNode)
        return ToXmlDocument((XmlNode)value);
      if (value is Stream)
        return ToXmlDocument((Stream)value);
      if (value is string)
        return ToXmlDocument((string)value);

      using (var memory = new MemoryStream())
      {
        var format = new XmlWriterSettings
        {
          Encoding = Encoding.UTF8,
          Indent = false,
        };
        var writer = XmlTextWriter.Create(memory, format);

        Serializar(value, writer);

        writer.Flush();
        memory.Position = 0;

        var xml = new XmlDocument();
        xml.PreserveWhitespace = false;
        xml.Load(memory);
        return xml;
      }
    }

    public static XmlDocument ToXmlDocument(this Stream value)
    {
      var xml = new XmlDocument();
      xml.PreserveWhitespace = false;
      xml.Load(value);
      return xml;
    }

    public static XmlDocument ToXmlDocument(this String value)
    {
      var xml = new XmlDocument();
      xml.PreserveWhitespace = false;
      xml.LoadXml(value);
      return xml;
    }

    public static XmlDocument ToXmlDocument(this XmlNode value)
    {
      if (value is XmlDocument)
      {
        return (XmlDocument)value;
      }
      else
      {
        var xml = new XmlDocument();
        using (XmlReader reader = value.CreateNavigator().ReadSubtree())
        {
          xml.Load(reader);
        }
        return xml;
      }
    }

    public static XmlDocument ToXmlDocument(this XContainer value)
    {
      using (var reader = value.CreateReader())
      {
        var xml = new XmlDocument();
        xml.PreserveWhitespace = false;
        xml.Load(reader);
        return xml;
      }
    }

    #endregion

    #region ToXElement

    public static XElement ToXElement(this Object value)
    {
      if (value is XContainer)
        return ToXElement((XContainer)value);
      if (value is XmlNode)
        return ToXElement((XmlNode)value);
      if (value is Stream)
        return ToXElement((Stream)value);
      if (value is string)
        return ToXElement((string)value);

      var xml = new XDocument();
      using (var output = xml.CreateWriter())
      {
        Serializar(value, output);
      }
      return xml.Root;
    }

    public static XElement ToXElement(this Stream value)
    {
      var xml = XDocument.Load(value);
      return xml.Root;
    }

    public static XElement ToXElement(this String value)
    {
      var xml = XDocument.Parse(value);
      return xml.Root;
    }

    public static XElement ToXElement(this XmlNode value)
    {
      using (XmlReader reader = value.CreateNavigator().ReadSubtree())
      {
        var xml = XDocument.Load(reader);
        return xml.Root;
      }
    }

    public static XElement ToXElement(this XContainer value)
    {
      return (value is XDocument) ? ((XDocument)value).Root : (XElement)value;
    }

    #endregion

    #region ToXDocument

    public static XDocument ToXDocument(this Object value)
    {
      if (value is XContainer)
        return ToXDocument((XContainer)value);
      if (value is XmlNode)
        return ToXDocument((XmlNode)value);
      if (value is Stream)
        return ToXDocument((Stream)value);
      if (value is string)
        return ToXDocument((string)value);

      var xml = new XDocument();
      using (var output = xml.CreateWriter())
      {
        Serializar(value, output);
      }
      return xml;
    }

    public static XDocument ToXDocument(this Stream value)
    {
      return XDocument.Load(value);
    }

    public static XDocument ToXDocument(this String value)
    {
      return XDocument.Parse(value);
    }

    public static XDocument ToXDocument(this XmlNode value)
    {
      using (XmlReader reader = value.CreateNavigator().ReadSubtree())
      {
        return XDocument.Load(reader);
      }
    }

    public static XDocument ToXDocument(this XContainer value)
    {
      return (value is XDocument) ? (XDocument)value : new XDocument(value);
    }

    #endregion

    #region ToXmlString

    public static String ToXmlString(this Object value)
    {
      if (value is XContainer)
        return ToXmlString((XContainer)value);
      if (value is XmlNode)
        return ToXmlString((XmlNode)value);
      if (value is Stream)
        return ToXmlString((Stream)value);
      if (value is string)
        return (string)value;

      using (var memory = new MemoryStream())
      {
        var format = new XmlWriterSettings
        {
          Encoding = Encoding.UTF8,
          Indent = false,
          OmitXmlDeclaration = true
        };
        var xmlWriter = XmlWriter.Create(memory, format);

        Serializar(value, xmlWriter);
        xmlWriter.Flush();

        memory.Position = 0;
        var xml = new StreamReader(memory).ReadToEnd();
        return xml;
      }
    }

    public static String ToXmlString(this Stream value)
    {
      using (var stream = new StreamReader(value))
      {
        return stream.ReadToEnd();
      }
    }

    public static String ToXmlString(this XmlNode value)
    {
      return value.OuterXml;
    }

    public static String ToXmlString(this XContainer value)
    {
      return value.ToString(SaveOptions.DisableFormatting);
    }

    #endregion

    #region ToXmlTrace

    public static void ToXmlTrace(this XContainer value)
    {
      Trace.WriteLine(value.ToString());
    }

    public static void ToXmlTrace(this String value)
    {
      var stringReader = new StringReader(value);
      var xmlTextReader = XmlTextReader.Create(stringReader);

      var output = new StringWriter();
      using (var xmlTextWriter = XmlTextWriter.Create(output, new XmlWriterSettings { Indent = true }))
      {
        xmlTextWriter.WriteNode(xmlTextReader, true);
      }

      Trace.WriteLine(output.ToString());
    }

    public static void ToXmlTrace(this XmlNode value)
    {
      using (XmlReader reader = value.CreateNavigator().ReadSubtree())
      {
        var formattedXml = XElement.Load(reader);
        Trace.WriteLine(formattedXml);
      }
    }

    public static void ToXmlTrace(this Stream value)
    {
      Trace.WriteLine(XElement.Load(value).ToString());
    }

    public static void ToXmlTrace(this Object value)
    {
      var xml = new XDocument();
      using (var output = xml.CreateWriter())
      {
        Serializar(value, output);
      }
      Trace.WriteLine(xml.Root.ToString());
    }

    #endregion

    #region serializeres

    private static void Serializar(object objeto, XmlWriter output)
    {
      var type = objeto.GetType();
      var isXmlType = type.GetCustomAttributes(typeof(XmlTypeAttribute), true).Any();
      var isDataContract =
           type.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).Any()
        || type.GetCustomAttributes(typeof(DataContractAttribute), true).Any();

      if (isXmlType || !isDataContract)
      {
        var serializer = new XmlSerializer(type);
        serializer.Serialize(output, objeto);
      }
      else
      {
        var serializer = new DataContractSerializer(type);
        serializer.WriteObject(output, objeto);
      }

      output.Flush();
    }

    private static object Deserializar(Type type, Stream entrada)
    {
      return Deserializar(type, XmlReader.Create(entrada));
    }

    private static object Deserializar(Type type, TextReader entrada)
    {
      return Deserializar(type, XmlReader.Create(entrada));
    }

    private static object Deserializar(Type type, XmlReader entrada)
    {
      bool isXmlType = type.GetCustomAttributes(typeof(XmlTypeAttribute), true).Any();
      bool isDataContract =
           type.GetCustomAttributes(typeof(DataContractAttribute), true).Any()
        || type.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).Any();

      if (IsDataContract(type))
      {
        var serializer = new DataContractSerializer(type);
        return serializer.ReadObject(entrada);
      }
      else
      {
        var serializer = new XmlSerializer(type);
        return serializer.Deserialize(entrada);
      }
    }

    internal static bool IsDataContract(Type type)
    {
      bool isXmlType = type.GetCustomAttributes(typeof(XmlTypeAttribute), true).Any();
      bool isDataContract =
           type.GetCustomAttributes(typeof(DataContractAttribute), true).Any()
        || type.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).Any();

      bool useXmlType = isXmlType || !isDataContract;
      return !useXmlType;
    }

    #endregion
  }
}