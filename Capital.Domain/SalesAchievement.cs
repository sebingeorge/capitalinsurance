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
      public int TotalTarget { get; set; }
      public int TotalAcvd { get; set; }
      public int Achvd1 { get; set; }
      public int Achvd2 { get; set; }
      public int Achvd3 { get; set; }
      public int Achvd4 { get; set; }
      public int AchvdPerc1 { get; set; }
      public int AchvdPerc2 { get; set; }
      public int AchvdPerc3 { get; set; }
      public int AchvdPerc4 { get; set; }
      public int Jan { get; set; }
      public int Feb { get; set; }
      public int Mar { get; set; }
      public int Apl { get; set; }
      public int May { get; set; }
      public int Jun { get; set; }
      public int July { get; set; }
      public int Aug { get; set; }
      public int Sep { get; set; }
      public int Oct { get; set; }
      public int Nov { get; set; }
      public int Dec { get; set; }
      public int Q1Shortfall { get; set; }
      public int Q2Shortfall { get; set; }
      public int Q3Shortfall { get; set; }
      public int Q4Shortfall { get; set; }
      public int Q1Excess { get; set; }
      public int Q2Excess { get; set; }
      public int Q3Excess { get; set; }
      public int Q4Excess { get; set; }
    }
 
}
