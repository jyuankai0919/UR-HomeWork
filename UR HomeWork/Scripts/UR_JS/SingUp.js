var VuePage = new Vue({
    el: '#VuePage'
    , data: function () {
        var data = {
            form: {}
        };
        return data;
    }
    , methods: {
        // 執行註冊按鈕
        DoRegister: function () {
            var self = this;

            // 組合表單資料
            var postData = {};
            postData['UserID'] = self.form.UserID;
            postData['UserPwd'] = self.form.UserPwd;
            postData['UserPwdChk'] = self.form.UserPwdChk;
            postData['UserName'] = self.form.UserName;
            postData['UserAdd'] = self.form.UserAdd;

            // 使用 Ajax 傳送至後端
            $.ajax({
                url: 'DoRegister',
                method: 'POST',
                dataType: 'json',
                data: { inModel: postData },
                success: function (datas) {
                    if (datas.ErrMsg) {
                        alert(datas.ErrMsg);
                        return;
                    }
                    alert(datas.ResultMsg);
                    window.location.href = 'LoginPage';
                },
                error: function (err) {
                    $('#ErrorMsg').html(err.responseText);
                    $('#ErrorAlert').modal('toggle');
                },
            });
        }
    }
})