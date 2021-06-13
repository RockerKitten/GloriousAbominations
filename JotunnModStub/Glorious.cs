using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using System;


namespace Glorious
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid, "2.0.12")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class Glorious : BaseUnityPlugin
    {
        public const string PluginGUID = "odinplus.glorious";
        public const string PluginName = "Glorious";
        public const string PluginVersion = "1.0.0";
        public AssetBundle assetBundle;
        public AssetBundle assetWigs;
        public AssetBundle assetRainbow;
        public EffectList buildStone;
        public EffectList buildWood;
        public EffectList buildTorch;
        public EffectList fxcraft;
        public EffectList fxdone;
        public EffectList fxrepair;
        public AudioSource torchVol;
        //public AudioSource bonfireVol;
        public EffectList breakStone;
        public EffectList breakWood;
        public EffectList breakTorch;
        public EffectList hitTorch;
        public EffectList hitWoodStone;
        public GameObject wigCFab;
        public CustomItem wigC;
        public GameObject wigMohawkPonyFab;
        public CustomItem wigMohawkPony;
        public GameObject sfxcraft;
        public GameObject sfxcraftdone;
        public GameObject sfxrepair;
        


        private void Awake()
        {

            AssetLoad();

            ItemManager.OnVanillaItemsAvailable += LoadSounds;

        }
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("misc", Assembly.GetExecutingAssembly());
            assetWigs= AssetUtils.LoadAssetBundleFromResources("rainbowwigs", Assembly.GetExecutingAssembly());
            assetRainbow = AssetUtils.LoadAssetBundleFromResources("rainbows", Assembly.GetExecutingAssembly());
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
                var sfxcraft = PrefabManager.Cache.GetPrefab<GameObject>("sfx_gui_craftitem_workbench");
                var sfxcraftdone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_gui_craftitem_workbench_end"); //(craft item done
                var sfxrepair = PrefabManager.Cache.GetPrefab<GameObject>("sfx_gui_repairitem_workbench"); //enabled = true repair item effect


                fxrepair = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxrepair } } };
                fxdone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcraftdone } } };
                fxcraft = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcraftdone } } };
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
                LoadWigClaire();
                LoadWigMohawkPony();
                LoadRainbowWorkbench();
                LoadRainbowHammer();
                LoadRainbowBanner();


                //torchVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
                //bonfireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;

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

            /*torchVol = torchFab.AddComponent<AudioSource>();
            torchVol.clip = assetBundle.LoadAsset<AudioClip>("torch_clip");
            torchVol.playOnAwake = true;
            torchVol.loop = true;
            torchVol.rolloffMode = AudioRolloffMode.Linear;*/
            if (AudioMan.instance == null) Jotunn.Logger.LogError("failed to load mixer");
            else
            {
                var torchBurn = torchFab.GetComponentInChildren<AudioSource>();
                torchBurn.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            }
           
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
                    CraftingStation = "rk_rainbowbridge",
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
                    CraftingStation = "rk_rainbowBridge",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1}
                    }
                });
            ItemManager.Instance.AddItem(wigC);
        }
        public void LoadRainbowWorkbench()
        {
            var workbenchFab = assetRainbow.LoadAsset<GameObject>("rk_rainbowbridge");
            var workbench = new CustomPiece(workbenchFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Raspberry", Amount = 3, Recover = true},
                        new RequirementConfig {Item = "Blueberries", Amount = 3, Recover = true},
                        new RequirementConfig {Item = "Dandelion", Amount = 3, Recover = true}
                    }
                });
            var workbenchBuild = workbenchFab.GetComponent<Piece>();
            workbenchBuild.m_placeEffect = buildWood;

            var workbenchCraft = workbenchFab.GetComponent<CraftingStation>();
            workbenchCraft.m_craftItemEffects = fxcraft;
            workbenchCraft.m_repairItemDoneEffects = fxrepair;
            workbenchCraft.m_craftItemDoneEffects = fxdone;

            var workbenchBreak = workbenchFab.GetComponent<WearNTear>();
            workbenchBreak.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(workbench);
            
        }
        private void LoadRainbowHammer()
        {
            var rainbowhammerFab = assetRainbow.LoadAsset<GameObject>("rk_rainbowhammer");
            var rainbowhammer = new CustomItem(rainbowhammerFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$rainbow_hammer",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "rk_rainbowbridge",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = ("Hammer"), Amount = 1, Recover = true},
                        new RequirementConfig {Item = ("Blueberies"), Amount = 3, Recover = true},
                        new RequirementConfig {Item = ("Raspberry"), Amount = 3, Recover = true},
                        new RequirementConfig {Item = ("Dandelion"), Amount = 3, Recover = true}
                    }

                });
            ItemManager.Instance.AddItem(rainbowhammer);
        }
        private void LoadRainbowBanner()
        {

        }
    }
}