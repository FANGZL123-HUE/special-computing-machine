using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1._3
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private static string connectionString = "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True;";

        protected void btnSearchPriceList_Click(object sender, EventArgs e)
        {
            // 获取用户输入的搜索条件
            string searchCriteria = txtSearch.Text;
            // 调用查询方法，传入搜索条件
            QueryPriceList(searchCriteria);
        }
        protected void btnSearchIncoming_Click(object sender, EventArgs e)
        {
            // 获取用户输入的搜索条件
            string searchCriteria = txtSearchIncoming.Text;
            // 调用查询方法，传入搜索条件
            QueryIncomingList(searchCriteria);
        }
        protected void btnSearchOutgoing_Click(object sender, EventArgs e)
        {
            // 获取用户输入的搜索条件
            string searchCriteria = txtSearchOutgoing.Text;
            // 调用查询方法，传入搜索条件
            QueryOutgoingList(searchCriteria);
        }
        private void QueryPriceList(string searchCriteria)
        {
            // 实现查询价格表的逻辑，使用 searchCriteria 作为查询条件
            // 显示结果到 results 控件
            DisplayQueryResults("价格表", searchCriteria);
        }
        private void QueryIncomingList(string searchCriteria)
        {
            // 实现查询入库表的逻辑，使用 searchCriteria 作为查询条件
            // 显示结果到 results 控件
            DisplayQueryResults("入库表", searchCriteria);
        }
        private void QueryOutgoingList(string searchCriteria)
        {
            // 实现查询出库表的逻辑，使用 searchCriteria 作为查询条件
            // 显示结果到 results 控件
            DisplayQueryResults("出库表", searchCriteria);
        }
        private void DisplayQueryResults(string tableName, string searchCriteria)
        {
            // 清除之前的反馈信息
            LiteralPriceList.Text = string.Empty;
            // 调用查询方法，传入搜索条件
            string query = $"SELECT * FROM {tableName}";
            if (!string.IsNullOrEmpty(searchCriteria))
            {
                query += " WHERE ItemID LIKE @SearchCriteria OR Name LIKE @SearchCriteria";
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(searchCriteria))
                        {
                            cmd.Parameters.AddWithValue("@SearchCriteria", $"%{searchCriteria}%");
                        }
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            if (dt.Rows.Count == 0)
                            {
                                // 如果没有结果，设置反馈信息
                                LiteralPriceList.Text = "没有找到匹配的编号。";
                            }
                            else
                            {
                                GridView gvResults = new GridView();
                                gvResults.DataSource = dt;
                                gvResults.DataBind();
                                results.Controls.Add(gvResults);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LiteralPriceList.Text = $"查询出错: {ex.Message}";
                }
            }
        }

        protected void btnOutgoing_Click(object sender, EventArgs e)
        {
            // 获取用户输入的物品编号和数量
            string itemIDText = txtOutgoingID.Text.Trim();
            string quantityText = txtQuantity.Text.Trim();
            // 检查文本是否为空
            if (string.IsNullOrEmpty(itemIDText) || string.IsNullOrEmpty(quantityText))
            {
                // 显示错误消息
                ScriptManager.RegisterStartupScript(this, GetType(), "InputError", "alert('输入不能为空！');", true);
                return; // 结束方法执行
            }
            int itemID;
            int quantity;
            // 尝试将文本转换为整数
            try
            {
                itemID = int.Parse(itemIDText);
                quantity = int.Parse(quantityText);
            }
            catch (FormatException)
            {
                // 如果转换失败，显示错误消息
                ScriptManager.RegisterStartupScript(this, GetType(), "ParseError", "alert('输入的编号或数量无效！');", true);
                return; // 结束方法执行
            }
            // 调用出库方法
            try
            {
                OutgoingItems(itemID, quantity);
                // 出库成功后调用JavaScript函数显示弹窗
                ScriptManager.RegisterStartupScript(this, GetType(), "OutgoingSuccess", "alert('出库成功！');", true);
            }
            catch (Exception ex)
            {
                // 出库失败时调用JavaScript函数显示弹窗
                ScriptManager.RegisterStartupScript(this, GetType(), "OutgoingError", $"alert('出库失败: {ex.Message}');", true);
            }
        }
        private void OutgoingItems(int itemID, int quantity)
        {
            // 这里调用你提供的方法，需要修改方法签名以接收数据库连接字符串
            string connectionString = "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // 更新库存
                    string updateStockQuery = "UPDATE 价格表 SET Stock = Stock - @Quantity WHERE ItemID = @ItemID AND Stock >= @Quantity";
                    using (SqlCommand updateCmd = new SqlCommand(updateStockQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateCmd.Parameters.AddWithValue("@ItemID", itemID);
                        if (updateCmd.ExecuteNonQuery() == 0)
                        {
                            throw new Exception("库存不足，出库失败。");
                        }
                    }
                    // 插入出库记录
                    string selectQuery = "SELECT Name, Category, Brand FROM 价格表 WHERE ItemID = @ItemID";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, conn))
                    {
                        selectCommand.Parameters.AddWithValue("@ItemID", itemID);
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read()) // 假设只有一个匹配记录或者说我们只关心第一个匹配记录
                            {
                                string name = reader.GetString(0); //名称
                                string category = reader.GetString(1); // 种类
                                string brand = reader.GetString(2); // 品牌
                                                                    // 关闭 reader 以避免冲突
                                reader.Close();
                                // 将数据插入Outgoing表
                                string insertQuery = "INSERT INTO 出库表 (ItemID, Name, Category, Brand, Quantity, OutgoingDate) VALUES (@ItemID, @Name, @Category, @Brand, @Quantity, GETDATE())";
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn))
                                {
                                    insertCommand.Parameters.AddWithValue("@ItemID", itemID);
                                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    insertCommand.Parameters.AddWithValue("@Category", category);
                                    insertCommand.Parameters.AddWithValue("@Brand", brand);
                                    insertCommand.Parameters.AddWithValue("@Name", name);
                                    if (insertCommand.ExecuteNonQuery() > 0)
                                    {
                                        // 出库成功，不需要做任何事情，因为已经在btnOutgoing_Click中处理了弹窗
                                    }
                                    else
                                    {
                                        throw new Exception("没有物品出库。");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("没有找到匹配的编号或数量不足。");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 出库失败，不需要做任何事情，因为已经在btnOutgoing_Click中处理了弹窗
                throw ex;
            }
        }
        protected void btnAddIncoming_Click(object sender, EventArgs e)
        {
            // 获取用户输入的物品编号和数量
            string itemIDText = txtIncomingItemID.Text.Trim();
            string quantityText = txtIncomingQuantity.Text.Trim();
            // 检查文本是否为空
            if (string.IsNullOrEmpty(itemIDText) || string.IsNullOrEmpty(quantityText))
            {
                // 显示错误消息
                ScriptManager.RegisterStartupScript(this, GetType(), "InputError", "alert('输入不能为空！');", true);
                return; // 结束方法执行
            }
            int itemID;
            int quantity;
            // 尝试将文本转换为整数
            try
            {
                itemID = int.Parse(itemIDText);
                quantity = int.Parse(quantityText);
            }
            catch (FormatException)
            {
                // 如果转换失败，显示错误消息
                ScriptManager.RegisterStartupScript(this, GetType(), "ParseError", "alert('输入的编号或数量无效！');", true);
                return; // 结束方法执行
            }
            // 调用入库方法
            try
            {
                AddIncomingItems(itemID, quantity);
                // 入库成功后调用JavaScript函数显示弹窗
                ScriptManager.RegisterStartupScript(this, GetType(), "IncomingSuccess", "alert('入库成功！');", true);
            }
            catch (Exception ex)
            {
                //// 入库失败时调用JavaScript函数显示弹窗
                ScriptManager.RegisterStartupScript(this, GetType(), "IncomingError", $"alert('入库失败: {ex.Message}');", true);
            }
        }

        private void AddIncomingItems(int itemID, int quantity)
        {
            // 这里调用你提供的方法，需要修改方法签名以接收数据库连接字符串
            string connectionString = "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // 更新库存
                    string updateStockQuery = @"
                UPDATE 价格表
                SET Stock = Stock + @Quantity
                WHERE ItemID = @ItemID";
                    using (SqlCommand updateCmd = new SqlCommand(updateStockQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateCmd.Parameters.AddWithValue("@ItemID", itemID);
                        if (updateCmd.ExecuteNonQuery() == 0)
                        {
                            throw new Exception("更新库存失败。");
                        }
                    }
                    // 插入入库记录
                    string selectQuery = @"
                SELECT Name, Category, Brand
                FROM 价格表
                WHERE ItemID = @ItemID";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, conn))
                    {
                        selectCommand.Parameters.AddWithValue("@ItemID", itemID);
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read()) // 假设只有一个匹配记录或者说我们只关心第一个匹配记录
                            {
                                string name = reader.GetString(0); // 名称
                                string category = reader.GetString(1); // 种类
                                string brand = reader.GetString(2); // 品牌
                                                                    // 关闭 reader 以避免冲突
                                reader.Close();
                                // 将数据插入入库表
                                string insertQuery = @"
                            INSERT INTO 入库表 (ItemID, Name, Category, Brand, Quantity, IncomingDate)
                            VALUES (@ItemID, @Name, @Category, @Brand, @Quantity, GETDATE())";
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn))
                                {
                                    insertCommand.Parameters.AddWithValue("@ItemID", itemID);
                                    insertCommand.Parameters.AddWithValue("@Name", name);
                                    insertCommand.Parameters.AddWithValue("@Category", category);
                                    insertCommand.Parameters.AddWithValue("@Brand", brand);
                                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    if (insertCommand.ExecuteNonQuery() > 0)
                                    {
                                        // 入库成功
                                    }
                                    else
                                    {
                                        throw new Exception("没有物品入库。");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("没有找到匹配的编号。");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 异常处理
                throw ex;
            }
        }


        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemID.Text) || string.IsNullOrEmpty(txtName.Text) ||
        string.IsNullOrEmpty(txtCategory.Text) || string.IsNullOrEmpty(txtBrand.Text) ||
        string.IsNullOrEmpty(txtPrice.Text) || string.IsNullOrEmpty(txtStock.Text))
            {
                // 如果有任何字段为空，显示错误消息
                ShowMessage("所有字段都是必填的，请确保没有字段为空！");
                return; // 结束方法执行
            }
            try
            {
                // 获取用户输入的数据
                string itemID = txtItemID.Text;
                string name = txtName.Text;
                string category = txtCategory.Text;
                string brand = txtBrand.Text;
                string price = txtPrice.Text;
                string stock = txtStock.Text;
                // 添加商品到数据库的逻辑
                AddProduct(itemID, name, category, brand, price, stock);
                // 显示商品添加成功消息
                ShowMessage("商品添加成功！");
            }
            catch (Exception ex)
            {
                // 显示商品添加失败消息
                ShowMessage($"商品添加失败: {ex.Message}");
            }
        }
        private void AddProduct(string itemID, string name, string category, string brand, string price, string stock)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "SET IDENTITY_INSERT 价格表 ON;\r\nINSERT INTO 价格表 (ItemID, Name, Category, Brand, Price, Stock) VALUES (@ItemID, @Name, @Category, @Brand, @Price, @Stock);";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        throw new Exception("添加商品失败，可能是因为输入的数据无效或数据库操作出错。");
                    }
                }
            }
        }
        private void ShowMessage(string message)
        {
            // 使用Literal控件显示消息
            Literal messageLiteral = new Literal();
            messageLiteral.Text = $"<div style='color: red'>{message}</div>";
            Page.Controls.Add(messageLiteral);
        }



        protected void btnUpdatePrice_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的商品编号和新价格
                string itemID = txtUpdateItemID.Text;
                string newPrice = txtNewPrice.Text;
                // 更新价格表中的价格
                UpdatePrice(itemID, newPrice);
                // 显示价格更新成功消息
                ShowMessage("价格更新成功！");
            }
            catch (Exception ex)
            {
                // 显示价格更新失败消息
                ShowMessage($"价格更新失败: {ex.Message}");
            }
        }
        private void UpdatePrice(string itemID, string newPrice)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE 价格表 SET Price = @NewPrice WHERE ItemID = @ItemID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@NewPrice", newPrice);
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        throw new Exception("更新价格失败，可能是因为输入的商品编号不存在或新价格无效。");
                    }
                }
            }
        }

        protected void btnDeletePrice_Click(object sender, EventArgs e)
        {
            // 获取用户输入的物品编号
            string itemID = TextBox1.Text;
            // 调用删除方法
            DeletePriceRecord(itemID);
        }
        private void DeletePriceRecord(string itemID)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // 删除价格表中的记录
                    string deleteQuery = "DELETE FROM 价格表 WHERE ItemID = @ItemID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", itemID);
                        int result = cmd.ExecuteNonQuery();
                        if (result <= 0)
                        {
                            throw new Exception("没有找到匹配的编号，删除失败。");
                        }
                        else
                        {
                            // 删除成功，可以在这里添加代码以更新UI或通知用户
                            litMessage.Text = "编号对应的记录已成功删除。";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 显示错误消息
                litMessage.Text = $"删除失败: {ex.Message}";
            }
        }


    }
}