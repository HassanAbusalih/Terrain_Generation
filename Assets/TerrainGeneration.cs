using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] Texture2D terrainTexture;
    [SerializeField] int terrainX;
    [SerializeField] int terrainZ;
    [SerializeField] float maxHeight;
    [SerializeField] Transform cam;
    MeshFilter meshFilter;
    Mesh mesh;
    float stepSizeX;
    float stepSizeZ;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        GenerateTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTerrain();
        }
    }

    private void GenerateTerrain()
    {
        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();
        mesh.RecalculateNormals();
        cam.position = new(terrainX / 2, (terrainX + terrainZ) / 2, terrainZ / 2);
    }

    private Vector3[] GenerateVertices()
    {
        Vector3[] vertices = new Vector3[terrainX * terrainZ];
        stepSizeX = (float)terrainTexture.width / (terrainX - 1);
        stepSizeZ = (float)terrainTexture.height / (terrainZ - 1);
        for (int z = 0; z < terrainZ; z++)
        {
            for (int x = 0; x < terrainX; x++)
            {
                int textureX = Mathf.FloorToInt(x * stepSizeX);
                int textureZ = Mathf.FloorToInt(z * stepSizeZ);
                float y = terrainTexture.GetPixel(textureX, textureZ).r * maxHeight;
                Vector3 vertexPosition = new Vector3(x, y, z);
                vertices[z * terrainX + x] = vertexPosition;
            }
        }
        return vertices;
    }

    private int[] GenerateTriangles()
    {
        int[] triangles = new int[(terrainX - 1) * (terrainZ - 1) * 6];
        int triangleIndex = 0;
        for (int z = 0; z < terrainZ - 1; z++)
        {
            for (int x = 0; x < terrainX - 1; x++)
            {
                int vertexIndex = z * terrainX + x;
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + terrainX;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + terrainX;
                triangles[triangleIndex + 5] = vertexIndex + terrainX + 1;
                triangleIndex += 6;
            }
        }
        return triangles;
    }
}