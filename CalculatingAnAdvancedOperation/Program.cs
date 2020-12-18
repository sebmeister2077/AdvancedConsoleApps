using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatingAnAdvancedOperation
{
    class Program
    {
         // <summary>
         //
         //introduceti o expresie oarecare,de ex :(a+b)*c-sin(d-e)
         //
         //
         //</summary>  


        static void Main(string[] args)
        {
            while (true)
            {
                string expression = Console.ReadLine();
                if(!Verificare(expression))
                { Console.WriteLine("Expresie Invalida.");System.Threading.Thread.Sleep(1000);continue; }
                string[] termeni =new string[expression.Length];
                int index = 0;
                bool numar=true;
                bool paranteza = false;
                
                foreach (char c in expression)
                {
                    if (termeni[0] == null)
                    {
                        termeni[0] += c.ToString();//primul element
                        numar = (c >= '0' && c <= '9') || c == 'P' || c == 'E';
                        if (c == '(')
                        { index++; paranteza = true; }
                    }
                    else
                        if (numar == true)
                        if ((c >= '0' && c <= '9') || c == 'I')
                            termeni[index] += c.ToString();
                        else
                        {
                            numar = false;
                            index++;
                            termeni[index] += c.ToString();
                            if (c == '(' || c == ')')
                            { paranteza = true; index++; }
                        }
                    else
                        if (c == '(' || c == ')')
                    { if (!paranteza) index++; paranteza = true; termeni[index] += c.ToString(); index++; }
                    else
                            if (c == 'i' || c == 'n' || c == 'o' || c == 's' || c == 't' || c == 'g' || c == 'a' || c == 'b' || c == 'q' || c == 'r')
                    { termeni[index] += c.ToString(); paranteza = false; }
                    else
                    {
                        if (paranteza == false)
                            index++;
                        paranteza = false;
                        termeni[index] += c.ToString();
                        numar = (c >= '0' && c <= '9') || c == 'P' || c == 'E';
                    }
                }
                
                StringBuilder formaPoloneza = new StringBuilder();//forma poloneza
                Stack<string> functions =new Stack<string>();//stiva pt. operatori/functii
                #region Determa Forma Poloneza
                
                //despart forma normala in cuvinte
                foreach (string termen in termeni)
                {
                    if (termen == null || termen == "")
                        break;
                    //determin daca este operand sau operator
                    bool operand = false;
                    if (termen == "+" || termen == "-" || termen == "%" ||
                        termen == "*" || termen == "/" ||
                        termen == "^" || termen == "sin" ||
                        termen == "cos" || termen == "tan" ||
                        termen == "log" || termen == "(" || termen == "abs"||
                        termen == "sqrt" | termen == ")")
                        operand = false;
                    else
                        operand = true;
                    //daca este operand
                    if (operand)
                    {
                    //se introduce in forma_poloneza
                    if (termen == "E")
                        formaPoloneza.Append(Math.E);
                        else
                            if (termen == "PI")
                            formaPoloneza.Append(Math.PI);
                        else
                            formaPoloneza.Append(termen);
                        formaPoloneza.Append(" ");
                        //poloneza.Text = forma_poloneza;
                    }
                    //terminat daca este operand

                    //daca este operator
                    if (operand == false)//este operator
                    {
                        //il pun in stiva
                        functions.Push(termen);

                        #region prioritate 1
                        if (termen == "*" && termen == "/" || functions.Peek() == "%" ||
                            termen == "^" || termen == "sin" ||
                            termen == "cos" || termen == "tan" || termen == "abs" ||
                            termen == "log"||termen== "sqrt")
                            ;//nu fac nimic
                        #endregion

                        #region prioritate 2
                        if (termen == "+" || termen == "-")
                        {
                            //il scot temporar din stiva
                            functions.Pop();
                            //mutam in forma_poloneza toti
                            //operatorii de prioritate 1
                            while (functions.Count > 0 && (functions.Peek() == "*" || functions.Peek() == "%" ||
                                functions.Peek() == "/" || functions.Peek() == "^" ||
                                functions.Peek() == "sin" || functions.Peek() == "cos" ||
                                functions.Peek() == "tan" || termen== "sqrt"||
                                functions.Peek() == "log" || termen == "abs"))
                                formaPoloneza .Append(functions.Pop()).Append(" ");
                        //il reintroducem in stiva
                        functions.Push(termen);
                        }
                        #endregion

                        #region prioritate 0 (parantezele)
                        if (termen == ")")
                        //mut in forma_poloneza toti operatorii
                        //pana la paranteza (
                        {
                        //MessageBox.Show("Varf="+varf+" "+termen);
                        functions.Pop();
                            while (functions.Count > 0 && functions.Peek() != "(")
                                formaPoloneza.Append(functions.Pop()).Append(" ");
                        if(functions.Count>0)
                        functions.Pop();
                        }
                        # endregion
                    }
                }
                while (functions.Count > 0)
                    formaPoloneza.Append(functions.Pop()).Append(" ");
                #endregion
                //am creat forma poloneza,ceea ce inseamna a+b devine a b +,
                //iar (a+b)*c devine a b + c *
                termeni = formaPoloneza.ToString().Split(' ');
                Console.WriteLine($"Forma poloneza:{formaPoloneza.ToString()}");
                Console.WriteLine(Calculeaza(termeni));

            }
        }

        private static bool Verificare(string expr)
        {
            expr += "  ";
            int parandes=0, paraninchis = 0;
            string allcharac = "()sincostanlogsqrtabs+-*/%^0123456789.PIE";
            string numbers = "0123456789PIE";
            string operatii = "+-*/^%";
            string functii = "sincostanlogsqrtabs";
            bool numar = false;
            bool operatie = false;
            bool functie = false;
            for(int i=0;i<expr.Length-2;i++)
            {
                if (expr[i] == '(' || expr[i] == ')')
                {
                    if (expr[i] == '(')
                        parandes++;
                    if (expr[i] == ')')
                        paraninchis++;
                    if (paraninchis > parandes)
                        return false;
                    if (expr[i] == ')' && numar == false)//ex sin(2+)4
                        return false;
                    continue;
                }
                if (!allcharac.Contains(expr[i].ToString())) //caracter invalid
                    return false;
                if (expr[i] == '.' && numar == false)//s-a pus virgula fara un nr in fata
                    return false;
                if (i > 1 && expr[i - 1] == '.' && !numbers.Contains(expr[i].ToString()))//nu s-a pus nr dupa virgula
                    return false;
                if (i > 0)
                {
                    if (operatie)
                    {
                        if (numbers.Contains(expr[i].ToString()) || functii.Contains(expr[i].ToString() + expr[i + 1].ToString() + expr[i + 2].ToString()))// dupa 3+ poate urma un numar sau o functie
                        { operatie=false; numar = true; if (expr[i] == 'P') i++; }
                    }
                    else
                        if (functie)//dupa sin poate urma DOAR un numar ex sin(2+5)
                        if (numbers.Contains(expr[i].ToString()))
                        { functie=false; numar = true; }
                    if (functii.Contains(expr[i].ToString() + expr[i+1].ToString() + expr[i+2].ToString()))
                    {
                        functie=true;
                        if (expr[1] == 'q')
                            i += 3;//skip these 4 charact
                        else
                            i += 2;//skip 3 charact (sin,cos,abs)
                    }
                    else
                    if (operatii.Contains(expr[i]))
                        operatie=true;
                }
                else
                {
                    if (operatii.Contains(expr[0]))// + a - b e incorect 
                        return false;
                    if (expr[0] == '.')
                        return false;
                    if (functii.Contains(expr[0].ToString() + expr[1].ToString() + expr[2].ToString()))
                    {
                        functie=true;
                        if (expr[1] == 'q')
                            i += 3;//skip these 4 charact
                        else
                            i += 2;//skip 3 charact (sin,cos,abs)
                    }
                    else
                    { numar = true; if (expr[i] == 'P') i++; }
                }
                    

            }
            if (parandes != paraninchis)
                return false;
            if (functie || operatie)// ultima chestie gasita a fost o functie sau un operator fara numar
                return false;
            return true;
        }

        private static bool IsOperator(string str)
        {
            return str == "+" || str == "-" || str == "%" ||
                        str == "*" || str == "/" ||
                        str == "^" || str == "sin" ||
                        str == "cos" || str == "tan" ||
                        str == "log" || str == "abs"|| str=="sqrt";
        }
        public static short TipDeOperatie(string str)
        {
            if (str == "+" || str == "-" ||str == "*" || str == "/" ||str == "^"||str == "%" )
                return 2;
            if (str == "sin" ||str == "cos" || str == "tan" || str == "abs"||str=="sqrt")
                return 1;
            return 0;//este numar
        }
        public static double Calc(double nr2,double nr1,string _operator)
        {
            switch(_operator)
            {
                case "+":
                    return nr1 + nr2;
                case "-":
                    return nr1 - nr2;
                case "*":
                    return nr1 * nr2;
                case "/":
                    return nr1 / nr2;
                case "%":
                    return nr1 % nr2;
                case "^":
                    return Math.Pow(nr1,nr2);
            }
            return double.NaN;
        }
        public static double Calc(double nr, string _operator)
        {
            switch(_operator)
            {
                case "sin":
                    return Math.Sin(nr);
                case "cos":
                    return Math.Cos(nr);
                case "tan":
                    return Math.Tan(nr);
                case "abs":
                    return Math.Abs(nr);
                case "sqrt":
                    return Math.Sqrt(nr);
                case "log":
                    return Math.Log(nr);
            }
            return double.NaN;
        }
        public static double Calculeaza(string[] termeni)
        {
            Stack<double> stivaCuNumere = new Stack<double>();
            try
            {
                stivaCuNumere.Push(double.Parse(termeni[0]));
                for(int i=1;i<termeni.Length-1;i++)
                {
                    if (IsOperator(termeni[i]))
                    {
                        if (TipDeOperatie(termeni[i]) == 2)
                            stivaCuNumere.Push(Calc(stivaCuNumere.Pop(), stivaCuNumere.Pop(), termeni[i]));
                        else
                            stivaCuNumere.Push(Calc(stivaCuNumere.Pop(), termeni[i]));
                    }
                    else
                        stivaCuNumere.Push(double.Parse(termeni[i]));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message);  }
            return stivaCuNumere.Pop();
        }
        
    }
}
