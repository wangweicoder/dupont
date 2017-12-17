using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using DuPont.Extensions;

namespace DuPont.Utility
{
    public class IOHelper
    {
        #region Directory
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>        
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                return Directory.GetFiles(directoryPath);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取指定目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                return Directory.GetDirectories(directoryPath);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void ClearDirectory(string directoryPath)
        {
            Directory.Delete(directoryPath, true);

            CreateDirectory(directoryPath);

        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <param name="isCover">如果目标文件夹已存在,则覆盖</param>
        public static void MoveDirectory(string fromPath, string toPath, bool isCover)
        {
            if (IsExistDirectory(fromPath))
            {
                if (isCover)
                {
                    DeleteDirectory(toPath);
                }
                Directory.Move(fromPath, toPath);
            }

        }
        #endregion

        #region File
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 创建文件,如果目标文件已存在,则覆盖
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateFile(string filePath)
        {
            WriteFile(filePath, "");
        }
        /// <summary>
        /// 清空文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void ClearFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                CreateFile(filePath);
            }
        }
        /// <summary>
        /// 写入内容到新文件,如果文件存在则覆盖
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
        /// <summary>
        /// 追加内容到文件,文件不存在,则创建新文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        /// <summary>
        /// 复制文件,支持覆盖已有文件
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <param name="isCover">是否覆盖已有文件</param>
        public static void CopyFile(string fromPath, string toPath, bool isCover = true)
        {
            if (IsExistFile(fromPath))
            {
                File.Copy(fromPath, toPath, isCover);
            }

        }
        /// <summary>
        /// 移动文件,支持覆盖已有文件
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <param name="isCover">目标文件存在,则覆盖</param>
        public static void MoveFile(string fromPath, string toPath, bool isCover = true)
        {
            if (IsExistFile(fromPath))
            {
                if (isCover)
                {
                    DeleteFile(toPath);
                }
                File.Move(fromPath, toPath);
            }

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }

        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string OpenFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return "";
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[] ReadFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                return File.ReadAllLines(filePath);
            }
            return new string[] { };
        }
        #endregion

        #region Log
        /// <summary>
        /// 记录日志到本地,一天一个文件夹,一小时一个日志文件
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLogToFile(string log, string dirPath = "")
        {
            string directoryPath = dirPath.DefaultIfEmpty(Directory.GetCurrentDirectory() + @"Log\" + DateTime.Now.ToString("yyyy-MM-dd"));
            if (!IOHelper.IsExistDirectory(directoryPath))
            {
                IOHelper.CreateDirectory(directoryPath);
            }
            IOHelper.AppendText(directoryPath + @"\" + DateTime.Now.ToString("yyyy-MM-dd HH-00-00") + ".txt", log);
        }
        #endregion
    }
}
