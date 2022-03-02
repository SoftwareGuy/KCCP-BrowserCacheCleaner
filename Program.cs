// Browser Cache Cleaner
// Written originally by Oradimi: https://github.com/Oradimi/KanColle-English-Patch-KCCP
// Converted to C# by SoftwareGuy/Coburn: https://github.com/SoftwareGuy
//
// This conversion of the software is licensed under the MIT license and contains parts of the
// original program licensed under MIT.
// 
// MIT License
// Copyright (c) 2022 Matt Coburn (SoftwareGuy/Coburn64)
// Original program copyright (c) 2021 - 2022 Oradimi.
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BrowserCacheCleaner
{
    internal class Program
    {
        static string[] browserProcesses =
        {
            "poi.exe",  // Poi KC Browser (also the name of a game of steam!)
            "ElectronicObserver.exe", // Electronic Observer KC Browser
            "chrome.exe", // Chromium and Google Chrome
            "msedge.exe", // Microsoft Edge, another Chrome clone
            "opera.exe", // Opera and Opera GX "Gaming" Browser
            "brave.exe", // Brave Browser
            "vivaldi.exe", // Vivaldi Browser
            "browser.exe" // Yandex. Russian browser?
        };

        static string KanColleCacheProxyExecutable = "kccacheproxy.exe";

        static void Main(string[] args)
        {
            Console.Title = "Browser Cache Cleaner";
            DoInformation();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Ready to continue?");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("PRESS Q NOW TO ABORT. ANY OTHER KEY WILL CLOSE YOUR BROWSERS AND CLEAR THEIR CACHE!");
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleKeyInfo a = Console.ReadKey(true);

            if (a.Key == ConsoleKey.Q)
            {
                // Bye bye cruel world!
                Environment.Exit(1);
            }

            Console.WriteLine("Alright, you asked for it...");

            // Kill the browsers and KCCacheProxy...
            KillBrowserAndKCCacheProxy();
            DeleteCaches();
            InvokeKCCP();

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("My job here is done.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Make sure to clear your browser/viewer's cache if you're not using a supported browser/viewer!");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press any key to close...");

            Console.ReadKey(true);
            Environment.Exit(0);
        }

        static void DoInformation()
        {
            Console.WriteLine("Welcome to the Browser Cache Clearer!");
            Console.WriteLine("Use this little program to just clear your cache. Every browser/viewer listed below will be closed, " +
                "so make sure your ongoing work/game is saved/done!");
            Console.WriteLine("");

            // Green text
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Supported browsers/viewers: ");
            Console.WriteLine("    - Poi");
            Console.WriteLine("    - Electronic Observer");
            Console.WriteLine("    - Chromium");
            Console.WriteLine("    - Google Chrome");
            Console.WriteLine("    - Microsoft Edge (Chromium version)");
            Console.WriteLine("    - Opera (Normal and GX)");
            Console.WriteLine("    - Brave");
            Console.WriteLine("    - Vivaldi");
            Console.WriteLine("    - Yandex");
            Console.WriteLine("");
            Console.WriteLine("Will also restart KCCacheProxy to reload your mods' data if it's already running. Restore your session " +
                "afterwards by manually restarting your browser/viewer.");
            // End green.
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("");

            // Yellow text.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("If your browser/viewer doesn't close automatically, you will have to clear your browser/viewer's " +
                "cache manually.");

            Console.WriteLine("This version of the application was converted by SoftwareGuy (Coburn64). Contact Oradimi#8947 on Discord so " +
                "they can bug Coburn to add support to your browser/viewer.");
        }

        static void KillBrowserAndKCCacheProxy()
        {
            Console.WriteLine("Killing web browser processes...");

            for (int i = 0; i < browserProcesses.Length; i++)
            {
                try
                {
                    Process grimReaper = new System.Diagnostics.Process();
                    grimReaper.StartInfo.RedirectStandardOutput = true;
                    grimReaper.StartInfo.UseShellExecute = false;
                    grimReaper.StartInfo.CreateNoWindow = true;
                    grimReaper.StartInfo.FileName = "taskkill.exe";
                    grimReaper.StartInfo.Arguments = $"/f /im:{browserProcesses[i]}";
                    grimReaper.Start();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"That did not go as expected. Error returned was: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            // Kill KCCacheProxy too.
            Console.WriteLine("Killing KCCacheProxy...");
            try
            {
                Process grimReaper = new System.Diagnostics.Process();
                grimReaper.StartInfo.RedirectStandardOutput = true;
                grimReaper.StartInfo.UseShellExecute = false;
                grimReaper.StartInfo.CreateNoWindow = true;
                grimReaper.StartInfo.FileName = "taskkill.exe";
                grimReaper.StartInfo.Arguments = $"/f /im:{KanColleCacheProxyExecutable}";
                grimReaper.Start();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"That did not go as expected. Error returned was: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();

            // Wait a few seconds before
            Thread.Sleep(3000);
        }

        static void DeleteCaches()
        {
            Console.WriteLine($"Traversing and looking for caches...");

            List<string> deathRow = new List<string>();
            string pathPrefix = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string[] browserShits = {
                "Chromium", // Chromium based browsers
                Path.Combine("Google", "Chrome"),
                Path.Combine("Microsoft", "Edge"),
                Path.Combine("BraveSoftware", "Brave-Software"),
                "Vivaldi",
                Path.Combine(Path.Combine("ElectronicObserver", "Webview2"), "EBWebView"), // Effin' heck, that's a path and a half!
                Path.Combine("Yandex", "YandexBrowser"),
                Path.Combine("Opera Stable", "Opera Stable"),
                Path.Combine("Opera Stable", "Opera GX Stable"),
            };

            // Build up the list...
            for (int i = 0; i < browserShits.Length; i++)
            {
                bool defaultDirectory = false;

                try
                {
                    // Fuck I forgot NET 3.5 doesn't allow more than 2 arguments for this
                    string workFolderPrefix = Path.Combine(pathPrefix, browserShits[i]);
                    string workFolder = Path.Combine(workFolderPrefix, "User Data");

                    if (Directory.Exists(workFolder))
                    {
                        Console.WriteLine($"-> Found browser storage directory at '{workFolder}'...");

                        // Now we need to see what folders are here.

                        // Do we have a default folder?
                        defaultDirectory = Directory.Exists(Path.Combine(workFolder, "Default"));
                        DirectoryInfo dirInfo = new DirectoryInfo(workFolder);

                        if (defaultDirectory)
                        {
                            Console.WriteLine("--> Default User directory exists and will be cleaned.");
                            deathRow.Add(Path.Combine(workFolder, "Default"));
                        }

                        var dirs = dirInfo.GetDirectories("Profile*", SearchOption.TopDirectoryOnly);
                        if (dirs.Length > 0)
                        {
                            Console.WriteLine("--> Looks like this browser has multiple profiles, the caches in them will be cleaned too.");

                            foreach (var dir in dirs)
                                deathRow.Add(dir.FullName);
                        }
                        else
                        {
                            Console.WriteLine("--> Looks like this browser only had one profile.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Skipped the path because it doesn't exist (browser not installed?): '{workFolder}'");
                    }

                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"While processing a browser cache directory, something did not go as expected. Error returned was: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.WriteLine($"-> Poi?");
            // Check poi too...
            if (Directory.Exists(Path.Combine(pathPrefix, "poi")))
            {
                Console.WriteLine($"-> Poi!");

                // Add Poi to the list too.
                deathRow.Add(Path.Combine(pathPrefix, "poi"));
            }

            // Eviction time...
            foreach (string path in deathRow)
            {
                Console.WriteLine($"-> Processing '{path}'...");

                if (!Directory.Exists(Path.Combine(path, "Cache")))
                    continue;

                // Take them out and shoot them.
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(path, "Cache"));
                    var a = dirInfo.GetFiles("*", SearchOption.AllDirectories);

                    if (a.Length > 0)
                    {
                        foreach (var file in a)
                        {
                            try
                            {
                                Console.WriteLine($"--> Deleting '{file.Name}'");
                                File.Delete(file.FullName);
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Could not delete '{file.Name}'");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"While deleting the browser cache directory contents, something did not go as expected. Error returned was: {e.Message}");
                    Console.WriteLine($"Beware that the browser may now complain about a corrupt cache. You may need to manually clear it instead!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        static void InvokeKCCP()
        {
            string kccpFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KCCacheProxy");
            DirectoryInfo dirInfo = new DirectoryInfo(kccpFolder);

            try
            {
                var a = dirInfo.GetFiles("kccacheproxy.exe", SearchOption.AllDirectories);

                if (a.Length > 0)
                {
                    if (a.Length == 1)
                    {
                        // Invoke it.
                        Process gudBote = new Process();
                        gudBote.StartInfo.FileName = $"{a[0]}";
                        gudBote.Start();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something happened and this program cannot (re)start KCCP. Please restart it manually.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: KCCacheProxy is not installed to the default path. Cannot (re)start KCCP - please (re)start it manually.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
