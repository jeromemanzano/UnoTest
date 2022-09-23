## Pros:

- ListView virtualization greatly improves performance of chat compared to Avalonia's ListView. I tried loading 10,000 items but app seems to handle it well.
- Lottie is supported
- WASM performace can still be improved by turning AOT on or enabling multithreading (I haven't tested this but more info can be found [here](https://platform.uno/blog/webassembly-threading-in-net/) and [here](https://platform.uno/blog/build-net-aot-for-webassembly-in-visual-studio-with-uno-platform/))
- Hot reload when developing using Windows machine.
- Didn't encounter the issues that we have in Avalonia for iOS and Android.
- ReactiveUI, SQLite, IndexDB, Daily, Notifications and Nakama should still work since it's using .NET 6 and Xamarin bindings

## Cons:
- Compiler issues on both Windows and MacOS. In MacOS intellisense will not work and bin/obj sometimes needs to be deleted for changes to reflect.
- ListView code needs to be updated so it supports updating when scrolled to the top most. Currently it only updates when scrolled to the bottom
- Not all components support all platforms. The documentation needs to be checked to see if it supports the platform we are targeting (e.g [drag and drop](https://github.com/unoplatform/uno/issues/1480) is not supported on Windows)

## Additional Info:
https://gallery.platform.uno/

## Samples:

### WASM
![wasm debug uno](https://user-images.githubusercontent.com/23254953/191890932-99252166-979f-4870-9d3d-0da8adc96e26.gif)

### iOS
![ios](https://user-images.githubusercontent.com/23254953/191891839-63033bca-2ff2-4658-8280-d3fe21c7cb70.gif)

### Android

![Android](https://user-images.githubusercontent.com/23254953/191892363-fbed51dc-5be3-4f7d-ac24-46a59d3d4767.gif)
