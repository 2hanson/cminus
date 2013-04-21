using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Analyzer
{
    public class SymbolTable
    {
        public BucketList[] hashTable = new BucketList[211];

        /// <summary>
        /// get hash number
        /// </summary>
        public int Hash(string key)
        {
	        int temp = 0;
	        int i = 0;
	        int	size = key.Length;

	        while (i < size)	{
		        temp = ((temp <<4) + key[i]) % 211;
		        ++i;
	        }

	        return temp;
        }

        /// <summary>
        /// main important interface
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public TokenType SearchType( string name, string scope)
        {
	        int h = Hash(name);
	        BucketList l = hashTable[h];

	        while ( (l!=null) && (l.Name == name) && (l.Scope == name)) l = l.next;

            if (l == null)
                return TokenType.ERROR;
            else
	            return l.Type;
        }

        /// <summary>
        /// important interface
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <param name="type"></param>
        /// <param name="lineno"></param>
        /// <param name="memloc"></param>
        /// <param name="isArr"></param>
        public void Insert( string name,  string scope,  TokenType type, int lineno,  long memloc,  bool isArr)
        {
	        int h = Hash(name);
	        BucketList l = hashTable[h];

	        while (l!=null && ((l.Name != name) || (l.Scope != scope))) l = l.next;
	        if (l == null)	// not found in table,
	        {
		        l = new BucketList();
		        l.Name = name;
		        l.Scope = scope;
		        l.Type = type;
		        l.memloc = (int)memloc;
		        l.isArr = isArr;
		        l.Lines = new LineList();
		        l.Lines.lineno = lineno;

		        l.next = hashTable[h];	
		        hashTable[h] = l;		// dot worry,
	        }
	        else
	        {	// if found in table, just add lineno,
		        LineList t = l.Lines;
        		
		        while (t.next != null)	t = t.next;

		        t.next = new LineList();
		        t.next.lineno = lineno;
	        }
        }

        /// <summary>
        /// return the varible memory location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public long LookUp( string name, string scope)
        {
	        int h = Hash(name);
	        BucketList l = hashTable[h];

	        while (l!=null &&  ! (l.Name == name && l.Scope == scope)) l = l.next;

	        return (l == null) ? -1 : l.memloc ;
        }
        
        /// <summary>
        /// return the varible  is or not is a array ?
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public bool SearchArray(string name,  string scope)
        {
	        int h = Hash(name);
	        BucketList l = hashTable[h];

	        while (l!=null && (l.Name != name || l.Scope != scope))	l=l.next;

	        return (l == null) ? false : l.isArr;
        }

    }
}
