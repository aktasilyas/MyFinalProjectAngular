
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        //params ile istedigimiz kadar parametre gonderebiliriz.
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    //bir hata var sa o hatayi dondurur
                    return logic;
                }
            }
            return null;
        }
    }
}
