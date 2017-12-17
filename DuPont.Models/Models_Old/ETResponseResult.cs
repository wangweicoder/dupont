// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-03-2015
// ***********************************************************************
// <copyright file="ResponseResult.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Extensions;
using System.Collections;
namespace DuPont.Models.Models
{
    public class ETResponseResult<T> : IDisposable where T : class, new()
    {
        private const string emptyResult = "没有找到符合条件的数据!";
        public bool IsSuccess { get; set; }
        public string Message { get; set; }        
        public ETStateInfo State { get; set; }
        public T Entity { get; set; }


        string[] _result = { "" };

        public ETResponseResult()
        {
            IsSuccess = true;
            Message = string.Empty;           
            Entity = new T();
            State = new ETStateInfo
            {
                Id = (int)ResponseStatusCode.Success,
                Description = ResponseStatusCode.Success.GetDescription()
            };
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || !this.IsSuccess) return;

            if (this.Entity is string)
            {
                if (string.IsNullOrEmpty(this.Entity as string))
                {
                    this.Message = emptyResult;
                }
            }
            else if (this.Entity is IEnumerable)
            {
                if (this.Entity == null || !((IEnumerable)this.Entity).GetEnumerator().MoveNext())
                {
                    this.Message = emptyResult;
                }
            }            
            //Clear Managed Resource

            // Clear Unmanageed Resource
        }

        ~ETResponseResult()
        {
            Dispose();
        }
    }
}

