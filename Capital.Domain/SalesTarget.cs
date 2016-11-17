using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
 public  class SalesTarget
    {
     public int FyId { get; set; }
     public int SalesMgId { get; set; }
     public string SalesMgName { get; set; }
     public decimal Quarer1 { get; set; }
     public decimal Quarer2 { get; set; }
     public decimal Quarer3 { get; set; }
     public decimal Quarer4 { get; set; }
    }
}
