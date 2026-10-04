using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
