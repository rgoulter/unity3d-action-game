#! /usr/bin/env bash

set -e

# from http://unity3d.com/get-unity/download/archive
BASE_URL="https://netstorage.unity3d.com/unity"
HASH="fc1d3344e6ea"
VERSION="2017.3.1f1"

download() {
  file=$1
  url="$BASE_URL/$HASH/$package"

  echo "Downloading from $url: "
  curl -o `basename "$package"` "$url"
}

install() {
  package=$1
  download "$package"

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

# See $BASE_URL/$HASH/unity-$VERSION-$PLATFORM.ini for complete list
# of available packages, where PLATFORM is `osx` or `win`

# https://unity3d.com/unity/whats-new/unity-2018.1.0
# https://unity3d.com/unity/whats-new/unity-2017.3.0

install "MacEditorInstaller/Unity-$VERSION.pkg"
# install "MacEditorTargetInstaller/UnitySetup-Windows-Mono-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-WebGL-Support-for-Editor-$VERSION.pkg"
install "MacStandardAssetsInstaller/StandardAssets-$VERSION.pkg"
# install "MacEditorTargetInstaller/UnitySetup-Mac-IL2CPP-Support-for-Editor-$VERSION.pkg"
