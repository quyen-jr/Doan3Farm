using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; // Bắt buộc phải có để dùng ContinueWithOnMainThread
using UnityEngine.SceneManagement;
using FairyField.Logic;
public class Login : MonoBehaviour
{
    [SerializeField] private InputField username; // Giờ sẽ đóng vai trò là Email
    [SerializeField] private InputField password;
    [SerializeField] private Button submitBtn;
    [SerializeField] private Button registerBtn;
    [SerializeField] private PlayerRefScriptableObject playerRef;
    private bool _showPassword = false;
    
    // Biến quản lý Firebase
    private FirebaseAuth auth;
    private bool isFirebaseReady = false;

    void Start()
    {
        registerBtn.onClick.AddListener(() => { UILoginManager.Instance.CreatePanel(1); });
        submitBtn.onClick.AddListener(() => { LoginAccount(); });

        // Khởi tạo Firebase ban đầu
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                isFirebaseReady = true;
            }
            else
            {
                Debug.LogError($"Không thể khởi tạo Firebase: {dependencyStatus}");
                UILoginManager.Instance.CreateNotification("Lỗi kết nối dịch vụ Firebase!");
            }
        });
    }

    public void LoginAccount()
    {
        // Nếu Firebase chưa khởi tạo xong thì không cho bấm
        if (!isFirebaseReady) return;

        // Khóa tương tác nút bấm để tránh người chơi spam click liên tục
        registerBtn.interactable = false;
        submitBtn.interactable = false;

        string emailText = username.text;
        string passwordText = password.text;

        // Tiến hành gọi API đăng nhập bất đồng bộ của Firebase
        auth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(task => {

            // Xử lý khi đăng nhập THẤT BẠI (Sai tài khoản, mật khẩu, lỗi mạng...)
            if (task.IsCanceled || task.IsFaulted)
            {
                System.AggregateException ex = task.Exception as System.AggregateException;
                if (ex != null)
                {
                    foreach (var innerEx in ex.Flatten().InnerExceptions)
                    {
                        Debug.LogError($"[LỖI FIREBASE]: {innerEx.Message}");
                    }
                }

                UILoginManager.Instance.CreateNotification("Tài khoản hoặc mật khẩu không chính xác");

                // Mở lại tương tác cho các nút bấm để người chơi nhập lại
                registerBtn.interactable = true;
                submitBtn.interactable = true;
                return;
            }

            // Xử lý khi đăng nhập THÀNH CÔNG
            AuthResult result = task.Result;
            FirebaseUser user = result.User;
            Debug.Log($"Đăng nhập thành công tài khoản: {user.Email}");

            // Lưu tên người dùng vào ScriptableObject
            playerRef.playerName = username.text;
            UserData.instance.SetUsername( username.text );

            // Bỏ qua bước kiểm tra EmailVerified -> Chuyển thẳng vào game luôn
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        });
    }
    public void TogglePassword()
    {
        _showPassword = !_showPassword;
        password.contentType = _showPassword ? InputField.ContentType.Standard : InputField.ContentType.Password;
        password.ActivateInputField();
    }
}