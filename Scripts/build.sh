#! /bin/sh

# Adapted from: https://github.com/JonathanPorta/ci-build

# Change this the name of your project. This will be the name of the final executables as well.
PROJECT_NAME="rgoulter-unity-action-game"

UNITY_BIN="/Applications/Unity/Unity.app/Contents/MacOS/Unity"

echo "Attempting to build $PROJECT_NAME for Windows"
"$UNITY_BIN" \
  -force-free \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity-windows.log \
  -projectPath $(pwd) \
  -buildWindowsPlayer "$(pwd)/Build/windows/$PROJECT_NAME.exe" \
  -quit

echo 'Logs from Windows build:'
cat $(pwd)/unity-windows.log

echo "Attempting to build $PROJECT_NAME for OS X"
"$UNITY_BIN" \
  -force-free \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity-osx.log \
  -projectPath $(pwd) \
  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$PROJECT_NAME.app" \
  -quit

echo 'Logs from MacOS build:'
cat $(pwd)/unity-osx.log

echo "Attempting to build $PROJECT_NAME for Linux"
"$UNITY_BIN" \
  -force-free \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity-linux.log \
  -projectPath $(pwd) \
  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$PROJECT_NAME.exe" \
  -quit

echo 'Logs from Linux build:'
cat $(pwd)/unity-linux.log

echo 'Attempting to zip builds'
zip -r "$(pwd)/Build/$PROJECT_NAME-linux.zip" $(pwd)/Build/linux/
zip -r "$(pwd)/Build/$PROJECT_NAME-mac.zip" $(pwd)/Build/osx/
zip -r "$(pwd)/Build/$PROJECT_NAME-windows.zip" $(pwd)/Build/windows/
