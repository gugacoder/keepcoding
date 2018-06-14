using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Toolset.Application
{
  class AppUser : IAppUser
  {
    public const string DefaultDomain = "";
    public const string AnonymousUser = "Anonymous";

    public AppUser(string login)
    {
      this.Login = login;
      this.Name = login.Split('@').First();
      this.Domain = login.Split('@').Skip(1).FirstOrDefault() ?? DefaultDomain;
    }

    public AppUser(string name, string domain)
    {
      this.Login = $"{name}@{domain}".ToLower();
      this.Name = name;
      this.Domain = domain;
    }

    public string Login { get; }

    public string Name { get; }

    public string Domain { get; }

    public override string ToString() => Login;
  }
}
