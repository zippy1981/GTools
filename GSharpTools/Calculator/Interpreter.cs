using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public class Interpreter
    {
        public readonly Dictionary<string, Function> Functions = new Dictionary<string, Function>();
        public readonly Dictionary<string, Value> Variables = new Dictionary<string, Value>();
       
        public Interpreter()
        {
            Reset();
        }

        public void Reset()
        {
            // create builtin constants
            Variables.Clear();
            Variables["pi"] = new Value(3.141592653589793238462643383279502884197169399375105820974944592307816406286m);
            Variables["e"] = new Value(2.718281828459045235360287471352662497757247093699959574966967627724076630353m);
            Variables["true"] = new Value(true);
            Variables["false"] = new Value(false);

            // create builtin functions
            Functions.Clear();
            Functions["abs"] = new Functions.MathFunction(Math.Abs);
            Functions["abs"] = new Functions.MathFunction(Math.Abs);
            Functions["acos"] = new Functions.MathFunction(Math.Acos);
            Functions["asin"] = new Functions.MathFunction(Math.Asin);
            Functions["atan"] = new Functions.MathFunction(Math.Atan);
            Functions["ceil"] = new Functions.MathFunction(Math.Ceiling);
            Functions["cos"] = new Functions.MathFunction(Math.Cos);
            Functions["cosh"] = new Functions.MathFunction(Math.Cosh);
            Functions["exp"] = new Functions.MathFunction(Math.Exp);
            Functions["floor"] = new Functions.MathFunction(Math.Floor);
            Functions["log"] = new Functions.MathFunction(Math.Log);
            Functions["log10"] = new Functions.MathFunction(Math.Log10);
            Functions["round"] = new Functions.MathFunction(Math.Round);
            Functions["sin"] = new Functions.MathFunction(Math.Sin);
            Functions["sinh"] = new Functions.MathFunction(Math.Sinh);
            Functions["sqrt"] = new Functions.MathFunction(Math.Sqrt);
            Functions["tan"] = new Functions.MathFunction(Math.Tan);
            Functions["tanh"] = new Functions.MathFunction(Math.Tanh);
            Functions["truncate"]  = new Functions.MathFunction(Math.Truncate);
            Functions["sum"] = new Functions.ApplyOperation((a, b) => new Operations.Add(a, b));
            Functions["or"]  = new Functions.ApplyOperation((a, b) => new Operations.Or(a, b));
            Functions["and"] = new Functions.ApplyOperation((a, b) => new Operations.And(a, b));
            Functions["mul"] = new Functions.ApplyOperation((a, b) => new Operations.Multiply(a, b));
            Functions["band"] = new Functions.ApplyOperation((a, b) => new Operations.BitwiseAnd(a, b));
            Functions["bor"] = new Functions.ApplyOperation((a, b) => new Operations.BitwiseOr(a, b));
            Functions["bxor"] = new Functions.ApplyOperation((a, b) => new Operations.BitwiseXor(a, b));
        }

        public Function LookupFunction(string name)
        {
            try
            {
                return Functions[name];
            }
            catch (KeyNotFoundException)
            {
                throw new BadFunctionNameError(name);
            }
        }
        
        public Value LookupVariable(string name)
        {
            try
            {
                return new Value(Variables[name]);
            }
            catch (KeyNotFoundException)
            {
                throw new BadFunctionNameError(name);
            }
        }
    }
}
