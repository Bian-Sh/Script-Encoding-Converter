# Script Encoding Converter
 Editor extension for covert script encoding from gb2312 to utf8
 
 这是一个将脚本编码从 GB2312 转换为 UTF8 的编辑器扩展
 
 # Feature / 功能
 1. 支持 Project 窗口单/多选文件、文件夹选中进行编码转换

    Muilt-files or folder can selected for onekey encoding convert 
  
 3. 支持设置为自动编码转换
 
    Can auto fix encoding issue if you prefer to.
 
 5. 简易的 UTF8-Bom 断言
 
    Just a simple way to check wether a script is under utf8-bom or not.
 
 # GIF / 动画
 ![](doc/converter.gif)
 **Tips:**
 * 演示了单文件、多文件 UTF8 - GB2312 互相转换 

    Demonstrated the single file multi - file UTF8 - GB2312 conversion
  
 * 演示了脚本被修改后自动修改 编码为 UTF8 
 
    Demonstrates that the script is automatically changed to UTF8 after being modified
 
# Summary / 结语
 这个组件对脚本中有汉字的人会有帮助，它解决了某些情况下脚本编码会变成 GB2312 导致 Unity 识别异常，异常表现为要么 Inspector 上可见乱码，要么 log 信息中出现乱码。
 
 This plugin would be helpful when script contians some Chinese characters. It solves the problem that in some cases the script will become a GB2312 Encoding and cause Unity  recognize abnormality. The details of the abnormality can be seen on the Inspector,as well as in the log context.

> 不同于json ，脚本恰恰需要 Bom 做编码格式标识，不会对开发产生任何的负面影响。脚本使用 UTF8 无 Bom 的不要使用本工具。
