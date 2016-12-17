using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
  public  class SalesAchievement
    {
      public int FyId { get; set; }
      public int SalesMgId { get; set; }
      public string SalesMgName { get; set; }
      public string SalesMgCode { get; set; }
      public int Target1 { get; set; }
      public int Target2 { get; set; }
      public int Target3 { get; set; }
      public int Target4 { get; set; }
      public int Achvd1 { get; set; }
      public int Achvd2 { get; set; }
      public int Achvd3 { get; set; }
      public int Achvd4 { get; set; }
      public int AchvdPerc1 { get; set; }
      public int AchvdPerc2 { get; set; }
      public int AchvdPerc3 { get; set; }
      public int AchvdPerc4 { get; set; }
      public int ShortFall { get; set; }
    }
 
}
