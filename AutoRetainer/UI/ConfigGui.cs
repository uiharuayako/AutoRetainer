﻿using AutoRetainer.Multi;
using AutoRetainer.Statistics;
using ECommons.Configuration;
using PunishLib.ImGuiMethods;

namespace AutoRetainer.UI;

unsafe internal class ConfigGui : Window
{
    public ConfigGui() : base($"{P.Name} configuration")
    {
        this.SizeConstraints = new()
        {
            MinimumSize = new(250, 100),
            MaximumSize = new(9999,9999)
        };
        P.ws.AddWindow(this);
    }

    public override void PreDraw()
    {
        P.Style.Push();
    }

    public override void Draw()
    {
        var en = P.IsEnabled();
        if (ImGui.Checkbox($"Enable {P.Name}", ref en))
        {
            if (en)
            {
                P.EnablePlugin();
            }
            else
            {
                P.DisablePlugin();
            }
        }
        ImGui.SameLine();
        ImGui.Checkbox("Auto Enable", ref P.config.AutoEnableDisable);

        ImGui.SameLine();
        ImGui.Checkbox("Multi", ref MultiMode.Enabled);

        if (Scheduler.turbo)
        {
            ImGui.SameLine();
            ImGuiEx.Text(Environment.TickCount % 1000 > 500 ? ImGuiColors.DalamudRed : ImGuiColors.DalamudYellow, "Turbo active");
        }
        ImGuiEx.EzTabBar("tabbar",

                ("Retainers", MultiModeUI.Draw, null, true),
                (P.config.RecordStats ? "Statistics" : null, StatisticsUI.Draw, null, true),
                ("Settings", Settings.Draw, null, true),
                ("Beta", TabBeta.Draw, null, true),
                ("About", delegate { AboutTab.Draw(P); }, null, true),
                (P.config.Verbose ? "Log" : null, InternalLog.PrintImgui, null, false),
                (P.config.Verbose?"Retainers (old)":null, Retainers.Draw, null, true),
                (P.config.Verbose?"Debug":null, Debug.Draw, null, true)
                );
    }

    public override void PostDraw()
    {
        P.Style.Pop();
    }

    public override void OnClose()
    {
        EzConfig.Save();
        if (P.config.DisableOnClose) 
        {
            P.DisablePlugin();
            StatisticsUI.Data.Clear();
            Notify.Success("Auto Retainer disabled");
        }
    }

}
