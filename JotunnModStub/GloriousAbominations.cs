﻿using BepInEx;
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
        public AssetBundle assetBundle;
        public EffectList buildStone;
        public EffectList buildWood;
        public EffectList buildTorch;
        public AudioSource torchVol;
        public AudioSource bonfireVol;
        public EffectList breakStone;
        public EffectList breakWood;
        public EffectList breakTorch;
        public EffectList hitTorch;
        public EffectList hitWoodStone;
        public GameObject wigCFab;
        public CustomItem wigC;
        public GameObject wigMohawkPonyFab;
        public CustomItem wigMohawkPony;


        private void Awake()
        {

            AssetLoad();
            LoadWigClaire();
            LoadWigMohawkPony();

            ItemManager.OnVanillaItemsAvailable += LoadSounds;

        }
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("misc", Assembly.GetExecutingAssembly());
            assetBundle = AssetUtils.LoadAssetBundleFromResources("rainbowwigs", Assembly.GetExecutingAssembly());
        }
        private void LoadSounds()
        {
            try
            {
                var sfxstone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
                var vfxstone = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
                var sfxwood = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_wood");
                var vfxwood = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_wood_wall");
                var sfxmetal = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_metal");
                var vfxmetal = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_wood_pole");
                var vfxmetalbreak = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HitSparks");
                var sfxwoodbreak = PrefabManager.Cache.GetPrefab<GameObject>("sfx_wood_destroyed");
                var sfxstonebreak = PrefabManager.Cache.GetPrefab<GameObject>("sfx_rock_destroyed");
                var vfxstonebreak = PrefabManager.Cache.GetPrefab<GameObject>("vfx_RockDestroyed");
                var vfxbreak = PrefabManager.Cache.GetPrefab<GameObject>("vfx_SawDust");
                var sfxstonehit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_rock_hit");



                buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfxstone } } };
                buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwood }, new EffectList.EffectData { m_prefab = vfxwood } } };
                buildTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxmetal }, new EffectList.EffectData { m_prefab = vfxmetal } } };
                breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstonebreak }, new EffectList.EffectData { m_prefab = vfxstonebreak } } };
                breakWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwoodbreak }, new EffectList.EffectData { m_prefab = vfxbreak } } };
                breakTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwoodbreak }, new EffectList.EffectData { m_prefab = vfxmetalbreak } } };
                hitTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxmetalbreak } } };
                hitWoodStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxbreak } } };


                Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");


                LoadTorch();
                
                torchVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
                bonfireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;

            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {
                ItemManager.OnVanillaItemsAvailable -= LoadSounds;

            }

        }
        public void LoadTorch()

        {
            var torchFab = assetBundle.LoadAsset<GameObject>("rk_torchnew");

            //piece_grill

            var torch = new CustomPiece(torchFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1, Recover = true }
                    }

                });

            torchVol = torchFab.AddComponent<AudioSource>();
            torchVol.clip = assetBundle.LoadAsset<AudioClip>("torch_clip");
            torchVol.playOnAwake = true;
            torchVol.loop = true;
            torchVol.rolloffMode = AudioRolloffMode.Linear;
            //if (AudioMan.instance == null) Jotunn.Logger.LogError("lolwut");
           
            var torchBreak = torchFab.GetComponent<WearNTear>();
            torchBreak.m_destroyedEffect = breakTorch;
            torchBreak.m_hitEffect = hitTorch;

            //torchVol.outputAudioMixerGroup

            var torchEffect = torchFab.GetComponent<Piece>();
            torchEffect.m_placeEffect = buildTorch;
            PieceManager.Instance.AddPiece(torch);
        }
        //$claire_kiara_rainbow
        //$mohawkpony_redearcat_rainbow
        public void LoadWigClaire()
        {
            wigCFab = assetBundle.LoadAsset<GameObject>("$claire_kiara_rainbow");

            wigC = new CustomItem(wigCFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$claire_kiara_rainbow",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "piece_workbench",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1}
                    }
                });
            ItemManager.Instance.AddItem(wigC);
        }
    public void LoadWigMohawkPony()
        {
            wigMohawkPonyFab = assetBundle.LoadAsset<GameObject>("$mohawkpony_redearcat_rainbow");
            wigMohawkPony = new CustomItem(wigMohawkPonyFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$mohawkpony_redearcat_rainbow",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "piece_workbench",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1}
                    }
                });
            ItemManager.Instance.AddItem(wigC);
        }
    }
}