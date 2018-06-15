using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paper.WebApp.Server.Proxies
{
  interface IProxyRegistry
  {
    void Add(Proxy proxy);

    void Remove(string path);

    Proxy FindExact(string path);

    Proxy FindByPrefix(string path);
  }
}