using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.Domain
{
    public abstract class BaseDomainModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? LastModifieldDate { get; set; }
        public string? LastModifieldBy { get; set; }

    }
}
