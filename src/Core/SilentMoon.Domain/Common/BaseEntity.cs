using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SilentMoon.Domain.Common
{
    public abstract class BaseEntity<TKey>
    {
        public virtual TKey Id { get; set; }
        public virtual DateTime CreateDate{ get; set; }= DateTime.Now;
        public virtual bool IsDeleted { get; set; }= false;
    }
}
