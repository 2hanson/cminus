using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Parse
{
    public class LL1StackElement
    {
        /// <summary>
        /// true NonTerminal
        /// false Terminal
        /// </summary>
        public bool Flag{get; set;}
        public NonTerminalType NonTerminalType{get; set;}
        public Lex.TokenType TokenType{get; set;}
        public int Ascii;
        public int ProductionNum{get; set;}

        public LL1StackElement(int productionNum)
        {
            ProductionNum = productionNum;
        }

        public LL1StackElement(bool flag, NonTerminalType nonTerminalType, int productionNum)
            : this(productionNum)
        {
            this.Flag = flag;
            this.NonTerminalType = nonTerminalType;
        }
        public LL1StackElement(bool flag, Lex.TokenType tokenType, int productionNum)
            : this(productionNum)
        {
            this.Flag = flag;
            this.TokenType = tokenType;
        }
        public LL1StackElement(bool flag, int ascii, int productionNum)
            : this(productionNum)
        {
            this.Flag = flag;
            this.Ascii = ascii;
        }
    }
}
