using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
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
        // private Harmony harmony;


        private Timer analyzeTimer;

        private String filePath;

        private void Awake()
        {

            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dsp_ILS_analyzer.txt");

            analyzeTimer = new System.Threading.Timer(AnalyzeCallback, null, 0, 5000);

            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");



        }

        private void AnalyzeCallback(object state)
        {
            Logger.LogDebug("Timer triggered");

            // var ilsInfo = new IlsInfo();

            File.WriteAllText(filePath, string.Empty);

            if (GameMain.universeSimulator.galaxyData.stars != null)
            {
                foreach (StarData star in GameMain.universeSimulator.galaxyData.stars)
                {
                    foreach (PlanetData planet in star.planets)
                    {
                        using (StreamWriter sw = File.AppendText(filePath))
                        {

                            sw.WriteLine("Planetname: {0}", planet.name);
                        }

                        foreach (StationComponent station in planet.factory.transport.stationPool)
                        {
                            if (station != null && station.isStellar)
                            {
                                using (StreamWriter sw = File.AppendText(filePath))
                                {
                                    var stationName = station.id.ToString();

                                    if(station.name != null){
                                        stationName = station.name;
                                    }

                                    sw.WriteLine("Station name or ID: {0}", stationName);

                                    foreach (StationStore storageItem in station.storage)
                                    {
                                        var itemLdb = LDB.items.Select(storageItem.itemId);
                                        if (itemLdb != null)
                                        {
                                            sw.WriteLine("\t{0}: {1} pcs, logic: {2}", itemLdb.name, storageItem.count, storageItem.remoteLogic);

                                        }
                                    }
                                }

                            }
                        }

                        using (StreamWriter sw = File.AppendText(filePath))
                        {
                            sw.WriteLine("-----------\n");
                        }
                    }

                }

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