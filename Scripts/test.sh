#!/usr/bin/env bash

UNITY_BIN="/Applications/Unity/Unity.app/Contents/MacOS/Unity"

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -runTests \
    -batchmode \
    -projectPath $(pwd) \
    -logFile $(pwd)/tests_playmode.log \
    -testResults $(pwd)/results_playmode.xml \
    -testPlatform playmode
