// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using Longbow.Tasks;
using System.Collections.Concurrent;

namespace BootstrapAdmin.Web.Jobs
{
    /// <summary>
    /// 資料庫腳本執行日誌任務實體類
    /// </summary>
    public class DBLogTask : ITask
    {
        private static readonly BlockingCollection<DBLog> _messageQueue = new(new ConcurrentQueue<DBLog>());

        /// <summary>
        /// 添加資料庫日誌實體類到內部集合中
        /// </summary>
        /// <param name="log"></param>
        public static System.Threading.Tasks.Task AddDBLog(DBLog log) => System.Threading.Tasks.Task.Run(() =>
        {
            if (!_messageQueue.IsAddingCompleted && !_pause)
            {
                _messageQueue.Add(log);
            }
        });

        private static bool _pause;
        /// <summary>
        /// 暫停接收腳本執行日誌
        /// </summary>
        public static void Pause() => _pause = true;

        /// <summary>
        /// 開始接收腳本執行日誌
        /// </summary>
        public static void Run() => _pause = false;

        /// <summary>
        /// 任務執行方法
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task Execute(CancellationToken cancellationToken)
        {
            var logs = new List<DBLog>();
            while (_messageQueue.TryTake(out var log))
            {
                if (log != null) logs.Add(log);
                if (logs.Count >= 100) break;
            }
            if (logs.Any())
            {
                //using var db = DbManager.Create(enableLog: false);
                //db.InsertBatch(logs);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
