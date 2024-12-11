<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="_1._3.WebForm1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>注册页面</title>
<style>
    body {
        background-color: #f0f0f0; /* 浅色背景 */
        font-weight: bold; /* 文字加粗 */
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh;
        margin: 0;
        /* 大背景图 */
        background-image:  url('1.3/zc1.jpg'); /* 替换为您的大背景图片路径 */
        background-size: cover; /* 覆盖整个背景 */
        background-position: center; /* 图片居中 */
        background-repeat: no-repeat; /* 防止图片重复 */
    }
    .form-container {
        width: 100%;
        max-width: 400px; /* 可以根据需要调整最大宽度 */
        aspect-ratio: 1 / 1.5; /* 设置为正方形 */
        margin-bottom: 10px;
        position: relative; /* 为了定位内部图片 */
        display: flex;
        flex-direction: column; /* 使输入框垂直排列 */
        align-items: center; /* 输入框居中对齐 */
        padding: 20px; /* 可选，为表单组添加一些内边距 */
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* 可选，为表单组添加阴影以增强层次感 */
        background-color: rgba(255, 255, 255, 0.5); /* 添加半透明背景，颜色浅度50 */
        border-radius: 10px; /* 圆角边框 */
        overflow: hidden; /* 确保内容不会溢出正方形边界 */
    }
    /* 在.form-container内部添加一个小背景图（原本在.form-group::before中的样式） */
    .form-container::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-image: url('1.3/zc1.jpg'); /* 背景图片路径 */
        background-size: cover; /* 覆盖整个背景 */
        background-repeat: no-repeat; /* 防止图片重复 */
        z-index: -1; /* 确保图片在表单内容下方 */
        border-radius: 10px; /* 圆角边框 */
    }
    .form-group {
        width: 100%;
        margin-bottom: 10px;
        text-align: center; /* 为了使label和输入框居中，可以直接在这里设置 */
    }
    .form-group label {
        margin-bottom: 5px;
    }
    .form-group input[type="text"],
    .form-group input[type="password"],
    .form-group select {
        width: calc(100% - 40px); /* 减去一些宽度以留出内边距的空间，这里假设左右各20px内边距 */
        text-align: center; /* 文本居中 */
    }
    .form-group select {
        margin-bottom: 10px; /* 下拉菜单下方的间距 */
    }
    .form-group button {
        width: calc(100% - 40px); /* 同上，减去一些宽度 */
        padding: 10px;
        background-color: #4CAF50; /* 绿色背景 */
        color: white; /* 白色文字 */
        border: none; /* 无边框 */
        border-radius: 4px; /* 圆角 */
        cursor: pointer; /* 鼠标指针变为手形 */
        font-size: 16px; /* 字体大小 */
        margin-top: 10px; /* 上方间距 */
    }
    .form-group button:hover {
        background-color: #45a049; /* 鼠标悬停时背景色变深 */
    }
        #movingImage {
            cursor: pointer; 
    position: absolute; /* 绝对定位 */
    top: 0; /* 初始位置（可以根据需要调整） */
    left: 0; /* 初始位置（可以根据需要调整） */
    width: calc(100vw / 10); /* 宽度设置为视口的十五分之一 */
    height: calc(100vh / 10); /* 高度设置为视口的十五分之一 */
    opacity: 0.6; /* 透明度设置为百分之六十 */
}
  /* 简单的弹窗样式 */
        #modalOverlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            display: none;
            justify-content: center;
            align-items: center;
            z-index: 1000;
        }
        #modalContent {
            background-color: white;
            padding: 20px;
            border-radius: 5px;
            text-align: center;
            position: relative;
        }
        #modalClose {
            position: absolute;
            top: 10px;
            right: 10px;
            cursor: pointer;
        }
       .avatar-preview {
    width: 100px; /* 设置为正方形 */
    height: 100px; /* 设置为正方形 */
    border: 1px solid #ccc;
    margin-top: 10px;
    /* 可以添加更多样式来美化预览框 */

}
    </style>
    <script type="text/javascript">
        function showModal() {
            var overlay = document.getElementById('modalOverlay');
            overlay.style.display = 'block'; // 或者 'flex'，取决于你的CSS
        }
        function closeModal() {
            var overlay = document.getElementById('modalOverlay');
            overlay.style.display = 'none';
        }
        function registerResult(result) {
            if (result === "Registered") {
                // 如果结果是注册成功
                alert('注册成功！');
                closeModal(); // 关闭模态框
            } else {
                // 如果结果是错误
                alert('注册失败！');
            }
        }
        function button1ClickedAndTriggerServer() {
         
            // 处理按钮1的点击事件
        /*    alert('注册成功');*/
            closeModal();
            // 触发隐藏按钮的点击事件，这将导致页面回发并调用服务器端的事件处理程序
           
            document.getElementById('<%= ButtonRegisterHidden.ClientID %>').click();
          
       }
        function button2Clicked() {
            // 处理按钮2的点击事件
            alert('取消注册');
            closeModal();
        }
    </script>

</head>
<body>
<form id="form1" runat="server">
<div>
    <img id="movingImage" src="zc1.jpg" alt="Moving Image" />
</div>
    <div class="form-container">   
      <div class="form-group">
    <label for="avatar">上传头像:</label>
    <asp:FileUpload ID="FileUploadAvatar" runat="server" OnUploadedComplete="FileUploadAvatar_UploadedComplete" />
    <asp:Image ID="ImageAvatarPreview" runat="server" CssClass="avatar-preview" Visible="false" />
    <div id="imgAvatarPreviewContainer" style="display: flex; justify-content: center; align-items: center; width: 100%; height: 100px;">
        <img id="imgAvatarPreview" class="avatar-preview" alt="Avatar Preview" style="max-width: 100%; max-height: 100%; object-fit: cover;" />
    </div>
