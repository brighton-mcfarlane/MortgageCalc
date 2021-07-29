using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageCalc
{
    class LoanInfo
    {
        public double purchasePrice;
        public double marketValue;
        public int term;
        public double downPayment;
        public double principle;
        public double hoaFees;
        public LoanInfo(double purchPrice, double marketVal, int termLim, double downPay, double hoaFee)
        {
            purchasePrice = purchPrice;
            marketValue = marketVal;
            term = termLim;
            downPayment = downPay;
            principle = (purchPrice - downPay) + (0.01f * purchPrice) + 2500;
            hoaFees = hoaFee;
        }

        public void PrintInfo()
        {
            Console.WriteLine("Purchase Price: {0}\nMarket Value: {1}\nDown Payment: {2}\nLoan Term: {3}\nLoan Total: {4}", purchasePrice, marketValue, downPayment, term, Math.Round(principle, 2));
        }


    }
}
