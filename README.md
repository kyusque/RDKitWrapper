# RDKitWrapper

RDKitWrapper functions for a personal use.

## Targets

- Windows x64
- Android

## Host Environment

- Windows 10

## Requirements

- Visual Studio 2019 +
- cmake
- vcpkg
- gitbash
- Make for Windows
- NDK(in Unity2021.2.9f1) for Android

## RDKit Build

### Preprocess

```bash
# on gitbash
cd c:
git clone https://github.com/rdkit/rdkit
# commit bed7eb1eb529be24c6317c62875b9926a948cd5f

sed -i -e 's|^.*static_assert.*$||g' -e 's|^.*T cannot be string_view.*$||g' Code/RDGeneral/Dict.h
sed -i -e 's|!getlogin_r(buffer, bufsize)|false|g' External/GA/util/Util.cpp 
sed -i -e 's|add_library(${RDKLIB_NAME} SHARED ${RDKLIB_SOURCES})|add_library(${RDKLIB_NAME} STATIC ${RDKLIB_SOURCES})|g' Code/cmake/Modules/RDKitUtils.cmake
```

### Windows

```
/c/src/vcpkg/vcpkg.exe install --triplet x64-windows-static eigen3 boost freetype --keep-going 
```


```PowerShell
# on Developer PowerShell
cd C:/rdkit
mkdir win64 ||:
cmake -S. -Bwin64 \
	-DCMAKE_TOOLCHAIN_FILE=c:/src/vcpkg/scripts/buildsystems/vcpkg.cmake \
	-DVCPKG_TARGET_TRIPLET=x64-windows-static \
	-DRDK_BUILD_PYTHON_WRAPPERS=OFF \
	-DRDK_BUILD_MAEPARSER_SUPPORT=OFF \
	-DRDK_TEST_MMFF_COMPLIANCE=OFF \
	-DRDK_BUILD_CPP_TESTS=OFF \
	-DRDK_TEST_MULTITHREADED=OFF \
	-DRDK_BUILD_CAIRO_SUPPORT=OFF
cd C:\rdkit\win64
msbuild ALL_BUILD.vcxproj /p:Configuration=Release
```

```bash
# on gitbash
cd /c/rdkit
cp win64/CMakeFiles/Export/C_/rdkit/lib/cmake/rdkit/* win64
sed -i -e 's|C:/rdkit/lib/|C:/rdkit/win64/lib/Release/|g' win64/rdkit-targets-*.cmake
```

### Android

```bash
# on gitbash
ANDROID_NDK_HOME='/c/Program Files/Unity/Hub/Editor/2021.2.9f1/Editor/Data/PlaybackEngines/AndroidPlayer/NDK' /c/src/vcpkg/vcpkg.exe install --triplet arm64-android eigen3 boost freetype --keep-going 
```

```bash
cd /c/rdkit
mkdir aarm64 ||:
cmake -S. -Baarm64 -G Ninja \
	-DANDROID_ABI=arm64-v8a \
	-DANDROID_PLATFORM=android-26 \
	-DCMAKE_TOOLCHAIN_FILE=C:/src/vcpkg/scripts/buildsystems/vcpkg.cmake \
	-DVCPKG_CHAINLOAD_TOOLCHAIN_FILE=C:/Program Files/Unity/Hub/Editor/2021.2.9f1/Editor/Data/PlaybackEngines/AndroidPlayer/NDK/build/cmake/android.toolchain.cmake \
	-DVCPKG_TARGET_TRIPLET=arm64-android \
	-DRDK_BUILD_PYTHON_WRAPPERS=OFF \
	-DRDK_TEST_MMFF_COMPLIANCE=OFF \
	-DRDK_BUILD_CPP_TESTS=OFF \
	-DRDK_BUILD_CAIRO_SUPPORT=OFF \
	-DRDK_TEST_MULTITHREADED=OFF \
	-DRDK_USE_BOOST_SERIALIZATION=OFF \
	-DRDK_USE_BOOST_IOSTREAMS=OFF \
	-DRDK_BUILD_MAEPARSER_SUPPORT=OFF
cd aarm64 && ninja -k 0
cp aarm64/CMakeFiles/Export/C_/rdkit/lib/cmake/rdkit/* aarm64
sed -i -e 's|C:/rdkit/lib/|C:/rdkit/aarm64/lib/|g' -e 's|_static.a|.a|g' aarm64/rdkit-targets-*.cmake
sed -i -e 's|${CMAKE_CURRENT_LIST_DIR}/../../..|C:/rdkit|' -e 's|${CMAKE_CURRENT_LIST_DIR}|C:/rdkit/aarm64|' aarm64/rdkit-config.cmake
```

## RDKitWrapper

```bash
cd /c/
git clone https://github.com/kyusque/RDKitWrapper
```

### Windows

```powershell
# on Developer PowerShell
make win64
```

### Android

```bash
make aarm64
```
