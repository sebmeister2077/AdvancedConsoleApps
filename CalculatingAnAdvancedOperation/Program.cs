using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatingAnAdvancedOperation
{
    class Program
    {
        static void Main(string[] args)
        {
            int ReturnPriority(string s)
            {
                switch(s)
                {
                    case "(":
                        return 1;
                    case "[":
                        return 1;
                    case "{":
                        return 1;
                    case ")":
                        return 1;
                    case "]":
                        return 1;
                    case "}":
                        return 1;
                    case "+":
                        return 1;
                    case "-":
                        return 1;
                    case "*":
                        return 1;
                    case "/":
                        return 1;
                    default:
                        return 5;
                }
            }

            while (true)
            {
                Stack<string> stivaOperatori = new Stack<string>();
                Stack<double> stivaOperanzi = new Stack<double>();
                string expression = Console.ReadLine();
                foreach (char c in expression)
                {
                    int nr;
                    if (double.TryParse(c.ToString(), out nr))
                        stivaOperanzi.Push(nr);
                    else
                    {




                    }


                }


            }
        }
    }
}
