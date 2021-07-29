using System;
/* Calcualtes loan payments for fixed-rate loans
 * 
 * Calculates loans for terms of 15 or 30 years
 * 
 * Determines the total loan value as the result of the home's purchase price, 
 * the down payment amount, origination fee of 1%. Additionally, $2500 for approx taxes
 * and closing costs on the sale should be included in total loan amount
 * 
 * Displays equity (difference between hv and outstanding balances)% and value of home owned by the buyer at 
 * inception based on purchase price, market value, and down payments
 * 
 * Loan insurance is required when total equity at inception < 10% of current market value 
 * (i.e. $450,000 home w/ $32,000 down, tmv of home is $429,500, buyer has to cover the 20,500 deficit
 * and cover at least 10% of total loan so at least 65,000 down (10% + deficit in hv vs loan value) required
 * to avoid paying insurance) Loan insurance is 1% of initial loan value per year, split into equal payments per year
 * 
 * Additional amounts gathered yearly for HOA, monthly total based on yearly fee divided per payment period, then
 * added to base payment.
 * 
 * Additional amounts gathered for Escrow (insurance/taxes): Property tax = 1.25% yearly split monthly,
 * homeowners insurance is 0.75% yearly split monthly, both based off of current mv of home at time of loan
 * inception. Compute payment per term period, most likely monthly, then add that payment to total monthly payment value
 * 
 * Determines if payment is >= 25% of buyer's monthly income and use that value to recommend approve/deny.
 * Deny when >= 25%, else approve.
 * 
 * When denied, then display messages to sugges Placing more money down and Look at buying a more affordable home
 * 
 * Monthly payments:
    Base amount for loan (principle and interest)
    Escrow amount (Homeowners insurance (0.75% yearly thv, split monthly), property tax (1.1% of thv by year, collected monthly)
    HOA Fees (if any, by year, collected monthly)
    Loan Insurance (if applicable), at 1% of the inital loan value per year

 * Payment Formula: P * (r / n) * [(1 + r / n) ^n(t)] / [(1 + r / n)^n(t) - 1]
 * 
 * P: Principle (loan amount)
   r: Annual Interest Rate
   n: Number of payments per year
   t: Term (number of years for the loan)
 * 
 * INFO TO GATHER:
 * P = hpa + dp + (0.01 * hpa) + 2500
 * hpa, Home Purchase amount
 * hmv, Home market value
 * Buyers income
 * t, Term for loan
 * dp, Down payment
 * r, Credit score (APR)
 */
namespace MortgageCalc
{
    class Program
    {
        static string adminPassword = "abcdef123456";
        static void Main(string[] args)
        {
            PasswordPrompt();
            MainMenu();
            LoanInfo newLoan = NewMortgageInfo();
            BuyerInfo newBuyer = NewBuyerInfo();
            Console.WriteLine("\n");
            newLoan.PrintInfo();
            newBuyer.PrintInfo();
            Console.WriteLine("\n");
            double equity = newLoan.principle - newLoan.marketValue;
            double totalMonthlyPayment = TotalMonthlyPayment(newLoan, newBuyer, equity);
            Console.WriteLine("Equity: {0}\nLoan Insurance Required: {1}\nTotal Monthly Payment: {2}", Math.Round(equity,2), CheckEquity(newLoan, equity), Math.Round(totalMonthlyPayment,2));
            ApproveDeny(newLoan, newBuyer, totalMonthlyPayment);
        }

