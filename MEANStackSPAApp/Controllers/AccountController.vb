Imports System.Globalization
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

<Authorize>
Public Class AccountController
    Inherits Controller
    Private _signInManager As ApplicationSignInManager
    Private _userManager As ApplicationUserManager

    Public Sub New()
    End Sub

    Public Sub New(appUserMan As ApplicationUserManager, signInMan As ApplicationSignInManager)
        UserManager = appUserMan
        SignInManager = signInMan
    End Sub

    Public Property SignInManager() As ApplicationSignInManager
        Get
            Return If(_signInManager, HttpContext.GetOwinContext().[Get](Of ApplicationSignInManager)())
        End Get
        Private Set
            _signInManager = value
        End Set
    End Property

    Public Property UserManager() As ApplicationUserManager
        Get
            Return If(_userManager, HttpContext.GetOwinContext().GetUserManager(Of ApplicationUserManager)())
        End Get
        Private Set
            _userManager = value
        End Set
    End Property

    ' La acción Authorize es el extremo que se invoca al acceder a cualquier
    ' Web API protegida. Si el usuario no ha iniciado sesión, se le redirigirá a 
    ' la página Login. Tras iniciar sesión correctamente, podrá invocar una Web API.
    <HttpGet>
    Public Function Authorize() As ActionResult
        Dim claims = New ClaimsPrincipal(User).Claims.ToArray()
        Dim identity = New ClaimsIdentity(claims, "Bearer")
        AuthenticationManager.SignIn(identity)
        Return New EmptyResult()
    End Function

    '
    ' GET: /Account/Login
    <AllowAnonymous>
    Public Function Login(returnUrl As String) As ActionResult
        ViewBag.ReturnUrl = returnUrl
        Return View()
    End Function

    '
    ' POST: /Account/Login
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function Login(model As LoginViewModel, returnUrl As String) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        ' No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
        ' Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout := True
        Dim result = Await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout := False)
        Select Case result
            Case SignInStatus.Success
                Return RedirectToLocal(returnUrl)
            Case SignInStatus.LockedOut
                Return View("Lockout")
            Case SignInStatus.RequiresVerification
                Return RedirectToAction("SendCode", New With {
                    .ReturnUrl = returnUrl,
                    .RememberMe = model.RememberMe
                })
            Case Else
                ModelState.AddModelError("", "Intento de inicio de sesión no válido.")
                Return View(model)
        End Select
    End Function

    '
    ' GET: /Account/VerifyCode
    <AllowAnonymous>
    Public Async Function VerifyCode(provider As String, returnUrl As String, rememberMe As Boolean) As Task(Of ActionResult)
        ' Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
        If Not Await SignInManager.HasBeenVerifiedAsync() Then
            Return View("Error")
        End If
        Return View(New VerifyCodeViewModel() With {
            .Provider = provider,
            .ReturnUrl = returnUrl,
            .RememberMe = rememberMe
        })
    End Function

    '
    ' POST: /Account/VerifyCode
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function VerifyCode(model As VerifyCodeViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        ' El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
        ' Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
        ' se bloqueará durante un período de tiempo especificado. 
        ' Puede configurar el bloqueo de la cuenta en IdentityConfig
        Dim result = Await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent := model.RememberMe, rememberBrowser := model.RememberBrowser)
        Select Case result
            Case SignInStatus.Success
                Return RedirectToLocal(model.ReturnUrl)
            Case SignInStatus.LockedOut
                Return View("Lockout")
            Case Else
                ModelState.AddModelError("", "Código no válido.")
                Return View(model)
        End Select
    End Function

    '
    ' GET: /Account/Register
    <AllowAnonymous>
    Public Function Register() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/Register
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function Register(model As RegisterViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = New ApplicationUser() With {
                .UserName = model.Email,
                .Email = model.Email,
                .Hometown = model.Hometown
            }
            Dim result = Await UserManager.CreateAsync(user, model.Password)
            If result.Succeeded Then
                Await SignInManager.SignInAsync(user, isPersistent := False, rememberBrowser := False)

                ' Para obtener más información sobre cómo habilitar la confirmación de la cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
                ' Enviar un correo electrónico con este vínculo
                ' Dim code = Await UserManager.GenerateEmailConfirmationTokenAsync(user.Id)
                ' Dim callbackUrl = Url.Action("ConfirmEmail", "Account", New With { .userId = user.Id, .code = code }, protocol := Request.Url.Scheme)
                ' Await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", "Para confirmar su cuenta, haga clic <a href=""" & callbackUrl & """>aquí</a>")

                Return RedirectToAction("Index", "Home")
            End If
            AddErrors(result)
        End If

        ' Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        Return View(model)
    End Function

    '
    ' GET: /Account/ConfirmEmail
    <AllowAnonymous>
    Public Async Function ConfirmEmail(userId As String, code As String) As Task(Of ActionResult)
        If userId Is Nothing OrElse code Is Nothing Then
            Return View("Error")
        End If
        Dim result = Await UserManager.ConfirmEmailAsync(userId, code)
        Return View(If(result.Succeeded, "ConfirmEmail", "Error"))
    End Function

    '
    ' GET: /Account/ForgotPassword
    <AllowAnonymous>
    Public Function ForgotPassword() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/ForgotPassword
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ForgotPassword(model As ForgotPasswordViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = Await UserManager.FindByNameAsync(model.Email)
            If user Is Nothing OrElse Not (Await UserManager.IsEmailConfirmedAsync(user.Id)) Then
                ' No revelar que el usuario no existe o que no está confirmado
                Return View("ForgotPasswordConfirmation")
            End If
            ' Para obtener más información sobre cómo habilitar la confirmación de la cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
            ' Enviar un correo electrónico con este vínculo
            ' Dim code = Await UserManager.GeneratePasswordResetTokenAsync(user.Id)
            ' Dim callbackUrl = Url.Action("ResetPassword", "Account", New With { .userId = user.Id, .code = code }, protocol := Request.Url.Scheme)
            ' Await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", "Restablezca su contraseña haciendo clic <a href=""" & callbackUrl & """>aquí</a>")
            ' Return RedirectToAction("ForgotPasswordConfirmation", "Account")
        End If

        ' Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        Return View(model)
    End Function

    '
    ' GET: /Account/ForgotPasswordConfirmation
    <AllowAnonymous>
    Public Function ForgotPasswordConfirmation() As ActionResult
        Return View()
    End Function

    '
    ' GET: /Account/ResetPassword
    <AllowAnonymous>
    Public Function ResetPassword(code As String) As ActionResult
        Return If(code Is Nothing, View("Error"), View())
    End Function

    '
    ' POST: /Account/ResetPassword
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ResetPassword(model As ResetPasswordViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If
        Dim user = Await UserManager.FindByNameAsync(model.Email)
        If user Is Nothing Then
            ' No revelar que el usuario no existe
            Return RedirectToAction("ResetPasswordConfirmation", "Account")
        End If
        Dim result = Await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password)
        If result.Succeeded Then
            Return RedirectToAction("ResetPasswordConfirmation", "Account")
        End If
        AddErrors(result)
        Return View()
    End Function

    '
    ' GET: /Account/ResetPasswordConfirmation
    <AllowAnonymous>
    Public Function ResetPasswordConfirmation() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/ExternalLogin
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Function ExternalLogin(provider As String, returnUrl As String) As ActionResult
        ' Solicitar redireccionamiento al proveedor de inicio de sesión externo
        Return New ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", New With {
            .ReturnUrl = returnUrl
        }))
    End Function

    '
    ' GET: /Account/SendCode
    <AllowAnonymous>
    Public Async Function SendCode(returnUrl As String, rememberMe As Boolean) As Task(Of ActionResult)
        Dim userId = Await SignInManager.GetVerifiedUserIdAsync()
        If userId Is Nothing Then
            Return View("Error")
        End If
        Dim userFactors = Await UserManager.GetValidTwoFactorProvidersAsync(userId)
        Dim factorOptions = userFactors.[Select](Function(purpose) New SelectListItem() With {
            .Text = purpose,
            .Value = purpose
        }).ToList()
        Return View(New SendCodeViewModel() With {
            .Providers = factorOptions,
            .ReturnUrl = returnUrl,
            .RememberMe = rememberMe
        })
    End Function

    '
    ' POST: /Account/SendCode
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function SendCode(model As SendCodeViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View()
        End If

        ' Generar el token y enviarlo
        If Not Await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider) Then
            Return View("Error")
        End If
        Return RedirectToAction("VerifyCode", New With { _
            .Provider = model.SelectedProvider,
            .ReturnUrl = model.ReturnUrl,
            .RememberMe = model.RememberMe
        })
    End Function

    '
    ' GET: /Account/ExternalLoginCallback
    <AllowAnonymous>
    Public Async Function ExternalLoginCallback(returnUrl As String) As Task(Of ActionResult)
        Dim loginInfo = Await AuthenticationManager.GetExternalLoginInfoAsync()
        If loginInfo Is Nothing Then
            Return RedirectToAction("Login")
        End If

        ' Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
        Dim result = Await SignInManager.ExternalSignInAsync(loginInfo, isPersistent := False)
        Select Case result
            Case SignInStatus.Success
                Return RedirectToLocal(returnUrl)
            Case SignInStatus.LockedOut
                Return View("Lockout")
            Case SignInStatus.RequiresVerification
                Return RedirectToAction("SendCode", New With {
                    .ReturnUrl = returnUrl,
                    .RememberMe = False
                })
            Case Else
                ' Si el usuario no tiene ninguna cuenta, solicitar que cree una
                ViewBag.ReturnUrl = returnUrl
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider
                Return View("ExternalLoginConfirmation", New ExternalLoginConfirmationViewModel() With {
                    .Email = loginInfo.Email
                })
        End Select
    End Function

    '
    ' POST: /Account/ExternalLoginConfirmation
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ExternalLoginConfirmation(model As ExternalLoginConfirmationViewModel, returnUrl As String) As Task(Of ActionResult)
        If User.Identity.IsAuthenticated Then
          Return RedirectToAction("Index", "Manage")
        End If

        If ModelState.IsValid Then
          ' Obtener datos del usuario del proveedor de inicio de sesión externo
          Dim info = Await AuthenticationManager.GetExternalLoginInfoAsync()
          If info Is Nothing Then
              Return View("ExternalLoginFailure")
          End If
          Dim userInfo = New ApplicationUser() With {
              .UserName = model.Email,
              .Email = model.Email,
              .Hometown = model.Hometown
          }
          Dim result = Await UserManager.CreateAsync(userInfo)
          If result.Succeeded Then
            result = Await UserManager.AddLoginAsync(userInfo.Id, info.Login)
            If result.Succeeded Then
                Await SignInManager.SignInAsync(userInfo, isPersistent := False, rememberBrowser := False)
                Return RedirectToLocal(returnUrl)
            End If
          End If
          AddErrors(result)
        End If

        ViewBag.ReturnUrl = returnUrl
        Return View(model)
    End Function

    '
    ' POST: /Account/LogOff
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Function LogOff() As ActionResult
        AuthenticationManager.SignOut()
        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' GET: /Account/ExternalLoginFailure
    <AllowAnonymous>
    Public Function ExternalLoginFailure() As ActionResult
        Return View()
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If _userManager IsNot Nothing Then
                _userManager.Dispose()
                _userManager = Nothing
            End If
            If _signInManager IsNot Nothing Then
                _signInManager.Dispose()
                _signInManager = Nothing
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub

    #Region "Aplicaciones auxiliares"
    ' Se usa para la protección XSRF al agregar inicios de sesión externos
    Private Const XsrfKey As String = "XsrfId"

    Private ReadOnly Property AuthenticationManager() As IAuthenticationManager
        Get
            Return HttpContext.GetOwinContext().Authentication
        End Get
    End Property

    Private Sub AddErrors(result As IdentityResult)
        For Each [error] In result.Errors
            ModelState.AddModelError("", [error])
        Next
    End Sub

    Private Function RedirectToLocal(returnUrl As String) As ActionResult
        If Url.IsLocalUrl(returnUrl) Then
            Return Redirect(returnUrl)
        End If
        Return RedirectToAction("Index", "Home")
    End Function

    Friend Class ChallengeResult
        Inherits HttpUnauthorizedResult
        Public Sub New(provider As String, redirectUri As String)
            Me.New(provider, redirectUri, Nothing)
        End Sub

        Public Sub New(provider As String, redirect As String, user As String)
            LoginProvider = provider
            RedirectUri = redirect
            UserId = user
        End Sub

        Public Property LoginProvider As String
        Public Property RedirectUri As String
        Public Property UserId As String

        Public Overrides Sub ExecuteResult(context As ControllerContext)
            Dim properties = New AuthenticationProperties() With {
                .RedirectUri = RedirectUri
            }
            If UserId IsNot Nothing Then
              properties.Dictionary(XsrfKey) = UserId
            End If
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider)
        End Sub
    End Class
    #End Region
End Class
