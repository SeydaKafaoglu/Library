using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text; //byte a çevirmek için gerekli Cryptography byte ister. 

namespace Library
{
    public static class Utility
    {
        public static string CalculateSHA(string userName, string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(userName + password);
            byte[] hashedData;
            string hashedStr;
            SHA256 sHA256 = new SHA256Managed();

            hashedData = sHA256.ComputeHash(data); //dizi döndürür
            hashedStr = BitConverter.ToString(hashedData).Replace("-", ""); //döndürülen byte dizisini stringe çevirme. hashedstr değişkenine yolladık //aralarındaki - leri yok et boşlukları kapat
            return hashedStr;       
        }
    }
}