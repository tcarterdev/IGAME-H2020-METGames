mergeInto(LibraryManager.library, {
	
	ShowMessage: function (message) {
		window.parent.postMessage(UTF8ToString(message), "*");
		console.log(UTF8ToString(message));
	},
	
	SendJsonString: function (JsonString)
	{
		//console.log(UTF8ToString(JsonString));
		const obj = JSON.parse(JsonString);
		
	},

});