# KCCP-BrowserCacheCleaner
[![Ko-Fi](https://img.shields.io/badge/Donate-Ko--Fi-red)](https://ko-fi.com/coburn) 
[![PayPal](https://img.shields.io/badge/Donate-PayPal-blue)](https://paypal.me/coburn64)
![MIT Licensed](https://img.shields.io/badge/license-MIT-green.svg)

A utility that nukes multiple browser installations' cache directory for easier KCCP updates.

## Running the program
This program is basically a C# rewrite (or partial copy paste, partial rewrite) of Oradimi's Browser Cache Cleaner which is contained in the [KanColle English Patch Repository](https://github.com/Oradimi/KanColle-English-Patch-KCCP).

It will seek out multiple browsers and KanColle web game viewers' caches and blast them. Beware that running the program will kill any browsers that the KC3 extension can run on. Some include Microsoft Edge, Chromium, Chrome, Opera et al. This is required because caches are usually locked during the browser runtime, and this program will skip any caches it cannot blast.

It will attempt to relaunch [KCCacheProxy](https://github.com/Tibowl/KCCacheProxy) which is a bloody excellent local cache proxy for the web version of KanColle if I may say so myself. However, *it is normal* to get the error message that it cannot relaunch KCCacheProxy if it's not installed or it's installed in some other path.

Questions? Bugs? Did the software sink your battleship? Feel free to open a ticket. You can also find me floating around numerous KanColle discords, but don't DM me unless I ask you to.

Thank you for using Australian Open Source Software!
