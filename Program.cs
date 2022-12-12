try
{
    string expression = "( 20 + 3 * 20 ) * 10 / ( 2 * 5 )"; //correct answer: 80
    Console.WriteLine(Calculator.Calculate(expression.Trim().Split().ToList()));

}
catch(ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}

public class Calculator {
    public static int Calculate(List<string> exp) 
    {
        //the minimum length to be a valid expression (lefthand argument, operation, righthand argument)
        if(exp.Count == 3) return Calculator.operations[exp[1]](int.Parse(exp[0]), int.Parse(exp[2]));

        // dealing with parenthesis...
        // if there is a opening parenthesis, mark that location, and then find the closing parenthesis and recurse
        bool parenFound = false;
        do
        {
            parenFound = false;
            int parenthesisStart = -1;
            int parenthesisEnd = -1;
            for(int i = 0; i < exp.Count; i++)
            {
                if(exp[i] == "(") 
                {
                    parenthesisStart = i;
                    parenFound = true;
                }
                if(exp[i] == ")") parenthesisEnd = i;
            }
            //if valid parenthesis
            if(parenthesisStart < parenthesisEnd) 
            {
                exp[parenthesisStart] = Calculate(exp.GetRange(parenthesisStart + 1, parenthesisEnd - parenthesisStart - 1)).ToString();
                exp = exp.Select((s, i) => {
                    if(i > parenthesisStart && i <= parenthesisEnd) return "";
                    else return s;
                }).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            }
        } while(parenFound);
        
        //if the expression is a bit more complex..
        //first get the higher priority operation (aka * and /)
        for(int i = 0; i < exp.Count; i++)
        {
            //calculate the result and replace the smaller expression with it 
            if((exp[i] == "*" || exp[i] == "/"))
            {
                int result = Calculator.operations[exp[i]](int.Parse(exp[i - 1]), int.Parse(exp[i + 1]));
                exp[i - 1] = "";
                exp[i + 1] = "";
                exp[i] = result.ToString();
                exp = exp.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                i = 0;
            }
        }
        for(int i = 0; i < exp.Count; i++)
        {
            //calculate the result and replace the smaller expression with it 
            if((exp[i] == "+" || exp[i] == "-"))
            {
                int result = Calculator.operations[exp[i]](int.Parse(exp[i - 1]), int.Parse(exp[i + 1]));
                exp[i - 1] = "";
                exp[i + 1] = "";
                exp[i] = result.ToString();
                exp = exp.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                i = 0;
            }
        }
        return int.Parse(exp[0]);
    }

    //A DS to translate symbols to operations.
    private static Dictionary<string, Func<int, int, int>> operations = new Dictionary<string, Func<int, int, int>> {
        {"+", Calculator.Add},
        {"-", Calculator.Subtract},
        {"*", Calculator.Multiply},
        {"/", Calculator.Divide}
    };
    private static int Add(int a, int b) => a + b;
    private static int Subtract(int a, int b) => a - b;
    private static int Multiply(int a, int b) => a * b;
    private static int Divide(int a, int b) => a / b;
}