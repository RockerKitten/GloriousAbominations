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
        //public AssetBundle assetBundle;
        public AssetBundle assetWigs;
        public AssetBundle assetRainbow;
        //public AssetBundle assetFire;
        public AssetBundle assetPlants;
        public EffectList buildStone;
        public EffectList buildWood;
        public EffectList buildTorch;
        public EffectList fxcraft;
        public EffectList fxdone;
        public EffectList fxrepair;
        public AudioSource torchVol;
        public AudioSource torch2Vol;
        public AudioSource walltorchVol;
        public AudioSource brazierVol;
        public EffectList breakStone;
        public EffectList breakWood;
        public EffectList breakTorch;
        public EffectList hitTorch;
        public EffectList hitWoodStone;
        public GameObject wigCFab;
        public CustomItem wigC;
        public GameObject wigMohawkPonyFab;
        public CustomItem wigMohawkPony;
        //public GameObject rb_trollarmor;
        //public CustomItem trollarmor;
        //public Texture torchtex;





        private void Awake()
        {
            LoadLocalizaion();
            AssetLoad();
            ItemManager.OnVanillaItemsAvailable += LoadSounds;
            

        }
        private void AssetLoad()
        {
            //assetFire = AssetUtils.LoadAssetBundleFromResources("rainbowfire", Assembly.GetExecutingAssembly());
            assetWigs = AssetUtils.LoadAssetBundleFromResources("rainbowwigs", Assembly.GetExecutingAssembly());
            assetRainbow = AssetUtils.LoadAssetBundleFromResources("rainbows", Assembly.GetExecutingAssembly());
            assetPlants = AssetUtils.LoadAssetBundleFromResources("rainbowplants", Assembly.GetExecutingAssembly());
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
            var vfsrainbows = assetRainbow.LoadAsset<GameObject>("vfxRainbowHearts");
            //var rb_trollarmor = PrefabManager.Cache.GetPrefab<GameObject>("ArmorTrollLeatherChest");



            fxrepair = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxrepair } } };
            fxdone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcraftdone } } };
            fxcraft = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcraft } } };
            buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfsrainbows } } };
            buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwood }, new EffectList.EffectData { m_prefab = vfsrainbows } } };
            buildTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxmetal }, new EffectList.EffectData { m_prefab = vfsrainbows } } };
            breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstonebreak }, new EffectList.EffectData { m_prefab = vfxstonebreak } } };
            breakWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwoodbreak }, new EffectList.EffectData { m_prefab = vfxbreak } } };
            breakTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwoodbreak }, new EffectList.EffectData { m_prefab = vfxmetalbreak } } };
            hitTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxmetalbreak } } };
            hitWoodStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxbreak } } };


            Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");


            LoadWigClaire();
            LoadWigMohawkPony();
            LoadRainbowWorkbench();
            LoadRainbowHammer();
            LoadRainbowBanner();
            LoadRainbowBannerPastel();
            LoadPrimaryPaint();
            LoadSecondaryPaint();
            LoadRainbowCone();
            LoadRainbowSword();
            LoadWig3();
            LoadWig4();
            LoadCirclet();
            LoadPlant1();
            LoadRainbowAxe();
            LoadPlant2();
            LoadPlant3();
            LoadPlant4();
            LoadPlant5();
            //LoadRainbowCape();
            LoadRainbowSwordv2();

            LoadWalls1();
            LoadWalls2();
            LoadWalls3();
            LoadWalls4();
            LoadWalls5();
            LoadWalls6();
            LoadWalls7();
            LoadWalls8();
            LoadPosts1();
            LoadPosts2();
            LoadPosts3();
            LoadPosts4();
            LoadPosts5();
            LoadPosts6();
            LoadPosts7();
            LoadPosts8();
            LoadFloor1();
            LoadFloor2();
            LoadFloor3();
            LoadFloor4();
            LoadRoof1();
            LoadRoof2();
            LoadRoof3();
            LoadRoof4();
            LoadTorch2();
            LoadTorchs();
            LoadWallTorch();
            LoadBrazier();



            torchVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            brazierVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            walltorchVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            torch2Vol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;

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
        public void LoadLocalizaion()
        {
            TextAsset[] textAssets = assetRainbow.LoadAllAssets<TextAsset>();
            foreach (var textAsset in textAssets)
            {
                var English = textAsset.name.Replace("English.json", null);
                LocalizationManager.Instance.AddJson(English, textAsset.ToString());
                
            }
        }
       
    public void LoadTorchs()

        {
            var torchFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowtorch");
            var torch = new CustomPiece(torchFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_RainbowPieceTable",
                    Requirements = new[]
                    {
                         new RequirementConfig { Item = "Iron", Amount = 1, Recover = true },
                         new RequirementConfig {Item = "Resin", Amount = 2, Recover = true},
                         new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true}
                    }

                });
            torchVol = torchFab.GetComponentInChildren<AudioSource>();
            
            var torchBreak = torchFab.GetComponent<WearNTear>();
            torchBreak.m_destroyedEffect = breakTorch;
            torchBreak.m_hitEffect = hitTorch;

            var torchEffect = torchFab.GetComponent<Piece>();
            torchEffect.m_placeEffect = buildTorch;
            PieceManager.Instance.AddPiece(torch);
        }
        public void LoadBrazier()

        {
            var torchFab = assetRainbow.LoadAsset<GameObject>("rb_brazier");
            var torch = new CustomPiece(torchFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_RainbowPieceTable",
                    Requirements = new[]
                    {
                         new RequirementConfig { Item = "Bronze", Amount = 1, Recover = true },
                         new RequirementConfig {Item = "Resin", Amount = 2, Recover = true},
                         new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true}
                    }

                });
            brazierVol = torchFab.GetComponentInChildren<AudioSource>();
            
            
            var torchBreak = torchFab.GetComponent<WearNTear>();
            torchBreak.m_destroyedEffect = breakTorch;
            torchBreak.m_hitEffect = hitTorch;

            var torchEffect = torchFab.GetComponent<Piece>();
            torchEffect.m_placeEffect = buildTorch;
            PieceManager.Instance.AddPiece(torch);
        }
        public void LoadWallTorch()

        {
            var torchFab = assetRainbow.LoadAsset<GameObject>("rb_walltorch");
            var torch = new CustomPiece(torchFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_RainbowPieceTable",
                    Requirements = new[]
                    {
                         new RequirementConfig { Item = "Copper", Amount = 1, Recover = true },
                         new RequirementConfig {Item = "Resin", Amount = 2, Recover = true},
                         new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true}
                    }

                });
            walltorchVol = torchFab.GetComponentInChildren<AudioSource>();
            
            var torchBreak = torchFab.GetComponent<WearNTear>();
            torchBreak.m_destroyedEffect = breakTorch;
            torchBreak.m_hitEffect = hitTorch;

            var torchEffect = torchFab.GetComponent<Piece>();
            torchEffect.m_placeEffect = buildTorch;
            PieceManager.Instance.AddPiece(torch);
        }
        private void LoadTorch2()
        { 
            var torch2Fab = assetRainbow.LoadAsset<GameObject>("rb_groundtorch_wood");
             var torch2 = new CustomPiece(torch2Fab,
                 new PieceConfig
                 {
                     CraftingStation = "rb_rainbowbridge",
                     AllowedInDungeons = false,
                     Enabled = true,
                     PieceTable = "_RainbowPieceTable",
                     Requirements = new[]
                     {
                         new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                         new RequirementConfig {Item = "Resin", Amount = 2, Recover = false},
                         new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true}
                     }


                 });

             
            torch2Vol = torch2Fab.GetComponentInChildren<AudioSource>();
            
            var torch2Break = torch2Fab.GetComponent<WearNTear>();
             torch2Break.m_destroyedEffect = breakTorch;
             torch2Break.m_hitEffect = hitTorch;
            

             var torch2Effect = torch2Fab.GetComponent<Piece>();
             torch2Effect.m_placeEffect = buildTorch;
            

            PieceManager.Instance.AddPiece(torch2);

         }
         /*public void LoadRainbowCape()
         {
            
           var rainbowcloakFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowcape");
           var rainbowcloak = new CustomItem(rainbowcloakFab, fixReference: false,
                new ItemConfig
                 {
                     Name = "$rainbow_cloak",
                     Enabled = true,
                     Amount = 1,
                     CraftingStation = "rb_rainbowbridge",
                     RepairStation = "rb_rainbowbridge",
                     MinStationLevel = 1,
                     Requirements = new[]
                     {
                         new RequirementConfig {Item = "Wood", Amount = 1, AmountPerLevel = 1}
                     }
                 });
            
             ItemManager.Instance.AddItem(rainbowcloak);
         }*/

        public void LoadWigClaire()
        {
            wigCFab = assetWigs.LoadAsset<GameObject>("$claire_kiara_rainbow");

            wigC = new CustomItem(wigCFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$claire_kiara_rainbow",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "LinenThread", Amount = 2, AmountPerLevel = 1},
                        new RequirementConfig {Item = "BlackMetal", Amount = 5, AmountPerLevel = 1},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, AmountPerLevel = 0},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, AmountPerLevel = 0}
                    }
                });
            ItemManager.Instance.AddItem(wigC);
        }
        public void LoadWigMohawkPony()
        {
            wigMohawkPonyFab = assetWigs.LoadAsset<GameObject>("$mohawkpony_redearcat_rainbow");
            wigMohawkPony = new CustomItem(wigMohawkPonyFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$mohawkpony_redearcat_rainbow",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "LinenThread", Amount = 2, AmountPerLevel = 1},
                        new RequirementConfig {Item = "BlackMetal", Amount = 5, AmountPerLevel = 1},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, AmountPerLevel = 0},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, AmountPerLevel = 0}

                    }
                });
            ItemManager.Instance.AddItem(wigMohawkPony);
        }
        public void LoadRainbowWorkbench()
        {
            var workbenchFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowbridge");
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
            workbenchBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(workbench);

        }
        private void LoadRainbowHammer()
        {
            var rainbowhammerFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowhammer");
            var rainbowhammer = new CustomItem(rainbowhammerFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$rainbow_hammer",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = ("Hammer"), Amount = 1, Recover = true},
                        new RequirementConfig {Item = ("Blueberries"), Amount = 3, Recover = true},
                        new RequirementConfig {Item = ("Raspberry"), Amount = 3, Recover = true},
                        new RequirementConfig {Item = ("Dandelion"), Amount = 3, Recover = true}
                    }

                });
            ItemManager.Instance.AddItem(rainbowhammer);
        }
        private void LoadRainbowBanner()
        {
            var rainbowbannerFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowbanner");
            var rainbowbanner = new CustomPiece(rainbowbannerFab,
                new PieceConfig
                {
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    AllowedInDungeons = false,
                    PieceTable = "_RainbowPieceTable",
                    Requirements = new[]
                   {
                       new RequirementConfig {Item = "LeatherScraps", Amount = 6, Recover = true},
                       new RequirementConfig {Item = "FineWood", Amount = 2, Recover = true},
                       new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                       new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                   }
                });
            var bannerbuild = rainbowbannerFab.GetComponent<Piece>();
            bannerbuild.m_placeEffect = buildWood;

            var bannerdestroy = rainbowbannerFab.GetComponent<WearNTear>();
            bannerdestroy.m_destroyedEffect = breakWood;
            bannerdestroy.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(rainbowbanner);
        }
        private void LoadRainbowBannerPastel()
        {
            var rainbowbannerFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowbanner_pastel");
            var rainbowbannerpastel = new CustomPiece(rainbowbannerFab,
                new PieceConfig
                {
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    AllowedInDungeons = false,
                    PieceTable = "_RainbowPieceTable",
                    Requirements = new[]
                   {
                       new RequirementConfig {Item = "LeatherScraps", Amount = 6, Recover = true},
                       new RequirementConfig {Item = "FineWood", Amount = 2, Recover = true},
                       new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                       new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                   }
                });
            var bannerbuild = rainbowbannerFab.GetComponent<Piece>();
            bannerbuild.m_placeEffect = buildWood;

            var bannerdestroy = rainbowbannerFab.GetComponent<WearNTear>();
            bannerdestroy.m_destroyedEffect = breakWood;
            bannerdestroy.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(rainbowbannerpastel);
        }
        private void LoadPrimaryPaint()
        {
            var primarypaintFab = assetRainbow.LoadAsset<GameObject>("rb_primary");
            var primarypaint = new CustomItem(primarypaintFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$primary",
                    Amount = 2,
                    Enabled = true,
                    CraftingStation = "rb_rainbowbridge",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Blueberries", Amount = 1 },
                        new RequirementConfig { Item = "Raspberry", Amount = 1 },
                        new RequirementConfig { Item = "Dandelion", Amount = 1 }
                    }
                });
            ItemManager.Instance.AddItem(primarypaint);
        }
        private void LoadSecondaryPaint()
        {
            var secondarypaintFab = assetRainbow.LoadAsset<GameObject>("rb_secondary");
            var secondarypaint = new CustomItem(secondarypaintFab, fixReference: false,
            new ItemConfig
            {
                Name = "$secondary",
                Amount = 2,
                CraftingStation = "rb_rainbowbridge",
                Enabled = true,
                Requirements = new[]
                {
                    new RequirementConfig {Item = "Guck", Amount = 1},
                    new RequirementConfig {Item = "Turnip", Amount = 1},
                    new RequirementConfig {Item = "Carrot", Amount = 1}
                }
            });
            ItemManager.Instance.AddItem(secondarypaint);
        }
        private void LoadRainbowCone()
        {
            var rainbowconeFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowcone");
            var rainbowcone = new CustomItem(rainbowconeFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$rainbow_cone",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "rb_primary", Amount = 1},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1},
                        new RequirementConfig {Item = "BarleyFlour", Amount = 1}
                    }
                });
            ItemManager.Instance.AddItem(rainbowcone);
        }
        private void LoadRainbowSword()
        {
            var rainbowswordFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowsword");
            var rainbowsword = new CustomItem(rainbowswordFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$rainbow_sword",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    RepairStation = "rb_rainbowbridge",
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "rb_primary", Amount = 3, AmountPerLevel = 1},
                        new RequirementConfig {Item = "rb_secondary", Amount = 3, AmountPerLevel = 1 },
                        new RequirementConfig {Item = "BlackMetal", Amount = 20, AmountPerLevel = 5},
                        new RequirementConfig {Item = "LinenThread", Amount = 5, AmountPerLevel = 1}
                    }
                });
            ItemManager.Instance.AddItem(rainbowsword);
        }
        private void LoadRainbowSwordv2()
        {
            var rainbowswordFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowswordv2");
            var rainbowswordv2 = new CustomItem(rainbowswordFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$rainbow_sword_v2",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    RepairStation = "rb_rainbowbridge",
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "rb_primary", Amount = 3, AmountPerLevel = 1},
                        new RequirementConfig {Item = "rb_secondary", Amount = 3, AmountPerLevel = 1 },
                        new RequirementConfig {Item = "BlackMetal", Amount = 20, AmountPerLevel = 5},
                        new RequirementConfig {Item = "LinenThread", Amount = 5, AmountPerLevel = 1}
                    }
                });
            ItemManager.Instance.AddItem(rainbowswordv2);
        }
        private void LoadWig3()
        {
            var Wig3Fab = assetWigs.LoadAsset<GameObject>("$claire_kiara_rainbow_dverger");
            var Wig3 = new CustomItem(Wig3Fab, fixReference: false,
                new ItemConfig
                {
                    Name = "$claire_kiara_rainbow_dverger",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = ("$claire_kiara_rainbow"), Amount = 1, AmountPerLevel = 0 },
                        new RequirementConfig { Item = ("$customitem_helmet_dverger_rainbow"), Amount = 1, AmountPerLevel = 0 },
                        new RequirementConfig { Item = ("rb_primary"), Amount = 0, AmountPerLevel = 1 },
                        new RequirementConfig { Item = ("rb_secondary"), Amount = 0, AmountPerLevel = 1 }
                    }

                });
            ItemManager.Instance.AddItem(Wig3);
        }
        private void LoadWig4()
        {
            var Wig4Fab = assetWigs.LoadAsset<GameObject>("$mohawkpony_redearcat_rainbow_dverger");
            var Wig4 = new CustomItem(Wig4Fab, fixReference: false,
                new ItemConfig
                {
                    Name = "$mohawkpony_redearcat_rainbow_dverger",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = ("$mohawkpony_redearcat_rainbow"), Amount = 1, AmountPerLevel = 0 },
                        new RequirementConfig { Item = ("$customitem_helmet_dverger_rainbow"), Amount = 1, AmountPerLevel = 0 },
                        new RequirementConfig { Item = ("rb_primary"), Amount = 0, AmountPerLevel = 1 },
                        new RequirementConfig { Item = ("rb_secondary"), Amount = 0, AmountPerLevel = 1 }
                    }

                });
            ItemManager.Instance.AddItem(Wig4);
        }
        private void LoadCirclet()
        {
            var circletFab = assetWigs.LoadAsset<GameObject>("$customitem_helmet_dverger_rainbow");
            var circlet = new CustomItem(circletFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$customitem_helmet_dverger_rainbow",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = ("HelmetDverger"), Amount = 1},
                        new RequirementConfig { Item = ("rb_primary"), Amount = 1},
                        new RequirementConfig { Item = ("rb_secondary"), Amount = 1 }
                    }

                });
            ItemManager.Instance.AddItem(circlet);
        }
        private void LoadPlant1()
        {
            var aeoniumsFab = assetPlants.LoadAsset<GameObject>("$custompiece_aeoniums_rainbow");
            var aeoniums = new CustomPiece(aeoniumsFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = aeoniumsFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = aeoniumsFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(aeoniums);
        }
        private void LoadRainbowAxe()
        {
            var rainbowaxeFab = assetRainbow.LoadAsset<GameObject>("rb_rainbowaxe");
            var rainbowaxe = new CustomItem(rainbowaxeFab, fixReference: false,
                new ItemConfig
                {
                    Name = "$rainbow_axe",
                    Amount = 1,
                    CraftingStation = "rb_rainbowbridge",
                    Enabled = true,
                    RepairStation = "rb_rainbowbridge",
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "rb_primary", Amount = 3, AmountPerLevel = 1},
                        new RequirementConfig {Item = "rb_secondary", Amount = 3, AmountPerLevel = 1 },
                        new RequirementConfig {Item = "BlackMetal", Amount = 20, AmountPerLevel = 5},
                        new RequirementConfig {Item = "LinenThread", Amount = 5, AmountPerLevel = 1}
                    }
                });
            ItemManager.Instance.AddItem(rainbowaxe);
        }
        private void LoadPlant2()
        {
            var coolpotFab = assetPlants.LoadAsset<GameObject>("$custompiece_coolpot_rainbow");
            var coolpot = new CustomPiece(coolpotFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = coolpotFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = coolpotFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(coolpot);
        }
        private void LoadPlant3()
        {
            var plantFab = assetPlants.LoadAsset<GameObject>("$custompiece_hangingplant_rainbow");
            var plant = new CustomPiece(plantFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = plantFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = plantFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(plant);
        }
        private void LoadPlant4()
        {
            var plantFab = assetPlants.LoadAsset<GameObject>("$custompiece_hangingplantlarge_rainbow");
            var plant = new CustomPiece(plantFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = plantFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = plantFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(plant);
        }
        private void LoadPlant5()
        {
            var plantFab = assetPlants.LoadAsset<GameObject>("$custompiece_orchid2_rainbow");
            var plant = new CustomPiece(plantFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = plantFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = plantFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(plant);
        }
        private void LoadWalls1()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_hf_rainbow");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls2()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_hf_rainbow_pastel");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls3()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_hh_rainbow");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls4()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_hh_rainbow_pastel");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls5()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_vf_rainbow");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls6()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_vf_rainbow_pastel");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls7()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_vh_rainbow");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadWalls8()
        {
            var wallFab = assetRainbow.LoadAsset<GameObject>("rb_vh_rainbow_pastel");
            var wall = new CustomPiece(wallFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = wallFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = wallFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(wall);
        }
        private void LoadPosts1()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_long_wood_beam");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts2()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_long_wood_beam_pastel");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts3()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_long_wood_pole");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts4()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_long_wood_pole_pastel");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts5()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_short_wood_beam");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts6()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_short_wood_beam_pastel");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts7()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_short_wood_pole");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadPosts8()
        {
            var poleFab = assetRainbow.LoadAsset<GameObject>("rb_short_wood_pole_pastel");
            var pole = new CustomPiece(poleFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = poleFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = poleFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(pole);
        }
        private void LoadFloor1()
        {
            var floorFab = assetRainbow.LoadAsset<GameObject>("rb_wood_floor");
            var floor = new CustomPiece(floorFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = floorFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = floorFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(floor);
        }
        private void LoadFloor2()
        {
            var floorFab = assetRainbow.LoadAsset<GameObject>("rb_wood_floorv2");
            var floor = new CustomPiece(floorFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = floorFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = floorFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(floor);
        }
        private void LoadFloor3()
        {
            var floorFab = assetRainbow.LoadAsset<GameObject>("rb_wood_floor_pastel");
            var floor = new CustomPiece(floorFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = floorFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = floorFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(floor);
        }
        private void LoadFloor4()
        {
            var floorFab = assetRainbow.LoadAsset<GameObject>("rb_wood_floor_pastelv2");
            var floor = new CustomPiece(floorFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = floorFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;

            var woodBreak = floorFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;

            PieceManager.Instance.AddPiece(floor);
        }
        private void LoadRoof1()
        {
            var roofFab = assetRainbow.LoadAsset<GameObject>("rb_45_roof");
            var roof = new CustomPiece(roofFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = roofFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;
            

            var woodBreak = roofFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(roof);
        }
        private void LoadRoof2()
        {
            var roofFab = assetRainbow.LoadAsset<GameObject>("rb_45_roof_pastel");
            var roof = new CustomPiece(roofFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = roofFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;


            var woodBreak = roofFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(roof);
        }
        private void LoadRoof3()
        {
            var roofFab = assetRainbow.LoadAsset<GameObject>("rb_45corner_roof");
            var roof = new CustomPiece(roofFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = roofFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;


            var woodBreak = roofFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(roof);
        }
        private void LoadRoof4()
        {
            var roofFab = assetRainbow.LoadAsset<GameObject>("rb_45corner_roof_pastel");
            var roof = new CustomPiece(roofFab,
                new PieceConfig
                {
                    AllowedInDungeons = false,
                    CraftingStation = "rb_rainbowbridge",
                    PieceTable = "rb_rainbowhammer",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_primary", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "rb_secondary", Amount = 1, Recover = true}
                    }

                });
            var woodBuild = roofFab.GetComponent<Piece>();
            woodBuild.m_placeEffect = buildWood;


            var woodBreak = roofFab.GetComponent<WearNTear>();
            woodBreak.m_destroyedEffect = breakWood;
            woodBreak.m_hitEffect = hitWoodStone;


            PieceManager.Instance.AddPiece(roof);
        }

    }
}