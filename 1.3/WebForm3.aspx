<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="_1._3.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>管理员页面</title>
     <style>
    body {
        font-family: Arial, sans-serif;
    }
    .button-container {
        text-align: center;
        margin-top: 20px;
    }
    .button-container button {
        margin: 5px;
        padding: 10px 20px;
        font-size: 16px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s;
    }
    .button-container button:hover {
        background-color: #0056b3;
    }
    #results {
        margin: 20px auto;
        width: 80%;
        background-color: #f0f0f0;
        border: 1px solid #ccc;
        padding: 20px;
        text-align: left;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    .search-container {
        margin: 20px auto;
        width: 80%;
        text-align: center;
    }
    input[type="text"], input[type="number"] {
        padding: 10px;
        margin: 5px;
        border: 1px solid #ccc;
        border-radius: 5px;
        transition: border-color 0.3s;
    }
    input[type="text"]:focus, input[type="number"]:focus {
        border-color: #007bff;
    }
    .form-group {
        margin-bottom: 15px;
    }
    .form-group label {
        display: block;
        margin-bottom: 5px;
    }
    .confirm-button {
        background-color: #dc3545;
    }
    .confirm-button:hover {
        background-color: #bd2130;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtSearch" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入编号或名称" />
            <asp:Button ID="btnSearchPriceList" runat="server" Text="价格表查询" OnClick="btnSearchPriceList_Click" />
                    <asp:Literal ID="LiteralPriceList" runat="server"></asp:Literal>
        </div>
        <div>
            <asp:TextBox ID="txtSearchIncoming" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入编号或名称" />
            <asp:Button ID="btnSearchIncoming" runat="server" Text="入库表查询" OnClick="btnSearchIncoming_Click" />
        </div>
        <div>
            <asp:TextBox ID="txtSearchOutgoing" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入编号或名称" />
            <asp:Button ID="btnSearchOutgoing" runat="server" Text="出库表查询" OnClick="btnSearchOutgoing_Click" />
        </div>
        <div>
            <asp:TextBox ID="txtOutgoingID" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入物品编号" />
            <asp:TextBox ID="txtQuantity" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入数量" />
            <asp:Button ID="btnOutgoing" runat="server" Text="出库" OnClick="btnOutgoing_Click" />
        </div>
        <div>
            <asp:TextBox ID="txtIncomingItemID" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入物品编号" />
            <asp:TextBox ID="txtIncomingQuantity" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入数量" />
            <asp:Button ID="btnAddIncoming" runat="server" Text="入库" OnClick="btnAddIncoming_Click" />
        </div>
        <div>
            <asp:TextBox ID="txtUpdateItemID" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入编号" />
            <asp:TextBox ID="txtNewPrice" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入新价格" />
            <asp:Button ID="btnUpdatePrice" runat="server" Text="修改价格" OnClick="btnUpdatePrice_Click" />
        </div>
          <div>
      <asp:TextBox ID="TextBox1" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入编号"></asp:TextBox>
      <asp:Button ID="btnDeletePrice" runat="server" Text="删除价格表数据" OnClick="btnDeletePrice_Click" />
      <asp:Literal ID="litMessage" runat="server"></asp:Literal>
  </div>
        <div>
            <asp:TextBox ID="txtItemID" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入编号" />
            <asp:TextBox ID="txtName" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入名称" />
            <asp:TextBox ID="txtCategory" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入类别" />
            <asp:TextBox ID="txtBrand" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入品牌" />
            <asp:TextBox ID="txtPrice" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入价格" />
            <asp:TextBox ID="txtStock" runat="server" Width="200px" style="text-align: left; padding: 5px; border: 1px solid #ccc;" placeholder="输入库存" />
            <asp:Button ID="btnAddProduct" runat="server" Text="添加商品" OnClick="btnAddProduct_Click" />
            <asp:Literal ID="literalMessage" runat="server" />
        </div>
      
        <div id="results" runat="server">
            <!-- 查询结果将显示在这里 -->
        </div>
    </form>
    <script type="text/javascript">
    function confirmOutgoing() {
        return confirm("确定要出库吗?");
    }
    </script>
</body>
</html>
