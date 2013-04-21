using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    /// <summary>
    /// DFA状态
    /// </summary>
    public enum StateType
    {
        START = 1,
        ID = 2,
        NUMBER = 3,
        COMMENT1 = 4,
        COMMENT2 = 5,
        COMMENT3 = 6,
        ASSIGN = 7,
        GREATERORLESS = 8,
        NOT = 9,
        DONE = 10,
        NIUBISTATE = 11
    }
}
