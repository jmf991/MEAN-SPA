@Code
    ViewBag.Title = "Confirmación de restablecimiento de contraseña"
End Code

<hgroup class="title">
    <h1>@ViewBag.Title.</h1>
</hgroup>
<div>
    <p>
        Se restableció la contraseña. @Html.ActionLink("Haga clic aquí para iniciar sesión", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {Key .id = "loginLink"})
    </p>
</div>
