.PHONY: win64
win64:
# on Developer PowerShell in Visual Studio
	mkdir win64 ||:
	cmake -S. -Bwin64 \
	-DRDKit_ROOT_DIR=C:/rdkit/win64 \
	-DCMAKE_TOOLCHAIN_FILE=C:/src/vcpkg/scripts/buildsystems/vcpkg.cmake \
	-DVCPKG_TARGET_TRIPLET=x64-windows \
	-DVCPKG_APPLOCAL_DEPS=OFF \
	-DLIB_PREFIX="" \
	-DADD_LINK_DIR=C:/rdkit/win64/lib
	cd win64 && msbuild .\ALL_BUILD.vcxproj /p:Configuration=Release

.PHONY: win64_clean
win64_clean:
	rm -rf win64/*

.PHONY: aarm64
aarm64:
	mkdir aarm64 ||:
	cmake -S. -Baarm64 -G Ninja \
	-DRDKit_ROOT_DIR=C:/rdkit/aarm64 \
	-DVCPKG_PREFER_SYSTEM_LIBS=ON \
	-DCMAKE_PREFIX_PATH=C:/rdkit/aarm64 \
	-DCMAKE_TOOLCHAIN_FILE=C:/src/vcpkg/scripts/buildsystems/vcpkg.cmake \
	-DVCPKG_CHAINLOAD_TOOLCHAIN_FILE=C:/Program Files/Unity/Hub/Editor/2021.2.9f1/Editor/Data/PlaybackEngines/AndroidPlayer/NDK/build/cmake/android.toolchain.cmake \
	-DVCPKG_TARGET_TRIPLET=arm64-android \
	-DANDROID_ABI=arm64-v8a \
	-DANDROID_PLATFORM=android-26 \
	-DVCPKG_APPLOCAL_DEPS=OFF \
	-DLIB_PREFIX="RDKit" \
	-DADD_LINK_DIR=C:/rdkit/aarm64/lib
	cd aarm64 && ninja -k 0

.PHONY: aarm64_clean
aarm64_clean:
	rm -rf aarm64/*