        static void PasswordPrompt()
        {
            while (true)
            {
                TypeWrite("Welcome to the Mortgage Calculator 3000\n");
                TypeWrite("Please enter Admin password: ");
                string pass = Console.ReadLine();
                if (pass != adminPassword)
                {
                    TypeWrite("Incorrect Password, are you sure you are an Admin? Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        static void MainMenu()
        {
            while (true)
            {
                TypeWrite("Welcome Administrator, please select an option\n");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("------         1. New Mortgage         -------");
                Console.WriteLine("------         2. View APR Guide       -------");
                Console.WriteLine("------         3. Quit                 -------");
                Console.WriteLine("----------------------------------------------");
                string menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        AprGuide();
                        continue;
                    case "3":
                        TypeWrite("Goodbye!...\nClosing.......");
                        Environment.Exit(1);
                        break;
                    default:
                        TypeWrite("Invalid Entry, press Enter to try again");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                }
                break;
            }
        }
        static LoanInfo NewMortgageInfo()
        {
            double purchasePrice;
            double marketValue;
            int termLength;
            double downPayment;
            double hoa;


            while (true)
            {
                TypeWrite("Time to calculate a new mortgage, I need a few bits of information first\nWhat is the Home Purchase Price?   ");
                string purchasePriceEntry = Console.ReadLine();

                if (!double.TryParse(purchasePriceEntry, out purchasePrice))
                {
                    TypeWrite("Whoops, thats not a valid price, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break;
            }
            while (true)
            {
                TypeWrite("\nGreat! What is the Home Market Value?   ");
                string marketValueEntry = Console.ReadLine();

                if (!double.TryParse(marketValueEntry, out marketValue))
                {
                    TypeWrite("Whoops, thats not a valid market value, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break;
            }
            while (true)
            {
                TypeWrite("\nAnnnnd stored! What is the buyer's down payment amount?   ");
                string downPaymentEntry = Console.ReadLine();

                if (!double.TryParse(downPaymentEntry, out downPayment))
                {
                    TypeWrite("Whoops, thats not a valid down payment, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break;
            }
            while (true)
            {
                TypeWrite("\nGot it! What is the loan term length? 15 or 30?    ");
                string termLengthEntry = Console.ReadLine();
                switch (termLengthEntry) 
                {
                    case "15":
                        termLength = 15;
                        break;
                    case "30":
                        termLength = 30;
                        break;
                    default:
                        TypeWrite("Whoops, thats not a valid option, please ensure you enter on of the displayed options. Press Enter to try again...\n");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                }
                break;
            }
            while (true)
            {
                TypeWrite("\nOk! What is the neighborhood HOA monthly fee?   ");
                string hoaEntry = Console.ReadLine();

                if (!double.TryParse(hoaEntry, out hoa))
                {
                    TypeWrite("Whoops, thats not a valid amount, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break;
            }
            
            LoanInfo newLoan = new LoanInfo(purchasePrice, marketValue, termLength, downPayment, hoa);
            

            return (newLoan);

        }
        static BuyerInfo NewBuyerInfo()
        {
            double income;
            int creditScore;
            while (true)
            {
                TypeWrite("\nAlright! What is the buyer's monthly income amount?   ");
                string incomeEntry = Console.ReadLine();

                if (!double.TryParse(incomeEntry, out income))
                {
                    TypeWrite("Whoops, thats not a valid income, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break;
            }
            while (true)
            {
                TypeWrite("\nAlright! What is the buyer's credit score?   ");
                string creditEntry = Console.ReadLine();

                if (!Int32.TryParse(creditEntry, out creditScore))
                {
                    TypeWrite("Whoops, thats not a valid score, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                if (creditScore < 300 || creditScore > 850)
                {
                    TypeWrite("Whoops, thats not a valid score, please ensure you enter a number with no extra characters. Press Enter to try again...\n");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break;
            }
            BuyerInfo newBuyer = new BuyerInfo(income, creditScore);
            Console.Clear();
            return newBuyer;

        }
        static void AprGuide()
        {
            TypeWrite("           APR Guid is as follows           \n");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("------ Credit Score:             APR   -------");
            Console.WriteLine("------    <600                  4.200% -------");
            Console.WriteLine("------   620-639                3.947% -------");
            Console.WriteLine("------   640-659                3.401% -------");
            Console.WriteLine("------   660-679                2.971% -------");
            Console.WriteLine("------   680-699                2.757% -------");
            Console.WriteLine("------   700-759                2.580% -------");
            Console.WriteLine("------   760-850                2.350% -------");
            Console.WriteLine("----------------------------------------------\n");
            TypeWrite("Press Enter when you are finished to return to Main Menu");
            Console.ReadLine();
            Console.Clear();

        }
        static double TotalMonthlyPayment(LoanInfo newLoan, BuyerInfo newBuyer, double equity)
        {
            //P * (r / n) * [(1 + r / n) ^ n(t)] / [(1 + r / n) ^ n(t) - 1]
            //P: Principle(loan amount)
            //r: Annual Interest Rate
            //n: Number of payments per year
            //t: Term(number of years for the loan)
            double monthlyBasePayment = newLoan.principle * (newBuyer.apr / 12 * (Math.Pow(1 + newBuyer.apr / 12, newLoan.term * 12))) / (Math.Pow(1 + newBuyer.apr / 12, 12 * newLoan.term) - 1);
            double totalMonthlyPayment;
            //Escrow(insurance / taxes): Property tax = 1.25 % yearly split monthly,
            //homeowners insurance is 0.75 % yearly split monthly, both based off of current mv of home at time of loan
            //inception
            double propertyTaxMonthly = 0.0125 / 12 * newLoan.marketValue;
            double homeownersInsuranceMonthly = 0.0075 / 12 * newLoan.marketValue;
            double loanInsuranceMonthly = 0.01 / 12 * newLoan.principle;
            if (CheckEquity(newLoan, equity))
            {
                totalMonthlyPayment = monthlyBasePayment + propertyTaxMonthly + homeownersInsuranceMonthly + newLoan.hoaFees;
                Console.WriteLine("Monthly Payment Breakdown:\nBase Payment: {0}\nProperty Tax: {1}\nHomeowner's Insurance: {2}\nLoan Insurance: {3}\nHOA Fees: {4}", Math.Round(monthlyBasePayment, 2), Math.Round(propertyTaxMonthly, 2), Math.Round(homeownersInsuranceMonthly, 2), Math.Round(loanInsuranceMonthly,2), newLoan.hoaFees);
            }
            else
            {
                totalMonthlyPayment = monthlyBasePayment + propertyTaxMonthly + homeownersInsuranceMonthly + loanInsuranceMonthly + newLoan.hoaFees;
                Console.WriteLine("Monthly Payment Breakdown:\nBase Payment: {0}\nProperty Tax: {1}\nHomeowner's Insurance: {2}\nHOA Fees: {3}", Math.Round(monthlyBasePayment, 2), Math.Round(propertyTaxMonthly, 2), Math.Round(homeownersInsuranceMonthly, 2), newLoan.hoaFees);
            }
           
            return totalMonthlyPayment;
        }
        static bool CheckEquity(LoanInfo newLoan, double equity)
        {
            if(equity + (0.10 * newLoan.principle) < newLoan.downPayment)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        static void ApproveDeny(LoanInfo newLoan, BuyerInfo newBuyer, double totalMonthlyPayment)
        {
            if (totalMonthlyPayment >= 0.25 * newBuyer.income)
            {
                TypeWrite("Recommended Verdict: Deny Loan...Try a larger down payment or cheaper house...\nPress Enter to close");
                Console.ReadLine();
            }
            else
            {
                TypeWrite("Recommended Verdict: Approve Loan...Congratulations on the new home...\nPress Enter to close");
            }
        }
        public static void TypeWrite(string message)
        {
            Random rdm = new Random();
            for (int i = 0; i < message.Length; i++)
            {
                Console.Write(message[i]);
                System.Threading.Thread.Sleep(rdm.Next(10, 30));
            }
        }
    }
}
