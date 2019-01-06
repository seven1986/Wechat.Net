# WeChat

Wechat.Net | WeChat.Net.Core
--------------- | ---------------
[![NuGet](https://img.shields.io/nuget/v/Wechat.Net.svg)](https://github.com/seven1986/Wechat.Net)|[![NuGet](https://img.shields.io/nuget/v/WeChat.Net.Core.svg)](https://github.com/seven1986/WeChat.Net.Core)

#### 1，初始化配置项

```csharp
 WeChatSDK.Initialize(
                "公众账号应用ID",
                "公众账号应用密钥",
                "微支付商户号",
                "微支付的key",
                @"微支付证书路径",
                "微信支付异步通知回调地址");
```

### 2，微信支付

```csharp
var p = new Pay();

// 【扫码付 NativePay】 
// 参数：订单号、金额、商品ID、商品名称
var result = p.NativePay("201701071230300001", 1, "10001", "测试商品A");
if (result.return_code.Equals("SUCCESS") && result.result_code.Equals("SUCCESS"))
{
    // 订单号
    var prepay_id = result.prepay_id;
    // 付款链接，将这个链接地址胜场二维码，用微信扫码支付
    var code_url = result.code_url;
}


// 【网页、公众号支付 H5Pay】
// 参数：订单号、订单金额、openid、商品名称
var result2=  p.H5Pay("201701071230300002", 1, "10001", "测试商品A");
if (result2.return_code.Equals("SUCCESS") && result2.result_code.Equals("SUCCESS"))
{
    // 订单号
    var prepay_id = result2.prepay_id;
}

// 【APP支付 APPPay】
// 参数：设备号、订单号、订单金额、商品名称
var result3=  p.APPPay("1234567","201701071230300003", 1, "测试商品A");
if (result3.return_code.Equals("SUCCESS") && result3.result_code.Equals("SUCCESS"))
{
    // 订单号
    var prepay_id = result3.prepay_id;
}
```
