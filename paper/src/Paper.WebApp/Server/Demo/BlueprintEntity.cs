﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Paper.Media;
using Paper.Media.Design;
using Paper.Media.Rendering.Entities;

namespace Paper.WebApp.Server.Demo
{
  [ExposeEntity("/Blueprint")]
  [DataContract(Namespace = Namespaces.Default, Name = "Entity")]
  public class BlueprintEntity : Entity
  {
    private readonly static string Guid = System.Guid.NewGuid().ToString();

    [DataMember(EmitDefaultValue = false, Order = 10)]
    public override NameCollection Class { get; set; }
      = "blueprint";

    [DataMember(EmitDefaultValue = false, Order = 30)]
    public override string Title { get; set; }
      = "Blueprint do Site";

    [DataMember(EmitDefaultValue = false, Order = 40)]
    public override PropertyCollection Properties { get; set; }
      = PropertyCollection.Create(new
      {
        HasNavBox = true,
        Info = new
        {
          Guid = Guid,
          Name = "Demo",
          Title = "Site Demonstrativo",
          Description = "Projeto de demonstração do Paper",
          Version = "0.0.1"
        }
      });

    [DataMember(EmitDefaultValue = false, Order = 50)]
    public override LinkCollection Links { get; set; }
      = new LinkCollection
      {
        new Link{ Rel = "index", Title = "Menu", Href = "{Api}/Menu" },
        new Link{ Rel = "link", Title = "Google.com", Href = "google.com" }
      };
  }
}
