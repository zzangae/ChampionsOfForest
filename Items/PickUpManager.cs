﻿using BuilderCore;
using System.Collections.Generic;
using UnityEngine;
namespace ChampionsOfForest
{
    public static class PickUpManager
    {

        public static Dictionary<int, ItemPickUp> PickUps = new Dictionary<int, ItemPickUp>();



        private static Material pickupMaterial;
        private static Material Heart_pickupMaterial;


        /// <summary>
        /// spawns a pickup for the clinet.
        /// </summary>
        public static void SpawnPickUp(Item item, Vector3 pos, int amount, int id)
        {
            try
            {
                GameObject spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
                spawn.AddComponent<Rigidbody>().mass = 5;
                spawn.transform.position = pos;
                MeshRenderer renderer = spawn.GetComponent<MeshRenderer>();
                MeshFilter filter = spawn.GetComponent<MeshFilter>();
                if (pickupMaterial == null)
                {
                    pickupMaterial = Core.CreateMaterial(new BuildingData() { MainColor = Color.gray, Metalic = 0.35f, Smoothness = 0.7f });
                    Heart_pickupMaterial = Core.CreateMaterial(new BuildingData() { MainColor = Color.white, Metalic = 0.1f, Smoothness = 0.4f, MainTexture = Res.ResourceLoader.GetTexture(103), BumpMap = Res.ResourceLoader.GetTexture(104) });
                }
                spawn.GetComponent<BoxCollider>().size = Vector3.one * 0.3f;
                renderer.material = pickupMaterial;

                switch (item._itemType)
                {
                    case BaseItem.ItemType.Shield:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[49];

                        break;
                    case BaseItem.ItemType.Offhand:


                        break;
                    case BaseItem.ItemType.Weapon:

                        if (item.weaponModel == BaseItem.WeaponModelType.GreatSword)
                        {
                            filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[52];
                        }
                        else if (item.weaponModel == BaseItem.WeaponModelType.LongSword)
                        {
                            filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[51];
                        }

                        break;
                    case BaseItem.ItemType.Other:
                        if (item.name == "Greater Mutated Heart")
                        {
                            filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[102];
                            renderer.material = Heart_pickupMaterial;
                            spawn.transform.localScale *= 2f;
                        }
                        else if (item.name == "Lesser Mutated Heart")
                        {
                            filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[102];
                            renderer.material = Heart_pickupMaterial;
                        }

                        break;
                    case BaseItem.ItemType.Material:


                        break;
                    case BaseItem.ItemType.Helmet:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[48];
                        spawn.transform.localScale *= 0.95f;
                        break;
                    case BaseItem.ItemType.Boot:

                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[44];
                        spawn.transform.localScale *= 1.1f;

                        break;
                    case BaseItem.ItemType.Pants:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[50];
                        spawn.transform.localScale *= 1.7f;

                        break;
                    case BaseItem.ItemType.ChestArmor:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[42];
                        spawn.transform.localScale *= 1.5f;

                        break;
                    case BaseItem.ItemType.ShoulderArmor:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[53];
                        spawn.transform.localScale *= 1.6f;


                        break;
                    case BaseItem.ItemType.Glove:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[41];
                        spawn.transform.localScale *= 2.2f;

                        break;
                    case BaseItem.ItemType.Bracer:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[45];

                        break;
                    case BaseItem.ItemType.Amulet:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[40];
                        spawn.transform.localScale *= 0.9f;

                        break;
                    case BaseItem.ItemType.Ring:
                        filter.mesh = Res.ResourceLoader.instance.LoadedMeshes[43];

                        break;
                    default:
                        break;
                }





                if (item.Rarity > 2)
                {
                    Light l = spawn.AddComponent<Light>();
                    l.type = LightType.Point;
                    l.shadowStrength = 1;
                    l.color = MainMenu.RarityColors[item.Rarity];
                    l.intensity = 1f;
                    l.range = 4f;
                    renderer.material.color = MainMenu.RarityColors[item.Rarity];
                    if (item.Rarity > 5)
                    {
                        l.range = 7f;
                        l.intensity = 1.7f;
                        l.cookie = Res.ResourceLoader.GetTexture(24);
                        l.cookieSize = 5f;
                        if (item.Rarity == 7)
                        {
                            l.intensity = 2.7f;
                        }
                    }
                }
                spawn.AddComponent<MeshCollider>();

                ItemPickUp pickup = spawn.AddComponent<ItemPickUp>();

                pickup.item = item;
                pickup.amount = amount;
                pickup.ID = id;
                PickUps.Add(id, pickup);
                if (Network.NetworkManager.lastDropID < id)
                {
                    Network.NetworkManager.lastDropID = id;
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write("Problem with creating a item pickup " + ex.ToString());
            }
        }
        /// <summary>
        /// removes a pickup with given id for the clinet
        /// </summary>
        public static void RemovePickup(int id)
        {
            if (PickUps.ContainsKey(id))
            {
                PickUps[id].Remove();
                PickUps.Remove(id);
            }
        }
    }
}
