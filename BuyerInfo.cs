using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageCalc
{
    class BuyerInfo
    {
        public double income;
        public int creditScore;
        public double apr;

        public BuyerInfo(double income1, int creditScore1)
        {
            income = income1;
            creditScore = creditScore1;

            if (creditScore >= 620 && creditScore <= 639)
            {
                apr = 0.03947;
            }
            else if (creditScore > 639 && creditScore <= 659)
            {
                apr = 0.03401;
            }
            else if (creditScore > 659 && creditScore <= 679)
            {
                apr = 0.02971;
            }
            else if (creditScore > 679 && creditScore <= 699)
            {
                apr = 0.02757;
            }
            else if (creditScore > 699 && creditScore <= 759)
            {
                apr = 0.02580;
            }
            else if (creditScore > 759 && creditScore <= 850)
            {
                apr = 0.02350;
            }
            else if (creditScore < 620 && creditScore >= 300)
            {
                apr = 0.0420;
            }



        }
        public void PrintInfo()
        {
            Console.WriteLine("Buyer Income: {0}\nCredit Score: {1}\nAPR: {2}\n", income, creditScore, apr);
        }
    }
}
