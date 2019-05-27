using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SaveModel localSaveModel;
    private bool hasInitialized = false;

    
    public Dictionary<float, GameObject> MovableObjects;
    public Dictionary<float, GameObject> Haybales;
    public Dictionary<float, FallingObject> FallingObjects;
    public Dictionary<float, Breakable> TrapObjects;
    public Dictionary<float, Peasant> Enemies;
    public Dictionary<float, GateScript> Gates;
    public Dictionary<float, Dashable> Dashables;
    public Dictionary<float, CoinPickup> Coins;

    private void Awake()
    {
        MovableObjects = new Dictionary<float, GameObject>();
        Haybales = new Dictionary<float, GameObject>();
        FallingObjects = new Dictionary<float, FallingObject>();
        TrapObjects = new Dictionary<float, Breakable>();
        Enemies = new Dictionary<float, Peasant>();
        Gates = new Dictionary<float, GateScript>();
        Dashables = new Dictionary<float, Dashable>();
        Coins = new Dictionary<float, CoinPickup>();
    }

    private void Start()
    {
        
    }

    private float GetId(GameObject go)
    {
        Saveable saveable = go.GetComponent<Saveable>();
        if (saveable != null)
        {
            return saveable.Id;
        }
        return -1f;
    }

    private void Update()
    {
        if(hasInitialized == false)
        {
            hasInitialized = true;
            //Load();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            Save();
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            Load();
        }
    }

    public void Save()
    {
        PlayerModel player = new PlayerModel(new VectorModel(GameManager.instance.player.transform.position), new VectorModel(GameManager.instance.player.transform.eulerAngles), GameManager.instance.coinCount, GameManager.instance.player.playerValues.health, GameManager.instance.player.playerValues.maxHealth);
        localSaveModel = new SaveModel(LevelManager.instance.LevelNumber, new VectorModel(LevelManager.instance.currentCheckpoint.position), LevelManager.instance.hasGateKey, player);
        //Moveable objects
        foreach (GameObject go in MovableObjects.Values)
        {
            ObjectModel model = new ObjectModel(GetId(go), go.activeSelf, new VectorModel(go.transform.position), new VectorModel(go.transform.eulerAngles));
            localSaveModel.MovableObjects.Add(model);
            //Debug.Log(model.Id + " " + model.IsDisabled + " " + model.Position + " " + model.Rotation);
        }

        foreach(GameObject go in Haybales.Values)
        {
            ObjectModel model = new ObjectModel(GetId(go), go.activeSelf, new VectorModel(go.transform.position), new VectorModel(go.transform.eulerAngles));
            localSaveModel.Haybales.Add(model);
        }

        foreach(FallingObject fo in FallingObjects.Values)
        {
            FallingObjectModel model = new FallingObjectModel(GetId(fo.gameObject), fo.gameObject.activeSelf, new VectorModel(fo.transform.position), new VectorModel(fo.transform.eulerAngles), fo.HasFallen);
            localSaveModel.FallingObjects.Add(model);
        }

        foreach (Breakable to in TrapObjects.Values)
        {
            FallingObjectModel model = new FallingObjectModel(GetId(to.gameObject), to.gameObject.activeSelf, new VectorModel(to.transform.position), new VectorModel(to.transform.eulerAngles), to.Broke);
            localSaveModel.TrapObjects.Add(model);
        }

        foreach(Peasant en in Enemies.Values)
        {
            EnemyModel model = new EnemyModel(GetId(en.gameObject), en.gameObject.activeSelf, new VectorModel(en.transform.position), new VectorModel(en.transform.eulerAngles), en.CurrentToughness, en.IsStunned);
            localSaveModel.Enemies.Add(model);
        }

        foreach(GateScript gs in Gates.Values)
        {
            GateModel model = new GateModel(GetId(gs.gameObject), gs.gameObject.activeSelf, new VectorModel(gs.transform.position), new VectorModel(gs.transform.eulerAngles), gs.IsOpened);
            localSaveModel.Gates.Add(model);
        }

        foreach(Dashable ds in Dashables.Values)
        {
            ObjectModel model = new ObjectModel(GetId(ds.gameObject), ds.gameObject.activeSelf, new VectorModel(ds.gameObject.transform.position), new VectorModel(ds.gameObject.transform.eulerAngles));
            localSaveModel.Dashables.Add(model);
        }

        foreach (CoinPickup co in Coins.Values)
        {
            ObjectModel model = new ObjectModel(GetId(co.gameObject), co.gameObject.activeSelf, new VectorModel(co.gameObject.transform.position), new VectorModel(co.gameObject.transform.eulerAngles));
            localSaveModel.Coins.Add(model);
        }

        //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");

        formatter.Serialize(saveFile, localSaveModel);
        Debug.Log("SPARAT BINÄRT!");
        saveFile.Close();

        //http://gram.gs/gramlog/xml-serialization-and-deserialization-in-unity/
        XmlSerializer serializer = new XmlSerializer(typeof(SaveModel));
        StreamWriter writer = new StreamWriter("Saves/save.xml");
        serializer.Serialize(writer.BaseStream, localSaveModel);
        Debug.Log("SPARAT XML!");
        writer.Close();

    }

    public void Load()
    {
        //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);

        localSaveModel = (SaveModel)formatter.Deserialize(saveFile);

        saveFile.Close();

        GameManager.instance.player.transform.position = localSaveModel.Player.Position.GetVector();
        GameManager.instance.player.transform.eulerAngles = localSaveModel.Player.Rotation.GetVector();
        GameManager.instance.player.playerValues.health = localSaveModel.Player.Health;
        GameManager.instance.player.playerValues.maxHealth = localSaveModel.Player.MaxHealth;
        GameManager.instance.coinCount = localSaveModel.Player.Coins;
        LevelManager.instance.hasGateKey = localSaveModel.HasKey;

        foreach (ObjectModel model in localSaveModel.MovableObjects)
        {
            if(MovableObjects.ContainsKey(model.Id))
            {
                GameObject go = MovableObjects[model.Id];
                go.transform.position = model.Position.GetVector();
                go.transform.eulerAngles = model.Rotation.GetVector();
                go.SetActive(model.IsActive);
            }
        }

        foreach (ObjectModel model in localSaveModel.Haybales)
        {
            if (MovableObjects.ContainsKey(model.Id))
            {
                GameObject go = Haybales[model.Id];
                go.transform.position = model.Position.GetVector();
                go.transform.eulerAngles = model.Rotation.GetVector();
                go.SetActive(model.IsActive);
            }
        }

        foreach (FallingObjectModel model in localSaveModel.FallingObjects)
        {
            if (FallingObjects.ContainsKey(model.Id))
            {
                FallingObject obj = FallingObjects[model.Id];
                obj.transform.position = model.Position.GetVector();
                obj.transform.eulerAngles = model.Rotation.GetVector();
                obj.HasFallen = model.HasFallen;
                obj.gameObject.SetActive(model.IsActive);
            }
        }

        foreach (FallingObjectModel model in localSaveModel.TrapObjects)
        {
            if (TrapObjects.ContainsKey(model.Id))
            {
                Breakable obj = TrapObjects[model.Id];
                obj.transform.position = model.Position.GetVector();
                obj.transform.eulerAngles = model.Rotation.GetVector();
                obj.Broke = model.HasFallen;
                obj.gameObject.SetActive(model.IsActive);
            }
        }

        foreach (EnemyModel model in localSaveModel.Enemies)
        {
            if (Enemies.ContainsKey(model.Id))
            {
                Peasant obj = Enemies[model.Id];
                obj.transform.position = model.Position.GetVector();
                obj.transform.eulerAngles = model.Rotation.GetVector();
                if(model.IsStunned)
                {
                    //    obj.PlayerDash(Vector3.zero);
                    obj.isDying = true;
                }
                obj.gameObject.SetActive(model.IsActive);
            }
        }

        foreach (GateModel model in localSaveModel.Gates)
        {
            if (Gates.ContainsKey(model.Id))
            {
                GateScript obj = Gates[model.Id];
                obj.transform.position = model.Position.GetVector();
                obj.transform.eulerAngles = model.Rotation.GetVector();
                if(model.IsOpen)
                {
                    obj.Open();
                }
                obj.gameObject.SetActive(model.IsActive);
            }
        }

        foreach (ObjectModel model in localSaveModel.Dashables)
        {
            if (Dashables.ContainsKey(model.Id))
            {
                Dashable obj = Dashables[model.Id];
                obj.transform.position = model.Position.GetVector();
                obj.transform.eulerAngles = model.Rotation.GetVector();
                obj.gameObject.SetActive(model.IsActive);
            }
        }

        foreach (ObjectModel model in localSaveModel.Coins)
        {
            if (Coins.ContainsKey(model.Id))
            {
                CoinPickup obj = Coins[model.Id];
                obj.transform.position = model.Position.GetVector();
                obj.transform.eulerAngles = model.Rotation.GetVector();
                obj.gameObject.SetActive(model.IsActive);
            }
        }

        Debug.Log("LOADED!");

    }
}
