// 确认注册并关闭弹窗（同时处理页面跳转）
function confirmRegistrationAndCloseModal() {
    // 使用jQuery发送AJAX请求
    $.ajax({
        type: "POST", // 使用POST方法
        url: "YourPage.aspx/ButtonRegister_Click", // 服务器端处理注册的URL
        data: {
            username: TextBoxUsername.value,
            account: TextBoxAccount.value,
            password: TextBoxPassword.value,
            gender: DropDownListGender.value,
            address: TextBoxAddress.value
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // 假设服务器返回的是JSON格式的成功消息
            if (response.d) {
                alert('注册成功！');
                closeModal(); // 关闭弹窗
                window.location.href = 'your-success-page.aspx'; // 跳转到成功页面
            } else {
                // 处理错误情况，显示错误信息
                alert('注册失败：' + response.d);
            }
        },
        error: function (xhr, status, error) {
            // 处理请求失败的情况
            alert('请求失败：' + error);
        }
    });
}