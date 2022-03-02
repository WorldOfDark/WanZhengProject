using UnityEngine;

public class Util
{
    #region 图片和字节相互转换
    /// <summary>
    /// Sprite转byte数组
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public static byte[] SpriteToByte(Sprite sprite)
    {
        Texture2D temp = sprite.texture;
        byte[] photoByte = temp.EncodeToPNG();
        return photoByte;
    }

    /// <summary>
    /// 字节转换Sprite
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static Sprite ByteToSprite(byte[] bytes)
    {
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        return sprite;

    }

    public static Sprite ByteToSprite(byte[] bytes, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sprite;

    }

    public static Texture2D ByteToTexture2D(byte[] bytes)
    {
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes);
        return texture;
    }

    public static Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }

    public static byte[] Texture2DToByte(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        return bytes;
    }

    public static byte[] TextureToByte(Texture texture)
    {
        byte[] bytes = TextureToTexture2D(texture).EncodeToPNG();
        return bytes;
    }
    #endregion

    #region 截图
    /// <summary>
    /// 对相机截图。 
    /// </summary>
    /// <returns>The screenshot2.</returns>
    /// <param name="camera">Camera.要被截屏的相机</param>
    /// <param name="rect">Rect.截屏的区域</param>
    public static Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        // 创建一个RenderTexture对象
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.targetTexture = rt;
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
        //ps: camera2.targetTexture = rt;
        //ps: camera2.Render();
        //ps: -------------------------------------------------------------------

        // 激活这个rt, 并从中中读取像素。
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件
        byte[] bytes = screenShot.EncodeToPNG();
        //picture = bytes;
        string filename = Application.dataPath + "/Screenshot.png";
        //string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张照片: {0}", filename));

        return screenShot;
    }
    #endregion
}