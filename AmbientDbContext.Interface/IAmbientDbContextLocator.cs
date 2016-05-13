﻿using ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientDbContext.Interface
{
    public interface IAmbientDbContextLocator
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
