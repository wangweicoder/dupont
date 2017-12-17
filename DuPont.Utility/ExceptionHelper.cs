// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 12-23-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-23-2015
// ***********************************************************************
// <copyright file="ExceptionHelper.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility
{
    public class ExceptionHelper
    {
        public static string Build(Exception exception)
        {

            StringBuilder sbMessage = new StringBuilder();
            Exception innerException = exception.InnerException;
            if (innerException == null)
            {
                sbMessage.AppendLine(GetErrorMessage(exception));
            }
            while (innerException != null)
            {
                sbMessage.AppendLine(GetErrorMessage(innerException));
                // sbMessage.AppendLine();

                innerException = innerException.InnerException;
            }

            return sbMessage.ToString();
        }

        private static string GetErrorMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
