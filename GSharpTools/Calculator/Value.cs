using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator
{
    public enum ValueType
    {
        Boolean = 0,
        Integer = 1,
        Decimal = 2,
    };

    public class Value : Operation
    {
        public bool Bool
        {
            get
            {
                return (bool)Data;
            }
        }

        public long Integer
        {
            get
            {
                return (long)Data;
            }
        }

        public decimal Decimal
        {
            get
            {
                return (decimal)Data;
            }
        }

        public SerializedNumber Serialized
        {
            get
            {
                SerializedNumber result = new SerializedNumber();
                result.Data = Data;
                result.Type = Type;
                return result;
            }
        }

        private object Data;
        public ValueType Type;
        private static readonly ValueType[,] CastArray = new ValueType[3,3] {
            {ValueType.Boolean, ValueType.Integer, ValueType.Decimal}, 
            {ValueType.Integer, ValueType.Integer, ValueType.Decimal}, 
            {ValueType.Decimal, ValueType.Decimal, ValueType.Decimal}, 
        };

        public Value(Operation o, Interpreter i)
        {
            Value src = o.Evaluate(i);
            Data = src.Data;
            Type = src.Type;
        }

        public Value(Value src)
        {
            Data = src.Data;
            Type = src.Type;
        }

        public Value(bool value)
        {
            Data = value;
            Type = ValueType.Boolean;
        }

        public Value(long value)
        {
            Data = value;
            Type = ValueType.Integer;
        }

        public Value(decimal value)
        {
            Data = value;
            Type = ValueType.Decimal;
        }

        public override string ToString()
        {
            if (Data == null)
                return "null";
            return Data.ToString();
        }

        public void CastSameType(Value other)
        {
            ValueType targetType = CastArray[(int)Type, (int)other.Type];

            CastAsType(targetType);
            other.CastAsType(targetType);
            Debug.Assert(Type == other.Type);
        }

        public void CastSameType(Value other, ValueType minType)
        {
            ValueType targetType = CastArray[(int)Type, (int)other.Type];
            switch (minType)
            {
                case ValueType.Integer:
                    if (targetType == ValueType.Boolean)
                        targetType = ValueType.Integer;
                    break;
                case ValueType.Decimal:
                    if (targetType == ValueType.Boolean || targetType == ValueType.Integer)
                        targetType = ValueType.Decimal;
                    break;
            }

            CastAsType(targetType);
            other.CastAsType(targetType);
            Debug.Assert(Type == other.Type);
        }


        public void CastAsBoolean()
        {
            Debug.Assert(Type != ValueType.Boolean);
            switch(Type)
            {
                case ValueType.Decimal:
                    Data = ((decimal)Data) != 0.0m;
                    break;
                case ValueType.Integer:
                    Data = ((long)Data) != 0;
                    break;
                default:
                    Debug.Assert(false, string.Format("Bad type {0} encountered", Type));
                    break;
            }
            Type = ValueType.Boolean;
        }

        public void CastAsInteger()
        {
            Debug.Assert(Type != ValueType.Integer);
            switch(Type)
            {
                case ValueType.Decimal:
                    Data = (long)Math.Round((decimal)Data);
                    break;
                case ValueType.Boolean:
                    Data = ((bool)Data) ? 1 : 0;
                    break;
                default:
                    Debug.Assert(false, string.Format("Bad type {0} encountered", Type));
                    break;
            }
            Type = ValueType.Integer;
        }

        public void CastAsDecimal()
        {
            Debug.Assert(Type != ValueType.Decimal);
            switch(Type)
            {
                case ValueType.Integer:
                    Data = (decimal)((long)Data);
                    break;
                case ValueType.Boolean:
                    Data = ((bool)Data) ? 1.0m : 0.0m;
                    break;
                default:
                    Debug.Assert(false, string.Format("Bad type {0} encountered", Type));
                    break;
            }
            Type = ValueType.Decimal;
        }

        override public Value Evaluate(Interpreter i)
        {
            return new Value(this);
        }

        public void CastAsType(ValueType targetType)
        {
            if (targetType != Type)
            {
                switch (targetType)
                {
                    case ValueType.Boolean:
                        CastAsBoolean();
                        break;
                    case ValueType.Integer:
                        CastAsInteger();
                        break;
                    case ValueType.Decimal:
                        CastAsDecimal();
                        break;
                    default:
                        Debug.Assert(false, string.Format("Bad target type {0} encountered", targetType));
                        break;
                }
            }
        }
    }
}
