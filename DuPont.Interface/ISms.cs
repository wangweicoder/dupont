// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 08-21-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-21-2015
// ***********************************************************************
// <copyright file="ISms.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>发送短信接口</summary>
// ***********************************************************************
namespace DuPont.Interface
{
    public interface ISms
    {
        bool Send(string phoneNumber, string validateCode);
    }
}
