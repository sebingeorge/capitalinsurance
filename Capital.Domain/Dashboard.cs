using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class Dashboard
    {

        public IEnumerable<MonthlyAcheivementcoveragewise> MonthlyAcheivementcoveragewise { get; set; }
        public IEnumerable<MonthlySales> MonthlySales { get; set; }

        public IEnumerable<EmployeeAchievementVsTraget> EmployeeAchievementVsTraget { get; set; }

        public IEnumerable<CoverageVsSales> CoverageVsSales { get; set; }
    }
}
