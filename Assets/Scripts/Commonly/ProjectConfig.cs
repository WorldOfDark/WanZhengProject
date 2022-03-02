using System.Collections.Generic;
using UnityEngine;

public class ProjectConfig
{
    #region 项目属性
    internal static UserData userData;//用户数据

    internal static string LoginToken;//客户端令牌

    public static string projectUUID = "1e09c0ae-e5c7-4e5b-884f-a400128bfcd3";

    public static string projectID = "1";
    #endregion

    #region AB包加载相关
    public static string serverPath = "http://192.168.1.107:80/StandaloneWindows/";

    public static string localPath = Application.persistentDataPath + "/" + Application.productName + "/";

    public static string versionTxt = "version.txt";

    public static string layerTxt = "layer.json";

    public static string abMainfest = "AssetBundles";

    public static string nowVersion = "1.0.0";

    public static string nowModelVersion = "1.0.0";

    public static float updateProgress = 0.0f;

    public static float nowDownloadSize = 0.0f;

    public static int nowDownloadIndex = 0;  //当前正在下载的AB资源序号

    public static float nowDownloadProgress = 0.0f; //当前正在下载的AB资源进度

    public static bool modelLoadEnded = false;

    public static List<string> assetBundleList = new List<string>();
    #endregion

    #region 接口
    /// <summary>
    /// 登录接口
    /// </summary>
    public static string loginUrl = "http://192.168.1.211:9035/api/v1/login/unityLogin";

    /// <summary>
    /// 验证码接口
    /// </summary>
    public static string authcodeUrl = "http://192.168.1.211:9035/api/v1/login/verifyCode";

    /// <summary>
    /// 文件Md5值校验
    /// </summary>
    public static string md5FileCheckUrl = "http://192.168.1.211:9035/api/v1/file/check/{0}";

    /// <summary>
    /// 人员列表接口
    /// </summary>
    public static string personalUrl = "http://192.168.1.211:9035/projCon/getProjByIdVue";

    /// <summary>
    /// 协同视角删除接口
    /// </summary>
    public static string deletViewUrl = "http://192.168.1.211:9035/unity/bim/location/delete";

    /// <summary>
    /// 维保清单删除接口
    /// </summary>
    public static string deleteMaintenanceUrl = "http://192.168.1.211:9035/api/v1/project/1e09c0ae-e5c7-4e5b-884f-a400128bfcd3/modelLocation/{0}";

    /// <summary>
    /// 人员列表接口
    /// </summary>
    public static string personListUrl = "http://192.168.1.211:9035/api/v1/user/list";

    /// <summary>
    /// 维保清单列表接口
    /// </summary>
    public static string maintenanceListUrl = "http://192.168.1.211:9035/api/v1/project/1e09c0ae-e5c7-4e5b-884f-a400128bfcd3/modelLocation/page?pageNo={0}&pageSize={1}";

    /// <summary>
    /// 项目列表接口
    /// </summary>
    public static string projectListUrl = "http://192.168.1.211:9035/api/v1/project/list";

    /// <summary>
    /// 新增视角的接口
    /// </summary>
    public static string addViewUrl = "http://192.168.1.211:9035/api/v1/project/1e09c0ae-e5c7-4e5b-884f-a400128bfcd3/modelLocation/{0}";

    /// <summary>
    /// 视角节点更新接口
    /// </summary>
    public static string updateViewUrl = "http://192.168.1.211:9035/api/v1/project/1e09c0ae-e5c7-4e5b-884f-a400128bfcd3/modelLocation/{0}/beforeImg";
    #endregion

    #region 光照
    public static Transform sunTrans;
    #endregion
}
