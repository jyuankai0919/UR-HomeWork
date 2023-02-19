var VuePage = new Vue({
    el: '#VuePage'
    , data: function () {
        var data = {
            form: {}
        };
        return data;
    }
    , methods: {
        // 執行登入按鈕
        DoLogin: function () {
            var self = this;

            // 組合表單資料
            var postData = {};
            postData['UserID'] = self.form.UserID;
            postData['UserPwd'] = self.form.UserPwd;
            postData['KeepLogin'] = self.form.KeepLogin;
            // 使用 jQuery Ajax 傳送至後端
            $.ajax({
                url: '/Account/DoLogin',
                method: 'POST',
                dataType: 'json',
                data: { inModel: postData },
                success: function (datas) {
                    if (datas.ErrMsg) {
                        alert(datas.ErrMsg);
                        return;
                    }
                    alert(datas.ResultMsg);
                },
                error: function (err) {
                    $('#ErrorMsg').html(err.responseText);
                    $('#ErrorAlert').modal('toggle');
                },
            });
        },
        // 跳轉註冊按鈕
        register: function () {
            window.location.href = "/Home/SingUpPage";
        }
    }
    , mounted: function () {
        if (UserKeepLogin == "Y") {
            alert("已登錄");
            window.location.href = "/Account/AccountPage";
        }
    }
})