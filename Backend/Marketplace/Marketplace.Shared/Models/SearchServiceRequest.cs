using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Shared.Models
{
    public class SearchServiceRequest
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TypeMethod Method { get; set; }
    }

    public enum TypeMethod
    {
        Add, Delete, Update
    }
}
