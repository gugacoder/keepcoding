using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Toolset.Application
{
  public interface IAppUser
  {
    string Login { get; }

    string Name { get; }

    string Domain { get; }
  }
}