---

## ⚠️ 注意事項：你必須自行設定 Gmail 寄信帳號！

本專案的寄信功能使用 Gmail SMTP，為了安全與隱私，**不會提供實際帳號與密碼**。  
請使用者自行申請並填入下列資訊：

### 🔑 步驟一：建立 Gmail 應用程式密碼

1. 登入你的 Gmail 帳號
2. 前往 [Google 帳戶安全設定](https://myaccount.google.com/security)
3. 開啟「兩步驟驗證」
4. 進入 [應用程式密碼](https://myaccount.google.com/apppasswords)
5. 建立新密碼（選擇「郵件」＋「Windows 電腦」或自行命名）
6. 複製產生的 **16 碼密碼**（只會顯示一次）

---

### ✏️ 步驟二：填入程式碼中指定位置

請打開：


並修改以下兩處：

```csharp
// 寄件人 Email（與登入帳號必須相同）
message.From.Add(new MailboxAddress("網站聯絡通知", "your_email@gmail.com"));

// 登入帳號與應用程式密碼
client.Authenticate("your_email@gmail.com", "your_app_password");
