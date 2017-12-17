function TuneHeight(iframeName) {    
  var frm = document.getElementById(iframeName);  
  var subWeb = document.frames ? document.frames[iframeName].document : frm.contentDocument;  
  if(frm != null && subWeb != null) {
     frm.height = subWeb.body.scrollHeight;
  }  
 }  