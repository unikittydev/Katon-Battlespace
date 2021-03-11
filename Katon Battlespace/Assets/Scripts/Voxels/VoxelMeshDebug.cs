using UnityEngine;

namespace Game.Voxels
{
    public class VoxelMeshDebug : MonoBehaviour
    {
        public VoxelWorldBuilder builder;
        public VoxelWorldComponent component;

        private bool updateMesh;

        private bool destructWorld;

        public float fpsUpdateInterval = 1f;
        private float fpsUpdateCounter = 1f;

        private float averageTime;

        private int framesPassed;
        private float totalTime;

        private void Awake()
        {

        }

        private void OnGUI()
        {
            if (GUILayout.Button(new GUIContent(updateMesh ? "Stop mesh update" : "Update mesh"), new GUIStyle(GUI.skin.button) { fontSize = 10 }, GUILayout.Width(100f), GUILayout.Height(40f)))
            {
                updateMesh = !updateMesh;
            }
            if (GUILayout.Button(new GUIContent(destructWorld ? "Stop destruction" : "Destruct world"), new GUIStyle(GUI.skin.button) { fontSize = 10 }, GUILayout.Width(100f), GUILayout.Height(40f)))
            {
                destructWorld = !destructWorld;
            }
            GUILayout.Label(string.Format("Avg. time: {0:0.00} FPS", 1f / averageTime), new GUIStyle(GUI.skin.label) { fontSize = 10 });
            GUILayout.Label(string.Format("Avg. time: {0:0.00} ms.", averageTime * 1000f), new GUIStyle(GUI.skin.label) { fontSize = 10 });
        }

        private void Update()
        {
            if (updateMesh)
            {
                //component.TestGenerate();
            }

            totalTime += Time.deltaTime;
            framesPassed++;

            fpsUpdateCounter += Time.deltaTime;
            if (fpsUpdateCounter > fpsUpdateInterval)
            {
                averageTime = totalTime / framesPassed;
                totalTime = 0f;
                framesPassed = 0;
                fpsUpdateCounter -= fpsUpdateInterval;
            }
        }

        private void FixedUpdate()
        {
            if (destructWorld)
                VoxelDestruction.DestructWorld(component);
        }
    }
}
