#!/bin/bash

# https://gmaker.funplus.io/teamcity/repository/download/Invader_BuildIOS/404:id/Unity.BuildIOS-1.2.1178.log

# system.teamcity.buildType.id	Invader_BuildIOS
# teamcity.build.id 			404

#echo $1, $2, $3

# config for teamcity:
# ./Build/Template/iOS/generate_manifest.sh ./Build/Platforms/iOS/Build-%env.buildVersion% https://gmaker.funplus.io/teamcity/repository/download/%system.teamcity.buildType.id%/%teamcity.build.id%:id com.funplus.invader.global-%env.buildVersion%.ipa com.funplus.invader.global %env.bundleVersion% Invader.global

# config for nginx
# ./Build/Template/iOS/generate_manifest.sh 
# ./Build/Platforms/iOS/Build-%env.buildVersion% 
# https://gmaker.funplus.io/download/iOS/Build-%env.buildVersion% 
# com.funplus.invader.global-%env.buildVersion%.ipa 
# com.funplus.invader.global %env.bundleVersion% Invader.global

target_dir=$1
base_url=$2

manifest_url=manifest.plist

software_package=$3
display_image=invader.57x57.png
full_size_image=invader.512x512.png
bundle_identifier=$4
bundle_version=$5
title=$6

cat << EOF > $target_dir/index.html
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="content-type" content="text/html; charset=utf-8">
		<title>Install $title $bundle_version</title>
		<style>
			body {
				width: 35em;
				margin: 0 auto;
				font-family: Tahoma, Verdana, Arial, sans-serif;
			}
		</style>
		<script type="text/javascript" src="./jquery-3.2.1.slim.min.js"></script>
		<script type="text/javascript" src="./jquery.qrcode.min.js"></script>
		<script>
		    \$(function(){  
		    	jQuery('#qrcode').qrcode("${base_url}/${software_package}");
		    });
		</script>
	</head>

	<body>
	
	<h2>$bundle_identifier $bundle_version</h2>
	<a href="${base_url}/${software_package}">Install $title</a>
	<p> 扫描二维码，在手机上打开本页面：</p>
	<div id="qrcode" style="margin-top: 20px;"></div>

	<HR align=center width=2048 color=#987cb9 SIZE=3>
	<p> Download game_version log:</p>
	<a href="${base_url}/Unity.GenerateGameVersion-${bundle_version}.log">Unity.GenerateGameVersion-${bundle_version}.log</a>

	<p> Download Unity Build log:</p>
	<a href="${base_url}/Unity.BuildAndroid-${bundle_version}.log">Unity.BuildAndroid-${bundle_version}.log</a>

	</body>
</html>
EOF