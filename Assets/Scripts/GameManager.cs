using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager> {
    public PandaScript PandaPrefab;
    public PandaHpBar PandaHpBarPrefab;

    public GameObject EnemyContainer;
    public GameObject CupcakeTowerContainer;

    public Waypoint FirstWaypoint;

    public Button LosingScreenButton;
    public Button WinningScreenButton;

    public MeshFilter MeshFilter;
    private Mesh _mesh;

    public GameObject SpawnPoint;
    public int numberOfWaves;
    public int numberOfPandasPerWave;

    //private HealthbarScript _healthbarScript;
    private HUDManager _hudManager;

    private bool _isPointerOnAllowedArea = false;
    private int numberOfPandasToDefeat;
    private float waveStep;

    void Start () {
        Time.timeScale = 1f;
        
        _hudManager = HUDManager.Inst;

        //if (_healthbarScript == null)
        //{
        //    _healthbarScript = FindObjectOfType<HealthbarScript>();
        //}

        _mesh = new Mesh();
        _mesh.name = "_mesh";
        MeshFilter.mesh = _mesh;

        LosingScreenButton.gameObject.SetActive(false);
        WinningScreenButton.gameObject.SetActive(false);

        PlayerManager.Inst.Init(Values.Inst.player_init_sugar, Values.Inst.player_init_health);

        StartCoroutine(WaveSpawner());
    }
	
	void Update () {
    }

    private IEnumerator WaveSpawner()
    {
        waveStep = 1f;

        for (int i = 0; i < numberOfWaves; i++)
        {
            yield return PandaSpawner();
        }

        GameOver(true);
    }

    private IEnumerator PandaSpawner()
    {
        numberOfPandasToDefeat = (int)(numberOfPandasPerWave * waveStep);

        for (int i = 0; i < numberOfPandasPerWave * waveStep; i++)
        {
            PandaScript panda = Instantiate<PandaScript>(PandaPrefab, SpawnPoint.transform.position, Quaternion.identity, EnemyContainer.transform);
            panda.PandaHpBar = Instantiate<PandaHpBar>(PandaHpBarPrefab, _hudManager.GaugeContainer.transform);

            float ratio = (float)(numberOfPandasPerWave - i) / numberOfPandasPerWave;
            float timeToWait = Mathf.Lerp(0.3f, 1.5f, ratio);

            yield return new WaitForSeconds(timeToWait);
        }

        yield return new WaitUntil(() => numberOfPandasToDefeat <= 0);

        waveStep += Values.Inst.game_wave_weight;

        yield return new WaitForSeconds(2f);
    }

    public void DrawAllowedArea(bool isDraw)
    {
        _mesh.Clear();

        if (isDraw)
        {
            // Cupcake 배치 가능 영역 그리기
            BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
            Vector3[] vertices = new Vector3[colliders.Length * 4];
            int[] triangles = new int[colliders.Length * 6];
            for (int i = 0; i < colliders.Length; i++)
            {
                int vertexIndex = i * 4;
                int triangleIndex = i * 6;
                BoxCollider2D collider = colliders[i];

                float halfSizeX = collider.size.x / 2;
                float halfSizeY = collider.size.y / 2;

                vertices[vertexIndex + 0] = new Vector3(collider.offset.x - halfSizeX, collider.offset.y - halfSizeY, -1);
                vertices[vertexIndex + 1] = new Vector3(collider.offset.x - halfSizeX, collider.offset.y + halfSizeY, -1);
                vertices[vertexIndex + 2] = new Vector3(collider.offset.x + halfSizeX, collider.offset.y + halfSizeY, -1);
                vertices[vertexIndex + 3] = new Vector3(collider.offset.x + halfSizeX, collider.offset.y - halfSizeY, -1);

                triangles[triangleIndex + 0] = vertexIndex + 0;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + 3;
                triangles[triangleIndex + 3] = vertexIndex + 3;
                triangles[triangleIndex + 4] = vertexIndex + 1;
                triangles[triangleIndex + 5] = vertexIndex + 2;
            }

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }
    }

    public bool IsPointerOnAllowedArea()
    {
        return _isPointerOnAllowedArea;
    }

    public void OneMorePandaInHaven()
    {
        numberOfPandasToDefeat--;
    }

    public void BiteTheCake(int damage)
    {
        bool isCakeAllEaten = PlayerManager.Inst.ApplyDamage(damage);
        //bool isCakeAllEaten = _hudManager.HealthbarScript.ApplyDamage(damage);

        if (isCakeAllEaten)
        {
            GameOver(false);
        }

        OneMorePandaInHaven();
    }

    private void OnMouseEnter()
    {
        _isPointerOnAllowedArea = true;
    }

    private void OnMouseExit()
    {
        _isPointerOnAllowedArea = false;
    }

    public void GameOver(bool playerHasWon)
    {
        if (playerHasWon)
        {
            WinningScreenButton.gameObject.SetActive(true);
        } else
        {
            LosingScreenButton.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;

        StopAllCoroutines();
    }

    public void OnLoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
