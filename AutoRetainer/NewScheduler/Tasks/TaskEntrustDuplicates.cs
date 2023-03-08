﻿using AutoRetainer.NewScheduler.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRetainer.NewScheduler.Tasks
{
    internal static class TaskEntrustDuplicates
    {
        internal static bool NoDuplicates = false;
        internal static void Enqueue()
        {
            NoDuplicates = false;
            P.TaskManager.Enqueue(RetainerHandlers.SelectEntrustItems);
            P.TaskManager.Enqueue(RetainerHandlers.ClickEntrustDuplicates);
            P.TaskManager.Enqueue(() => { if (NoDuplicates) return true; return RetainerHandlers.ClickEntrustDuplicatesConfirm(); }, 600 * 60, false);
            P.TaskManager.Enqueue(() => { if (NoDuplicates) return true; return RetainerHandlers.ClickCloseEntrustWindow(); }, false);
            P.TaskManager.Enqueue(RetainerHandlers.CloseRetainerInventory);
        }
    }
}