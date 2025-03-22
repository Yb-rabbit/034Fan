using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelReloader : MonoBehaviour
{
    public List<string> levelPrefabPaths; // 所有关卡预制体的路径列表
    private GameObject currentLevelInstance; // 当前激活的关卡物体实例
    private string currentLevelPrefabPath; // 当前关卡预制体的路径
    private float reloadCooldown = 1.0f; // 重新加载的冷却时间
    private float lastReloadTime = 0.0f; // 上次重新加载的时间

    private void Start()
    {
        // 初始化时加载第一个关卡物体
        if (levelPrefabPaths.Count > 0)
        {
            LoadLevel(levelPrefabPaths[0]); // 加载第一个关卡
        }
        else
        {
            Debug.LogError("No level prefab paths provided.");
        }
    }

    private void Update()
    {
        // 检测 Tab 键按下
        if (Input.GetKeyDown(KeyCode.Tab) && Time.time - lastReloadTime > reloadCooldown)
        {
            ReloadLevel();
            lastReloadTime = Time.time; // 更新上次重新加载的时间
        }
    }

    // 加载关卡物体
    private void LoadLevel(string prefabPath)
    {
        // 从 Resources 文件夹中加载预制体
        GameObject levelPrefab = Resources.Load<GameObject>(prefabPath);
        if (levelPrefab != null)
        {
            if (currentLevelInstance != null)
            {
                Destroy(currentLevelInstance); // 销毁当前关卡物体
            }
            currentLevelInstance = Instantiate(levelPrefab);
            currentLevelPrefabPath = prefabPath; // 更新当前关卡预制体路径
        }
        else
        {
            Debug.LogError($"Prefab not found: {prefabPath}");
        }
    }

    // 重新加载关卡物体
    private void ReloadLevel()
    {
        if (!string.IsNullOrEmpty(currentLevelPrefabPath))
        {
            LoadLevel(currentLevelPrefabPath); // 使用当前关卡预制体路径重新加载
        }
        else
        {
            Debug.LogError("No active level to reload.");
        }
    }
}