using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Управление_заказами.Models.DataBase
{
    abstract class Equipment
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

    }
}
