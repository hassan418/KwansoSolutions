using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwanso.Model.Poco
{
    public abstract class BaseClass
    {
        public int Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public bool IsDeleted { get; set; }
    }
}
