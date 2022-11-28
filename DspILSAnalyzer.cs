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

        private String jsonFilePath;

        private void Awake()
        {

            jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dsp_ILS_analyzer.json");

            analyzeTimer = new System.Threading.Timer(AnalyzeCallback, null, 0, 5000);

            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");



        }

        private void AnalyzeCallback(object state)
        {
            Logger.LogInfo("Timer triggered");
            // Logger.LogInfo($"{GameMain.data.galacticTransport.stationPool[0]}");
            // Logger.LogInfo($"{GameMain.data.}");
            // if( GameMain.data.galacticTransport.stationPool != null) {
            // foreach (StationComponent component in  GameMain.data.galacticTransport.stationPool) {
            //     Logger.LogInfo($"GameState universeSimulator is Running? {component.deliveryShips}");

            // }
            // }

            // Logger.LogInfo($"GameState universeSimulator is Running? {GameMain.universeSimulator}");
            // Logger.LogInfo($"GameState universeSimulator galaxyData is Running? {GameMain.universeSimulator.galaxyData}");
            // Logger.LogInfo($"GameState universeSimulator  stars is Running? {GameMain.universeSimulator.galaxyData.stars}");

            // GameMain.data.galacticTransport.stationPool


            //  Logger.LogInfo($"uiGame?: {UIRoot.instance.uiGame.planetDetail.planet}");

            // Logger.LogInfo($"{GameMain.data.factories[0].transport.stationPool[0]}");



            // works
            // foreach (StationComponent station in GameMain.data.galacticTransport.stationPool) {
            //     if(station != null) {
            //         Logger.LogInfo($"name: {station.storage[0].itemId}");
            //         Logger.LogInfo($"aaaaaaaaa: {station.isStellar}");
            //         Logger.LogInfo($"aaaaaaaaa: {station.deliveryShips}");

            //     }
            // }

            // File.Delete(jsonFilePath);

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

                                    // Logger.LogInfo($"id: {station.id}");
                                    // Logger.LogInfo($"stoarageItemId: {station.storage[0].itemId}");
                                    // Logger.LogInfo($"aaaaaaaaa: {station.isStellar}");
                                    // Logger.LogInfo($"aaaaaaaaa: {station.deliveryShips}");

                                    sw.WriteLine("-----------\n");
                                }

                            }
                        }

                        // var planetFactoryTransport = planet.factory.transport;
                        // var stationCursor = planetFactoryTransport.stationCursor;
                        // Logger.LogInfo($"stationCount: {planetFactoryTransport.stationCount}");

                        // Logger.LogInfo($"stationCursor: {stationCursor}");


                        // Logger.LogInfo($"stationPool: {planetFactoryTransport.stationPool[1].name}");
                        // Logger.LogInfo($"storage: {planet.factory.factoryStorage}");





                        // foreach (PlanetTransport transport in planetFactoryTransport.stationCount) {
                        //     Logger.LogInfo($"trnsport: {transport}");

                        // }

                        // for ( int i = 0; i< stationCursor; i++)  {
                        //     Logger.LogInfo($"Found planet in loop");
                        //     Logger.LogInfo($"Found planet with name: {planetFactoryTransport.stationPool}");

                        // }

                        // UIRoot.instance.uiGame.planetDetail
                    }
                    
                    ilsInfo.planets = planets;
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

            // harmony.UnpatchSelf();
        }
    }
}