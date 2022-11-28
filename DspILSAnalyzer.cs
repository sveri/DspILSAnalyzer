using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;

namespace CreativeStuff
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("DSPGAME.exe")]
    public class CreativeStuff : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "de.sveri.dspilsanalyzer";
        public const string PLUGIN_NAME = "DspILSAnalyzer";
        public const string PLUGIN_VERSION = "0.1.0";
        private Harmony harmony;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }
        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}