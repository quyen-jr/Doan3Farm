using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Nếu muốn hiển thị thanh tải

public class SceneLoader : Singleton<SceneLoader>
{
    public GameObject loadingScreen; // Đối tượng UI cho màn hình tải
    public Slider progressBar; // Thanh tiến trình (nếu cần)

    // Hàm gọi để tải cảnh bất đồng bộ
    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    // Coroutine thực hiện quá trình tải cảnh
    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        // Bật màn hình tải (nếu có)
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Bắt đầu tải cảnh bất đồng bộ
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        // Kiểm tra quá trình tải và cập nhật thanh tiến trình (nếu cần)
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Tính tiến độ tải

            // Cập nhật thanh tiến trình nếu có
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            // Kiểm tra nếu tải xong (100% tiến độ)
            if (operation.progress >= 0.9f)
            {
                // Tùy chọn: Chờ người dùng xác nhận trước khi chuyển cảnh
                // Hoặc tự động kích hoạt cảnh mới sau khi tải xong
                operation.allowSceneActivation = true;
            }

            yield return null; // Chờ một frame
        }

        // Ẩn màn hình tải sau khi hoàn tất
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
}
