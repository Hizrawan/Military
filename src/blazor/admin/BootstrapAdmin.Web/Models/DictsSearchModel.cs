﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 字典維護自訂高級搜索模型
    /// </summary>
    public class DictsSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 獲得/設置 字典標籤
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 獲得/設置 字典名稱
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 獲得/設置 字典代碼
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 獲得/設置 字典類型
        /// </summary>
        public EnumDictDefine? Define { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFilterAction> GetSearches()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(Name))
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Name), Name));
            }

            if (!string.IsNullOrEmpty(Code))
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Code), Code));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Category), Category));
            }

            if (Define.HasValue)
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Define), Define.Value, FilterAction.Equal));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            Category = null;
            Name = null;
            Define = null;
        }
    }
}
