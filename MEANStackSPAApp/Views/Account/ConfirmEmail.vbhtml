@Code
    ViewBag.Title = "Confirmar correo electrónico"
End Code

<h2>@ViewBag.Title.</h2>
<div>
    <p>
        Gracias por confirmar su correo electrónico. @Html.ActionLink("Haga clic aquí para iniciar sesión", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {Key .id = "loginLink"})
    </p>
</div>
