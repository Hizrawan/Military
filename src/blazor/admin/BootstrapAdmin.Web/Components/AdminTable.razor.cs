// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security.Blazor;
using BootstrapAdmin.Web.Services;
using BootstrapBlazor.Components;
using System.Drawing;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingTypeParameter(nameof(TItem))]
    public partial class AdminTable<TItem> where TItem : class, new()
    {

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<TItem>? DetailRowTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>


        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IEnumerable<int>? PageItemsSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int ExtendButtonColumnWidth { get; set; } = 130;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? SortString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? SearchModalTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? EditModalTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? AddModalTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? EditButtonText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<TItem>? TableColumns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? RowButtonTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<IEnumerable<TItem>>? FooterTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>

        [Parameter]
        public EditMode EditModes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<ITableSearchModel>? CustomerSearchTemplate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? EditTemplate { get; set; }
        /// <summary>
        /// 获得/设置 导出按钮下拉菜单模板 默认 null
        /// </summary>
        [Parameter]
        public RenderFragment<ITableExportContext<TItem>>? ExportButtonDropdownTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment? TableToolbarTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<TItem>? EditFooterTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsPagination { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowExportButton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowLineNo { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowColumnList { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsMultipleSelect { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsFixedHeader { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsTree { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowToolbar { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowEmpty { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowLoading { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowSearch { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowAdvancedSearch { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowDefaultButtons { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowExtendButtons { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowAddButton { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowDeleteButton { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowCardView { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowEditButton { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool AutoGenerateColumns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowToastAfterSaveOrDeleteModel { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ClickToSelect { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowExtendEditButton { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowExtendDeleteButton { get; set; } = true;


        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ITableSearchModel? CustomerSearchModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<QueryPageOptions, Task<QueryData<TItem>>>? OnQueryAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, Task<IEnumerable<TableTreeNode<TItem>>>>? OnTreeExpand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, Task<IEnumerable<TableTreeNode<TItem>>>>? TreeNodeConverter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, ItemChangedType, Task<bool>>? OnSaveAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, Task>? OnAfterSaveAsync { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowFooter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<ITableExportDataContext<TItem>, Task<bool>>? OnExportAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, Task<bool>>? OnDeleteAsync { get; set; }

        /// <summary>
        /// 增加開啟, 如：點擊記錄行時，彈出明細框
        /// </summary>
        [Parameter]
        public Func<TItem, Task>? OnClickRowCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public List<TItem>? SelectedRows { get; set; } = new List<TItem>();
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem>> SelectedRowsChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, bool>? ShowEditButtonCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, bool>? ShowDeleteButtonCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<Task>? OnAfterModifyAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, TItem, bool>? ModelEqualityComparer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ScrollMode ScrollMode { get; set; }



        [NotNull]
        private Table<TItem>? Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask ToggleLoading(bool v) => Instance.ToggleLoading(v);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task QueryAsync() => Instance.QueryAsync();
        [Inject]
        [NotNull]
        private IBootstrapAdminService? AdminService { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        private bool AuthorizeButton(string operate)
        {
            var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            return AdminService.AuthorizingBlock(AppContext.UserName, url, operate);
        }
    }
}

