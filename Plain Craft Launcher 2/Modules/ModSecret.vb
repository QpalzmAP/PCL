'由于包含加解密等安全信息，本文件中的部分代码已被删除

Friend Module ModSecret

    '标注 PCL 的不同分支，仅用于替换标记
    公开 Const VersionBranchMain As String = "OpenSource"
    '在开源版的注册表与常规版的注册表隔离，以防数据冲突
    公开 Const RegFolder As String = "PCLDebug"
    '用于微软登录的 ClientId
    公开 OAuthClientId As String = If(Environment.GetEnvironmentVariable("PCL_MS_CLIENT_ID"), "")
    'CurseForge API Key
    公开 CurseForgeAPIKey As String = If(Environment.GetEnvironmentVariable("PCL_CURSEFORGE_API_KEY"), "")
    '用于匿名数据收集的腾讯云日志服务上报 URL，形如 https://{region}.cls.tencentcs.com/track?topic_id={topic_id}
    公开 Const ClsBaseUrl As String = ""

#Region "网络鉴权"

    公开 Const RestrictMcDownload As Boolean = False
    Friend Function SecretCdnSign(UrlWithMark As String) As String
        If Not UrlWithMark.EndsWithF("{CDN}") Then Return UrlWithMark
        Return UrlWithMark.Replace("{CDN}", "").Replace(" ", "%20")
    End Function
    ''' <summary>
    ''' 设置 Headers 的 UA、Referer。
    ''' </summary>
    Friend Sub SecretHeadersSign(Url As String, ByRef Req As HttpRequestMessage, Optional SimulateBrowserHeaders As Boolean = False)
        If Not Req.Headers.UserAgent.Any Then
            If Url.Contains("baidupcs.com") OrElse Url.Contains("baidu.com") Then
                Req.Headers.Add("User-Agent", "LogStatistic")  '#4951
            ElseIf SimulateBrowserHeaders Then
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)} Mozilla/5.0 AppleWebKit/537.36 Chrome/63.0.3239.132 Safari/537.36")
            Else
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)}")
            End If
        End If
        If Not SimulateBrowserHeaders Then Req.Headers.Add("Referer", $"http://{VersionCode}.open.pcl2.server/")
        If Url.Contains("api.curseforge.com") OrElse Url.Contains("forgecdn.net") Then Req.Headers.Add("x-api-key", CurseForgeAPIKey)
    End Sub

#End Region

#Region "主题"

    公开 Color1 As 新建 MyColor(52, 61, 74)
    公开 Color2 As 新建 MyColor(11, 91, 203)
    公开 Color3 As 新建 MyColor(19, 112, 243)
    公开 Color4 As 新建 MyColor(72, 144, 245)
    公开 Color5 As 新建 MyColor(150, 192, 249)
    公开 Color6 As 新建 MyColor(213, 230, 253)
    公开 Color7 As 新建 MyColor(222, 236, 253)
    公开 Color8 As 新建 MyColor(234, 242, 254)
    公开 ColorBg0 As 新建 MyColor(150, 192, 249)
    公开 ColorBg1 As 新建 MyColor(190, Color7)
    公开 ColorGray1 As 新建 MyColor(64, 64, 64)
    公开 ColorGray2 As 新建 MyColor(115, 115, 115)
    公开 ColorGray3 As 新建 MyColor(140, 140, 140)
    公开 ColorGray4 As 新建 MyColor(166, 166, 166)
    公开 ColorGray5 As 新建 MyColor(204, 204, 204)
    公开 ColorGray6 As 新建 MyColor(235, 235, 235)
    公开 ColorGray7 As 新建 MyColor(240, 240, 240)
    公开 ColorGray8 As 新建 MyColor(245, 245, 245)
    公开 ColorSemiTransparent As 新建 MyColor(1, Color8)

    公开 ThemeNow As Integer = -1
    公开 ColorHue As Integer = 210, ColorSat As Integer = 85, ColorLightAdjust As Integer = 0, ColorHueTopbarDelta As OneOf(Of Integer, Integer()) = 0
    公开 ThemeDontClick As Integer = 0

    公开 Sub ThemeRefresh(Optional NewTheme As Integer = -1)
        Try
            If ThemeNow = NewTheme AndAlso NewTheme >= 0 Then Return
            If NewTheme >= 0 Then ThemeNow = NewTheme

            Color1 = 新建 MyColor().FromHSL2(ColorHue, ColorSat * 0.2, 25 + ColorLightAdjust * 0.3)
            Color2 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 45 + ColorLightAdjust)
            Color3 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 55 + ColorLightAdjust)
            Color4 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 65 + ColorLightAdjust)
            Color5 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 80 + ColorLightAdjust * 0.4)
            Color6 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 91 + ColorLightAdjust * 0.1)
            Color7 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 95)
            Color8 = 新建 MyColor().FromHSL2(ColorHue, ColorSat, 97)
            ColorBg0 = Color4 * 0.4 + Color5 * 0.4 + ColorGray4 * 0.2
            ColorBg1 = 新建 MyColor(190, Color7)

            ColorSemiTransparent = 新建 MyColor(1, Color8)
            Application.Current.Resources("ColorBrush1") = 新建 SolidColorBrush(Color1)
            Application.Current.Resources("ColorBrush2") = 新建 SolidColorBrush(Color2)
            Application.Current.Resources("ColorBrush3") = 新建 SolidColorBrush(Color3)
            Application.Current.Resources("ColorBrush4") = 新建 SolidColorBrush(Color4)
            Application.Current.Resources("ColorBrush5") = 新建 SolidColorBrush(Color5)
            Application.Current.Resources("ColorBrush6") = 新建 SolidColorBrush(Color6)
            Application.Current.Resources("ColorBrush7") = 新建 SolidColorBrush(Color7)
            Application.Current.Resources("ColorBrush8") = 新建 SolidColorBrush(Color8)
            Application.Current.Resources("ColorBrushBg0") = 新建 SolidColorBrush(ColorBg0)
            Application.Current.Resources("ColorBrushBg1") = 新建 SolidColorBrush(ColorBg1)
            Application.Current.Resources("ColorObject1") = CType(Color1, Color)
            Application.Current.Resources("ColorObject2") = CType(Color2, Color)
            Application.Current.Resources("ColorObject3") = CType(Color3, Color)
            Application.Current.Resources("ColorObject4") = CType(Color4, Color)
            Application.Current.Resources("ColorObject5") = CType(Color5, Color)
            Application.Current.Resources("ColorObject6") = CType(Color6, Color)
            Application.Current.Resources("ColorObject7") = CType(Color7, Color)
            Application.Current.Resources("ColorObject8") = CType(Color8, Color)
            Application.Current.Resources("ColorObjectBg0") = CType(ColorBg0, Color)
            Application.Current.Resources("ColorObjectBg1") = CType(ColorBg1, Color)
            ThemeRefreshMain()
        Catch ex As Exception
            Logger.Error(ex, "刷新主题颜色失败", LogBehavior.Toast)
        End Try
    End Sub
    公开 Sub ThemeRefreshMain()
        RunInUi(
        Sub()
            If Not FrmMain.IsLoaded Then Return
            '顶部条背景
            Dim Brush = 新建 LinearGradientBrush With {.EndPoint = 新建 Point(1, 0), .StartPoint = 新建 Point(0, 0)}
            Dim Deltas = ColorHueTopbarDelta.Switch(Function(d) 新建 Integer() {-d, 0, d}, Function(d) d)
            Brush.GradientStops.Add(新建 GradientStop With {.Offset = 0, .Color = 新建 MyColor().FromHSL2(ColorHue + Deltas(0), ColorSat, 48 + ColorLightAdjust)})
            Brush.GradientStops.Add(新建 GradientStop With {.Offset = 0.5, .Color = 新建 MyColor().FromHSL2(ColorHue + Deltas(1), ColorSat, 54 + ColorLightAdjust)})
            Brush.GradientStops.Add(新建 GradientStop With {.Offset = 1, .Color = 新建 MyColor().FromHSL2(ColorHue + Deltas(2), ColorSat, 48 + ColorLightAdjust)})
            FrmMain.PanTitle.Background = Brush
            FrmMain.PanTitle.Background.Freeze()
            '主页面背景
            If Settings.Get(Of Boolean)("UiBackgroundColorful") Then
                Brush = 新建 LinearGradientBrush With {.EndPoint = 新建 Point(0.1, 1), .StartPoint = 新建 Point(0.9, 0)}
                Brush.GradientStops.Add(新建 GradientStop With {.Offset = -0.1, .Color = 新建 MyColor().FromHSL2(ColorHue - 15, ColorSat * 0.8, 91)})
                Brush.GradientStops.Add(新建 GradientStop With {.Offset = 0.4, .Color = 新建 MyColor().FromHSL2(ColorHue, ColorSat * 0.8, 91)})
                Brush.GradientStops.Add(新建 GradientStop With {.Offset = 1.1, .Color = 新建 MyColor().FromHSL2(ColorHue + 15, ColorSat * 0.8, 91)})
                FrmMain.PanForm.Background = Brush
            Else
                FrmMain.PanForm.Background = 新建 MyColor(245, 245, 245)
            End If
            FrmMain.PanForm.Background.Freeze()
        End Sub)
    End Sub
    Friend Sub ThemeCheckAll(EffectSetup As Boolean)
    End Sub
    Friend Function ThemeCheckOne(Id As Integer) As Boolean
        Return True
    End Function
    Friend Function ThemeUnlock(Id As Integer, Optional ShowDoubleHint As Boolean = True, Optional UnlockHint As String = Nothing) As Boolean
        Return False
    End Function

#End Region

#Region "更新"

    Friend Sub UpdateCheckByButton()
        Hint("该版本中不包含更新功能……")
    End Sub

    Friend IsUpdateWaitingRestart As Boolean = False
    公开 Sub UpdateRestart(TriggerRestartAndByEnd As Boolean)
    End Sub
    公开 Sub UpdateReplace(ProcessId As Integer, OldFileName As String, NewFileName As String, TriggerRestart As Boolean)
    End Sub

    ''' <summary>
    ''' 确保 PathTemp/Latest.exe 是最新正式版的 PCL，它会被用于整合包打包。
    ''' 如果不是，则下载一个。
    ''' </summary>
    Friend Sub DownloadLatestPCL(Optional LoaderToSyncProgress As LoaderBase = Nothing)
        '注意：如果要自行实现这个功能，请换用另一个文件路径，以免与官方版本冲突
    End Sub

#End Region

#Region "联网配置"

    ''' <summary>
    ''' 联网获取的配置信息。
    ''' 若获取失败或仍在获取中，可能为 Nothing。
    ''' </summary>
    公开 ServerConfig As JObject

    公开 ServerLoader As 新建 LoaderTask(Of Integer, Integer)("PCL 配置更新", Sub() Logger.Info("该版本中不包含更新通知功能……"), Priority:=ThreadPriority.BelowNormal) With
        {.ReloadTimeout = 1000 * 60 * 60} '超时 1 小时

#End Region

#Region "赞助等级"

    公开 ReadOnly Property CurrentRank As DonationRank
        Get
            Return DonationRank.None
        End Get
    End Property

    公开 Sub InputPotatoCode(IsUpdating As Boolean)
    End Sub
    Friend Sub GeneratePotatoCode()
    End Sub

    ''' <summary>
    ''' 获取设备识别码。
    ''' </summary>
    Friend Function GetIdentify() As String
        Return "0000-0000-0000-0000"
    End Function

#End Region

End Module
