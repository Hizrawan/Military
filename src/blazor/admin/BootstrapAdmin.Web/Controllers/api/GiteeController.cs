﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BootstrapAdmin.Web.Controllers.Api
{
    /// <summary>
    /// Gitee 網站信息介面類別
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class GiteeController : ControllerBase
    {
        /// <summary>
        /// 獲取 Gitee 網站 Issues 資訊
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Issues([FromServices] GiteeHttpClient client, [FromQuery] string? userName = "LongbowEnterprise", [FromQuery] string? repoName = "BootstrapAdmin", [FromQuery] string? label = "custom badge", [FromQuery] string? color = "orange")
        {
            var content = await client.HttpClient.GetStringAsync($"https://gitee.com/{userName}/{repoName}/issues");
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "progressing", "closed", "rejected" };
            var result = string.IsNullOrEmpty(content) ? new string[] { "unknown" } : regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            var msg = string.Join(" ", result);
            color = msg.StartsWith("open 0 progressing 0", StringComparison.OrdinalIgnoreCase) ? "success" : color;
            return new JsonResult(new { schemaVersion = 1, label, message = msg, color });
        }

        /// <summary>
        /// 獲取 Gitee 網站 Pulls 資訊
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Pulls([FromServices] GiteeHttpClient client, [FromQuery] string? userName = "LongbowEnterprise", [FromQuery] string? repoName = "BootstrapAdmin", [FromQuery] string? label = "custom badge", [FromQuery] string? color = "orange")
        {
            var content = await client.HttpClient.GetStringAsync($"https://gitee.com/{userName}/{repoName}/pulls") ?? "";
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "merged", "closed" };
            var result = string.IsNullOrEmpty(content) ? new string[] { "unknown" } : regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            var msg = string.Join(" ", result);
            return new JsonResult(new { schemaVersion = 1, label, message = msg, color });
        }

        /// <summary>
        /// 獲取 Gitee 網站 Releases 資訊
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Releases([FromServices] GiteeHttpClient client, [FromQuery] string? userName = "LongbowEnterprise", [FromQuery] string? repoName = "BootstrapAdmin", [FromQuery] string? label = "custom badge", [FromQuery] string? color = "orange")
        {
            var content = await client.HttpClient.GetStringAsync($"https://gitee.com/{userName}/{repoName}/releases") ?? "";
            var regex = Regex.Match(content, $"<a href=\"/{userName}/{repoName}/releases/([^\\s]+)\" target=\"_blank\">", RegexOptions.IgnoreCase);
            var msg = regex?.Groups[1].Value ?? "unknown";
            return new JsonResult(new { schemaVersion = 1, label, message = msg, color });
        }

        /// <summary>
        /// 獲取 Gitee 網站 Builds 資訊
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <param name="projName"></param>
        /// <param name="branchName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Builds([FromServices] GiteeHttpClient client, [FromQuery] string? userName = "ArgoZhang", [FromQuery] string? projName = "bootstrapadmin", [FromQuery] string? branchName = "master", [FromQuery] string? label = "custom badge", [FromQuery] string? color = "orange")
        {
            var content = await client.HttpClient.GetFromJsonAsync<AppveyorBuildResult>($"https://ci.appveyor.com/api/projects/{userName}/{projName}/branch/{branchName}", new CancellationTokenSource(10000).Token);
            return new JsonResult(new { schemaVersion = 1, label, message = content?.Build.Version ?? "unknown", color });
        }

        private class AppveyorBuildResult
        {
            /// <summary>
            /// Appveyor 編譯版本實例
            /// </summary>
            public Build Build { get; set; } = new Build();
        }

        private class Build
        {
            /// <summary>
            /// Build 版本資訊
            /// </summary>
            public string Version { get; set; } = "";
        }
    }
}
