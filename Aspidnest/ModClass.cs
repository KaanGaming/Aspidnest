using Modding;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aspidnest
{
    public partial class Aspidnest : Mod, IGlobalSettings<AspidnestSettings>
    {
        internal static Aspidnest Instance;

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                ("Deepnest_East_07","Super Spitter"),
                ("Mines_07","Crystal Flyer"),
                ("Deepnest_43","Mantis Heavy Flyer"),
                ("Fungus3_48","Grass Hopper (1)"),
                ("Hive_03_c","Bee Stinger (4)"),
                ("Hive_03_c","Big Bee"),
		("Fungus1_22","Mosquito"),
		("Waterways_02","Fluke Fly"),
		("Fungus3_13","Moss Flyer"),
		("Ruins1_17","Ruins Flying Sentry Javelin")
            };
        }

        public Aspidnest() : base("Aspidnest")
        {
            Instance = this;
        }

        public override string GetVersion() => "1.0.0";

        #region Variables
        private GameObject aspid = null;
        private GameObject hunter = null;
        private GameObject frog = null;
        private GameObject petra = null;
        private GameObject soldier = null;
        private GameObject guardian = null;
	private GameObject squit = null;
	private GameObject fluke = null;
	private GameObject mossy = null;
	private GameObject lance = null;

        private float sceneTimer = 0.0f;
        private bool aspidSpawned = false;

        private bool enableAspids = true;

        private List<GameObject> managed = new List<GameObject>();
        #endregion

        #region Settings
        private AspidnestSettings stngs = new AspidnestSettings();

        public void OnLoadGlobal(AspidnestSettings s)
        {
            stngs = s;
        }

        public AspidnestSettings OnSaveGlobal()
        {
            return stngs;
        }
        #endregion

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            aspid = preloadedObjects["Deepnest_East_07"]["Super Spitter"];
            hunter = preloadedObjects["Mines_07"]["Crystal Flyer"];
            frog = preloadedObjects["Fungus3_48"]["Grass Hopper (1)"];
            petra = preloadedObjects["Deepnest_43"]["Mantis Heavy Flyer"];
            soldier = preloadedObjects["Hive_03_c"]["Bee Stinger (4)"];
            guardian = preloadedObjects["Hive_03_c"]["Big Bee"];
	    squit = preloadedObjects["Fungus1_22"]["Mosquito"];
	    fluke = preloadedObjects["Waterways_02"]["Fluke Fly"];
	    mossy = preloadedObjects["Fungus3_13"]["Moss Flyer"];
	    lance = preloadedObjects["Ruins1_17"]["Ruins Flying Sentry Javelin"];

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
            ModHooks.HeroUpdateHook += HeroUpdateHook;
            //TODO: create option to disable soul gain

            Instance = this;

            Log("Initialized");
        }

        #region Support Methods
        List<GameObject> GetChildren(GameObject parent)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                children.Add(child.gameObject);
            }
            return children;
        }

        private enum EnemyType
        {
            aspid,
            hunter,
            frog,
            petra,
            soldier,
            guardian,
	    squit,
	    fluke,
	    mossy,
	    lance
        }

        private void SetEnemyType(HealthManager hm, int type)
        {
            Type thm = hm.GetType();
            var et = thm.GetField("enemyType", BindingFlags.NonPublic | BindingFlags.Instance);
            et.SetValue(hm, type);
        }

        GameObject AddPrimalAspid(Vector2 position, EnemyType type)
        {
            #region Primal Aspid
            if (type == EnemyType.aspid)
            {
                foreach (var pfsm in aspid.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewAspid = GameObject.Instantiate(aspid);
                NewAspid.SetActive(true);
                NewAspid.SetActiveChildren(true);
                List<GameObject> AspidChildren = GetChildren(NewAspid);
                foreach (var child in AspidChildren)
                {
                    if (child.name == "Alert Range New")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                    if (child.name == "Unalert Range")
                        child.GetComponent<CircleCollider2D>().radius = 25;
                }
                HealthManager AspidHP = NewAspid.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(AspidHP, 3); // setting it to 3 or 6 disables soul gain
                AspidHP.hp = int.MaxValue;

                var aspidZ = NewAspid.transform.position.z;
                NewAspid.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewAspid.transform.GetScaleX();
                var yscale = NewAspid.transform.GetScaleY();
                NewAspid.transform.SetScaleX(xscale * stngs.scaler);
                NewAspid.transform.SetScaleY(yscale * stngs.scaler);
                return NewAspid;
            }
            #endregion

            #region Crystal Hunter
            if (type == EnemyType.hunter)
            {
                foreach (var pfsm in hunter.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewHunter = GameObject.Instantiate(hunter);
                NewHunter.SetActive(true);
                NewHunter.SetActiveChildren(true);
                List<GameObject> HunterChildren = GetChildren(NewHunter);
                foreach (var child in HunterChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager HunterHP = NewHunter.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(HunterHP, 3); // setting it to 3 or 6 disables soul gain
                HunterHP.hp = int.MaxValue;

                var aspidZ = NewHunter.transform.position.z;
                NewHunter.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewHunter.transform.GetScaleX();
                var yscale = NewHunter.transform.GetScaleY();
                NewHunter.transform.SetScaleX(xscale * stngs.scaler);
                NewHunter.transform.SetScaleY(yscale * stngs.scaler);
                return NewHunter;
            }
            #endregion

            #region Loodle
            if (type == EnemyType.frog)
            {
                foreach (var pfsm in frog.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewFrog = GameObject.Instantiate(frog);
                NewFrog.SetActive(true);
                NewFrog.SetActiveChildren(true);
                List<GameObject> FrogChildren = GetChildren(NewFrog);
                foreach (var child in FrogChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager FrogHP = NewFrog.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(FrogHP, 3); // setting it to 3 or 6 disables soul gain
                FrogHP.hp = int.MaxValue;

                var aspidZ = NewFrog.transform.position.z;
                NewFrog.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewFrog.transform.GetScaleX();
                var yscale = NewFrog.transform.GetScaleY();
                NewFrog.transform.SetScaleX(xscale * stngs.scaler);
                NewFrog.transform.SetScaleY(yscale * stngs.scaler);
                return NewFrog;
            }
            #endregion

            #region Mantis Petra
            if (type == EnemyType.petra)
            {
                foreach (var pfsm in petra.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewPetra = GameObject.Instantiate(petra);
                NewPetra.SetActive(true);
                NewPetra.SetActiveChildren(true);
                List<GameObject> PetraChildren = GetChildren(NewPetra);
                foreach (var child in PetraChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager PetraHP = NewPetra.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(PetraHP, 3); // setting it to 3 or 6 disables soul gain
                PetraHP.hp = int.MaxValue;

                var aspidZ = NewPetra.transform.position.z;
                NewPetra.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewPetra.transform.GetScaleX();
                var yscale = NewPetra.transform.GetScaleY();
                NewPetra.transform.SetScaleX(xscale * stngs.scaler);
                NewPetra.transform.SetScaleY(yscale * stngs.scaler);
                return NewPetra;
            }
            #endregion

            #region Hive Soldier
            if (type == EnemyType.soldier)
            {
                foreach (var pfsm in soldier.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewSoldier = GameObject.Instantiate(soldier);
                NewSoldier.SetActive(true);
                NewSoldier.SetActiveChildren(true);
                List<GameObject> SoldierChildren = GetChildren(NewSoldier);
                foreach (var child in SoldierChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager SoldierHP = NewSoldier.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(SoldierHP, 3); // setting it to 3 or 6 disables soul gain
                SoldierHP.hp = int.MaxValue;

                var aspidZ = NewSoldier.transform.position.z;
                NewSoldier.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewSoldier.transform.GetScaleX();
                var yscale = NewSoldier.transform.GetScaleY();
                NewSoldier.transform.SetScaleX(xscale * stngs.scaler);
                NewSoldier.transform.SetScaleY(yscale * stngs.scaler);
                return NewSoldier;
            }
            #endregion

            #region Hive Guardian
            if (type == EnemyType.guardian)
            {
                foreach (var pfsm in guardian.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewGuardian = GameObject.Instantiate(guardian);
                NewGuardian.SetActive(true);
                NewGuardian.SetActiveChildren(true);
                List<GameObject> GuardianChildren = GetChildren(NewGuardian);
                foreach (var child in GuardianChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager GuardianHP = NewGuardian.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(GuardianHP, 3); // setting it to 3 or 6 disables soul gain
                GuardianHP.hp = int.MaxValue;

                var aspidZ = NewGuardian.transform.position.z;
                NewGuardian.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewGuardian.transform.GetScaleX();
                var yscale = NewGuardian.transform.GetScaleY();
                NewGuardian.transform.SetScaleX(xscale * stngs.scaler);
                NewGuardian.transform.SetScaleY(yscale * stngs.scaler);
                return NewGuardian;
            }
            #endregion
	    
	    #region Squit
            if (type == EnemyType.squit)
            {
                foreach (var pfsm in squit.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewSquit = GameObject.Instantiate(squit);
                NewSquit.SetActive(true);
                NewSquit.SetActiveChildren(true);
                List<GameObject> SquitChildren = GetChildren(NewSquit);
                foreach (var child in SquitChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager SquitHP = NewSquit.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(SquitHP, 3); // setting it to 3 or 6 disables soul gain
                SquitHP.hp = int.MaxValue;

                var aspidZ = NewSquit.transform.position.z;
                NewSquit.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewSquit.transform.GetScaleX();
                var yscale = NewSquit.transform.GetScaleY();
                NewSquit.transform.SetScaleX(xscale * stngs.scaler);
                NewSquit.transform.SetScaleY(yscale * stngs.scaler);
                return NewSquit;
            }
            #endregion
	    
	    #region Fluke
            if (type == EnemyType.fluke)
            {
                foreach (var pfsm in fluke.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewFluke = GameObject.Instantiate(fluke);
                NewFluke.SetActive(true);
                NewFluke.SetActiveChildren(true);
                List<GameObject> FlukeChildren = GetChildren(NewFluke);
                foreach (var child in FlukeChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager FlukeHP = NewFluke.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(FlukeHP, 3); // setting it to 3 or 6 disables soul gain
                FlukeHP.hp = int.MaxValue;

                var aspidZ = NewFluke.transform.position.z;
                NewFluke.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewFluke.transform.GetScaleX();
                var yscale = NewFluke.transform.GetScaleY();
                NewFluke.transform.SetScaleX(xscale * stngs.scaler);
                NewFluke.transform.SetScaleY(yscale * stngs.scaler);
                return NewFluke;
            }
            #endregion
	    
	    #region Mossy
            if (type == EnemyType.mossy)
            {
                foreach (var pfsm in mossy.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewMossy = GameObject.Instantiate(mossy);
                NewMossy.SetActive(true);
                NewMossy.SetActiveChildren(true);
                List<GameObject> MossyChildren = GetChildren(NewMossy);
                foreach (var child in MossyChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager MossyHP = NewMossy.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(MossyHP, 3); // setting it to 3 or 6 disables soul gain
                MossyHP.hp = int.MaxValue;

                var aspidZ = NewMossy.transform.position.z;
                NewMossy.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewMossy.transform.GetScaleX();
                var yscale = NewMossy.transform.GetScaleY();
                NewMossy.transform.SetScaleX(xscale * stngs.scaler);
                NewMossy.transform.SetScaleY(yscale * stngs.scaler);
                return NewMossy;
            }
            #endregion
	    
	    #region Lance
            if (type == EnemyType.lance)
            {
                foreach (var pfsm in lance.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewLance = GameObject.Instantiate(lance);
                NewLance.SetActive(true);
                NewLance.SetActiveChildren(true);
                List<GameObject> LanceChildren = GetChildren(NewLance);
                foreach (var child in LanceChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager LanceHP = NewLance.GetComponent<HealthManager>();
                if (!stngs.enemysoul)
                    SetEnemyType(LanceHP, 3); // setting it to 3 or 6 disables soul gain
                LanceHP.hp = int.MaxValue;

                var aspidZ = NewLance.transform.position.z;
                NewLance.transform.position = new Vector3(position.x, position.y, aspidZ);

                var xscale = NewLance.transform.GetScaleX();
                var yscale = NewLance.transform.GetScaleY();
                NewLance.transform.SetScaleX(xscale * stngs.scaler);
                NewLance.transform.SetScaleY(yscale * stngs.scaler);
                return NewLance;
            }
            #endregion

            return null;
        }
        #endregion

        private void SpawnAspid()
        {
            aspidSpawned = true;

            managed.Clear();

            if (!GameManager.instance.IsGameplayScene()) return;

            bool knightfacesright = HeroController.instance.cState.facingRight;
            if (knightfacesright)
            {
                for (int t = 0; t < 10; t++)
                {
                    int count = 0;
                    EnemyType type = EnemyType.aspid;
                    if (t == 0) { count = stngs.aspidCount; type = EnemyType.aspid; }
                    if (t == 1) { count = stngs.hunterCount; type = EnemyType.hunter; }
                    if (t == 2) { count = stngs.frogCount; type = EnemyType.frog; }
                    if (t == 3) { count = stngs.petraCount; type = EnemyType.petra; }
                    if (t == 4) { count = stngs.soldierCount; type = EnemyType.soldier; }
                    if (t == 5) { count = stngs.guardianCount; type = EnemyType.guardian; }
		    if (t == 6) { count = stngs.squitCount; type = EnemyType.squit; }
		    if (t == 7) { count = stngs.flukeCount; type = EnemyType.fluke; }
		    if (t == 8) { count = stngs.mossyCount; type = EnemyType.mossy; }
		    if (t == 9) { count = stngs.lanceCount; type = EnemyType.lance; }

                    for (int i = 0; i < count; i++)
                    {
                        System.Random rng = new System.Random();
                        if (stngs.enemytp)
                        {
                            managed.Add(AddPrimalAspid(new Vector2(HeroController.instance.transform.GetPositionX() + 4f + (float)(rng.NextDouble() - 0.5), HeroController.instance.transform.GetPositionY() + 1.5f + (float)(rng.NextDouble() * 0.6 - 0.3)), type));
                        }
                        else
                        {
                            AddPrimalAspid(new Vector2(HeroController.instance.transform.GetPositionX() + 4f + (float)(rng.NextDouble() - 0.5), HeroController.instance.transform.GetPositionY() + 1.5f + (float)(rng.NextDouble() * 0.6 - 0.3)), type);
                        }
                    }
                }
            }
            else
            {
                for (int t = 0; t < 10; t++)
                {
                    int count = 0;
                    EnemyType type = EnemyType.aspid;
                    if (t == 0) { count = stngs.aspidCount; type = EnemyType.aspid; }
                    if (t == 1) { count = stngs.hunterCount; type = EnemyType.hunter; }
                    if (t == 2) { count = stngs.frogCount; type = EnemyType.frog; }
                    if (t == 3) { count = stngs.petraCount; type = EnemyType.petra; }
                    if (t == 4) { count = stngs.soldierCount; type = EnemyType.soldier; }
                    if (t == 5) { count = stngs.guardianCount; type = EnemyType.guardian; }
		    if (t == 6) { count = stngs.squitCount; type = EnemyType.squit; }
		    if (t == 7) { count = stngs.flukeCount; type = EnemyType.fluke; }
		    if (t == 7) { count = stngs.flukeCount; type = EnemyType.fluke; }
		    if (t == 8) { count = stngs.mossyCount; type = EnemyType.mossy; }
		    if (t == 9) { count = stngs.lanceCount; type = EnemyType.lance; }
		    

                    for (int i = 0; i < count; i++)
                    {
                        System.Random rng = new System.Random();
                        if (stngs.enemytp)
                        {
                            managed.Add(AddPrimalAspid(new Vector2(HeroController.instance.transform.GetPositionX() - 4f + (float)(rng.NextDouble() - 0.5), HeroController.instance.transform.GetPositionY() + 1.5f + (float)(rng.NextDouble() * 0.6 - 0.3)), type));
                        }
                        else
                        {
                            AddPrimalAspid(new Vector2(HeroController.instance.transform.GetPositionX() - 4f + (float)(rng.NextDouble() - 0.5), HeroController.instance.transform.GetPositionY() + 1.5f + (float)(rng.NextDouble() * 0.6 - 0.3)), type);
                        }
                    }
                }
            }
        }

        private void HeroUpdateHook()
        {
            if (!GameManager.instance.isPaused) sceneTimer += Time.deltaTime;

            if (Input.GetKeyDown(stngs.togglebind))
            {
                if (stngs.enemytp == false)
                    return;

                if (enableAspids)
                {
                    enableAspids = false;
                    for (int i = 0; i < managed.Count; i++)
                    {
                        GameObject.Destroy(managed[i]);
                    }
                    managed.Clear();
                }
                else
                {
                    enableAspids = true;
                    SpawnAspid();
                }
            }

            if (sceneTimer > 2f && !aspidSpawned && enableAspids) SpawnAspid();

            var herox = HeroController.instance.gameObject.transform.position.x;
            var heroy = HeroController.instance.gameObject.transform.position.y;
            if (HeroController.instance.cState.onGround)
            {
                int c = managed.Count;

                for (int i = 0; i < c; i++)
                {
                    var maspid = managed[i];

                    if (maspid == null)
                    {
                        managed.Remove(maspid);
                        i--;
                        c--;
                        continue;
                    }
                    if (maspid.transform.position.x - herox <= -25 || maspid.transform.position.x - herox >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy + 6f);
                    }
                    if (maspid.transform.position.y - heroy <= -25 || maspid.transform.position.y - heroy >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy + 6f);
                    }
                    LogDebug($"Player location: {herox}, {heroy}\nAspid location: {maspid.transform.position.x}, {maspid.transform.position.y}\nCalculation: {maspid.transform.position.x - herox}, {maspid.transform.position.y - heroy}");
                }
            }
            if (!HeroController.instance.cState.onGround)
            {
                foreach (GameObject maspid in managed)
                {
                    if (maspid == null)
                    {
                        managed.Remove(maspid);
                        continue;
                    }
                    if (maspid.transform.position.x - herox <= -25 || maspid.transform.position.x - herox >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy - 7f);
                    }
                    if (maspid.transform.position.y - heroy <= -25 || maspid.transform.position.y - heroy >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy - 7f);
                    }
                    LogDebug($"Player location: {herox}, {heroy}\nAspid location: {maspid.transform.position.x}, {maspid.transform.position.y}\nCalculation: {maspid.transform.position.x - herox}, {maspid.transform.position.y - heroy}");
                }
            }
        }

        private void ActiveSceneChanged(Scene from, Scene to)
        {
            sceneTimer = 0;
            aspidSpawned = false;
        }
    }
}
