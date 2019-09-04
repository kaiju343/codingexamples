/*

	publish file to :	D:\FXTD_Mentorship\devFXTDT\FXAssetBrowser\Library
	and name based on input name
	make directory based on input name
	
	generate marker file

*/

-- Define Lib Path and naming

LibPath = @"D:\FXTD_Mentorship\devFXTDT\FXAssetBrowser\Library\"
-- LibPubName = "RainMaker"


rollout _AssetPub "Asset Publisher"
(
	edittext _AssetName "Asset Name:" text:"AssetName"
	button _publish "Publish Asset to Library" align:#left
	on _publish pressed do 
		(
			if _AssetName.text != "AssetName" then
				(
					LibPubName = _AssetName.text
					
					-- Create Asset Directory and save max file asset

					AssetPath = LibPath + LibPubName
					MakeDir AssetPath
					AssetMaxFileOut = AssetPath + @"\" + "FXS_Asset_" + LibPubName + "_pub.max"
					
					if (doesfileexist AssetMaxFileOut) != true then
						(
							savemaxfile AssetMaxFileOut
						)
						else
						(
							if querybox "Do you wish to overwrite?" then(savemaxfile AssetMaxFileOut) else ("No")
						)
					-- Save marker file to Lib
					global AssetMarkerFileOut = AssetPath + @"\" +  "FXS_Asset_" + LibPubName  +"_Marker.ms"
					Outfile = createfile AssetMarkerFileOut
					format "%\n" ("mergemaxfile @\"" + AssetMaxFileOut + "\"") to:Outfile
					close Outfile
				)
				else (messagebox "Name your asset!")

	)
	button _EditMarkerFile "Open Marker File" align:#left
	on _EditMarkerFile pressed do
	(
		shellLaunch @"C:\Windows\System32\notepad.exe" AssetMarkerFileOut
	)
	button _goLibPath "Explore Asset Library" align:#left
	
	on _goLibPath pressed do
	(
		shellLaunch LibPath ""
	)
)
createdialog _AssetPub



