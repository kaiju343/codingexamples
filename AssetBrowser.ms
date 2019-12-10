
_AssetItems = getdirectories @"H:\projects\emptyspace\FXmodsAssetBrowser\Library\*"
_AssetItemsUI = #()

for i=1 to _AssetItems.count do
	(
		_AssetItemsUI[i] = (filterstring _AssetItems[i] @"\")[6]
	)
		




rollout _AssetBrowser "Asset Browser"
(
	listbox _AssetList "Asset List" items:_AssetItemsUI
	button _Go "Go"
		on _Go pressed do
		(
			FXAssetDir = _AssetItems[_AssetList.selection]
			FXAssetMarkerFile = "FXS_Asset_" + _AssetItemsUI[_AssetList.selection] + "_Marker.ms"
			print (FXAssetDir + FXAssetMarkerFile)
			::filein (FXAssetDir + FXAssetMarkerFile)
		)
)
createdialog _AssetBrowser