</div>
        <div class="form-group">
            <label for="username">用户名:</label>
            <asp:TextBox ID="TextBoxUsername" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="account">账号:</label>
            <asp:TextBox ID="TextBoxAccount" runat="server" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="password">密码:</label>
            <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="gender">性别:</label>
            <asp:DropDownList ID="DropDownListGender" runat="server">
                <asp:ListItem Text="男" Value="男"></asp:ListItem>
                <asp:ListItem Text="女" Value="女"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="address">收货地址:</label>
            <asp:TextBox ID="TextBoxAddress" runat="server"></asp:TextBox>
        </div>
         <div class="form-group">
    <!-- 这个按钮将不会直接显示给用户 -->
    <asp:Button ID="ButtonRegisterHidden" runat="server" Text="Hidden Register Button" Style="display:none;" OnClick="ButtonRegister_Click" />
    <!-- 用户将点击这个按钮来显示模态框 -->
    <asp:Button ID="ButtonShowModal" runat="server" Text="注册" OnClientClick="showModal(); return false;" />
</div>
<!-- 弹窗内容 -->
<div id="modalOverlay" style="display:none;">
    <div id="modalContent">
        <span id="modalClose" onclick="closeModal()">&times;</span>
        <p>是否确定注册？</p>
        <!-- 移除 OnClientClick 属性 -->
        <asp:Button ID="ButtonConfirmRegister" runat="server" Text="确定注册" Style="display:none;" />
        <button type="button" onclick="button1ClickedAndTriggerServer()">确定注册</button>
        <button onclick="button2Clicked()">取消注册</button>
    </div>
</div>

    </div>
</form>
    <script type="text/javascript">
        // 获取默认图片的路径
        var defaultImagePath = "<%= ResolveUrl("~/1.3/zc1.jpg") %>";
    // 当文件选择器的值发生变化时触发
    document.getElementById('<%= FileUploadAvatar.ClientID %>').addEventListener('change', function (e) {
            var file = this.files[0];
            var imgPreview = document.getElementById('imgAvatarPreview');
            var imgPreviewContainer = document.getElementById('imgAvatarPreviewContainer');
            // 如果选择了文件，则读取文件
            if (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    imgPreview.src = e.target.result;
                    imgPreview.style.display = 'block';
                    imgPreviewContainer.style.display = 'flex'; // 显示预览容器
                };
                reader.readAsDataURL(file);
            } else {
                // 如果没有选择文件，则显示默认图片
                imgPreview.src = defaultImagePath;
                imgPreview.style.display = 'block';
                imgPreviewContainer.style.display = 'flex'; // 显示预览容器
            }
        });
        // 页面加载完成后，如果默认图片未显示，则显示默认图片
        window.onload = function () {
            var imgPreview = document.getElementById('imgAvatarPreview');
            var imgPreviewContainer = document.getElementById('imgAvatarPreviewContainer');
            // 检查img元素是否已经设置了src属性（即是否有文件被选中）
            if (!imgPreview.src) {
                imgPreview.src = defaultImagePath;
                imgPreview.style.display = 'block';
                imgPreviewContainer.style.display = 'flex'; // 显示预览容器
            }
        };
    </script>
        <script>
           
            // JavaScript代码
            var image = document.getElementById('movingImage');

                image.addEventListener('click', function() {
        // 指定点击后要跳转的网页URL
        var targetUrl = 'https://www.example.com'; // 请将此URL替换为你想要跳转的实际URL
                // 跳转到指定网页
                window.location.href = targetUrl;
    });
            var positionX = 0;
            var positionY = 0;
            var stepX = Math.random() * 10 - 5; // 初始化一个随机的X轴移动步长，范围在-5到5之间
            var stepY = Math.random() * 10 - 5; // 初始化一个随机的Y轴移动步长，范围在-5到5之间
            var speed = 100; // 移动和回弹的基准速度，单位毫秒
            function getRandomStep() {
                // 生成一个随机的步长，方向和大小都是随机的
                return {
                    x: (Math.random() * 2 - 1) * (Math.random() * 10 + 5), // 范围在-15到15之间，但更偏向于小值
                    y: (Math.random() * 2 - 1) * (Math.random() * 10 + 5)  // 同上
                };
            }
            function moveImage() {
                positionX += stepX;
                positionY += stepY;
                // 更新图片的位置
                image.style.left = positionX + 'px';
                image.style.top = positionY + 'px';
                // 检查是否撞到边缘，并调整步长以实现回弹
                if (positionX > window.innerWidth - image.offsetWidth || positionX < 0) {
                    // X轴撞到边缘，生成新的随机步长并反转X方向
                    var newStep = getRandomStep();
                    stepX = -newStep.x;
                }
                if (positionY > window.innerHeight - image.offsetHeight || positionY < 0) {
                    // Y轴撞到边缘，生成新的随机步长并反转Y方向
                    var newStep = getRandomStep();
                    stepY = -newStep.y;
                }
                // 可以选择性地添加一些逻辑来限制回弹的速度或步长的最大值
            }
            // 使用setInterval定时器每隔speed毫秒移动图片
            setInterval(moveImage, speed);
        </script>
</body>
    </html>