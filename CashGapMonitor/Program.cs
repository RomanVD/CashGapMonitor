using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CashGapMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new CultureInfo("ru-Ru");
            Console.WriteLine("Enter start date");
            DateTime startDate = DateTime.Parse(Console.ReadLine(), provider);
            Console.WriteLine("How mach money do you have on start date?");
            var startSum = Decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter end date");
            DateTime endDate = DateTime.Parse(Console.ReadLine(), provider);
            Console.WriteLine("Enter all your incomming and outcomming money in this perion. For example - '10000 21.12.17'");
            Console.WriteLine("Enter 'stop' word, when you finish");

            var moneyFlow = new List<MoneyFlow>();
            string enteredString = Console.ReadLine();
            do
            {            
                var parsedData = enteredString.Split(" ");
                var sum = Decimal.Parse(parsedData[0]);                
                var date = DateTime.Parse(parsedData[1], provider);
                moneyFlow.Add(new MoneyFlow() { Date = date, Value = sum });

                enteredString = Console.ReadLine();
            }
            while (enteredString != "stop");

            var reportPeriod = new List<DateBalance>();
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                reportPeriod.Add(new DateBalance() { Date = currentDate, Balance = startSum });
                currentDate = currentDate.AddDays(1);
            }

            foreach (var moneyFlowItem in moneyFlow)
            {
                var periodAfterFlowItemDate = reportPeriod.Where(p => p.Date >= moneyFlowItem.Date);
                foreach (var date in periodAfterFlowItemDate)
                {
                    date.Balance += moneyFlowItem.Value;
                }
            }

            foreach (var dateBalance in reportPeriod.OrderBy(p => p.Date))
            {
                Console.WriteLine($"{dateBalance.Date.ToString("dd.MM.yy", provider)}: {dateBalance.Balance}");
            }
            Console.ReadKey();
        }

        class MoneyFlow
        {
            public decimal Value { get; set; }

            public DateTime Date { get; set; }
        }

        class DateBalance
        {
            public decimal Balance { get; set; }

            public DateTime Date { get; set; }
        }
    }
}
