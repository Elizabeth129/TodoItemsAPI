﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.DataRepository
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        { 
        }
        public TodoContext() : base()
        {

        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
