<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DuPontApiTest.aspx.cs" Inherits="DuPont.WebApplication.DuPontApiTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>测试接口</title>
  <script src="Scripts/jquery-1.10.2.js"></script>
  <script src="Scripts/layer/layer.js"></script>
    <style>
        body{   
     width: 100%;   
     height: 100%;   
     font-family: 'Open Sans',sans-serif;   
     margin: 0;   
     background-color: #808080;   
}   
#num{   
    position:absolute;  
    left:15%;   
    margin: -20px;   
    
}  
    </style>
 <script charset="utf-8" type="text/javascript">
     if (top.window.location.pathname !== location.pathname) {
         if (location.pathname && location.pathname.toLowerCase() === '/account/login') {
             top.window.location.href = location.origin + location.pathname;
         }
     }
        
     function getnum()
     {         //接口1       
        
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,
                 url: "Values/Getnum",
                 data: { id: 0 },
                 success: function (data, state) {
                     if (state == "success") {
                         $("#num").text(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);       

     }
     $(function () {
         //window.setInterval("getnum()", 5000);
         setTimeout("getnum()", 20000);
         var ws;
         $("#btnConnect").click(function () {

             $("#messageSpan").text("Connection...");
             ws = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/WsChat/Get");
             ws.onopen = function () {
                 $("#messageSpan").text("Connected!");
             };
             ws.onmessage = function (result) {
                 $("#messageSpan").text(result.data);
             };
             ws.onerror = function (error) {
                 $("#messageSpan").text(error.data);
             };
             ws.onclose = function () {
                 $("#messageSpan").text("Disconnected!");
             };
         });
         $("#btnSend").click(function () {
             if (ws.readyState == WebSocket.OPEN) {
                 ws.send($("#txtInput").val());
             }
             else {
                 $("messageSpan").text("Connection is Closed!");
             }
         }); 
         $("#btnDisConnect").click(function () {
             alert("1");
             ws.close();
         });
         //退出
         $("#refresh").click(function () {
             var options = {
                 type: "DELETE",
                 datatype: "json",
                 data: { id: "234" },
                 async: true,
                 url: "Values/Delete",
                 success: function (data, state) {                     
                      window.location.href = "/Login.html";                    
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);


         });

         //接口1
         var farmerorder=$("#txtarea1").val();
         $("#submitone").click(function () {
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,
                 url: "/Values/SaveRequirement",
                 data: { order:farmerorder },
                 success: function (data, state) {
                     if (state == "success") {
                         layer.alert(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);
         });
         //接口2
         $("#submittwo").click(function () {
             var farmerorder = $("#txtarea2").val();
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,
                 url: "/Values/CommentRequirement",
                 data: { order: farmerorder },
                 success: function (data, state) {
                     if (state == "success") {
                         layer.alert(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);
         });
         //接口3
         $("#submitthree").click(function () {
             var farmerorder = $("#txtarea3").val();
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,                
                 url: "/Values/ReplyFarmerRequirement",
                 data: { order: farmerorder },
                 success: function (data, state) {
                     if (state == "success") {
                         layer.alert(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);
         });
         //接口4
         $("#submitfour").click(function () {
             var farmerorder = $("#txtarea4").val();
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,
                 url: "/Values/CommentRequirementForOperator",
                 data: { order: farmerorder },
                 success: function (data, state) {
                     if (state == "success") {
                         layer.alert(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);
         });
         //接口5
         $("#submitfive").click(function () {
             var farmerorder = $("#txtarea5").val();
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,
                 url: "/Values/EtCommentRequirement",
                 data: { order: farmerorder },
                 success: function (data, state) {
                     if (state == "success") {
                         layer.alert(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);
         });
         //
         $("#submitsex").click(function () {
             var farmerorder = $("#txtarea6").val();
             var options = {
                 type: "get",
                 datatype: "json",
                 async: true,
                 url: "/Values/CancelFarmerRequirement",
                 data: { order: farmerorder },
                 success: function (data, state) {
                     if (state == "success") {
                         layer.alert(data);
                     }
                 },
                 error: function (data) {
                     alert(data);
                 }
             };
             $.ajax(options);
         });
     });
    

  </script>  

</head>
<body>
    <form id="form1" runat="server">
    <div> 
        
    <div><asp:Label ID="lbltitle1"  runat="server" Text="大农户发需求单" Font-Size="X-Large"></asp:Label></div>
        <fieldset>
            <textarea id="txtarea1" cols="50" rows="10" runat="server">               
            </textarea>
             <input id="submitone" type="button" style="text-align:center" value="提交" />
        </fieldset> 
        <div><asp:Label ID="lbltitle2"  runat="server" Text="先锋帮的农机手评价大农户" Font-Size="X-Large"></asp:Label></div>
        <fieldset>
            <textarea id="txtarea2" cols="50" rows="8" runat="server">               
            </textarea>
             <input id="submittwo" type="button" style="text-align:center" value="提交" />
        </fieldset> 
        <div><asp:Label ID="lbltitle3"  runat="server" Text="靠谱作业的农机手接单" Font-Size="X-Large"></asp:Label></div>
        <fieldset>
            <textarea id="txtarea3" cols="50" rows="8" runat="server">               
            </textarea>
             <input id="submitthree" type="button" style="text-align:center" value="提交" />
        </fieldset> 
        <div><asp:Label ID="lbltitle4"  runat="server" Text="先锋帮大农户评价靠谱作业的农机手" Font-Size="X-Large"></asp:Label></div>
        <fieldset>
            <textarea id="txtarea4" cols="50" rows="8" runat="server">               
            </textarea>
             <input id="submitfour" type="button" style="text-align:center" value="提交" />
        </fieldset> 
        <div><asp:Label ID="lbltitle5"  runat="server" Text="靠谱作业的农机手评价先锋帮大农户" Font-Size="X-Large"></asp:Label></div>
        <fieldset>
            <textarea id="txtarea5" cols="50" rows="8"  runat="server">               
            </textarea>
             <input id="submitfive" type="button" style="text-align:center" value="提交" />
        </fieldset>
        <div><asp:Label ID="lbltitle6"  runat="server" Text="靠谱作业的农机手取消订单" Font-Size="X-Large"></asp:Label></div>
        <fieldset>
            <textarea id="txtarea6" cols="50" rows="8"  runat="server">               
            </textarea>
             <input id="submitsex" type="button" style="text-align:center" value="提交" />
        </fieldset>   
    </div>
       <div>   
           <input id="refresh" type="button" style="text-align:center" value="退出登录" />
            请求次数： <div id="num"> </div>
       </div>
        


<fieldset>
    <input type="button" value="Connect" id="btnConnect" />
    <input type="button" value="DisConnect" id="btnDisConnect" />
    <hr />
    <input type="text" id="txtInput" />
    <input type="button" value="Send" id="btnSend" />
    <br />
    <span id="messageSpan" style="color:red;"></span>
</fieldset>
    </form>
</body> 
</html>
