using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FlowerBrushEditor : EditorWindow
{
    public GameObject flowerPrefab;
    public float brushSize = 5f;
    public int density = 5;
    
    private bool isPainting = false;

    [MenuItem("Tools/Flower Brush")]
    public static void ShowWindow()
    {
        GetWindow<FlowerBrushEditor>("Flower Brush");
    }

    void OnGUI()
    {
        GUILayout.Label("Flower Brush Settings", EditorStyles.boldLabel);
        flowerPrefab = (GameObject)EditorGUILayout.ObjectField("Flower Prefab", flowerPrefab, typeof(GameObject), false);
        brushSize = EditorGUILayout.Slider("Brush Size", brushSize, 1f, 20f);
        density = EditorGUILayout.IntSlider("Density", density, 1, 20);

        if (GUILayout.Button(isPainting ? "Stop Painting" : "Start Painting"))
        {
            isPainting = !isPainting;
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!isPainting || flowerPrefab == null) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Handles.color = new Color(0, 1, 0, 0.3f);
            Handles.DrawSolidDisc(hit.point, Vector3.up, brushSize);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                PlaceFlowers(hit.point);
                e.Use();
            }
        }

        sceneView.Repaint();
    }

    void PlaceFlowers(Vector3 center)
    {
        for (int i = 0; i < density; i++)
        {
            Vector3 randomPos = center + new Vector3(Random.Range(-brushSize, brushSize), 0, Random.Range(-brushSize, brushSize));
            RaycastHit hit;
            if (Physics.Raycast(randomPos + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity))
            {
                GameObject flower = (GameObject)PrefabUtility.InstantiatePrefab(flowerPrefab);
                flower.transform.position = hit.point;
                flower.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Undo.RegisterCreatedObjectUndo(flower, "Paint Flower");
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
