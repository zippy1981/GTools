using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public class Parser
    {
        private string InputString;
        private int ReadPos;

        public Parser()
        {
        }
        
        public Operation Parse(string inputString)
        {
            InputString = inputString;
            ReadPos = 0;

            return Expression();            
        }

        private class OperationDescription
        {
            public delegate Operation Constructor(Operation a, Operation b);

            public readonly string Name;
            public readonly Constructor Create;

            public OperationDescription(string name, Constructor create)
            {
                Name = name;
                Create = create;
            }
        };

        private static List<OperationDescription[]> ExpressionLevels = null;

        private void CreateExpressionLevels()
        {
            ExpressionLevels = new List<OperationDescription[]>();

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( ":=", (a, b) => new Operations.Assign(a, b) ),
            });

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "||", (a, b) => new Operations.Or(a, b) ),
            });

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "&&", (a, b) => new Operations.And(a, b) ),
            });

            ExpressionLevels.Add( new OperationDescription[] {
                new OperationDescription( "==", (a, b) => new Operations.IsEqual(a, b) ),
                new OperationDescription( "!=", (a, b) => new Operations.IsNotEqual(a, b) )
            } );

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "<=", (a, b) => new Operations.IsLessThanOrEqual(a, b) ),
                new OperationDescription( ">=", (a, b) => new Operations.IsGreaterThanOrEqual(a, b) ),
                new OperationDescription( "<", (a, b) => new Operations.IsLessThan(a, b) ),
                new OperationDescription( ">", (a, b) => new Operations.IsGreaterThan(a, b) ),
            } );

            ExpressionLevels.Add( new OperationDescription[] {
               new OperationDescription( "+", (a, b) => new Operations.Add(a, b) ),
               new OperationDescription( "-", (a, b) => new Operations.Subtract(a, b) )
            } );

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "*", (a, b) => new Operations.Multiply(a, b) ),
                new OperationDescription( "/", (a, b) => new Operations.Divide(a, b) ),
                new OperationDescription( "%", (a, b) => new Operations.Modulo(a, b) )
            });

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "|", (a, b) => new Operations.BitwiseOr(a, b) ),
            });

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "^", (a, b) => new Operations.BitwiseXor(a, b) )
            });

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "&", (a, b) => new Operations.BitwiseAnd(a, b) ),
            });

            ExpressionLevels.Add(new OperationDescription[] {
                new OperationDescription( "**", (a, b) => new Operations.Power(a, b) ),
                new OperationDescription( ">>", (a, b) => new Operations.RightShift(a, b) ),
                new OperationDescription( "<<", (a, b) => new Operations.LeftShift(a, b) )
            });

        }

        private Operation Expression()
        {            
            return Expression(0);
        }

        private Operation Expression(int oplevel)
        {
            if (null == ExpressionLevels)
                CreateExpressionLevels();

            if (oplevel == ExpressionLevels.Count)
                return Number();

            Operation a = Expression(oplevel + 1);
            if( a == null )
                throw new SyntaxError(ReadPos, InputString);

            while(true)
            {
                OperationDescription od = FindMatchingDescription(oplevel);
                if (od == null)
                    return a;

                Operation b = Expression(oplevel + 1);
                if (b == null)
                    throw new SyntaxError(ReadPos, InputString);

                a = od.Create(a, b);
            }
        }

        private OperationDescription FindMatchingDescription(int oplevel)
        {
            foreach (OperationDescription od in ExpressionLevels[oplevel])
            {
                if (Is(od.Name))
                {
                    return od;
                }
            }
            return null;
        }

        private Operation Number()
        {
            bool negate = false;
            bool not = false;
            bool negative = false;

            if (Is("!"))
            {
                not = true;
            }

            if (Is("~"))
            {
                negate = true;
            }

            if (Is("-"))
            {
                negative = true;
            }

            Operation o = INumber();
            if (negative)
                o = new Operations.Negative(o);
            if (negate)
                o = new Operations.BitwiseNegative(o);
            if (not)
                o = new Operations.Not(o);
            return o;
        }

        private Operation INumber()
        {
            // bracketed number ?
            string name = null;
            if (IsName(ref name))
            {
                // function call expected. functions can have arbitrary numbers of arguments;
                // the actual number of arguments is determined only at runtime!
                if (Is("("))
                {
                    List<Operation> args = new List<Operation>();
                    if( !Is(")") )
                    {
                        while (true)
                        {

                            // this is a bracket, use dat instead
                            Operation o = Expression();
                            if (o == null)
                                throw new SyntaxError(ReadPos, InputString);

                            args.Add(o);

                            if (Is(")"))
                                break;

                            if( !Is(",") )
                                throw new SyntaxError(ReadPos, InputString);

                        }
                    }
                    return new Operations.FunctionCall(name, args);
                }
                else
                {
                    return new Operations.Variable(name);
                }
            }

            if (Is("("))
            {
                // this is a bracket, use dat instead
                Operation o = Expression();
                if ((o == null) || !Is(")"))
                    throw new SyntaxError(ReadPos, InputString);

                return o;
            }

            long ln = 0;
            if (IsNumber(ref ln))
            {
                // check if this is really a decimal number
                if ((ReadPos < InputString.Length) && (InputString[ReadPos] == '.'))
                {
                    int sp = ReadPos;
                    ++ReadPos;
                    decimal dn = ln;
                    if (IsFraction(ref dn))
                    {
                        return new Value(dn);
                    }
                    ReadPos = sp;
                }
                return new Value(ln);
            }
            throw new SyntaxError(ReadPos, InputString);
        }


        private bool IsFraction(ref decimal dn)
        {
            int digit = 0;
            decimal value = 1;
            if (IsDecDigit(ref digit))
            {
                do
                {
                    value /= 10;
                    dn += value * digit;
                }
                while (IsDecDigit(ref digit));
                return true;
            }
            return false;
        }

        private bool IsNumber(ref long result)
        {
            int digit = 0;
            result = 0;            
            if( IsDecDigit(ref digit) )
            {
                // special case: this is a fraction indicator
                if (digit == 0)
                {
                    if (Is("x") || Is("X"))
                    {
                        do
                        {
                            result *= 16;
                            result += digit;
                        }
                        while (IsHexDigit(ref digit));
                        return true;
                    }
                    else if (Is("o"))
                    {
                        do
                        {
                            result *= 8;
                            result += digit;
                        }
                        while (IsOctDigit(ref digit));
                        return true;
                    }
                    else if (Is("b"))
                    {
                        do
                        {
                            result *= 2;
                            result += digit;
                        }
                        while (IsBinDigit(ref digit));
                        return true;
                    }
                }
                do
                {
                    result *= 10;
                    result += digit;
                }
                while( IsDecDigit( ref digit ) );
                return true;
            }
            return false;
        }

        private bool IsDecDigit(ref int digit)
        {            
            if (ReadPos < InputString.Length)
            {
                digit = "0123456789".IndexOf(InputString[ReadPos]);
                if (digit >= 0)
                {
                    ++ReadPos;
                    return true;
                }
            }
            return false;
        }

        private bool IsHexDigit(ref int digit)
        {
            if (ReadPos < InputString.Length)
            {
                digit = "0123456789ABCDEFabcdef".IndexOf(InputString[ReadPos]);
                if (digit >= 0)
                {
                    if (digit > 15)
                        digit -= 6;

                    ++ReadPos;
                    return true;
                }
            }
            return false;
        }

        private bool IsOctDigit(ref int digit)
        {
            if (ReadPos < InputString.Length)
            {
                digit = "01234567".IndexOf(InputString[ReadPos]);
                if (digit >= 0)
                {
                    ++ReadPos;
                    return true;
                }
            }
            return false;
        }

        private bool IsBinDigit(ref int digit)
        {
            if (ReadPos < InputString.Length)
            {
                digit = "01".IndexOf(InputString[ReadPos]);
                if (digit >= 0)
                {
                    ++ReadPos;
                    return true;
                }
            }
            return false;
        }

        private void SkipWhitespaces()
        {
            while (true)
            {
                if (ReadPos >= InputString.Length)
                    break;

                char c = InputString[ReadPos];
                if ((c == ' ') || (c == '\t'))
                {
                    ++ReadPos;
                }
                else break;
            }
        }

        private bool IsChar()
        {
            if (ReadPos < InputString.Length)
            {
                if( "ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz".IndexOf(InputString[ReadPos]) >= 0 )
                {
                    ++ReadPos;
                    return true;
                }
            }
            return false;
        }

        private bool IsCharOrDigit()
        {
            if (ReadPos < InputString.Length)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz0123456789".IndexOf(InputString[ReadPos]) >= 0)
                {
                    ++ReadPos;
                    return true;
                }
            }
            return false;
        }

        private bool IsName(ref string name)
        {
            SkipWhitespaces();

            int sp = ReadPos;
            if (IsChar())
            {
                while (IsCharOrDigit()) ;

                name = InputString.Substring(sp, ReadPos - sp);
                return true;
            }
            return false;
        }

        private bool Is(string name)
        {
            SkipWhitespaces();
            if (ReadPos+name.Length <= InputString.Length)
            {
                if (InputString.Substring(ReadPos, name.Length) == name)
                {
                    ReadPos += name.Length;
                    return true;
                }
            }
            return false;
        }
    }
}
