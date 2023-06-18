using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities
{
    [Index(nameof(Name), IsUnique = true)]

    public class ProductType : BaseEntity
    {
        public string Name { get; set; }
    }
}
