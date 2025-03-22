using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelReloader : MonoBehaviour
{
    public List<string> levelPrefabPaths; // ���йؿ�Ԥ�����·���б�
    private GameObject currentLevelInstance; // ��ǰ����Ĺؿ�����ʵ��
    private string currentLevelPrefabPath; // ��ǰ�ؿ�Ԥ�����·��
    private float reloadCooldown = 1.0f; // ���¼��ص���ȴʱ��
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
}