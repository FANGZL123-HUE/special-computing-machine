using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Data;
using System.IO;
namespace _1._3
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string connectionString = "Data Source=.;Initial Catalog=账号信息;Integrated Security=True;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBoxAccount.Text = GenerateRandomAccount();
            }
            // 指定要创建的子文件夹名称
            string subFolderName = "Images";
            // 获取 "1.3" 文件夹的路径
            string parentFolderPath = Server.MapPath("~/1.3");
            // 获取子文件夹的完整路径
            string subFolderPath = Path.Combine(parentFolderPath, subFolderName);
            // 确保在 "1.3" 文件夹内创建一个名为 "Images" 的子文件夹
            CreateFolderIfNotExists(subFolderPath);
        }
        private void CreateFolderIfNotExists(string folderPath)
        {
            // 检查文件夹是否存在
            if (!Directory.Exists(folderPath))
            {
                // 文件夹不存在，创建文件夹
                Directory.CreateDirectory(folderPath);
            }
        }


        private string GenerateRandomAccount()
        {
            Random random = new Random();
            char[] chars = "123456789".ToCharArray(); // 移除了字符'0'
            string account = "";
            for (int i = 0; i < 11; i++)
            {
                // 确保第一个字符不是'0'
                if (i == 0 && chars.Length > 1)
                {
                    account += chars[random.Next(1, chars.Length)]; // 从1开始，确保第一个字符不是'0'
                }
                else
                {
                    account += chars[random.Next(chars.Length)]; // 其他位置可以是任何字符
                }
            }
            // 检查账号是否已存在（这里只是简单示例，实际应查询数据库）
            if (IsAccountExist(account))
            {
                return GenerateRandomAccount(); // 如果已存在，则重新生成
            }
            return account;
        }

        private bool IsAccountExist(string account)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM sys.tables WHERE name = @tableName AND OBJECT_DEFINITION(OBJECT_ID) LIKE '%\"账号\" NVARCHAR(50) PRIMARY KEY,%'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tableName", "普通用户" + account); // 假设用户表名以用户名+账号构成
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }


        private bool IsUsernameOrAccountExist(string username, string account)
        {
            bool exists = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // 合并检查用户名和账号
                    string checkSql = "SELECT COUNT(*) FROM 用户对应账号 WHERE 用户名 = @username OR 账号 = @account";
                    using (SqlCommand cmd = new SqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@account", account);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        exists = count > 0; // 如果计数大于 0，则表示用户名或账号存在
                    }
                }
                catch (Exception ex)
                {
                    // 如果需要，这里处理异常
                    // 记录异常等
                    throw; // 在处理后需要重新抛出异常
                }
                finally
                {
                    // 确保即使发生异常也能关闭连接
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            return exists;
        }

        protected void ButtonRegister_Click(object sender, EventArgs e)
        {
            // 获取表单数据
            string username = TextBoxUsername.Text.Trim();
            string account = TextBoxAccount.Text.Trim();
            string password = TextBoxPassword.Text.Trim(); // 注意：密码应加密存储
            string gender = DropDownListGender.SelectedValue.Trim();
            string address = TextBoxAddress.Text.Trim();
            string avatarPath = string.Empty; // 用于存储上传的头像路径
                                              // 验证数据
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(address))
            {
                // 显示错误信息
                Response.Write("<script>alert('请填写所有必填字段！');</script>");
                return;
            }
            if (IsUsernameOrAccountExist(username, account))
            {
                // 显示弹窗提示用户或账号重复
                Response.Write("<script>alert('用户或账号已存在，请重新注册！');</script>");
                // 清空输入
                TextBoxUsername.Text = "";
                TextBoxAccount.Text = "";
                TextBoxPassword.Text = "";
                TextBoxAddress.Text = "";
                // 重新生成账号
                account = GenerateRandomAccount();
                TextBoxAccount.Text = account;
                return;
            }
            // 指定要创建的子文件夹名称为账号
            string subFolderName = account; // 从"Images"更改为account变量
                                            // 确保账号名称不包含创建文件夹时无效的字符
            subFolderName = Path.GetInvalidFileNameChars().Aggregate(subFolderName, (current, invalidChar) => current.Replace(invalidChar.ToString(), ""));
            // 确保账号名称不为空
            if (string.IsNullOrWhiteSpace(subFolderName))
            {
                Response.Write("<script>alert('账号名称不合法，无法创建文件夹。');</script>");
                return;
            }
            // 获取 "1.3" 文件夹的路径
            string parentFolderPath = Server.MapPath("~/1.3");
            // 获取子文件夹的完整路径
            string subFolderPath = Path.Combine(parentFolderPath, subFolderName);
            // 确保在 "1.3" 文件夹内创建一个名为账号的子文件夹
            CreateFolderIfNotExists(subFolderPath);
            // 检查是否有文件上传
            if (FileUploadAvatar.HasFile)
            {
                try
                {
                    // 获取文件名
                    string fileName = Path.GetFileName(FileUploadAvatar.FileName);
                    // 保存文件到服务器上的指定文件夹，即账号命名的文件夹
                    string filePath = Path.Combine(subFolderPath, fileName);
                    FileUploadAvatar.SaveAs(filePath);
                    // 更新头像路径
                    avatarPath = filePath;
                }
                catch (Exception ex)
                {
                    // 如果需要，这里处理异常
                    // 记录异常等
                    Response.Write("上传失败：" + ex.Message);
                    return;
                }
            }
                string tableName = "普通用户" + account;
                string createUserTableSql = @"
                CREATE TABLE " + tableName + @" (
                    用户名 NVARCHAR(50) PRIMARY KEY,
                    账号 NVARCHAR(50) NOT NULL,
                    密码 NVARCHAR(255) NOT NULL,
                    账号等级 INT NOT NULL DEFAULT 1,
                    性别 NVARCHAR(10) NOT NULL,
                    收货地址 NVARCHAR(255) NOT NULL,
                    历史密码 NVARCHAR(MAX) NULL,
                    账号余额 DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
                    信用等级 INT NOT NULL DEFAULT 550,
                    待收货 INT NOT NULL DEFAULT 0,
                    特发货 INT NOT NULL DEFAULT 0,
                    已收货 INT NOT NULL DEFAULT 0,
                    退款 INT NOT NULL DEFAULT 0,
                    售后 INT NOT NULL DEFAULT 0,
                    账号注册时间 DATETIME NOT NULL DEFAULT GETDATE(),
                    密码修改时间 DATETIME NOT NULL DEFAULT GETDATE(),
                    用户头像 NVARCHAR(255) NULL
                );
            ";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(createUserTableSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        string insertUserSql = @"
                        INSERT INTO " + tableName + @" (用户名, 账号, 密码, 性别, 收货地址, 用户头像)
                        VALUES (@username, @account, @password, @gender, @address, @avatarPath);
                    ";

                        using (SqlCommand cmd = new SqlCommand(insertUserSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@account", account);
                            cmd.Parameters.AddWithValue("@password", password); // 注意：密码应加密存储
                            cmd.Parameters.AddWithValue("@gender", gender);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@avatarPath", avatarPath); // 添加头像路径参数
                            cmd.ExecuteNonQuery();
                        }
                    string insertUserAccountMappingSql = @"
        INSERT INTO 用户对应账号 (用户名, 账号, 密码)
        VALUES (@username, @account, @password);
        ";
                    using (SqlCommand cmdMapping = new SqlCommand(insertUserAccountMappingSql, conn))
                    {
                        cmdMapping.Parameters.AddWithValue("@username", username);
                        cmdMapping.Parameters.AddWithValue("@account", account);
                        cmdMapping.Parameters.AddWithValue("@password", password); // 注意：密码应加密存储
                        cmdMapping.ExecuteNonQuery();
                        string result = "Registered";
                        Response.Write("<script>parent.registerResult('" + result + "');</script>");
                        Response.Redirect("HtmlPage2.html");
                    }
                   
                }
                    catch (Exception ex)
                    {
                        Response.Write("注册失败：" + ex.Message);
                    string result = "Error";
                    Response.Write("<script>parent.registerResult('" + result + "');</script>");
                }
                }
            
        }
        
    }
   
}
