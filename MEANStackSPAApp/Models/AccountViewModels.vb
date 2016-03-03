Imports System.ComponentModel.DataAnnotations

Public Class ExternalLoginConfirmationViewModel
    <Required>
    <Display(Name:="Correo electrónico")>
    Public Property Email As String

    <Display(Name:="Ciudad natal")>
    Public Property Hometown As String
End Class

Public Class ExternalLoginListViewModel
    Public Property ReturnUrl As String
End Class

Public Class SendCodeViewModel
    Public Property SelectedProvider As String
    Public Property Providers As ICollection(Of System.Web.Mvc.SelectListItem)
    Public Property ReturnUrl As String
    Public Property RememberMe As Boolean
End Class

Public Class VerifyCodeViewModel
    <Required>
    Public Property Provider As String
    
    <Required>
    <Display(Name:="Código")>
    Public Property Code As String
    
    Public Property ReturnUrl As String
    
    <Display(Name:="¿Recordar este explorador?")>
    Public Property RememberBrowser As Boolean
    
    Public Property RememberMe As Boolean
End Class

Public Class ForgotViewModel
    <Required>
    <Display(Name:="Correo electrónico")>
    Public Property Email As String
End Class

Public Class LoginViewModel
    <Required>
    <Display(Name:="Correo electrónico")>
    <EmailAddress>
    Public Property Email As String

    <Required>
    <DataType(DataType.Password)>
    <Display(Name:="Contraseña")>
    Public Property Password As String

    <Display(Name:="¿Recordar cuenta?")>
    Public Property RememberMe As Boolean
End Class

Public Class RegisterViewModel
    <Required>
    <EmailAddress>
    <Display(Name:="Correo electrónico")>
    Public Property Email As String

    <Required>
    <StringLength(100, ErrorMessage:="El número de caracteres de {0} debe ser al menos {2}.", MinimumLength:=6)>
    <DataType(DataType.Password)>
    <Display(Name:="Contraseña")>
    Public Property Password As String

    <DataType(DataType.Password)>
    <Display(Name:="Confirmar contraseña")>
    <Compare("Password", ErrorMessage:="La contraseña y la contraseña de confirmación no coinciden.")>
    Public Property ConfirmPassword As String

    <Display(Name:="Ciudad natal")>
    Public Property Hometown As String
End Class

Public Class ResetPasswordViewModel
    <Required>
    <EmailAddress>
    <Display(Name:="Correo electrónico")>
    Public Property Email() As String

    <Required>
    <StringLength(100, ErrorMessage:="El número de caracteres de {0} debe ser al menos {2}.", MinimumLength:=6)>
    <DataType(DataType.Password)>
    <Display(Name:="Contraseña")>
    Public Property Password() As String

    <DataType(DataType.Password)>
    <Display(Name:="Confirmar contraseña")>
    <Compare("Password", ErrorMessage:="La contraseña y la contraseña de confirmación no coinciden.")>
    Public Property ConfirmPassword() As String

    Public Property Code() As String
End Class

Public Class ForgotPasswordViewModel
    <Required>
    <EmailAddress>
    <Display(Name:="Correo electrónico")>
    Public Property Email() As String
End Class
