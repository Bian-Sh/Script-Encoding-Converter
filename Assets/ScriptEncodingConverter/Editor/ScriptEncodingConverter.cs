using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
//BOM(Byte Order Mark)

public class ScriptEncodingConverter : MonoBehaviour
{
    const string key = "SEC_AutoFix";
    const string menu_to_utf8 = "Assets/Script Encoding Converter/To UTF8";
    const string menu_to_gb2312 = "Assets/Script Encoding Converter/To GB2312";
    const string menu_auto = "Assets/Script Encoding Converter/Auto Fix";
    /// <summary> 将脚本编码格式转换为 UTF8 </summary>
    [MenuItem(menu_to_utf8)]
    static void Convert2UTF8()
    {
        var settings = new ConvertSettings
        {
            predicate = x =>!DetectFileEncoding(x),
            from = Encoding.GetEncoding(936),
            to = Encoding.UTF8,
        };
        EncodingConverter(settings);
    }

    public static bool DetectFileEncoding(string file)
    {
        var Utf8EncodingVerifier = Encoding.GetEncoding("utf-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());
        using (var reader = new StreamReader(file, Utf8EncodingVerifier,true, 1024))
        {
            string detectedEncoding;
            try
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                }
                detectedEncoding = reader.CurrentEncoding.BodyName;
            }
            catch (Exception e)
            {
                detectedEncoding = "ISO-8859-1";
            }
            return detectedEncoding=="utf-8";
        }
    }


    /// <summary> 是否开启Encoding自动修正</summary>
    [MenuItem(menu_auto, priority = 100)]
    static void SwitchAutoFixState()
    {
        var value = !EditorPrefs.GetBool(key, false);
        EditorPrefs.SetBool(key, value);
        Menu.SetChecked(menu_auto, value);
    }

    /// <summary> 将脚本编码格式转换为 GB2312 (测试用) </summary>
    [MenuItem(menu_to_gb2312)]
    static void Convert2GB2312()
    {
        var settings = new ConvertSettings
        {
            predicate = x =>DetectFileEncoding(x),
            from = Encoding.UTF8,
            to = Encoding.GetEncoding(936),
        };
        EncodingConverter(settings);
    }

    static void EncodingConverter(ConvertSettings settings)
    {
        MonoScript[] msarr = Selection.GetFiltered<MonoScript>(SelectionMode.DeepAssets);
        if (null != msarr && msarr.Length > 0)
        {
            List<string> files = new List<string>();
            foreach (var item in msarr)
            {
                string path = AssetDatabase.GetAssetPath(item);
                if (settings.predicate.Invoke(path))
                {
                    var text = File.ReadAllText(path, settings.from);
                    File.WriteAllText(path, text, settings.to);
                    files.Add(path);
                }
            }
            var info = files.Count > 0 ? $"处理文件 {files.Count} 个，更多 ↓ \n{string.Join("\n", files)}" : "没有发现编码问题！";
            Debug.Log($"{nameof(ScriptEncodingConverter)}: 转换 {settings.to} 完成，{info}");
            AssetDatabase.Refresh();
        }
    }

    class ConvertSettings
    {
        /// <summary>断言是否为 UTF8 编码</summary>
        public Func<string, bool> predicate;
        public Encoding from, to;
    }

    class ScriptEncodingAutoFixHandler : AssetPostprocessor
    {
        //所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //如果用户不选择自动修正则不处理
            if (!EditorPrefs.GetBool(key, false)) return;
            //仅对有修改的脚本进行处理
            var scripts = importedAsset.Where(v => v.EndsWith(".cs"));
            if (scripts.Count() == 0) return;
            List<string> files = new List<string>();
            foreach (var path in scripts)
            {
                
                if (!DetectFileEncoding(path)) //如果不是 UTF8 编码
                {
                    var text = File.ReadAllText(path, Encoding.GetEncoding(936));
                    File.WriteAllText(path, text, Encoding.UTF8);
                    files.Add(path);
                }
            }
            if (files.Count > 0)
            {
                var info = $"处理文件 {files.Count} 个，更多 ↓ \n{string.Join("\n", files)}";
                Debug.Log($"Auto fix to UTF8 , {info}");
                AssetDatabase.Refresh();
            }
        }
    }
}
