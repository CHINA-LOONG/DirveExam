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

### upload_hockey
```
fastlane upload_hockey
```


----

## iOS
### ios info
```
fastlane ios info
```

### ios update_version
```
fastlane ios update_version
```
GenerateGameVersion from svn version
### ios generate_asset
```
fastlane ios generate_asset
```
Generate Game Resources
### ios copy_asset
```
fastlane ios copy_asset
```
Copy asset bundles Game Resources
### ios export
```
fastlane ios export
```

### ios icon
```
fastlane ios icon
```

### ios teamcity_export_parameter
```
fastlane ios teamcity_export_parameter
```

### ios teamcity_artifacts
```
fastlane ios teamcity_artifacts
```

### ios entitlements
```
fastlane ios entitlements
```

### ios certificates
```
fastlane ios certificates
```

### ios assemble
```
fastlane ios assemble
```

### ios upload_assets
```
fastlane ios upload_assets
```

### ios upload_testflight
```
fastlane ios upload_testflight
```

### ios upload_bugly
```
fastlane ios upload_bugly
```

### ios deploy
```
fastlane ios deploy
```

### ios beta
```
fastlane ios beta
```
Submit a new Beta Build
### ios teamcity
```
fastlane ios teamcity
```
lane for teamcity

----

## Android
### android info
```
fastlane android info
```

### android update_version
```
fastlane android update_version
```
GenerateGameVersion from svn version
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

### android assemble3
```
fastlane android assemble3
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
### android fork
```
fastlane android fork
```
lane for fork
### android teamcity
```
fastlane android teamcity
```
lane for teamcity

----

## Mac
### mac info
```
fastlane mac info
```

### mac update_version
```
fastlane mac update_version
```
GenerateGameVersion from svn version
### mac generate_asset
```
fastlane mac generate_asset
```
Generate Game Resources
### mac upload_assets
```
fastlane mac upload_assets
```

### mac teamcity_export_parameter
```
fastlane mac teamcity_export_parameter
```

### mac teamcity_artifacts
```
fastlane mac teamcity_artifacts
```

### mac beta
```
fastlane mac beta
```
build for develop
### mac teamcity
```
fastlane mac teamcity
```
lane for teamcity

----

## windows
### windows info
```
fastlane windows info
```

### windows update_version
```
fastlane windows update_version
```
GenerateGameVersion from svn version
### windows generate_asset
```
fastlane windows generate_asset
```
Generate Game Resources
### windows upload_assets
```
fastlane windows upload_assets
```

### windows teamcity_export_parameter
```
fastlane windows teamcity_export_parameter
```

### windows teamcity_artifacts
```
fastlane windows teamcity_artifacts
```

### windows beta
```
fastlane windows beta
```
build for develop
### windows teamcity
```
fastlane windows teamcity
```
lane for teamcity

----

This README.md is auto-generated and will be re-generated every time [fastlane](https://fastlane.tools) is run.
More information about fastlane can be found on [fastlane.tools](https://fastlane.tools).
The documentation of fastlane can be found on [docs.fastlane.tools](https://docs.fastlane.tools).
