using UnityEngine;

public class SimpleSingleSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform prefab;
    [SerializeField]
    private Material emissiveMat;

    public int objectCount;
    public float groundLevel;
    public float levelRadius;

    public int minObjectWidth;
    public int maxObjectWidth;
    public int minObjectHeight;
    public int maxObjectHeight;

    private void Awake()
    {
        for (int i = 0; i < objectCount; i++)
        {
            Transform objectTr = Instantiate(prefab, transform);

            float width = Random.Range(minObjectWidth, maxObjectWidth), height = Random.Range(minObjectHeight, maxObjectHeight);
            objectTr.GetChild(0).localScale = new Vector3(width, height, width);
            objectTr.GetChild(0).localPosition = new Vector3(0f, height / 2f, 0f);
            objectTr.position = new Vector3(Random.Range(-levelRadius, levelRadius), groundLevel, Random.Range(-levelRadius, levelRadius));

            if (Random.value > .65f)
            {
                Renderer r = objectTr.GetChild(0).GetComponent<Renderer>();
                r.material = emissiveMat;
                r.UpdateGIMaterials();
            }
        }
    }
}
