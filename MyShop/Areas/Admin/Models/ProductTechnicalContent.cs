using MyShop.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductTechnicalContent
    {
        public ProductTechnicalContent()
        {

        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ContentType ContentType { get; set; }
        public string Content { get; set; }
        public string AltTextForImage { get; set; }
        public int ContentOrder { get; set; }
    }
}
