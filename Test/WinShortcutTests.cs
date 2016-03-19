using IWshRuntimeLibrary;
using LnkParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    [TestClass]
    public class WinShortcutTests
    {
        static void CreateShortcut(string location, string targetPath, string hotkey)
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(location);

            if (!string.IsNullOrEmpty(hotkey))
                shortcut.Hotkey = hotkey;
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }

        [TestMethod]
        public void WinShortcut_FileCorrectlyParsed()
        {
            var basePath = Path.GetFullPath(@".\test files");

            // { location, target path, is direstory, hotkey }
            var shortcuts = new List<Tuple<string, string, bool, string>>
            {
                Tuple.Create(Path.Combine(basePath, "file.lnk"), Path.Combine(basePath, @"Program Files\Corporation Name\App.exe"), false, ""),
                Tuple.Create(Path.Combine(basePath, "file with shortcut.lnk"), Path.Combine(basePath, @"Program Files\Utility.exe"), false, "ctrl+alt+a"),
                Tuple.Create(Path.Combine(basePath, "directory.lnk"), Path.Combine(basePath, @"娱乐\电影"), true, ""),
                Tuple.Create(Path.Combine(basePath, "directory with shortcut.lnk"), Path.Combine(basePath, @"娱乐\音乐"), true, "ctrl+alt+b"),
                Tuple.Create(Path.Combine(basePath, "directory with shortcut 1.lnk"), Path.Combine(basePath, @"娱乐\游戏"), true, "ctrl+shift+c")
            };

            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);
            Directory.CreateDirectory(basePath);

            foreach (var i in shortcuts)
            {
                if (i.Item3)
                    Directory.CreateDirectory(i.Item2);
                else
                {
                    var dir = Path.GetDirectoryName(i.Item2);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    System.IO.File.Create(i.Item2);
                }
                CreateShortcut(i.Item1, i.Item2, i.Item4);
            }

            foreach (var i in shortcuts)
            {
                var shortcut = new WinShortcut(i.Item1);
                Assert.AreEqual(shortcut.HotKey.ToLower(), i.Item4.ToLower(), "the hotkey isn't parsed correctly");
                Assert.AreEqual(shortcut.IsDirectory, i.Item3, "the isdirectory isn't parsed correctly");
                Assert.AreEqual(shortcut.TargetPath.ToLower(), i.Item2.ToLower(), "the target path isn't parsed correctly");
            }
        }
    }
}
