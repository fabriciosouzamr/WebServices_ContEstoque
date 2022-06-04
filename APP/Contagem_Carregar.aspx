<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contagem_Carregar.aspx.cs" Inherits="APP.Contagem_Carregar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="Content/style.css" />
    <script src="Scripts/jquery-3.3.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <form id="form2" runat="server">
        Data da Contagem
        <br />
        <asp:TextBox TextMode="Date" ID="txtData" runat="server"></asp:TextBox>
        <br /><br />
        A contagem foi realizada
        <br />
         <asp:RadioButtonList id="optContagemRealizada" runat="server">
            <asp:ListItem Value="1" Selected="True">Antes do expediente</asp:ListItem>
            <asp:ListItem Value="2">Depois do expediente</asp:ListItem>
         </asp:RadioButtonList>
        <div> 
        <br /><br />
        Pesquisar o produto pelo
        <br />
         <asp:RadioButtonList id="optPesquisaProduto" runat="server">
            <asp:ListItem Value="1" Selected="True">Código</asp:ListItem>
            <asp:ListItem Value="2">Código de Barra</asp:ListItem>
         </asp:RadioButtonList>
        <div> 
        <br />
            - A contagem precisa ser realizada com a loja fechada, antes ou depois do expediente
        </div>  
        <br /><br />
        Arquivo de contagem, em csv
        <br />
        <asp:FileUpload id="FileUploadControl" runat="server" Width="596px" />
        <div> 
            <br />
            - A estrutura da planilha é <strong>Filial, Código do Produto, Descrição do Produto e quantidade</strong><br />
            - Na coluna de quantidade não pode ter informações não númericas, e ter zero e não campo campo vazio<br />
            - Só serão processadas produtos com códigos válidos informados, e não informar o código de barra no campo de código
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