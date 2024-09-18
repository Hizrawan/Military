// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Utils;

static class LookupHelper
{
    public static List<SelectedItem> GetTargets() => new()
    {
        new SelectedItem("_self", "本窗口"),
        new SelectedItem("_blank", "新窗口"),
        new SelectedItem("_parent", "父級窗口"),
        new SelectedItem("_top", "頂級窗口"),
    };

    public static List<SelectedItem> GetExceptionCategory() => new()
    {
        new SelectedItem("App", "應用程式"),
        new SelectedItem("DB", "資料庫")
    };

    public static List<SelectedItem> GetCheckItems() => new()
    {
        new("db", "資料庫"),
        new("environment", "環境變數"),
        new("dotnet-runtime", "運行時"),
        new("file", "檔案系統"),
        new("gc", "回收器"),
        new("app-mem", "程式記憶體"),
        new("sys-mem", "系統記憶體"),
        new("Gitee", "Gitee")
    };
}
