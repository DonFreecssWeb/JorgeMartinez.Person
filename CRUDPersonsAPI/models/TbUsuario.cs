using System;
using System.Collections.Generic;

namespace CRUDPersonsAPI.models;

public partial class TbUsuario
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
