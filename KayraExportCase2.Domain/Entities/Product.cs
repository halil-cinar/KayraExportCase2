using KayraExportCase2.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Domain.Entities
{
    public class Product:Entity
    {
        public string Title { get; set; }=string.Empty;
        public decimal Price { get; set; }
    }
}
