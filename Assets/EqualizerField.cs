using UnityEngine;

public class EqualizerField : MonoBehaviour
{
	public AudioSpectrum audioSpectrum;
	public Material material;

	private float[,] heightMap = new float[128, 128];

	void Start ()
	{
		TerrainData terrainData = new TerrainData ();
		terrainData.size = new Vector3 (10, 50, 10);
		terrainData.heightmapResolution = 128;

		GetComponent<Terrain> ().terrainData = terrainData;
	}
	
	void Update ()
	{
		for (int h = 127; h >= 0; --h) {
			for (int w = 0; w < 128; w++) {
				if (h > 0) {
					heightMap [h, w] = heightMap [h - 1, w];
				} else {
					float b = w / 128.0f * (audioSpectrum.Levels.Length - 1);
					int ib = Mathf.FloorToInt (b);
					float pb = b - ib;
					heightMap [0, w] = Mathf.Clamp01(audioSpectrum.Levels[ib] * (1 - pb) + audioSpectrum.Levels[ib + 1] * pb);
				}
			}
		}
		
		GetComponent<Terrain> ().terrainData.SetHeights (0, 0, heightMap);
	}
}
