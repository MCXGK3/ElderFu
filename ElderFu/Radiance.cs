using IL;
using UnityEngine;
using Satchel;
using HutongGames;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections;
using Satchel.Futils;
using UnityEngine.Rendering;

namespace ElderFu
{
    internal class Radiance:MonoBehaviour
    {
        PlayMakerFSM _con;
        PlayMakerFSM _com;
        PlayMakerFSM _cho;
        PlayMakerFSM _tele;
        HealthManager _hm;
        GameObject knight;
        List<GameObject> bus=new();
        float angle = 0f;
        float anglev = 0.5f;
        float r=10f;
        float ascendv = 3f;
        bool removed = false;

        private void Awake()
        {
            _con = gameObject.LocateMyFSM("Control");
            _com = gameObject.LocateMyFSM("Attack Commands");
            _cho = gameObject.LocateMyFSM("Attack Choices");
            _tele = gameObject.LocateMyFSM("Teleport");
            _hm=gameObject.GetComponent<HealthManager>();
            knight =GameObject.Find("Knight");

        }
        private void Start()
        {
            bus.Add(_com.FsmVariables.GetFsmGameObject("Eye Beam Burst1").Value);
            bus.Add(_com.FsmVariables.GetFsmGameObject("Eye Beam Burst2").Value);
            bus.Add(_com.FsmVariables.GetFsmGameObject("Eye Beam Burst3").Value);
            _con.GetAction<WaitRandom>("Arena 1 Idle", 0).timeMin=0.02f;
            _con.GetAction<WaitRandom>("Arena 1 Idle", 0).timeMax=0.02f;
            _con.GetAction<Wait>("A1 Cast End", 1).time = 0f;
            _cho.GetAction<Wait>("Orb Recover", 0).time = 0f;
            _cho.GetAction<Wait>("Nail Fan Recover", 0).time = 0f;
            _cho.GetAction<Wait>("Eye Beam Recover", 0).time = 0f;
            _cho.GetState("Nail Fan").AddAction(_cho.GetAction<SendEventByName>("Nail Fan Recover", 1));
            _cho.GetState("Orbs").AddAction(_cho.GetAction<SendEventByName>("Orb Recover", 1));
            _cho.GetState("Eye Beam").AddAction(_cho.GetAction<SendEventByName>("Eye Beam Recover", 1));
            _con.GetAction<DecelerateV2>("A1 Cast Antic", 2).deceleration = 1.5f;
            _tele.GetAction<Wait>("Flash", 5).time = 0.01f;
            _cho.GetAction<SendRandomEventV3>("A1 Choice", 1).weights = new FsmFloat[] { 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f };
            _cho.GetAction<SendRandomEventV3>("A1 Choice", 1).eventMax = new FsmInt[] { 0, 0, 0, 999999999, 0, 0,0, 0 };
            _cho.GetAction<SendRandomEventV3>("A1 Choice", 1).missedMax = new FsmInt[] { 9999999, 9999999, 9999999, 9999999, 999999, 9999999, 9999999, 9999999 };
            _con.GetAction<WaitRandom>("Arena 2 Idle", 0).timeMin = 0.02f;
            _con.GetAction<WaitRandom>("Arena 2 Idle", 0).timeMax = 0.02f;
            _con.GetAction<Wait>("A2 Cast End", 1).time = 0f;
            _con.GetAction<DecelerateV2>("A2 Cast Antic", 2).deceleration = 1.5f;
            _cho.GetAction<SendRandomEventV3>("A2 Choice", 1).weights = new FsmFloat[] { 0f, 0f, 0f, 1f, 0f, 0f, };
            _cho.GetAction<SendRandomEventV3>("A2 Choice", 1).eventMax = new FsmInt[] { 0, 0, 0, 999999999, 0,0 };
            _cho.GetAction<SendRandomEventV3>("A2 Choice", 1).missedMax = new FsmInt[] { 9999999, 9999999, 9999999, 9999999, 999999, 9999999 };
            _con.GetAction<WaitRandom>("Final Idle", 0).timeMax = 0.02f;
            _con.GetAction<WaitRandom>("Final Idle", 0).timeMin = 0.02f;
            _con.GetAction<SendEventByName>("Scream", 1).sendEvent = new FsmString() { Value = "ASCEND BEAM" };// _con.GetAction<SendEventByName>("Ascend Cast", 1).sendEvent;
            _com.InsertCustomAction("EB 1", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[0].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f,1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 2", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[1].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 3", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[2].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 4", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[0].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 5", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[1].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 6", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[2].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 7", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[0].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 8", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[1].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _com.InsertCustomAction("EB 9", () =>
            {
                List<GameObject> childs = new List<GameObject>();
                bus[2].FindAllChildren(childs);
                foreach (var beam in childs)
                {
                    beam.GetComponent<tk2dSprite>().color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                }
            }, 0);
            _con.GetAction<SendEventByName>("Rage1 Tele", 1).delay = 0.1f;
            _con.GetAction<SendEventByName>("Ascend Tele", 1).delay = 0.1f;
            _com.InsertCustomAction("AB Start", () =>
            {
                foreach(var bu in bus)
                {
                    bu.SetActive(false);
                }
            }, 0);
            _com.InsertCustomAction("Aim", () =>
            {
                GameObject ab = _com.FsmVariables.FindFsmGameObject("Ascend Beam").Value;
                ab.GetComponent<tk2dSprite>().color= new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            }, 0);
            _con.GetAction<SendEventByName>("Ascend Cast", 1).sendEvent = new FsmString() { Value = "RAGE EYES" };
            _con.GetAction<FloatCompare>("Ascend Cast", 3).float2 = 157f;
            SendEventByName plats = _con.GetAction<SendEventByName>("Plat Setup", 2);
            _con.RemoveAction("Plat Setup", 2);
            _con.GetState("Scream").AddAction(plats);
           /* _com.InsertCustomAction("Aim", () =>
            {
                StartCoroutine(ThrowEyeBeam());
            }, 6);
            _com.GetAction<Wait>("Aim", 12).time = 4f;*/
        }

