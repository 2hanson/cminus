using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    public class CMinusFile
    {

        public CMinusFile()
        {
 
        }

        public ArrayList ReadFile(string fileName)
        {
            StreamReader objReader = new StreamReader(fileName);
            string sLine = "";
            ArrayList arrText = new ArrayList();

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }

            objReader.Close();

            return arrText;
        }

    }
}
