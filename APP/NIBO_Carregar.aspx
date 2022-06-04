<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NIBO_Carregar.aspx.cs" Inherits="APP.NIBO_Carregar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>NIBO</title>
</head>
<body>
    <form id="form2" runat="server">
        <br /><br />
        Arquivo da NIBO
        <br />
        <asp:FileUpload id="FileUploadControl" runat="server" Width="596px" />
        <div> 
            <br />
            - Selecione a planilha do sistema NIBO
        </div>  
        <br /><br />
        <asp:Button runat="server" id="cmdProcessar" text="Processar" onclick="cmdProcessar_Click" data-loading-text="Processando..." />
        <br /><br />
        <asp:Label runat="server" id="StatusLabel" text="Status do processamento: " />
    </form>
<script type="text/javascript">
    var formID = document.getElementById("form2");

    $("#FileUploadControl").on("change", function () {
        $("#StatusLabel").text("Status do processamento: ");
    });

    $(formID).submit(function(event){
        if (formID.checkValidity()) { 
            $("input[data-loading-text]").button( "loading" );
        }
    });
</script>
</body>
</html>
