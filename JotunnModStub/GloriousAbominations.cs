using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using System;


namespace GloriousAbomination
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid, "2.0.12")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class GloriousAbominations : BaseUnityPlugin
    {
        public const string PluginGUID = "com.odinplus.gloriousabominations";
        public const string PluginName = "GloriousAbominations";
        public const string PluginVersion = "0.0.1";

        private void Awake()
        {

        }
    }
}