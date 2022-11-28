using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Text.Json;

namespace DspILSAnalyzer
{

    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("DSPGAME.exe")]
    public class DspILSAnalyzer : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "de.sveri.dspilsanalyzer";
        public const string PLUGIN_NAME = "DspILSAnalyzer";
        public const string PLUGIN_VERSION = "0.1.0";
        // private Harmony harmony;


        private Timer analyzeTimer;

        private String jsonFilePath;

        private void Awake()
        {

            jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dsp_ILS_analyzer.json");

            analyzeTimer = new System.Threading.Timer(AnalyzeCallback, null, 0, 5000);

            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");



        }

        private void AnalyzeCallback(object state)
        {
            Logger.LogDebug("Timer triggered");

            var ilsInfo = new IlsInfo();

            File.WriteAllText(jsonFilePath, string.Empty);

            if (GameMain.universeSimulator.galaxyData.stars != null)
            {
                foreach (StarData star in GameMain.universeSimulator.galaxyData.stars)
                {
                    var planets = new List<Planet>();
                    foreach (PlanetData planet in star.planets)
                    {
                        Logger.LogInfo($"in loop: {planet.name}");
                        var jsonPlanet = new Planet();
                        jsonPlanet.name = planet.name;
                        planets.Add(jsonPlanet);

                        foreach (StationComponent station in planet.factory.transport.stationPool)
                        {
                            if (station != null && station.isStellar)
                            {
                                using (StreamWriter sw = File.AppendText(jsonFilePath))
                                {
                                    sw.WriteLine(planet.id);

                                    foreach (StationStore storageItem in station.storage)
                                    {
                                        sw.WriteLine(storageItem.itemId);
                                    }

                                    sw.WriteLine("-----------\n");
                                }

                            }
                        }
                    }

                    ilsInfo.planets = planets;
                }

            }

            using (StreamWriter sw = File.AppendText(jsonFilePath))
            {
                sw.WriteLine(JsonSerializer.Serialize(ilsInfo));
            }

        }

        private void OnDestroy()
        {
            if (analyzeTimer != null)
            {
                analyzeTimer.Dispose();

            }



            // harmony.UnpatchSelf();
        }
    }
}