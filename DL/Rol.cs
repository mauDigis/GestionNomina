using System;
using System.Collections.Generic;

namespace DL;

public partial class Rol
{
    public byte IdPuesto { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