        private IEnumerator ThrowEyeBeam()
        {
            GameObject eb = Instantiate(bus[0], bus[0].transform);
            Vector3 pos = knight.transform.position-eb.transform.position;
            
            if (eb == null)
            {
               
            }
            else
            {
                eb.SetActive(true);
                ElderFu.Instance.Log(eb.transform.position);
                foreach(var mo in eb.GetComponents<MonoBehaviour>()) {
                    ElderFu.Instance.Log(mo);
                    ElderFu.Instance.Log(mo.name);
                }
                
                ElderFu.Instance.Log(knight);
                ElderFu.Instance.Log(eb.GetComponent<iTween>());
                List<GameObject> children = new List<GameObject>();
                eb.FindAllChildren(children);
          
                foreach (var beam in children)
                {
                    if (beam.name.Contains("Beam"))
                    {
                        //ElderFu.Instance.Log(beam);
                        beam.LocateMyFSM("Control").SendEvent("Antic");
                    }
                    else
                    {
                        ElderFu.Instance.Log(beam);
                    }
                }
                float v = _com.GetVariable<FsmFloat>("Rotation").Value;
                //while (eb.transform.GetPositionY() > 75f)
                //{
                while (eb.transform.GetPositionY() > 75f)
                {
                    eb.transform.position += pos * (Time.deltaTime / 4f);
                    ElderFu.Instance.Log(eb.transform.position);
                    yield return null;
                }
            }
            yield return new WaitForSeconds(4f);
                Destroy(eb);
            yield break;
            //}

        }

        private void Update()
        {
            if(_con.ActiveStateName=="Ascend Cast")
            {
                // angle += Time.deltaTime * anglev;
                //angle %= 360f
                //gameObject.transform.position = knight.transform.position - new Vector3(0,1.4f,0);
                //gameObject.transform.SetPositionY(knight.transform.position.y);
                gameObject.transform.SetPositionY(gameObject.transform.position.y+ Time.deltaTime * ascendv);
                if (gameObject.transform.position.y - knight.transform.position.y > 10f)
                {
                    gameObject.transform.SetPositionY(40.7f);
                }


                //gameObject.transform.position = knight.transform.position + new Vector3(r * Mathf.Cos(angle), r * Mathf.Sin(angle), 0); 
            }
            if (!removed && _cho.ActiveStateName == "A2 End" && gameObject.transform.position.y >= 150f)
            {
                GameObject pit = GameObject.Find("Abyss Pit");
                PlayMakerFSM _asc = pit.LocateMyFSM("Ascend");
                _asc.RemoveTransition("Idle", "ASCEND");
                removed = true;
                Modding.Logger.Log("Have Removed Abyss Pit");
            }
        }
        private void OnDestroy()
        {

        }
    }
}