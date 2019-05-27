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

    public List<GameObject> MovableObjects;
    public List<GameObject> Haybales;
    public List<FallingObject> FallingObjects;
    public List<Breakable> TrapObjects;
    public List<Peasant> Enemies;
    public List<GateScript> Gates;
    public List<Dashable> Dashables;
    public List<CoinPickup> Coins;

    private void Awake()
    {
        MovableObjects = new List<GameObject>();
        Haybales = new List<GameObject>();
        FallingObjects = new List<FallingObject>();
        TrapObjects = new List<Breakable>();
        Enemies = new List<Peasant>();
        Gates = new List<GateScript>();
        Dashables = new List<Dashable>();
        Coins = new List<CoinPickup>();
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
            Load();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            Save();
        }
    }

    public void Save()
    {
        PlayerModel player = new PlayerModel(new VectorModel(GameManager.instance.player.transform.position), new VectorModel(GameManager.instance.player.transform.eulerAngles), GameManager.instance.coinCount, GameManager.instance.player.playerValues.health, GameManager.instance.player.playerValues.maxHealth);
        localSaveModel = new SaveModel(LevelManager.instance.LevelNumber, new VectorModel(LevelManager.instance.currentCheckpoint.position), LevelManager.instance.hasGateKey, player);
        //Moveable objects
        foreach (GameObject go in MovableObjects)
        {
            ObjectModel model = new ObjectModel(GetId(go), go.activeSelf, new VectorModel(go.transform.position), new VectorModel(go.transform.eulerAngles));
            localSaveModel.MovableObjects.Add(model);
            //Debug.Log(model.Id + " " + model.IsDisabled + " " + model.Position + " " + model.Rotation);
        }

        foreach(GameObject go in Haybales)
        {
            ObjectModel model = new ObjectModel(GetId(go), go.activeSelf, new VectorModel(go.transform.position), new VectorModel(go.transform.eulerAngles));
            localSaveModel.Haybales.Add(model);
        }

        foreach(FallingObject fo in FallingObjects)
        {
            FallingObjectModel model = new FallingObjectModel(GetId(fo.gameObject), fo.gameObject.activeSelf, new VectorModel(fo.transform.position), new VectorModel(fo.transform.eulerAngles), fo.HasFallen);
            localSaveModel.FallingObjects.Add(model);
        }

        foreach (Breakable to in TrapObjects)
        {
            FallingObjectModel model = new FallingObjectModel(GetId(to.gameObject), to.gameObject.activeSelf, new VectorModel(to.transform.position), new VectorModel(to.transform.eulerAngles), to.Broke);
            localSaveModel.TrapObjects.Add(model);
        }

        foreach(Peasant en in Enemies)
        {
            EnemyModel model = new EnemyModel(GetId(en.gameObject), en.gameObject.activeSelf, new VectorModel(en.transform.position), new VectorModel(en.transform.eulerAngles), en.CurrentToughness);
            localSaveModel.Enemies.Add(model);
        }

        foreach(GateScript gs in Gates)
        {
            GateModel model = new GateModel(GetId(gs.gameObject), gs.gameObject.activeSelf, new VectorModel(gs.transform.position), new VectorModel(gs.transform.eulerAngles), gs.IsOpened);
            localSaveModel.Gates.Add(model);
        }

        foreach(Dashable ds in Dashables)
        {
            ObjectModel model = new ObjectModel(GetId(ds.gameObject), ds.gameObject.activeSelf, new VectorModel(ds.gameObject.transform.position), new VectorModel(ds.gameObject.transform.eulerAngles));
            localSaveModel.Dashables.Add(model);
        }

        foreach (CoinPickup co in Coins)
        {
            ObjectModel model = new ObjectModel(GetId(co.gameObject), co.gameObject.activeSelf, new VectorModel(co.gameObject.transform.position), new VectorModel(co.gameObject.transform.eulerAngles));
            localSaveModel.Coins.Add(model);
        }

        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");

        formatter.Serialize(saveFile, localSaveModel);
        Debug.Log("SPARAT BINÄRT!");
        saveFile.Close();

        XmlSerializer serializer = new XmlSerializer(typeof(SaveModel));
        StreamWriter writer = new StreamWriter("Saves/save.xml");
        serializer.Serialize(writer.BaseStream, localSaveModel);
        Debug.Log("SPARAT XML!");
        writer.Close();

    }

    public void Load()
    {

    }
}
