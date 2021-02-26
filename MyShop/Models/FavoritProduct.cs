using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class FavoritProduct
    {
        public FavoritProduct()
        {

        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
