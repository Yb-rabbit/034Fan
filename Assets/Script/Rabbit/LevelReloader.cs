using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelReloader : MonoBehaviour
{
    public List<string> levelPrefabPaths = new List<string>
    {
        "Levels/Tutorial"
    };
    private GameObject currentLevelInstance; // ��ǰ����Ĺؿ�����ʵ��
    private string currentLevelPrefabPath; // ��ǰ�ؿ�Ԥ�����·��
    public float reloadCooldown = 1.0f; // ���¼��ص���ȴʱ��
    private float lastReloadTime = 0.0f; // �ϴ����¼��ص�ʱ��

    private void Start()
    {
        // ��ʼ��ʱ���ص�һ���ؿ�����
        if (levelPrefabPaths.Count > 0)
        {
            LoadLevel(levelPrefabPaths[0]); // ���ص�һ���ؿ�
        }
        else
        {
            Debug.LogError("No level prefab paths provided.");
        }
    }

    private void Update()
    {
        // ��� Tab ������
        if (Input.GetKeyDown(KeyCode.Tab) && Time.time - lastReloadTime > reloadCooldown)
        {
            ReloadLevel();
            lastReloadTime = Time.time; // �����ϴ����¼��ص�ʱ��
        }

        // ��⵱ǰ�ؿ��Ƿ����
        if (IsCurrentLevelCompleted())
        {
            LoadNextLevel();
        }
    }

    // ���عؿ�����
    private void LoadLevel(string prefabPath)
    {
        // �� Resources �ļ����м���Ԥ����
        GameObject levelPrefab = Resources.Load<GameObject>(prefabPath);
        if (levelPrefab != null)
        {
            if (currentLevelInstance != null)
            {
                Destroy(currentLevelInstance); // ���ٵ�ǰ�ؿ�����
            }
            currentLevelInstance = Instantiate(levelPrefab);
            currentLevelPrefabPath = prefabPath; // ���µ�ǰ�ؿ�Ԥ����·��
        }
        else
        {
            Debug.LogError($"Prefab not found: {prefabPath}");
        }
    }

    // �����������عؿ�����
    private void LoadLevelByIndex(int index)
    {
        if (index >= 0 && index < levelPrefabPaths.Count)
        {
            LoadLevel(levelPrefabPaths[index]);
        }
        else
        {
            Debug.LogError("Invalid level index.");
        }
    }

    // ���¼��عؿ�����
    private void ReloadLevel()
    {
        if (!string.IsNullOrEmpty(currentLevelPrefabPath))
        {
            LoadLevel(currentLevelPrefabPath); // ʹ�õ�ǰ�ؿ�Ԥ����·�����¼���
        }
        else
        {
            Debug.LogError("No active level to reload.");
        }
    }

    // ��鵱ǰ�ؿ��Ƿ����
    private bool IsCurrentLevelCompleted()
    {
        // ������Ը��ݾ������Ϸ�߼����жϹؿ��Ƿ����
        // ���磬���ĳ�������Ƿ����㣬����ĳ�������Ƿ�����
        // �������ؿ���ɵ������ǵ�ǰ�ؿ�ʵ��������
        return currentLevelInstance != null && !currentLevelInstance.activeSelf;
    }

    // ������һ���ؿ�
    private void LoadNextLevel()
    {
        int currentIndex = levelPrefabPaths.IndexOf(currentLevelPrefabPath);
        int nextIndex = (currentIndex + 1) % levelPrefabPaths.Count;
        LoadLevelByIndex(nextIndex);
    }
}
