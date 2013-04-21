using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Parse
{

    public static class LL1Table
    {
        const int TABLESIZE = 300;
        //LL1分析表
        public static int[,] LL1table = new int[TABLESIZE, TABLESIZE];

        /// <summary>
        /// 创建LL1分析表:初始数组（表）中的每一项都为0；根据LL1文法给数组赋值（填表）；填好后，若值为0，表示无产生式可选，其他，为选中的产生式  
        /// </summary>
        public static void CreatLL1Table()
        {
            //初始化LL1表元素
            for (int i = 0; i < TABLESIZE; i++)
            {
                for (int j = 0; j < TABLESIZE; j++)
                {
                    LL1table[i, j] = 0;
                }
            }
            //1
            LL1table[Convert.ToInt16(NonTerminalType.Program), Convert.ToInt16(TokenType.INT)] = 1;
            LL1table[Convert.ToInt16(NonTerminalType.Program), Convert.ToInt16(TokenType.VOID)] = 1;
            //2
            LL1table[Convert.ToInt16(NonTerminalType.DeclarationList), Convert.ToInt16(TokenType.INT)] = 2;
            LL1table[Convert.ToInt16(NonTerminalType.DeclarationList), Convert.ToInt16(TokenType.VOID)] = 2;
            //3
            LL1table[Convert.ToInt16(NonTerminalType.DeclarationList2), Convert.ToInt16(TokenType.EOF)] = 3;
            //4
            LL1table[Convert.ToInt16(NonTerminalType.DeclarationList2), Convert.ToInt16(TokenType.INT)] = 4;
            LL1table[Convert.ToInt16(NonTerminalType.DeclarationList2), Convert.ToInt16(TokenType.VOID)] = 4;
            //5
            LL1table[Convert.ToInt16(NonTerminalType.Declaration), Convert.ToInt16(TokenType.INT)] = 5;
            LL1table[Convert.ToInt16(NonTerminalType.Declaration), Convert.ToInt16(TokenType.VOID)] = 5;
            //6?
            LL1table[Convert.ToInt16(NonTerminalType.Declaration), Convert.ToInt16(TokenType.INT)] = 6;
            LL1table[Convert.ToInt16(NonTerminalType.Declaration), Convert.ToInt16(TokenType.VOID)] = 6;
            //7
            LL1table[Convert.ToInt16(NonTerminalType.VarDeclaration), Convert.ToInt16(TokenType.INT)] = 7;
            LL1table[Convert.ToInt16(NonTerminalType.VarDeclaration), Convert.ToInt16(TokenType.VOID)] = 7;
            //8
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(TokenType.INT)] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(TokenType.VOID)] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(TokenType.ID)] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(TokenType.NUMBER)] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16('(')] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(';')] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16('}')] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(TokenType.EOF)] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(',')] = 8;
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16(')')] = 8;
            //9
            LL1table[Convert.ToInt16(NonTerminalType.Array), Convert.ToInt16('[')] = 9;
            //10
            LL1table[Convert.ToInt16(NonTerminalType.TypeSpecifier), Convert.ToInt16(TokenType.INT)] = 10;
            //11
            LL1table[Convert.ToInt16(NonTerminalType.TypeSpecifier), Convert.ToInt16(TokenType.VOID)] = 11;
            //12
            LL1table[Convert.ToInt16(NonTerminalType.FunDeclaration), Convert.ToInt16(TokenType.INT)] = 12;
            LL1table[Convert.ToInt16(NonTerminalType.FunDeclaration), Convert.ToInt16(TokenType.VOID)] = 12;
            //13
            LL1table[Convert.ToInt16(NonTerminalType.Params), Convert.ToInt16(TokenType.INT)] = 13;
            LL1table[Convert.ToInt16(NonTerminalType.Params), Convert.ToInt16(TokenType.VOID)] = 13;
            //14
            LL1table[Convert.ToInt16(NonTerminalType.Params), Convert.ToInt16(')')] = 14;
            //15
            LL1table[Convert.ToInt16(NonTerminalType.ParamList), Convert.ToInt16(TokenType.INT)] = 15;
            LL1table[Convert.ToInt16(NonTerminalType.ParamList), Convert.ToInt16(TokenType.VOID)] = 15;
            //16
            LL1table[Convert.ToInt16(NonTerminalType.ParamList2), Convert.ToInt16(')')] = 16;
            //17
            LL1table[Convert.ToInt16(NonTerminalType.ParamList2), Convert.ToInt16(',')] = 17;
            //18
            LL1table[Convert.ToInt16(NonTerminalType.Param), Convert.ToInt16(TokenType.INT)] = 18;
            LL1table[Convert.ToInt16(NonTerminalType.Param), Convert.ToInt16(TokenType.VOID)] = 18;
            //19
            LL1table[Convert.ToInt16(NonTerminalType.CompoundStmt), Convert.ToInt16('{')] = 19;
            //20
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.ID)] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.NUMBER)] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16('(')] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(';')] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16('}')] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.IF)] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.WHILE)] = 20;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.RETURN)] = 20;
            //21
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.INT)] = 21;
            LL1table[Convert.ToInt16(NonTerminalType.LocalDeclarations), Convert.ToInt16(TokenType.VOID)] = 21;
            //22
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16('}')] = 22;
            //23
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16(TokenType.ID)] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16(TokenType.NUMBER)] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16('(')] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16(';')] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16('{')] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16(TokenType.IF)] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16(TokenType.WHILE)] = 23;
            LL1table[Convert.ToInt16(NonTerminalType.StatementList), Convert.ToInt16(TokenType.RETURN)] = 23;
            //24
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16(TokenType.ID)] = 24;
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16(TokenType.NUMBER)] = 24;
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16('(')] = 24;
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16(';')] = 24;
            //25
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16('{')] = 25;
            //26
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16(TokenType.IF)] = 26;
            //27
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16(TokenType.WHILE)] = 27;
            //28
            LL1table[Convert.ToInt16(NonTerminalType.Statement), Convert.ToInt16(TokenType.RETURN)] = 28;
            //29
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16(TokenType.ID)] = 29;
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16(TokenType.NUMBER)] = 29;
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16('(')] = 29;
            //30
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16(';')] = 30;
            //31
            LL1table[Convert.ToInt16(NonTerminalType.SelectionStmt), Convert.ToInt16(TokenType.IF)] = 31;
            
            ///////////////////////////////////////////////error 公共前缀 32，33 else
            //32
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(TokenType.ID)] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(TokenType.NUMBER)] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16('(')] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(';')] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16('}')] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16('{')] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(TokenType.IF)] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(TokenType.WHILE)] = 32;
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(TokenType.RETURN)] = 32;
            //33
            LL1table[Convert.ToInt16(NonTerminalType.ElseStatement), Convert.ToInt16(TokenType.ELSE)] = 33;
            //34
            LL1table[Convert.ToInt16(NonTerminalType.IterationStmt), Convert.ToInt16(TokenType.WHILE)] = 34;
            //35
            LL1table[Convert.ToInt16(NonTerminalType.ReturnStmt), Convert.ToInt16(TokenType.RETURN)] = 35;
            //36
            LL1table[Convert.ToInt16(NonTerminalType.Res), Convert.ToInt16(';')] = 36;
            //37
            LL1table[Convert.ToInt16(NonTerminalType.Res), Convert.ToInt16(TokenType.ID)] = 37;
            LL1table[Convert.ToInt16(NonTerminalType.Res), Convert.ToInt16(TokenType.NUMBER)] = 37;
            LL1table[Convert.ToInt16(NonTerminalType.Res), Convert.ToInt16('(')] = 37;
            //38预测两个
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16(TokenType.ID)] = 38;
            //39预测两个
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16(TokenType.ID)] = 39;
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16(TokenType.NUMBER)] = 39;
            LL1table[Convert.ToInt16(NonTerminalType.ExpressionStmt), Convert.ToInt16('(')] = 39;
            //40
            LL1table[Convert.ToInt16(NonTerminalType.Var), Convert.ToInt16(TokenType.ID)] = 40;
            //41
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('=')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(';')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('}')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(')')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(',')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(TokenType.LESSEQUAL)] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(TokenType.GREATERQUAL)] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(TokenType.EQUAL)] =41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16(TokenType.NOTEQUAL)] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('<')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('>')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('+')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('-')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('*')] = 41;
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('/')] = 41;
            //42
            LL1table[Convert.ToInt16(NonTerminalType.Array2), Convert.ToInt16('[')] = 42;
            
            //43
            LL1table[Convert.ToInt16(NonTerminalType.SimpleExpression), Convert.ToInt16('(')] = 43;
            LL1table[Convert.ToInt16(NonTerminalType.SimpleExpression), Convert.ToInt16(TokenType.ID)] = 43;
            LL1table[Convert.ToInt16(NonTerminalType.SimpleExpression), Convert.ToInt16(TokenType.NUMBER)] = 43;
            //44
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(';')] = 44;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16('}')] = 44;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(')')] = 44;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(',')] = 44;
            //45
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(TokenType.LESSEQUAL)] = 45;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(TokenType.GREATERQUAL)] = 45;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(TokenType.EQUAL)] = 45;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16(TokenType.NOTEQUAL)] = 45;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16('<')] = 45;
            LL1table[Convert.ToInt16(NonTerminalType.RelopExpression), Convert.ToInt16('>')] = 45;
            //46
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16(TokenType.LESSEQUAL)] = 46;
            //47
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression), Convert.ToInt16(TokenType.ID)] = 47;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression), Convert.ToInt16(TokenType.NUMBER)] = 47;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression), Convert.ToInt16('(')] = 47;
            //48
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(';')] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16('}')] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(')')] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(',')] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(TokenType.LESSEQUAL)] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(TokenType.GREATERQUAL)] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(TokenType.EQUAL)] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16(TokenType.NOTEQUAL)] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16('<')] = 48;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16('>')] = 48;
           //49
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16('+')] = 49;
            LL1table[Convert.ToInt16(NonTerminalType.AdditiveExpression2), Convert.ToInt16('-')] = 49;
            //50
            LL1table[Convert.ToInt16(NonTerminalType.Addop), Convert.ToInt16('+')] = 50;
            //51
            LL1table[Convert.ToInt16(NonTerminalType.Term), Convert.ToInt16(TokenType.ID)] = 51;
            LL1table[Convert.ToInt16(NonTerminalType.Term), Convert.ToInt16(TokenType.NUMBER)] = 51;
            LL1table[Convert.ToInt16(NonTerminalType.Term), Convert.ToInt16('(')] = 51;
            //52
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(';')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('}')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(')')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(',')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(TokenType.LESSEQUAL)] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(TokenType.GREATERQUAL)] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(TokenType.EQUAL)] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16(TokenType.NOTEQUAL)] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('<')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('>')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('+')] = 52;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('-')] = 52;
            //53
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('*')] = 53;
            LL1table[Convert.ToInt16(NonTerminalType.Term2), Convert.ToInt16('/')] = 53;
            //54
            LL1table[Convert.ToInt16(NonTerminalType.Mulop), Convert.ToInt16('*')] = 54;
            //55
            LL1table[Convert.ToInt16(NonTerminalType.Factor), Convert.ToInt16(TokenType.ID)] = 55;
            //56
            LL1table[Convert.ToInt16(NonTerminalType.Factor), Convert.ToInt16('(')] = 56;
            //57
            LL1table[Convert.ToInt16(NonTerminalType.Factor), Convert.ToInt16(TokenType.NUMBER)] = 57;
            //58
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('[')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(';')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('}')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(')')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(',')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(TokenType.LESSEQUAL)] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(TokenType.GREATERQUAL)] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(TokenType.EQUAL)] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16(TokenType.NOTEQUAL)] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('<')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('>')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('+')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('-')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('*')] = 58;
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('/')] = 58;
            //59
            LL1table[Convert.ToInt16(NonTerminalType.A2), Convert.ToInt16('(')] = 59;
            //60
            LL1table[Convert.ToInt16(NonTerminalType.Call), Convert.ToInt16(TokenType.ID)] = 60;
            //61
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16(TokenType.ID)] = 61;
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16(TokenType.NUMBER)] = 61;
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16('(')] = 61;
            //62
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16(')')] = 62;
            //63
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16(TokenType.ID)] = 63;
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16(TokenType.NUMBER)] = 63;
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16('(')] = 63;
            //64
            LL1table[Convert.ToInt16(NonTerminalType.ArgList2), Convert.ToInt16(')')] = 64;
            //65
            LL1table[Convert.ToInt16(NonTerminalType.Args), Convert.ToInt16(',')] = 65;
            //66
            LL1table[Convert.ToInt16(NonTerminalType.Addop), Convert.ToInt16('-')] = 66;
            //67
            LL1table[Convert.ToInt16(NonTerminalType.Mulop), Convert.ToInt16('/')] = 67;
            //68
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16(TokenType.GREATERQUAL)] = 68;
            //69
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16(TokenType.EQUAL)] = 69;
            //70
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16(TokenType.NOTEQUAL)] = 70;
            //71
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16('<')] = 71;
            //72
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16('>')] = 72;
            //73
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16(TokenType.ID)] = 73;
            //74
            LL1table[Convert.ToInt16(NonTerminalType.Relop), Convert.ToInt16(TokenType.NUMBER)] = 74;

        }

    }
}
