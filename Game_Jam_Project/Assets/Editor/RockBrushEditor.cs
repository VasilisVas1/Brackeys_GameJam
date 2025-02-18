using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RockBrushEditor : EditorWindow
{
    public GameObject rockPrefab;
    public float brushSize = 5f;
    public int density = 5;
    
    private bool isPainting = false;

    [MenuItem("Tools/Rock Brush")]
    public static void ShowWindow()
    {
        GetWindow<RockBrushEditor>("Rock Brush");
    }

    void OnGUI()
    {
        GUILayout.Label("Rock Brush Settings", EditorStyles.boldLabel);
        rockPrefab = (GameObject)EditorGUILayout.ObjectField("Rock Prefab", rockPrefab, typeof(GameObject), false);
        brushSize = EditorGUILayout.Slider("Brush Size", brushSize, 0.1f, 20f);
        density = EditorGUILayout.IntSlider("Density", density, 1, 20);

        if (GUILayout.Button(isPainting ? "Stop Painting" : "Start Painting"))
        {
            isPainting = !isPainting;
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!isPainting || rockPrefab == null) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Handles.color = new Color(1, 0, 0, 0.3f);
            Handles.DrawSolidDisc(hit.point, hit.normal, brushSize);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                PlaceRocks(hit.point, hit.normal);
                e.Use();
            }
        }

        sceneView.Repaint();
    }

    void PlaceRocks(Vector3 center, Vector3 normal)
    {
        for (int i = 0; i < density; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-brushSize, brushSize), Random.Range(-brushSize, brushSize), Random.Range(-brushSize, brushSize));
            Vector3 spawnPoint = center + randomOffset;
            
            RaycastHit hit;
            if (Physics.Raycast(spawnPoint + normal * 10, -normal, out hit, 20f))
            {
                GameObject rock = (GameObject)PrefabUtility.InstantiatePrefab(rockPrefab);
                rock.transform.position = hit.point;
                rock.transform.rotation = Quaternion.LookRotation(hit.normal);
                Undo.RegisterCreatedObjectUndo(rock, "Paint Rock");
            }
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
