@ModelType IndexViewModel
@Code
    ViewBag.Title = "Gestionar"
End Code

<h2>@ViewBag.Title.</h2>

<p class="text-success">@ViewBag.StatusMessage</p>
<div>
    <h4>Cambiar la configuración de la cuenta</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>Contraseña:</dt>
        <dd>
            [
            @If Model.HasPassword Then
                @Html.ActionLink("Cambiar la contraseña", "ChangePassword")
            Else
                @Html.ActionLink("Crear", "SetPassword")
            End If
            ]
        </dd>
        <dt>Inicios de sesión externos:</dt>
        <dd>
            @Model.Logins.Count [
            @Html.ActionLink("Administrar", "ManageLogins") ]
        </dd>
        @*
            Los números de teléfono se pueden usar como un segundo factor de comprobación en un sistema de autenticación en dos fases.
             
             Consulte <a href="http://go.microsoft.com/fwlink/?LinkId=403804">este artículo</a>
                para obtener más información acerca de la configuración de esta aplicación ASP.NET para admitir la autenticación en dos fases con SMS.
             
             Quite la marca de comentario del siguiente bloque una vez haya configurado la autenticación en dos fases
        *@
        @* 
            <dt>Número de teléfono:</dt>
            <dd>
                @(If(Model.PhoneNumber, "Ninguno")) [
                @If (Model.PhoneNumber <> Nothing) Then
                    @Html.ActionLink("Cambiar", "AddPhoneNumber")
                    @: &nbsp;|&nbsp;
                    @Html.ActionLink("Quitar", "RemovePhoneNumber")
                Else
                    @Html.ActionLink("Agregar", "AddPhoneNumber")
                End If
                ]
            </dd>
        *@
        <dt>Autenticación de dos factores:</dt>
        <dd>
            <p>
                No hay ningún proveedor de autenticación en dos fases configurado. Consulte <a href="http://go.microsoft.com/fwlink/?LinkId=403804">este artículo</a>
                para obtener más información acerca de la configuración de esta aplicación ASP.NET para admitir la autenticación en dos fases.
            </p>
            @*
                @If Model.TwoFactor Then
                    @Using Html.BeginForm("DisableTwoFactorAuthentication", "Manage", FormMethod.Post, New With { .class = "form-horizontal", .role = "form" })
                      @Html.AntiForgeryToken()
                      @<text>
                      Habilitado
                      <input type="submit" value="Deshabilitar" class="btn btn-link" />
                      </text>
                    End Using
                Else
                    @Using Html.BeginForm("EnableTwoFactorAuthentication", "Manage", FormMethod.Post, New With { .class = "form-horizontal", .role = "form" })
                      @Html.AntiForgeryToken()
                      @<text>
                      Deshabilitado
                      <input type="submit" value="Habilitar" class="btn btn-link" />
                      </text>
                    End Using
                End If
	     *@
        </dd>
    </dl>
</div>
