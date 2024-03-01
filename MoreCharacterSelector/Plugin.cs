using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using ModelReplacement;
using BepInEx.Configuration;
using System;
using System.Xml.Linq;

namespace MoreCharacterSelector
{
    [BepInPlugin("com.Humyo.MoreCharacterSelector", "MoreCharacterSelector", "1.1.0")]
    [BepInDependency("meow.ModelReplacementAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigFile config;

        // Example Config for single model mod
        //public static ConfigEntry<bool> enableModelForAllSuitsAkula { get; private set; }
        //public static ConfigEntry<bool> enableModelAsDefaultAkula { get; private set; }
        //public static ConfigEntry<bool> enableModelForAllSuitsPepe { get; private set; }
        //public static ConfigEntry<bool> enableModelAsDefaultPepe { get; private set; }
        public static ConfigEntry<string> suitNamesToEnableModelAkula { get; private set; }
        public static ConfigEntry<string> suitNamesToEnableModelPepe { get; private set; }

        private static void InitConfig()
        {
            //enableModelForAllSuitsAkula = config.Bind<bool>("Suits to Replace Settings", "Enable Model for all Suits (Akula)", false, "Enable to model replace every suit. Set to false to specify suits");
            //enableModelAsDefaultAkula = config.Bind<bool>("Suits to Replace Settings", "Enable Model as default (Akula)", false, "Enable to model replace every suit that hasn't been otherwise registered.");
            //enableModelForAllSuitsPepe = config.Bind<bool>("Suits to Replace Settings", "Enable Model for all Suits (Pepe)", false, "Enable to model replace every suit. Set to false to specify suits");
            //enableModelAsDefaultPepe = config.Bind<bool>("Suits to Replace Settings", "Enable Model as default (Pepe)", false, "Enable to model replace every suit that hasn't been otherwise registered.");
            suitNamesToEnableModelAkula = config.Bind<string>("Suits to Replace Settings (Akula)", "Suits to enable Model for(적용할 슈트 이름)", "Akula", "Enter a comma separated list of suit names.(Additionally, [Default,Orange suit,Green suit,Pajama suit,Hazard suit])(한국어패치기준, [Default,주황색슈트,초록색슈트,파자마슈트,방호복슈트,보라색슈트)");
            suitNamesToEnableModelPepe = config.Bind<string>("Suits to Replace Settings (Pepe)", "Suits to enable Model for(적용할 슈트 이름)", "Pepe", "Enter a comma separated list of suit names.(Additionally, [Default,Orange suit,Green suit,Pajama suit,Hazard suit])(한국어패치기준, [Default,주황색슈트,초록색슈트,파자마슈트,방호복슈트,보라색슈트)");

        }
        private void Awake()
        {
            config = base.Config;
            InitConfig();
            Assets.PopulateAssets();

            // Plugin startup logic
            /*if (enableModelForAllSuitsAkula.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementOverride(typeof(MRAKULA));

            }
            if (enableModelAsDefaultAkula.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementDefault(typeof(MRAKULA));

            }*/
            var commaSepListAkula = suitNamesToEnableModelAkula.Value.Split(',');
            foreach (var item in commaSepListAkula)
            {
                ModelReplacementAPI.RegisterSuitModelReplacement(item, typeof(MRAKULA));
            }
            /*if (enableModelForAllSuitsPepe.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementOverride(typeof(MRPEPE));

            }
            if (enableModelAsDefaultPepe.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementDefault(typeof(MRPEPE));

            }*/
            var commaSepListPepe = suitNamesToEnableModelPepe.Value.Split(',');
            foreach (var item in commaSepListPepe)
            {
                ModelReplacementAPI.RegisterSuitModelReplacement(item, typeof(MRPEPE));
            }


            Harmony harmony = new Harmony("com.Humyo.MoreCharacterSelector");
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {"com.Humyo.MoreCharacterSelector"} is loaded!");
        }
    }
    public static class Assets
    {
        // Replace mbundle with the Asset Bundle Name from your unity project 
        public static string mainAssetBundleName = "MoreCharacterSelector";
        public static AssetBundle MainAssetBundle = null;

        private static string GetAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name.Replace(" ","_");
        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                Console.WriteLine(GetAssemblyName() + "." + mainAssetBundleName);
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetAssemblyName() + "." + mainAssetBundleName))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }

            }
        }
    }

}