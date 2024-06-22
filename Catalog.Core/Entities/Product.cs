using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Entities
{
    public class Product
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Id")]
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public ProductBrand Brand { get; set; }
        public ProductType Type { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}
