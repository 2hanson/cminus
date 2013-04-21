using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Parse
{
    /// <summary>
    /// 进行LL1语法分析用到的类型及对应的变量. 所有非终极符，其各自含义可参考LL1文法
    /// </summary>
    public enum NonTerminalType
    {
        Program,
        DeclarationList,
        Declaration,
        DeclarationList2,
        VarDeclaration,
        FunDeclaration,
        TypeSpecifier,
        Array,
        CompoundStmt,
        Params,
        ParamList,
        Param,
        ParamList2,
        LocalDeclarations,
        StatementList,
        Statement,
        ExpressionStmt,
        SelectionStmt,
        Relop,
        ReturnStmt,
        Res,
        IterationStmt,
        Expression,
        ElseStatement,
        SimpleExpression,
        AdditiveExpression,
        Var,
        RelopExpression,
        Term,
        AdditiveExpression2,
        Addop,
        Term2,
        Mulop,
        Factor,
        A2,
        Array2,
        Call,
        Args,
        ArgList,
        ArgList2,
        ID2,
        NUM2
    }
}
