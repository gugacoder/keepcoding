using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Toolset.Application
{
  static class AppContext
  {
    private static AsyncLocal<ClaimsPrincipal> _claimsPrincipal = new AsyncLocal<ClaimsPrincipal>();

    public static ClaimsPrincipal ClaimsPrincipal
    {
      get
      {
        if (_claimsPrincipal.Value == null)
        {
          _claimsPrincipal.Value = FindClaimsPrincipal();
        }
        return _claimsPrincipal.Value;
      }
    }

    private static ClaimsPrincipal FindClaimsPrincipal()
    {
      if (ClaimsPrincipal.Current != null)
        return ClaimsPrincipal.Current;

      if (Thread.CurrentPrincipal is ClaimsPrincipal)
        return (ClaimsPrincipal)Thread.CurrentPrincipal;

      if (Thread.CurrentPrincipal != null)
        return new ClaimsPrincipal(Thread.CurrentPrincipal);

      var identity = new GenericIdentity("anonymous");
      var claims = new ClaimsIdentity(identity);
      return new ClaimsPrincipal(claims);
    }
  }
}
