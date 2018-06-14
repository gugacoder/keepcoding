using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolset.Application
{
  class AppSettings : IAppSettings
  {
    //private readonly string domain;
    //private readonly IConfigurationRoot configuration;

    public AppSettings(string domain)
    {
      //  this.domain = string.IsNullOrWhiteSpace(domain) ? null : domain;
      //  this.configuration = Configurations.GetConfiguration("AppSettings");
    }

    //public string this[string key]
    //{
    //  get
    //  {
    //    var realKey = GetUnderlyingKey(section: null, key: key);
    //    if (realKey == null)
    //      return null;

    //    string value = configuration[realKey];
    //    return value;
    //  }
    //}

    //public string this[string section, string key]
    //{
    //  get
    //  {
    //    var appSettings = GetUnderlyingCollection(section);
    //    if (appSettings == null)
    //      return null;

    //    var keyName = GetSystemWideKeyName(key, section);
    //    return (keyName != null) ? appSettings[keyName] : null;
    //  }
    //}

    //public string[] GetKeys()
    //{
    //  return GetKeys(null);
    //}

    //public string[] GetKeys(string section)
    //{
    //  return EnumerateKeys(section).ToArray();
    //}

    //public string GetUnderlyingKey(string key)
    //{
    //  return GetUnderlyingKey(null, key);
    //}

    //public string GetUnderlyingKey(string section, string key)
    //{
    //  var nameFound = (
    //    from keyName in KeyNames.EnumerateKeyNames(domain, key)
    //    let value = configuration[keyName]
    //    where value != null
    //    select keyName
    //  ).FirstOrDefault();
    //  return nameFound;
    //}
    public string this[string key] => throw new NotImplementedException();

    public string this[string section, string key] => throw new NotImplementedException();

    public string[] GetKeys()
    {
      throw new NotImplementedException();
    }

    public string[] GetKeys(string section)
    {
      throw new NotImplementedException();
    }

    public string GetUnderlyingKey(string key)
    {
      throw new NotImplementedException();
    }

    public string GetUnderlyingKey(string key, string section)
    {
      throw new NotImplementedException();
    }
  }
}
