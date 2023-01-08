 mergeInto(LibraryManager.library, {
  
     GetURLFromQueryStr: function () {
         var returnStr = window.top.location.href;
         var bufferSize = lengthBytesUTF8(returnStr) + 1
         var buffer = _malloc(bufferSize);
         stringToUTF8(returnStr, buffer, bufferSize);
		 console.log("Warren Buffer");
		 console.log(buffer);
         return buffer;
     }
 });