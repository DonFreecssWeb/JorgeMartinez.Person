using System;
using System.Collections.Generic;

namespace CRUDPersonsAPI.models;

public partial class TbPersona
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public DateTime? Fnacimiento { get; set; }
}
