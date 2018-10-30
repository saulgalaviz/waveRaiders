using UnityEngine;
using System.Collections.Generic;

public class Wave : MonoBehaviour
{
    [System.Serializable]
    public class WaveHog
    {
        public float initialPosition;
        public float maxheight;
        public float velocity;
        public float wavelength;
        public float timer;
        public bool valid;
    }

    public Transform firstPosition;
    public Transform lastPosition;
    public int numPoints;
    public LineRenderer lineRenderer;
    public LineRenderer belowRenderer;
    public LineRenderer bottomRenderer;
    public EdgeCollider2D edgeHog;
    public WaveHog[] globalWaves;
    public float randomWaveTimerMax;
    public float randomWaveTimerVariance;
    public WaveHog[] randomWaves;

    private float stepWidth;
    private float borderLeft;
    private float borderRight;
    private float randomWaveTimerTempMax;
    private float randomWaveTimer;

    private float[] waterHeights;
    private List<WaveHog> waveHogs;
    private Vector2[] sonicEdge;

	public GameObject splashPart;

    //y(x,t) = A*sin(2*pi/wavelength*(x-v*t)-phi)

    // Use this for initialization
    void Start ()
    {
        stepWidth = (lastPosition.position.x - firstPosition.position.x) / numPoints;
        lineRenderer.positionCount = numPoints;
        belowRenderer.positionCount = numPoints;
        bottomRenderer.positionCount = numPoints;
        waterHeights = new float[numPoints];
        edgeHog.points = new Vector2[numPoints];
        waveHogs = new List<WaveHog>();
        for (int i = 0; i < numPoints; ++i)
        {
            waterHeights[i] = 0;
            edgeHog.points[i] = new Vector2(firstPosition.position.x + i * stepWidth, 0);
        }
        randomWaveTimer = 0;
        randomWaveTimerTempMax = randomWaveTimerMax + Random.Range(0, randomWaveTimerVariance);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (randomWaveTimer < randomWaveTimerTempMax)
        {
            randomWaveTimer += Time.deltaTime;
        }
        else
        {
            if (randomWaves.Length > 0)
            {
                WaveHog wave = randomWaves[Random.Range(0, randomWaves.Length)];
                AddWave(wave.initialPosition, wave.maxheight, wave.velocity, wave.wavelength);
            }
            randomWaveTimerTempMax = randomWaveTimerMax + Random.Range(0, randomWaveTimerVariance);
            randomWaveTimer = 0;
        }
		sonicEdge = edgeHog.points;
		for (int i = 0; i < numPoints; ++i)
        {
            waterHeights[i] = 0;
        }
        foreach (WaveHog waveHog in waveHogs)
        {
            waveHog.valid = false;
        }
        for (int i = 0; i < numPoints; ++i)
        {
            foreach (WaveHog globalWave in globalWaves)
            {
                waterHeights[i] += globalWave.maxheight * Mathf.Cos(2 * Mathf.PI / globalWave.wavelength * (i * stepWidth - globalWave.velocity * globalWave.timer) + 2 * Mathf.PI / globalWave.wavelength * globalWave.initialPosition) + globalWave.maxheight;
            }
            foreach (WaveHog waveHog in waveHogs)
            {
                if (i * stepWidth > waveHog.initialPosition + waveHog.velocity * waveHog.timer - (waveHog.wavelength / 2) && i * stepWidth < waveHog.initialPosition + waveHog.velocity * waveHog.timer + (waveHog.wavelength / 2))
                {
                    waveHog.valid = true;
                    waterHeights[i] += waveHog.maxheight * Mathf.Cos(2 * Mathf.PI / waveHog.wavelength * (i * stepWidth - waveHog.velocity * waveHog.timer) + 2 * Mathf.PI / waveHog.wavelength * waveHog.initialPosition) + waveHog.maxheight;
                }
            }
            lineRenderer.SetPosition(i, new Vector3(firstPosition.position.x + i * stepWidth, waterHeights[i]));
            belowRenderer.SetPosition(i, new Vector3(firstPosition.position.x + i * stepWidth, waterHeights[i] - lineRenderer.startWidth / 2  - belowRenderer.startWidth / 2, 1));
            bottomRenderer.SetPosition(i, new Vector3(firstPosition.position.x + i * stepWidth, waterHeights[i] - lineRenderer.startWidth / 2 - belowRenderer.startWidth - bottomRenderer.startWidth / 2, 2));
            sonicEdge[i] = new Vector2 (firstPosition.position.x + i * stepWidth, waterHeights[i] + lineRenderer.startWidth / 2);
        }
        foreach (WaveHog globalWave in globalWaves)
        {
            globalWave.timer += Time.deltaTime;
        }
        for (int i = waveHogs.Count - 1; i >= 0; --i)
        {
            if (!waveHogs[i].valid)
            {
                waveHogs.RemoveAt(i);
            }
            else
            {
                waveHogs[i].timer += Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        edgeHog.points = sonicEdge;
    }

    public void AddWave(float initialPosition, float maxheight, float velocity, float wavelength)
    {
        WaveHog waveHog = new WaveHog();
        waveHog.initialPosition = initialPosition;
        waveHog.maxheight = maxheight;
        waveHog.velocity = velocity;
        waveHog.wavelength = wavelength;
        waveHog.timer = 0;
        waveHog.valid = true;
        waveHogs.Add(waveHog);
    }

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.CompareTag ("CannonBall") && splashPart != null) {
			Instantiate (splashPart, col.gameObject.transform.position, splashPart.transform.rotation);
			if (col.gameObject.GetComponent<Torpedo> () == null) {
				Destroy (col.gameObject);
			}
		}
	}
}
