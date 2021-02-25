﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTA.Finance.Model;

namespace WTA.AuthenticationCenter.Utility
{
    /// <summary>
    /// 简单封装个注入
    /// </summary>
    public interface IJWTService
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string GetToken(CurrentUserModel userInfo);
    }
}
