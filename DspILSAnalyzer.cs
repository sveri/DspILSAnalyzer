using BepInEx;
using System;
using System.IO;
using System.Threading;

namespace DspILSAnalyzer
{

    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("DSPGAME.exe")]
    public class DspILSAnalyzer : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "de.sveri.dspilsanalyzer";
        public const string PLUGIN_NAME = "DspILSAnalyzer";
        public const string PLUGIN_VERSION = "0.1.0";


        private Timer analyzeTimer;

        private String filePath;

        private void Awake()
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dsp_ILS_analyzer.txt");
            analyzeTimer = new System.Threading.Timer(AnalyzeCallback, null, 0, 10000);
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }

        private void AnalyzeCallback(object state)
        {
            Logger.LogDebug("Timer triggered");

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

                        if (planet.factory != null && planet.factory.transport != null) {



                            foreach (StationComponent station in planet.factory.transport.stationPool)
                            {
                                if (station != null && station.isStellar)
                                {
                                    using (StreamWriter sw = File.AppendText(filePath))
                                    {
                                        var stationName = station.id.ToString();

                                        if (station.name != null)
                                        {
                                            stationName = station.name;
                                        }

                                        int latd = 0, latf = 0, logd = 0, logf = 0;
                                        bool north, south , west, east;
                                        Maths.GetLatitudeLongitude(planet.factory.entityPool[station.entityId].pos, out latd, out latf, out logd, out logf, out north, out south, out west, out east);
                                        sw.WriteLine("Station name or ID: {0} - pos: {1}N {2}E", stationName, latd, logd);


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
        }
    }
}