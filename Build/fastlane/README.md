fastlane documentation
================
# Installation

Make sure you have the latest version of the Xcode command line tools installed:

```
xcode-select --install
```

Install _fastlane_ using
```
[sudo] gem install fastlane -NV
```
or alternatively using `brew cask install fastlane`

# Available Actions
### do_set_package_setting
```
fastlane do_set_package_setting
```
Description of what the lane does

----

## Android
### android info
```
fastlane android info
```
android info
### android update_version
```
fastlane android update_version
```
GenerateGameVersion from version
### android generate_asset
```
fastlane android generate_asset
```
Generate Game Resources
### android copy_asset
```
fastlane android copy_asset
```
Copy asset bundles Game Resources
### android export
```
fastlane android export
```

### android icon
```
fastlane android icon
```

### android teamcity_export_parameter
```
fastlane android teamcity_export_parameter
```

### android teamcity_artifacts
```
fastlane android teamcity_artifacts
```

### android assemble
```
fastlane android assemble
```

### android upload_assets
```
fastlane android upload_assets
```

### android upload_bugly
```
fastlane android upload_bugly
```

### android manifest
```
fastlane android manifest
```

### android deploy
```
fastlane android deploy
```

### android beta
```
fastlane android beta
```
Submit a new Beta Build

----

This README.md is auto-generated and will be re-generated every time [fastlane](https://fastlane.tools) is run.
More information about fastlane can be found on [fastlane.tools](https://fastlane.tools).
The documentation of fastlane can be found on [docs.fastlane.tools](https://docs.fastlane.tools).
