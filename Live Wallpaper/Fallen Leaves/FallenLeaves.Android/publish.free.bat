"C:\Program Files (x86)\Java\jdk1.6.0_31\bin\jarsigner.exe" -verbose -sigalg MD5withRSA -digestalg SHA1  -keystore "E:\KamGame.keys\kamgame.keystore" -signedjar bin\Release.Free\com.divarc.fallenleaves.free-Signed.apk bin\Release.Free\com.divarc.fallenleaves.free.apk kamgame
"C:\Users\KAM\AppData\Local\Android\android-sdk\tools\zipalign.exe" -f -v 4 bin\Release.free\com.divarc.fallenleaves.free-Signed.apk com.divarc.fallenleaves.free.apk