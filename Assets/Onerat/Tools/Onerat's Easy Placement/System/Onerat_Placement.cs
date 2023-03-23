#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class Onerat_Placement : EditorWindow
{
    [MenuItem("Tools/Onerat's Easy Placement #b")]
    private static void Init() {
        Onerat_Placement window = (Onerat_Placement)GetWindow(typeof(Onerat_Placement));
        window.titleContent = new GUIContent("Onerat's Easy Placement");
        window.Show();
    }

    bool alignToNormal = false;
    bool useAbsoluteValuesRot = false;
    bool useAbsoluteValuesScale = false;

    bool active = false;

    float xAmount = 0;
    float yAmount = 0;
    float zAmount = 0;
    private float NormalAlignBlend = 0;

    Vector3 scaleAmount = new Vector3(0,0,0);
    float scaleAmountLocked = 0;

    Vector2 scrollPos;

    private bool toggleGeneral, toggleRotations, toggleScales, lockedScale, toggleAdditional, toggleMask;
    private string textGeneral = "Open", textRotations = "Open", textScales = "Open", textLockedScale = "Three Axis", textUseGrouping = "Single", textAlignmentLock = "Dont Align To Normals", textAdditional = "Open", textRotAbsolute = "Random Range", textScaleAbsolute = "Random Range", textMask = "No Mask";

    private string EnabledButtonText = "DISABLED";
    private Color buttonColor = new Color(150f / 255f, 79f / 255f, 44f / 255f, 1);
    private Color SubColor = new Color(150f / 255f, 79f / 255f, 44f / 255f, 1);

    bool useGroups = false;
    private float Spread = 15;
    private float MinDistance = 1;
    private int ObjectCount = 15;

    private Texture2D MakeTex(int width, int height, Color col) {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i) { pix[i] = col; }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    LayerMask mask;
    bool init = false;

    void OnGUI()
    {
        if(!init) { active = false; init = true; }
        bool hasPro = InternalEditorUtility.HasPro();

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = 15, wordWrap = true }; style.richText = true;
        GUIStyle styleInfo = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12, wordWrap = true }; styleInfo.richText = true;
        if (hasPro) { styleInfo.normal.textColor = new Color(.8f, .8f, .8f, 1); }
        else { styleInfo.normal.textColor = Color.black; }

        var stylePreset = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12, wordWrap = true }; stylePreset.richText = true;
        var styleButton = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, wordWrap = true, fixedWidth = 100, richText = true };
        var styleButtonMaster = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 20, wordWrap = true };
        var styleButtonClose = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 10, wordWrap = true };
        var styleButtonSub = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 10, wordWrap = true };

        styleButtonMaster.normal.textColor = new Color(.8f, .8f, .8f, 1);
        styleButtonMaster.normal.background = MakeTex(2, 2, buttonColor);
        styleButtonMaster.active.background = MakeTex(2, 2, SubColor);

        styleButton.normal.textColor = new Color(.8f, .8f, .8f, 1);
        styleButton.normal.background = MakeTex(2, 2, buttonColor);
        styleButton.active.background = MakeTex(2, 2, SubColor);

        styleButtonClose.normal.textColor = new Color(.8f, .8f, .8f, 1);
        styleButtonClose.normal.background = MakeTex(2, 2, buttonColor);
        styleButtonClose.active.background = MakeTex(2, 2, SubColor);

        styleButtonSub.normal.textColor = new Color(.8f, .8f, .8f, 1);
        styleButtonSub.normal.background = MakeTex(2, 2, SubColor);
        styleButtonSub.active.background = MakeTex(2, 2, buttonColor);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

        if (GUILayout.Button(EnabledButtonText, styleButtonMaster)) {
            if (active) {
                active = false;
                EnabledButtonText = "Disabled";
                buttonColor = new Color(150f / 255f, 79f / 255f, 44f / 255f, 1);
                SubColor = new Color(150f / 255f, 79f / 255f, 44f / 255f, 1);
                if (gizmo) { DestroyImmediate(gizmo); }
            }
            else {
                active = true;
                EnabledButtonText = "Enabled";
                buttonColor = new Color(43f / 255f, 122f / 255f, 44f / 255f, 1);
                SubColor = new Color(68f / 255f, 92f / 255f, 51f / 255f, 1);
            }
        }
        EditorGUILayout.Space();
        PlaceKey = (KeyCode)EditorGUILayout.EnumPopup("Placement Button", PlaceKey);
        EditorGUILayout.Space();

        //Section one
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Space(7);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("<color=white>General</color>", style);
        if (GUILayout.Button(textGeneral, styleButton)) {
            if (toggleGeneral) {
                toggleGeneral = false;
                textGeneral = "Open";
            } else {
                toggleGeneral = true;
                textGeneral = "Hide";
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (toggleGeneral) {
            GUILayout.BeginVertical("GroupBox");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(textMask, styleButtonSub)) {
                if (toggleMask) {
                    textMask = "Not Using Mask";
                    toggleMask = false;
                } else {
                    textMask = "Using Mask";
                    toggleMask = true;
                }
            }
            GUILayout.EndHorizontal();

            if (toggleMask) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Layer Mask", GUILayout.Width(100));
                LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(mask), InternalEditorUtility.layers);
                mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
                GUILayout.EndHorizontal();
            }


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(textAlignmentLock, styleButtonSub)) {
                if (alignToNormal) {
                    textAlignmentLock = "Dont Align To Normals";
                    alignToNormal = false;
                } else {
                    textAlignmentLock = "Align To Normals";
                    alignToNormal = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(textUseGrouping, styleButtonSub)) {
                if (useGroups) {
                    textUseGrouping = "Single";
                    useGroups = false;
                } else {
                    textUseGrouping = "Paint";
                    useGroups = true;
                }
            }

            EditorGUILayout.EndHorizontal();

            if (useGroups) {
                EditorGUILayout.Space();              
                EditorGUILayout.LabelField("Brush Size");
                Spread = EditorGUILayout.Slider(Spread, 0, 50);
                EditorGUILayout.LabelField("Brush Thickness");
                ObjectCount = (int)EditorGUILayout.Slider(ObjectCount, 0, 1000);
                EditorGUILayout.LabelField("Min Distance Between Objects");
                MinDistance = EditorGUILayout.Slider(MinDistance, 0, 50);           
                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Space(7);

        //Section two
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("<color=white>Rotation</color>", style);
        if (GUILayout.Button(textRotations, styleButton)) {
            if (toggleRotations) {
                toggleRotations = false;
                textRotations = "Open";
            } else {
                toggleRotations = true;
                textRotations = "Hide";
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (toggleRotations) {
            GUILayout.BeginVertical("GroupBox");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(textRotAbsolute, styleButtonSub)) {
                if (useAbsoluteValuesRot) {
                    textRotAbsolute = "Random Range";
                    useAbsoluteValuesRot = false;
                } else {
                    textRotAbsolute = "Absolute";
                    useAbsoluteValuesRot = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            if(alignToNormal) {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Normal Align Blend");
                NormalAlignBlend = EditorGUILayout.Slider(NormalAlignBlend, 0f, 1f);
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("X");
            xAmount = EditorGUILayout.Slider(xAmount, 0, 360);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Y");
            yAmount = EditorGUILayout.Slider(yAmount, 0, 360);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Z");
            zAmount = EditorGUILayout.Slider(zAmount, 0, 360);
            EditorGUILayout.Space();

            GUILayout.EndVertical();

        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Space(7);

        //Section three
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("<color=white>Scale</color>", style);
        if (GUILayout.Button(textScales, styleButton)) {
            if (toggleScales) {
                toggleScales = false;
                textScales = "Open";
            } else { toggleScales = true;
                textScales = "Hide";
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (toggleScales) {
            GUILayout.BeginVertical("GroupBox");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(textLockedScale, styleButtonSub)) {
                if (lockedScale) {
                    textLockedScale = "Three Axis";
                    lockedScale = false;
                } else {
                    textLockedScale = "Locked Axis";
                    lockedScale = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            if(lockedScale) {
                EditorGUILayout.LabelField("Scale Power");
                scaleAmountLocked = EditorGUILayout.Slider(scaleAmountLocked, 0f, 1f);
                EditorGUILayout.Space();
            } else {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(textScaleAbsolute, styleButtonSub)) {
                    if (useAbsoluteValuesScale) {
                        textScaleAbsolute = "Random Range";
                        useAbsoluteValuesScale = false;
                    } else {
                        textScaleAbsolute = "Absolute";
                        useAbsoluteValuesScale = true;
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("X");
                scaleAmount.x = EditorGUILayout.Slider(scaleAmount.x, 0f, 1f);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Y");
                scaleAmount.y = EditorGUILayout.Slider(scaleAmount.y, 0f, 1f);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Z");
                scaleAmount.z = EditorGUILayout.Slider(scaleAmount.z, 0f, 1f);
                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();
        }
        
        GUILayout.EndVertical();
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Space(7);

        //Section five
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("<color=white>Additional</color>", style);
        if (GUILayout.Button(textAdditional, styleButton)) {
            if (toggleAdditional) {
                toggleAdditional = false;
                textAdditional = "Open";
            } else {
                toggleAdditional = true;
                textAdditional = "Hide";
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (toggleAdditional) {
            GUILayout.BeginVertical("GroupBox");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Help", styleButtonMaster)) { Application.OpenURL("https://docs.google.com/document/d/1Cv4HPoDJeLHtl_zLEuAwZ2n3CkR7MmxpgDVWmRW1ahI/edit?usp=sharing"); }
            if (GUILayout.Button("Onerat Games", styleButtonMaster)) { Application.OpenURL("https://www.oneratgames.com/"); }

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.Box("Select a prefab in the project window to get started. Right mouse button + Placement Button to place an object.\n\n Version 1.2 \n\n Created by @oneratdylan ", styleInfo);
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        if (GUILayout.Button("EXIT WINDOW", styleButtonClose)) {
            active = false;
            this.Close();
        }
        EditorGUILayout.EndScrollView();     
    }

    private void Update() { SceneView.duringSceneGui += OnScene; }

    void OnDestroy () { active = false; }
    private KeyCode PlaceKey = KeyCode.C;
    private bool ready = false;

    void OnScene(SceneView scene) {
        if (!active) return;
        Event e = Event.current;
        Vector3 mousePos = e.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;
        Ray ray = scene.camera.ScreenPointToRay(mousePos);

        if (e.type == EventType.KeyDown && e.keyCode == PlaceKey) {
            ready = true;
        } else if (e.type == EventType.KeyUp && e.keyCode == PlaceKey) {
            ready = false;
            Onerat_Placement_ActionList.SceneGameObjects.Clear();
            if (gizmo) { DestroyImmediate(gizmo); }
        }

        if (ready) {
            if (e.type == EventType.MouseUp && e.button == 1) {
                Onerat_Placement_ActionList.SceneGameObjects.Clear();
            }
            if (useGroups) {
                Gizmo(ray);
                if (e.type == EventType.MouseDrag && e.button == 1) {
                    Place(ray);
                    e.Use();
                }
            }
            else {
                if (gizmo) { DestroyImmediate(gizmo); }
                if (e.type == EventType.MouseDown && e.button == 1) {
                    Place(ray);
                    e.Use();
                }
            }
        }
    }

    GameObject gizmo = null;
    void Gizmo(Ray ray) {
        RaycastHit gizmoHit;
        if (Physics.Raycast(ray, out gizmoHit, Mathf.Infinity)) {
            if (gizmo == null) {
                gizmo = Instantiate(Resources.Load("EasyPlacementGizmo") as GameObject);
            }
            gizmo.transform.position = gizmoHit.point;
            gizmo.transform.localScale = new Vector3(Spread * 2, Spread * 2, Spread * 2);
        }
        else {
            if (gizmo) { DestroyImmediate(gizmo); }
        }
    }

    void Place(Ray ray) {
        if(toggleMask) {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
                var startPos = hit.point;
                if(useGroups) {
                    foreach (var item in positions(hit.point)) { //groups
                        if (Selection.activeObject as GameObject) {
                            Onerat_Placement_ActionList.PlaceObject(
                                Selection.activeObject as GameObject,
                                item,
                                MinDistance,
                                useAbsoluteValuesRot,
                                new Vector3(xAmount, yAmount, xAmount),
                                useAbsoluteValuesScale,
                                lockedScale,
                                scaleAmount, 
                                scaleAmountLocked,
                                alignToNormal,
                                NormalAlignBlend, 
                                true, 
                                true,
                                mask);
                        }           
                    }
                } else {
                    Onerat_Placement_ActionList.PlaceObject(
                        Selection.activeObject as GameObject,
                        startPos,
                        MinDistance, 
                        useAbsoluteValuesRot,
                        new Vector3(xAmount, yAmount, xAmount),
                        useAbsoluteValuesScale,
                        lockedScale,
                        scaleAmount,
                        scaleAmountLocked,
                        alignToNormal,
                        NormalAlignBlend, 
                        true,
                        true,
                        mask);
                }
            }
        } else {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {               
                var startPos = hit.point;
                if(useGroups) {
                    foreach (var item in positions(hit.point)) { //groups
                        if (Selection.activeObject as GameObject) {
                            Onerat_Placement_ActionList.PlaceObject(
                                Selection.activeObject as GameObject,
                                item,
                                MinDistance,
                                useAbsoluteValuesRot,
                                new Vector3(xAmount, yAmount, xAmount),
                                useAbsoluteValuesScale, 
                                lockedScale,
                                scaleAmount, 
                                scaleAmountLocked, 
                                alignToNormal,
                                NormalAlignBlend, 
                                true, 
                                false);
                        }
                    }
                } else {
                    if (Selection.activeObject as GameObject) {
                        Onerat_Placement_ActionList.PlaceObject(
                            Selection.activeObject as GameObject,
                            startPos, 
                            MinDistance,
                            useAbsoluteValuesRot,
                            new Vector3(xAmount, yAmount, xAmount),
                            useAbsoluteValuesScale, 
                            lockedScale, 
                            scaleAmount,
                            scaleAmountLocked, 
                            alignToNormal,
                            NormalAlignBlend,
                            true, 
                            false);
                    }
                }                   
            }
        }
    }

    List<Vector3> positions(Vector3 hitPoint) {
        List<Vector3> toReturn = new List<Vector3>();
        for (int i = 0; i < ObjectCount; i++) {
            Vector3 temp = hit.point + new Vector3(Random.Range(-Spread, Spread),
                                                   0,
                                                   Random.Range(-Spread, Spread));
            if (!toCloseOrOutOfRange(temp, toReturn, MinDistance, hitPoint, Spread)) { toReturn.Add(temp); }
        }
        return toReturn;
    }

    bool toCloseOrOutOfRange(Vector3 position, List<Vector3> allPositions, float minDistance, Vector3 hitPoint, float Spread) {
        if(outOfRange(hitPoint, Spread, position)) { return true; }
        foreach (Vector3 item in allPositions) {
            if(Vector3.Distance(position, item) < minDistance) { return true; }
        }
        return false;
    }

    bool outOfRange(Vector3 center, float range, Vector3 input) {
        bool toReturn = false;
        float distance = Vector3.Distance(center, input);
        if(distance < range) { toReturn = false; }
        else { toReturn = true; }
        return toReturn;
    }

    RaycastHit hit;
}
#endif
