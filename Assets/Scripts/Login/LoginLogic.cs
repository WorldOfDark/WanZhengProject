/*********************************************************************
文件名:            LoginLogic.cs
Unity版本：        #UNITYVERSION#
创建日期:          2022.2.10 11:42
文件描述:          用户登录
**********************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginLogic : MonoBehaviour
{
    public InputField userInput, pwdInput, authcodeInput;

    public Button authcodeImage, loginButton, resultClickButton;

    public Toggle rememberUser;

    public Text resultText;

    /// <summary>
    /// 登录结果提示框
    /// </summary>
    public RectTransform resulttipPanel;

    private Dictionary<string, string> loginDic = new Dictionary<string,string>();

    private AuthCodeData authcodeData;

    private UserData userData;

    void Start()
    {
        loginButton.onClick.AddListener(Login);

        authcodeImage.onClick.AddListener(GetAuthCode);

        resultClickButton.onClick.AddListener(() => { resulttipPanel.anchoredPosition = new Vector2(-(Screen.width + 1000), 0); });

        GetDefaultUser();

        GetAuthCode();
    }

    #region 登录
    void GetDefaultUser()
    {
        userInput.text = PlayerPrefs.GetString("username","admin");
        pwdInput.text = PlayerPrefs.GetString("password", "123456");
    }

    /// <summary>
    /// 登录事件
    /// </summary>
    private void Login()
    {
        if (rememberUser.isOn)
        {
            PlayerPrefs.SetString("userName", userInput.text);
            PlayerPrefs.SetString("password", pwdInput.text);
        }
        else
        {
            PlayerPrefs.DeleteKey("userName");
            PlayerPrefs.DeleteKey("password");
        }

        loginDic.Clear();
        loginDic.Add("username", userInput.text);
        loginDic.Add("password", GetEncryptedPassword(authcodeData));
        loginDic.Add("txtRand", authcodeInput.text);
        loginDic.Add("sessionToken", authcodeData.sessionToken);
        loginDic.Add("projectId", "74");
        loginDic.Add("projectUuid", "1e09c0ae-e5c7-4e5b-884f-a400128bfcd3");

        StartCoroutine(NetWorkManager.InternalPost(ProjectConfig.loginUrl, loginDic, "application/x-www-form-urlencoded",LoginCallBack));
    }

    /// <summary>
    /// 登录请求发送后各种情况对应的回调
    /// </summary>
    private void LoginCallBack(string receiveStr)
    {
        userData = JsonConvert.DeserializeObject<UserData>(receiveStr);
        if (userData.code == 1)
        {
            Debug.Log("登录成功");
            ProjectConfig.LoginToken = userData.data.loginToken;
            SceneManager.LoadScene(1);
        }
        else
        {
            resultText.text = "登录失败," + userData.message;
            resulttipPanel.anchoredPosition = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// 获取加密的密码
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private string GetEncryptedPassword(AuthCodeData info)
    {
        if (info == null)
        {
            return string.Empty;
        }

        string encryptedData = string.Empty;

        try
        {
            RSAParameters RSAKeyInfo = new RSAParameters();
            RSAKeyInfo.Modulus = HexStringToByteArray(info.pubmodulus);
            RSAKeyInfo.Exponent = HexStringToByteArray(info.pubexponent);

            UTF8Encoding ByteConverter = new UTF8Encoding();
            byte[] passwordInBytes = ByteConverter.GetBytes(pwdInput.text);
            byte[] data = RSAEncrypt(passwordInBytes, RSAKeyInfo, false);

            encryptedData = this.ByteArrayToHexString(data);
        }
        catch (Exception error)
        {
            Debug.Log(error.ToString());
        }

        return encryptedData;
    }
    #endregion

    #region 验证码
    /// <summary>
    /// 获取验证码
    /// </summary>
    private void GetAuthCode()
    {
        StartCoroutine(NetWorkManager.InternalGet(ProjectConfig.authcodeUrl,LoadAuthCodeImage));
    }

    /// <summary>
    /// 加载验证码图片
    /// </summary>
    private void LoadAuthCodeImage(string receiveStr)
    {
        authcodeData = JsonConvert.DeserializeObject<AuthCodeReceive>(receiveStr).data;
        string a = authcodeData.imageUrl.Split(',')[1];
        authcodeImage.GetComponent<RawImage>().texture = Base64ToTexture2d(a);
    }
    #endregion

    #region 工具类方法集合

    /// <summary>
    /// RSA加密
    /// </summary>
    /// <param name="DataToEncrypt"></param>
    /// <param name="RSAKeyInfo"></param>
    /// <param name="DoOAEPPadding"></param>
    /// <returns></returns>
    private byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
    {
        try
        {
            byte[] encryptedData;

            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
        //Catch and display a CryptographicException  
        //to the console.
        catch (CryptographicException error)
        {
            Debug.Log(error.Message);
            return null;
        }
    }

    /// <summary>
    /// 字节数据转换为16进制字符串
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    private string ByteArrayToHexString(byte[] array)
    {
        StringBuilder s = new StringBuilder();

        if (array != null)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++)
            {
                s.AppendFormat("{0:X2}", array[i]);
            }
        }

        return s.ToString();
    }

    /// <summary>
    /// 16进制字符串转换为字节数组
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private byte[] HexStringToByteArray(string hex)
    {
        int count = hex.Length;
        byte[] data = new byte[(count + 1) / 2];

        try
        {
            int index = 0;
            int shift = 0;

            if (count % 2 != 0)
            {
                data[0] = Convert.ToByte(hex.Substring(0, 1), 16);
                index = 1;
                shift = 1;
            }

            for (; index < data.Length; index++)
            {
                data[index] = Convert.ToByte(hex.Substring(index * 2 - shift, 2), 16);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        return data;
    }

    /// <summary>
    /// 字节转换为图片
    /// </summary>
    /// <param name="Base64STR">字节数据</param>
    /// <returns></returns>
    private Texture2D Base64ToTexture2d(string Base64STR)
    {
        Texture2D pic = new Texture2D(200, 200);
        byte[] data = System.Convert.FromBase64String(Base64STR);
        pic.LoadImage(data);

        return pic;
    }

    #endregion
}

#region 网络请求数据类
/// <summary>
/// 验证码请求返回数据
/// </summary>
public class AuthCodeReceive
{
    public string code;
    public string message;
    public AuthCodeData data;
}

/// <summary>
/// 验证码数据
/// </summary>
public class AuthCodeData
{
    public string imageUrl;
    public string sessionToken;
    public string pubexponent;
    public string pubmodulus;
}

/// <summary>
/// 登录请求返回数据
/// </summary>
public class UserData
{
    public class ITEMS
    {
        /// <summary>
        /// 
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string loginName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passwd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int roleId { get; set; }
        /// <summary>
        /// 项目总经理
        /// </summary>
        public string roleStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string newpwd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string newpwdconfirm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public List<RoleListItem> roleList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string menuList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public List<MenuVueListItem> menuVueList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string adminFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string roleFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string roleDescr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string picPreffix { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string loginName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isUsed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createdTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string roleUuid { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string loginToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userIcon { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public Data data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 登录成功
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ITEMS ITEMs { get; set; }
}
#endregion
