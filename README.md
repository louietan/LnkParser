LnkParser
=========

LnkParser is a .NET library used for parsing Windows shortcut file.

It's currently only support partial structures of the MS-SHELLINK format.

Sample
======

```cs
var shortcut = new WinShortcut("the path of the shortcut file");
var target = shortcut.TargetPath; //get the real path of the target this shortcut refers to
var isdirectory = shortcut.IsDirectory; //get whether the target is a directory
var hotkey = shortcut.HotKey; //get the hotkey of this shortcut
```
