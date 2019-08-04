using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using ModCommon;
using System.Collections;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using Logger = Modding.Logger;
using UnityEngine.SceneManagement;

namespace ElderbugGod
{
    class ElderFinder : MonoBehaviour
    {
        public static bool died;
        private GameObject _target;
        private void Start()
        {
            _target = HeroController.instance.gameObject;
            USceneManager.activeSceneChanged += SceneChanged;
            ModHooks.Instance.AfterPlayerDeadHook += AfterPlayerDied;
            Log("wow we be here");
            ElderbugGod.preloadedGO["beam"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = ElderbugGod.SPRITES[0].texture;
            ElderbugGod.preloadedGO["ball"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = ElderbugGod.SPRITES[0].texture;
            ElderbugGod.preloadedGO["beam"].GetComponent<DamageHero>().damageDealt = 99;
            Log("wow did it");
        }

        private void AfterPlayerDied()
        {
            died = true;
        }

        private void SceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Town" && !died)
            {
                GameObject elder = GameObject.Find("Elderbug");
                StartCoroutine(DelayedBoio(elder));
            }
            else if (arg1.name == "Town")
            {
                StartCoroutine(AfterDeed());
            }
        }

        IEnumerator DelayedBoio(GameObject Elder)
        {
            yield return null;
            yield return new WaitWhile(() => _target.transform.GetPositionX() - Elder.transform.GetPositionX() < 13f);
            Log("oh oh");
            while (!died)
            {
                GameObject goBeam = Instantiate(ElderbugGod.preloadedGO["beam"]);
                GameObject goBall = Instantiate(ElderbugGod.preloadedGO["ball"]);
                goBeam.SetActive(true);
                goBall.SetActive(true);
                goBeam.transform.localScale *= 1.7f;
                goBall.transform.localScale *= 1.7f;
                var pos2 = new Vector2(Elder.transform.GetPositionX() + 1.7f, Elder.transform.GetPositionY()+1f);
                var pos1 = HeroController.instance.transform.position;
                goBeam.transform.SetPosition2D(pos2);
                goBall.transform.SetPosition2D(pos2);
                var ang = Mathf.Atan2(pos1.y - pos2.y, pos1.x - pos2.x) * Mathf.Rad2Deg;
                Log(ang);
                goBeam.transform.SetRotation2D(ang);
                goBall.transform.SetRotation2D(ang);
                //yield return null;//Beam Extender
                Destroy(goBeam.LocateMyFSM("destroy_if_gameobject_null"));
                goBeam.GetComponent<tk2dSpriteAnimator>().enabled = true;
                goBeam.GetComponent<MeshRenderer>().enabled = true;
                Destroy(goBall.LocateMyFSM("destroy_if_gameobject_null"));
                goBall.GetComponent<tk2dSpriteAnimator>().enabled = true;
                goBall.GetComponent<MeshRenderer>().enabled = true;
                yield return null;
                goBall.GetComponent<tk2dSpriteAnimator>().Play("Ball Antic");
                goBeam.GetComponent<tk2dSpriteAnimator>().Play("Beam Antic");
                yield return new WaitForSeconds(0.7f);
                goBall.GetComponent<tk2dSpriteAnimator>().Play("Ball Shoot");
                goBeam.GetComponent<tk2dSpriteAnimator>().Play("Beam Shoot");
                yield return new WaitForSeconds(0.8f);
                goBall.GetComponent<tk2dSpriteAnimator>().Play("Ball End");
                goBeam.GetComponent<tk2dSpriteAnimator>().Play("Beam End");
                yield return new WaitWhile(() => goBeam.GetComponent<tk2dSpriteAnimator>().IsPlaying("Beam End"));
                Destroy(goBall);
                Destroy(goBeam);
                yield return null;
            }
        }
        IEnumerator AfterDeed()
        {
            Log("am here");
            yield return new WaitWhile(() => !Input.GetKey(KeyCode.Mouse0));
            GameObject eld = GameObject.Find("Elderbug");
            Log("click of ma");
            for (int i = 0; i < 10f; i++)
            {
                eld.transform.Rotate(Vector3.forward * -250f * Time.deltaTime);
                yield return null;
            }
            Log("heree");
            yield return new WaitForSeconds(1f);
            eld.AddComponent<Rigidbody2D>().gravityScale = 0f;
            eld.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(45f) * 30f, Mathf.Sin(45f) * 30f);
            yield return new WaitForSeconds(0.05f);
            eld.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            Log("hererere");

            GameObject goBeam = Instantiate(ElderbugGod.preloadedGO["beam"]);
            GameObject goBall = Instantiate(ElderbugGod.preloadedGO["ball"]);
            goBeam.SetActive(true);
            goBall.SetActive(true);
            goBeam.transform.localScale *= 1.4f;
            goBall.transform.localScale *= 1.4f;
            var pos2 = new Vector2(eld.transform.GetPositionX() - 2f, eld.transform.GetPositionY() - 1f);
            var pos1 = new Vector2(pos2.x-6f,pos2.y-5f);
            goBeam.transform.SetPosition2D(pos2);
            goBall.transform.SetPosition2D(pos2);
            var ang = Mathf.Atan2(pos1.y - pos2.y, pos1.x - pos2.x) * Mathf.Rad2Deg;
            Log(ang);
            goBeam.transform.SetRotation2D(ang);
            goBall.transform.SetRotation2D(ang);
            //yield return null;//Beam Extender
            Destroy(goBeam.LocateMyFSM("destroy_if_gameobject_null"));
            goBeam.GetComponent<tk2dSpriteAnimator>().enabled = true;
            goBeam.GetComponent<MeshRenderer>().enabled = true;
            Destroy(goBall.LocateMyFSM("destroy_if_gameobject_null"));
            goBall.GetComponent<tk2dSpriteAnimator>().enabled = true;
            goBall.GetComponent<MeshRenderer>().enabled = true;
            yield return null;
            goBall.GetComponent<tk2dSpriteAnimator>().Play("Ball Antic");
            goBeam.GetComponent<tk2dSpriteAnimator>().Play("Beam Antic");
            yield return new WaitForSeconds(0.7f);
            goBall.GetComponent<tk2dSpriteAnimator>().Play("Ball Shoot");
            goBeam.GetComponent<tk2dSpriteAnimator>().Play("Beam Shoot");

            eld.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(45f) * 30f, Mathf.Sin(45f) * 30f);

            yield return new WaitForSeconds(1f);
            goBall.GetComponent<tk2dSpriteAnimator>().Play("Ball End");
            goBeam.GetComponent<tk2dSpriteAnimator>().Play("Beam End");
            yield return new WaitWhile(() => goBeam.GetComponent<tk2dSpriteAnimator>().IsPlaying("Beam End"));
            Destroy(goBall);
            Destroy(goBeam);
        }

        private static void Log(object obj)
        {
            Logger.Log("[GodBug] " + obj);
        }
    }
}