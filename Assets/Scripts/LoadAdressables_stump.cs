using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAdressables_stump : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> _handle;

    private async void Start()
    {
        // ロード & インスタンス化
        _handle = Addressables.InstantiateAsync("MainGame_stump", Vector3.zero, Quaternion.identity);
        await _handle.Task;
    }

    private void OnDestroy()
    {
        // 解放
        Addressables.ReleaseInstance(_handle);
    }
}
