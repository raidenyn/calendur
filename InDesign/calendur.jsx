main();
function main(){
	replaceText("||", "^p");
	replaceText("|", "^p");
	
	changeStyle("(?<=(\~name\~)).+", "имя в планинге");
	changeStyle("(?<=(\~info\~)).+", "информация в планинге");
	changeStyle("(?<=(\~!name\~)).+", "имя в планинге (красн)");
	changeStyle("(?<=(\~!info\~)).+", "информация в планинге (красн)");
	
	replaceGrep("^\.+?\~", "");
}
function replaceText(pattern, text){
	//<fragment>
	//Clear the find/change text preferences.
	app.findTextPreferences = NothingEnum.nothing;
	app.changeTextPreferences = NothingEnum.nothing;
	//Set the find options.
	app.findChangeTextOptions.caseSensitive = false;
	app.findChangeTextOptions.includeFootnotes = false;
	app.findChangeTextOptions.includeHiddenLayers = false;
	app.findChangeTextOptions.includeLockedLayersForFind = false;
	app.findChangeTextOptions.includeLockedStoriesForFind = false;
	app.findChangeTextOptions.includeMasterPages = false;
	app.findChangeTextOptions.wholeWord = false;
	//Search the document for the string "copy" and change it to "text".
	app.findTextPreferences.findWhat = pattern;
	app.changeTextPreferences.changeTo = text;
	app.documents.item(0).changeText();
	//Clear the find/change text preferences after the search.
	app.findTextPreferences = NothingEnum.nothing;
	app.changeTextPreferences = NothingEnum.nothing;
	//</fragment>
}
function replaceGrep(pattern, text){
	//Clear the find/change preferences.
	app.findGrepPreferences = NothingEnum.nothing;
	app.changeGrepPreferences = NothingEnum.nothing;
	//Set the find options.
	app.findChangeGrepOptions.includeFootnotes = false;
	app.findChangeGrepOptions.includeHiddenLayers = false;
	app.findChangeGrepOptions.includeLockedLayersForFind = false;
	app.findChangeGrepOptions.includeLockedStoriesForFind = false;
	app.findChangeGrepOptions.includeMasterPages = false;
	//Search the document for the string "copy" and change it to "text".
	app.findGrepPreferences.findWhat = pattern;
	app.changeGrepPreferences.changeTo = text;
	app.documents.item(0).changeGrep();
	//Clear the find/change text preferences after the search.
	app.findGrepPreferences = NothingEnum.nothing;
	app.changeGrepPreferences = NothingEnum.nothing;
	//</fragment>
}
function changeStyle(pattern, styleName){
	//Clear the find/change preferences.
	app.findGrepPreferences = NothingEnum.nothing;
	app.changeGrepPreferences = NothingEnum.nothing;
	//Set the find options.
	app.findChangeGrepOptions.includeFootnotes = false;
	app.findChangeGrepOptions.includeHiddenLayers = false;
	app.findChangeGrepOptions.includeLockedLayersForFind = false;
	app.findChangeGrepOptions.includeLockedStoriesForFind = false;
	app.findChangeGrepOptions.includeMasterPages = false;
	//Search the document for the 24 point text and change it to 10 point text.
	app.findGrepPreferences.findWhat = pattern;
	app.changeGrepPreferences.appliedParagraphStyle = app.activeDocument.paragraphStyles.item(styleName);
	app.documents.item(0).changeGrep();
	//Clear the find/change preferences after the search.
	app.findGrepPreferences = NothingEnum.nothing;
	app.changeGrepPreferences = NothingEnum.nothing;
	//</fragment>
}