using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Threading;
using UnityEngine;

namespace DspILSAnalyzer
{

    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("DSPGAME.exe")]
    public class DspILSAnalyzer : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "de.sveri.dspilsanalyzer";
        public const string PLUGIN_NAME = "DspILSAnalyzer";
        public const string PLUGIN_VERSION = "0.1.0";
        private Harmony harmony;


        private Timer analyzeTimer;

        private void Awake()
        {

            analyzeTimer = new System.Threading.Timer(AnalyzeCallback, null, 0, 5000);

            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");



        }

        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(UniverseSimulator), "OnGameLoaded")]
        // public static void UniverseSimulator_OnGameLoaded(ref UniverseSimulator __instance)
        // {

        //     Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        //     // analyzeTimer = new System.Threading.Timer(AnalyzeCallback, null, 0, 5000);

        // }

        private void AnalyzeCallback(object state)
        {

            // Logger.LogInfo($"GameState universeSimulator is Running? {GameMain.universeSimulator}");
            // Logger.LogInfo($"GameState universeSimulator galaxyData is Running? {GameMain.universeSimulator.galaxyData}");
            // Logger.LogInfo($"GameState universeSimulator  stars is Running? {GameMain.universeSimulator.galaxyData.stars}");

            if (GameMain.universeSimulator.galaxyData.stars != null)
            {
                foreach (StarData star in GameMain.universeSimulator.galaxyData.stars)
                {
                    foreach (PlanetData planet in star.planets)
                    {
                        Logger.LogInfo($"Found planet with name: {planet.name}");
                    }
                }

            }



            // Do your work...
        }

        private void OnDestroy()
        {
            if (analyzeTimer != null)
            {
                analyzeTimer.Dispose();

            }

            harmony.UnpatchSelf();
        }
    }
}