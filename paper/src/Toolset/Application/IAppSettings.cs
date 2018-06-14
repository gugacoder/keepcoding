using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Toolset.Application
{
  public interface IAppSettings
  {
    string this[string key] { get; }

    string this[string section, string key] { get; }

    string[] GetKeys();

    string[] GetKeys(string section);

    string GetUnderlyingKey(string key);

    string GetUnderlyingKey(string key, string section);
  }
}