#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Onerat_Placement_ActionList
{
    private static int X;
    private static int Y;
    private static int Z;

    public static bool CollectData = false;
    public static List<GameObject> SceneGameObjects = new List<GameObject>();

    public static void PlaceObject(GameObject gameObject,
                                   Vector3 pos,
                                   float range,
                                   bool useAbsoluteValuesRot,
                                   Vector3 Amount,
                                   bool useAbsoluteValuesScale,
                                   bool lockedScale,
                                   Vector3 scaleAmount,
                                   float scaleAmountLocked,
                                   bool alignToNormal, 
                                   float NormalAlignBlend,
                                   bool Raycast = false,
                                   bool useLayerMask = false, 
                                   LayerMask layerMask = new LayerMask()) {
        RaycastHit hit;
        Vector3 normal = new Vector3();
        GameObject objectToSpawn = UnityEditor.PrefabUtility.InstantiatePrefab(gameObject) as GameObject;

        if (objectToSpawn) {
            UnityEditor.Undo.RegisterCreatedObjectUndo(objectToSpawn, "Created objectToSpawn");
            if(!Raycast) {
                objectToSpawn.transform.position = pos;
            } else {
                if(useLayerMask) {
                    if (Physics.Raycast(pos + new Vector3(0, 50, 0), Vector3.down, out hit, Mathf.Infinity, layerMask)) {
                        objectToSpawn.transform.position = hit.point;
                        normal = hit.normal;
                    }
                }
                else {
                    if (Physics.Raycast(pos + new Vector3(0, 50, 0), Vector3.down, out hit, Mathf.Infinity)) {
                        objectToSpawn.transform.position = hit.point;
                        normal = hit.normal;
                    }
                }                          
            }

            //valadate
            if (toClose(objectToSpawn.transform.position, SceneGameObjects, range)) {
                GameObject.DestroyImmediate(objectToSpawn.gameObject);
                return;
            }
            SceneGameObjects.Add(objectToSpawn);

            Vector3 Scale = new Vector3();
            if(!useAbsoluteValuesRot) {
                X = Random.Range(0, (int)Amount.x);
                Y = Random.Range(0, (int)Amount.y);
                Z = Random.Range(0, (int)Amount.z);
            } else {
                X = (int)Amount.x;
                Y = (int)Amount.y;
                Z = (int)Amount.z;
            }
            if(!useAbsoluteValuesScale) {
                Scale.x = Random.Range(0f, scaleAmount.x);
                Scale.y = Random.Range(0f, scaleAmount.y);
                Scale.z = Random.Range(0f, scaleAmount.z);
            } else {
                Scale.x = scaleAmount.x;
                Scale.y = scaleAmount.y;
                Scale.z = scaleAmount.z;
            }
            if (lockedScale) {
                Scale.x = scaleAmountLocked;
                Scale.y = scaleAmountLocked;
                Scale.z = scaleAmountLocked;
            }
            Quaternion randomRotation = Quaternion.Euler(X, Y, Z);
            Quaternion weightedRotation = randomRotation;
            if (alignToNormal) { weightedRotation = Quaternion.Lerp(Quaternion.FromToRotation(Vector3.up, normal) * randomRotation, randomRotation, NormalAlignBlend); }
            objectToSpawn.transform.rotation = weightedRotation;
            objectToSpawn.transform.localScale += Scale;
        }
    }
    static bool toClose(Vector3 position, List<GameObject> allPositions, float minDistance)
    {
        foreach (GameObject item in allPositions)
        {
            if (Vector3.Distance(position, item.transform.position) < minDistance) { return true; }
        }
        return false;
    }
}
#endif

