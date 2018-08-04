#!/usr/bin/env bash

UNITY_BIN="/Applications/Unity/Unity.app/Contents/MacOS/Unity"

echo "Running playmode tests"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -force-free \
  -runTests \
  -batchmode \
  -projectPath $(pwd) \
  -logFile $(pwd)/tests_playmode.log \
  -testResults $(pwd)/results_playmode.xml \
  -testPlatform playmode

echo "Logs of playmode tests":
cat $(pwd)/tests_playmode.log

echo "XML of playmode tests":
cat $(pwd)/results_playmode.xml
