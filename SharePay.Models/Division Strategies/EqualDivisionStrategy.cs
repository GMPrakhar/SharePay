using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models.Division_Strategies
{
    public class EqualDivisionStrategy : DivisionStrategy
    {
        public override string Name => "Divide Equally";

        public override string Description => "Divides the amount equally between users.";
    }
}
