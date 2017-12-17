﻿// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 08-05-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="Encrypt.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DuPont.Utility
{
    public class Encrypt
    {
        public Encrypt()
        { }

        #region MD5加密
        /// <summary>
        ///  MD5加密
        /// </summary>
        public static string MD5Encrypt(string Text)
        {
            return MD5Encrypt(Text, "dupontapp");
        }

        public static string MD5EncryptWithoutKey(string Text)
        {
            var md5 = new MD5CryptoServiceProvider();
            var byteArray = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Text));
            var result = new StringBuilder();
            foreach (byte b in byteArray)
            {
                result.AppendFormat("{0:X2}", b);
            }
            return result.ToString();
        }
            

        /// <summary> 
        ///  MD5加密 
        /// </summary> 
        public static string MD5Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(MD5EncryptWithoutKey(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(MD5EncryptWithoutKey(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        #endregion

        #region MD5解密
        /// <summary>
        ///  MD5解密
        /// </summary>
        public static string MD5Decrypt(string Text)
        {
            return MD5Decrypt(Text, "dupontapp");
        }

        /// <summary> 
        ///  MD5解密
        /// </summary>  
        public static string MD5Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(MD5EncryptWithoutKey(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(MD5EncryptWithoutKey(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region TripleDES加密
        /// <summary>
        /// TripleDES加密
        /// </summary>
        public static string TripleDESEncrypting(string strSource)
        {
            try
            {
                byte[] bytIn = Encoding.Default.GetBytes(strSource);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };  //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateEncryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                byte[] bytOut = ms.ToArray();
                return System.Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
            }
        }
        #endregion

        #region TripleDES解密
        /// <summary>
        /// TripleDES解密
        /// </summary>
        public static string TripleDESDecrypting(string Source)
        {
            try
            {
                byte[] bytIn = System.Convert.FromBase64String(Source);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };   //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateDecryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader strd = new StreamReader(cs, Encoding.Default);
                return strd.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// use sha256 to encrypt string
        /// </summary>
        public string SHA256_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            //HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            HashAlgorithm iSHA = new SHA256CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
    }
}
